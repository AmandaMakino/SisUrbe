using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Core;
using SysCEF.Common.Interface;
using SysCEF.DAO.Interface;
using SysCEF.Model;
using SysCEF.Web.Models;
using System.Text;
using System.Security.Cryptography;

namespace SysCEF.Web.Controllers
{
    public class UsuarioController : Controller
    {
        public ISysCEFWorkLifetimeManager WorkLifetimeManager { get; set; }
        public IUsuarioRepositorio UsuarioRepositorio { get; set; }

        public ActionResult Lista()
        {
            var usuarios = UsuarioRepositorio.BuscarTodos(WorkLifetimeManager.Value).ToList();

            return PartialView(new ListaUsuarioViewModel { Usuarios = usuarios });
        }

        public ActionResult Index(int id = 0)
        {
            var usuario = id > 0 ? UsuarioRepositorio.Obter(WorkLifetimeManager.Value, id) : new Usuario { Perfil = (int)EnumPerfil.UsuarioComum};

            return PartialView(new UsuarioViewModel
                                   {
                                       Usuario = new UsuarioModel
                                                     {
                                                         Id = usuario.UsuarioId,
                                                         Nome = usuario.Nome,
                                                         Email = usuario.Email,
                                                         Perfil = usuario.Perfil,
                                                         CPF = usuario.CPF,
                                                         CREA = usuario.CREA
                                                     },

                                       OpcoesPerfil = RadioButtonHelper.ParseEnumToRadioButtonList((EnumPerfil) usuario.Perfil)
                                   });
        }

        public ActionResult Salvar(UsuarioViewModel viewModel)
        {
            string mensagem;

            WorkLifetimeManager.Value.BeginTransaction(IsolationLevel.Serializable);
            
            try
            {
                Usuario usuario;

                if (viewModel.Usuario.Id > 0)
                    usuario = UsuarioRepositorio.Obter(WorkLifetimeManager.Value, viewModel.Usuario.Id);
                else
                {
                    // Para evitar duplicação de dados.
                    if (UsuarioRepositorio.ObterPorEmail(WorkLifetimeManager.Value, viewModel.Usuario.Email) != null)
                        throw new InvalidOperationException("Já existe um usuário cadastrado com esse e-mail.");
                    usuario = new Usuario();
                }

                usuario.Nome = viewModel.Usuario.Nome;
                usuario.CPF = viewModel.Usuario.CPF;
                usuario.CREA = viewModel.Usuario.CREA;
                usuario.Email = viewModel.Usuario.Email;
                usuario.Perfil = (int) viewModel.OpcoesPerfil.SelectedValue;

                if (!string.IsNullOrEmpty(viewModel.Usuario.Senha))
                    usuario.Senha = Convert.ToBase64String(new SHA512Managed().ComputeHash(Encoding.ASCII.GetBytes(viewModel.Usuario.Senha)));

                UsuarioRepositorio.Salvar(WorkLifetimeManager.Value, usuario);
                WorkLifetimeManager.Value.Commit();

                mensagem = "Operação realizada com sucesso!";
            }
            catch (Exception exception)
            {
                WorkLifetimeManager.Value.Rollback();
                mensagem = "Não foi possível realizar operação: " + exception.Message;
            }
            
            var usuarios = UsuarioRepositorio.BuscarTodos(WorkLifetimeManager.Value);
            return PartialView("Lista", new ListaUsuarioViewModel { Usuarios = usuarios, Mensagem = mensagem });

        }
        
        public ActionResult Excluir(int id = 0)
        {
            if (id == 0)
                throw new InvalidOperationException("Não foi possível excluir usuário. Faltando passar o id do usuário!");

            string mensagem;

            WorkLifetimeManager.Value.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                UsuarioRepositorio.Excluir(WorkLifetimeManager.Value, id);
                WorkLifetimeManager.Value.Commit();

                mensagem = "Operação realizada com sucesso!";
            }
            catch (Exception exception)
            {
                WorkLifetimeManager.Value.Rollback();
                mensagem = "Não foi possível realizar operação: " + exception.Message;
            }

            var usuarios = UsuarioRepositorio.BuscarTodos(WorkLifetimeManager.Value);
            return PartialView("Lista", new ListaUsuarioViewModel { Usuarios = usuarios, Mensagem = mensagem });
        }
        
        //public ActionResult AlterarConta()
        //{
        //    return View();
        //}

        //public ActionResult SalvarConta()
        //{
        //    //if (ModelState.IsValid)
        //    //{

        //    //    // ChangePassword will throw an exception rather
        //    //    // than return false in certain failure scenarios.
        //    //    bool changePasswordSucceeded;
        //    //    try
        //    //    {
        //    //        MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
        //    //        changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
        //    //    }
        //    //    catch (Exception)
        //    //    {
        //    //        changePasswordSucceeded = false;
        //    //    }

        //    //    if (changePasswordSucceeded)
        //    //    {
        //    //        return RedirectToAction("ChangePasswordSuccess");
        //    //    }
        //    //    else
        //    //    {
        //    //        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
        //    //    }
        //    //}

        //    // If we got this far, something failed, redisplay form
        //    return View("AlterarConta");
        //}
    }
}
