using System.Collections.Generic;
using SysCEF.Model;
using DataAccess;

namespace SysCEF.DAO.Interface
{
    public interface IProdutoRepositorio
    {
        Produto Obter(IUnitOfWork iUnitOfWork, int id);
        IEnumerable<Produto> BuscarTodos(IUnitOfWork unitOfWork);
    }
}
