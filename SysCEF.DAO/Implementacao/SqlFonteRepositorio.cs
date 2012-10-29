using System.Collections.Generic;
using System.Linq;
using SysCEF.DAO.Interface;
using SysCEF.Model;
using DataAccess;

namespace SysCEF.DAO.Implementacao
{
    public class SqlFonteRepositorio : IFonteRepositorio
    {
        public IPersistenceBroker PersistenceBroker { get; set; }
        
        public Fonte Obter(IUnitOfWork unitOfWork, int id)
        {
            return PersistenceBroker.Obter<Fonte>(unitOfWork, id);
        }

        public IEnumerable<Fonte> BuscarTodos(IUnitOfWork unitOfWork)
        {
            return PersistenceBroker.GetQueryable<Fonte>(unitOfWork);
        }
    }
}
