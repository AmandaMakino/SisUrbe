using System;
using System.Web;
using DataAccess;
using SysCEF.DAO.Interface;
using SysCEF.Model;

namespace SysCEF.Common.Implementacao
{
    public class UsuarioLogado : UsuarioLogadoProvider
    {
        // ReSharper disable InconsistentNaming
        public const string SESSION_ID_KEY = "SessionID";
        public const string USUARIO_ID_SESSION_KEY = "UsuarioID";
        public const string E_ADMINISTRADOR_KEY = "EAdministrador";
        private const string USUARIO_KEY = "UsuarioLogado.Usuario";
        // ReSharper restore InconsistentNaming

        public UsuarioLogado() : base(SysCEFPerRequestWorkLifetimeManager.Instance)
        {
        }

        public UsuarioLogado(IWorkLifetimeManager workLifetimeManager, IUsuarioRepositorio usuarioRepositorio)
            : base(workLifetimeManager, usuarioRepositorio)
        {
        }

        public override int UsuarioId
        {
            get
            {
                var usuarioId = HttpContext.Current.Session[USUARIO_ID_SESSION_KEY] as int?;

                if (!usuarioId.HasValue)
                {
                    throw new InvalidOperationException("Id do usuário não disponível em sessão. Sessão foi expirada?");
                }

                return usuarioId.Value;
            }
        }

        public override bool EAdministrador
        {
            get
            {
                var eAdministrador = HttpContext.Current.Session[E_ADMINISTRADOR_KEY] as bool?;

                return eAdministrador.HasValue && eAdministrador.Value;
            }
        }

        protected override Usuario ObterUsuarioCache()
        {
            return HttpContext.Current.Items[USUARIO_KEY] as Usuario;
        }

        protected override void GuardarUsuarioCache(Usuario usuario)
        {
            HttpContext.Current.Items[USUARIO_KEY] = usuario;
        }
    }
}