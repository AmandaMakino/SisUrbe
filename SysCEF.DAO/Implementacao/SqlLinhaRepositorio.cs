using System.Collections.Generic;
using System.Linq;
using SysCEF.DAO.Interface;
using SysCEF.Model;
using DataAccess;

namespace SysCEF.DAO.Implementacao
{
    public class SqlLinhaRepositorio : ILinhaRepositorio
    {
        public IPersistenceBroker PersistenceBroker { get; set; }
        
        public Linha Obter(IUnitOfWork unitOfWork, int id)
        {
            return PersistenceBroker.Obter<Linha>(unitOfWork, id);
        }

        public IEnumerable<Linha> BuscarTodos(IUnitOfWork unitOfWork)
        {
            return PersistenceBroker.GetQueryable<Linha>(unitOfWork);
        }
    }
}
