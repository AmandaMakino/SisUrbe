using System.Collections.Generic;
using SysCEF.Model;
using DataAccess;

namespace SysCEF.DAO.Interface
{
    public interface IEstadoRepositorio
    {
        Estado Obter(IUnitOfWork unitOfWork, int id);
        Estado ObterPorSigla(IUnitOfWork unitOfWork, string sigla);
        IEnumerable<Estado> BuscarTodos(IUnitOfWork unitOfWork);
    }
}
