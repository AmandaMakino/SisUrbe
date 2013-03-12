using System.Collections.Generic;
using SysCEF.Model;
using DataAccess;

namespace SysCEF.DAO.Interface
{
    public interface IProdutoRepositorio
    {
        Produto Obter(IUnitOfWork iUnitOfWork, int id);
        Produto ObterPorCodigo(IUnitOfWork iUnitOfWork, int codgo);
        IEnumerable<Produto> BuscarTodos(IUnitOfWork unitOfWork);
        void Salvar(IUnitOfWork unitOfWork, Produto produto);
        void ExcluirTudo(IUnitOfWork unitOfWork);
    }
}
