using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using DataAccess;
using DataAccess.NHibernate;
using SysCEF.Common.Implementacao;
using SysCEF.Common.Interface;
using SysCEF.DAO.Implementacao;
using SysCEF.DAO.Interface;
using SysCEF.Model;

namespace SysCEF.Common
{
    public abstract class UsuarioLogadoProvider : IUsuarioLogado
    {
        protected UsuarioLogadoProvider(IWorkLifetimeManager workLifetimeManager)
        {
            UnitOfWork = workLifetimeManager.Value;
            UsuarioRepositorio = new SqlUsuarioRepositorio
            {
                PersistenceBroker = new PersistenceBroker()
            };
        }

        protected UsuarioLogadoProvider(IWorkLifetimeManager workLifetimeManager, IUsuarioRepositorio usuarioRepositorio)
        {
            UnitOfWork = workLifetimeManager.Value;
            UsuarioRepositorio = usuarioRepositorio;
        }

        public abstract int UsuarioId { get; }

        public abstract bool EAdministrador { get; }

        public IUsuarioRepositorio UsuarioRepositorio { get; set; }

        public IUnitOfWork UnitOfWork { get; set; }
        
        public virtual Usuario Usuario
        {
            get
            {
                var resultado = ObterUsuarioCache();
                if (resultado == null)
                {
                    resultado = UsuarioRepositorio.Obter(UnitOfWork, UsuarioId);

                    if (resultado != null)
                        GuardarUsuarioCache(resultado);
                }

                return resultado;
            }
        }

        protected abstract Usuario ObterUsuarioCache();
        protected abstract void GuardarUsuarioCache(Usuario usuario);
    }
}
