using System.Collections.Generic;
using System.Linq;
using SysCEF.DAO.Interface;
using SysCEF.Model;
using DataAccess;

namespace SysCEF.DAO.Implementacao
{
    public class SqlTipoLogradouroRepositorio : ITipoLogradouroRepositorio
    {
        public IPersistenceBroker PersistenceBroker { get; set; }
        
        public TipoLogradouro Obter(IUnitOfWork unitOfWork, int id)
        {
            return PersistenceBroker.Obter<TipoLogradouro>(unitOfWork, id);
        }

        public TipoLogradouro ObterPorSigla(IUnitOfWork unitOfWork, string sigla)
        {
            return PersistenceBroker.GetQueryable<TipoLogradouro>(unitOfWork)
                .Where(p => p.Sigla.Equals(sigla))
                .Select(p => p)
                .SingleOrDefault();
        }

        public IEnumerable<TipoLogradouro> BuscarTodos(IUnitOfWork unitOfWork)
        {
            return PersistenceBroker.GetQueryable<TipoLogradouro>(unitOfWork);
        }
    }
}
