using System.Collections.Generic;
using SysCEF.Model;
using DataAccess;

namespace SysCEF.DAO.Interface
{
    public interface ITipoLogradouroRepositorio
    {
        TipoLogradouro Obter(IUnitOfWork iUnitOfWork, int id);
        TipoLogradouro ObterPorSigla(IUnitOfWork unitOfWork, string sigla);
        IEnumerable<TipoLogradouro> BuscarTodos(IUnitOfWork unitOfWork);
    }
}
