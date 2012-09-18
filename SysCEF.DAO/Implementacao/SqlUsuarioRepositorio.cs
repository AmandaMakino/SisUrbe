using System.Collections.Generic;
using System.Linq;
using SysCEF.DAO.Interface;
using SysCEF.Model;
using DataAccess;

namespace SysCEF.DAO.Implementacao
{
    public class SqlUsuarioRepositorio : IUsuarioRepositorio
    {
        public IPersistenceBroker PersistenceBroker { get; set; }

        public void Salvar(IUnitOfWork unitOfWork, Usuario usuario)
        {
            PersistenceBroker.Salvar(unitOfWork, usuario);
        }

        public void Atualizar(IUnitOfWork unitOfWork, Usuario usuario)
        {
            PersistenceBroker.Atualizar(unitOfWork, usuario);
        }

        public void Excluir(IUnitOfWork unitOfWork, int usuarioId)
        {
            var usuario = Obter(unitOfWork, usuarioId);
            if (usuario != null)
                PersistenceBroker.Excluir(unitOfWork, usuario);
        }

        public Usuario Obter(IUnitOfWork unitOfWork, int usuarioId)
        {
            return PersistenceBroker.GetQueryable<Usuario>(unitOfWork)
                .Where(p => p.UsuarioId == usuarioId)
                .Select(p => p)
                .SingleOrDefault();
        }

        public Usuario ObterPorEmail(IUnitOfWork unitOfWork, string email)
        {
            return PersistenceBroker.GetQueryable<Usuario>(unitOfWork)
                .Where(p => p.Email == email)
                .Select(p => p)
                .SingleOrDefault();
        }

        public IEnumerable<Usuario> BuscarTodos(IUnitOfWork unitOfWork)
        {
            return PersistenceBroker.GetQueryable<Usuario>(unitOfWork);
        }

        public IEnumerable<Usuario> BuscarPorPerfil(IUnitOfWork unitOfWork, EnumPerfil enumPerfil)
        {
            return PersistenceBroker.GetQueryable<Usuario>(unitOfWork)
                .Where(p => p.Perfil == (int) enumPerfil)
                .Select(p => p);
        }
    }
}
