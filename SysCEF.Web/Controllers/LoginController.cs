using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using SysCEF.Common.Implementacao;
using SysCEF.Common.Interface;
using SysCEF.DAO.Interface;
using SysCEF.Model;
using SysCEF.Web.Models;

namespace SysCEF.Web.Controllers
{
    public class LoginController : Controller
    {
        public ISysCEFWorkLifetimeManager WorkLifetimeManager { get; set; }
        public IUsuarioRepositorio UsuarioRepositorio { get; set; }
        
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (ValidarUsuario(model.Email, model.Senha))
                {
                    if (new UsuarioLogado().Usuario.DeveDefinirNovaSenha)
                        return RedirectToAction("RedefinirSenha");
                    
                    return RedirectToAction("Index", "Home");
                }
                
                model.DadosInvalidos = true;
            }

            return View("Index", model);
        }

        public string GerarNovaSenha(string email)
        {
            WorkLifetimeManager.Value.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                if (string.IsNullOrEmpty(email))
                    throw new InvalidOperationException("É necessário informar seu e-mail.");

                var usuario = UsuarioRepositorio.ObterPorEmail(WorkLifetimeManager.Value, email);
                if (usuario == null)
                    throw new InvalidOperationException("E-mail incorreto. Verifique e tente novamente.");

                var senhaAleatoria = new GeradorSenha().Gerar();
                var senhaCodificada = Convert.ToBase64String(new SHA512Managed().ComputeHash(Encoding.ASCII.GetBytes(senhaAleatoria)));

                usuario.Senha = senhaCodificada;
                usuario.DeveDefinirNovaSenha = true;

                UsuarioRepositorio.Salvar(WorkLifetimeManager.Value, usuario);
                WorkLifetimeManager.Value.Commit();

                return string.Format("Sua senha temporária é: {0} ", senhaAleatoria);
            }
            catch (Exception ex)
            {
                WorkLifetimeManager.Value.Rollback();
                return ex.Message;
            }
        }

        public ActionResult RedefinirSenha()
        {
            return View(new AlterarSenhaModel());
        }

        public ActionResult AlterarSenha(AlterarSenhaModel senhaModel)
        {
            WorkLifetimeManager.Value.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                var usuarioId = new UsuarioLogado().UsuarioId;

                var usuario = UsuarioRepositorio.Obter(WorkLifetimeManager.Value, usuarioId);
                if (usuario == null)
                    throw new InvalidOperationException(string.Format("Não foi possível encontrar usuário com id: {0}.", usuarioId));

                var senhaCodificada = Convert.ToBase64String(new SHA512Managed().ComputeHash(Encoding.ASCII.GetBytes(senhaModel.NovaSenha)));

                usuario.Senha = senhaCodificada;
                usuario.DeveDefinirNovaSenha = false;

                UsuarioRepositorio.Salvar(WorkLifetimeManager.Value, usuario);
                WorkLifetimeManager.Value.Commit();

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                WorkLifetimeManager.Value.Rollback();
                return View("RedefinirSenha", new AlterarSenhaModel { MensagemErro = ex.Message });
            }
        }

        public ActionResult Logoff()
        {
            Session.Clear();

            return RedirectToAction("Index", "Login");
        }

        private bool ValidarUsuario(string email, string senha)
        {
            var usuario = UsuarioRepositorio.ObterPorEmail(WorkLifetimeManager.Value, email);

            if (usuario == null)
                return false;

            var senhaCodificada = Convert.ToBase64String(new SHA512Managed().ComputeHash(Encoding.ASCII.GetBytes(senha.Trim())));

            if (senhaCodificada != usuario.Senha)
                return false;

            Session[UsuarioLogado.SESSION_ID_KEY] = Session.SessionID;
            Session[UsuarioLogado.USUARIO_ID_SESSION_KEY] = usuario.UsuarioId;
            Session[UsuarioLogado.E_ADMINISTRADOR_KEY] = usuario.Perfil == (int) EnumPerfil.Administrador;

            return true;
        }
    }
}
