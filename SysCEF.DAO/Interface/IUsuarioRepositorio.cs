using System.Collections.Generic;
using SysCEF.Model;
using DataAccess;

namespace SysCEF.DAO.Interface
{
    public interface IUsuarioRepositorio
    {
        void Salvar(IUnitOfWork unitOfWork, Usuario usuario);
        void Atualizar(IUnitOfWork unitOfWork, Usuario usuario);
        void Excluir(IUnitOfWork unitOfWork, int usuarioId);
        Usuario Obter(IUnitOfWork unitOfWork, int usuarioId);
        Usuario ObterPorEmail(IUnitOfWork unitOfWork, string email);
        IEnumerable<Usuario> BuscarTodos(IUnitOfWork unitOfWork);
        IEnumerable<Usuario> BuscarPorPerfil(IUnitOfWork iUnitOfWork, EnumPerfil enumPerfil);
    }
}
