using System.Collections.Generic;
using System.Linq;
using SysCEF.DAO.Interface;
using SysCEF.Model;
using DataAccess;

namespace SysCEF.DAO.Implementacao
{
    public class SqlEstadoRepositorio : IEstadoRepositorio
    {
        public IPersistenceBroker PersistenceBroker { get; set; }
        
        public Estado Obter(IUnitOfWork unitOfWork, int id)
        {
            return PersistenceBroker.Obter<Estado>(unitOfWork, id);
        }

        public Estado ObterPorSigla(IUnitOfWork unitOfWork, string sigla)
        {
            return PersistenceBroker.GetQueryable<Estado>(unitOfWork)
                .Where(p => p.Sigla == sigla)
                .Select(p => p)
                .SingleOrDefault();
        }

        public IEnumerable<Estado> BuscarTodos(IUnitOfWork unitOfWork)
        {
            return PersistenceBroker.GetQueryable<Estado>(unitOfWork);
        }
    }
}
