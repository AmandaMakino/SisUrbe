using System.Collections.Generic;
using SysCEF.Model;
using DataAccess;

namespace SysCEF.DAO.Interface
{
    public interface ILinhaRepositorio
    {
        Linha Obter(IUnitOfWork iUnitOfWork, int id);
        IEnumerable<Linha> BuscarTodos(IUnitOfWork unitOfWork);
    }
}
