using System.Collections.Generic;
using System.Linq;
using SysCEF.DAO.Interface;
using SysCEF.Model;
using DataAccess;

namespace SysCEF.DAO.Implementacao
{
    public class SqlProdutoRepositorio : IProdutoRepositorio
    {
        public IPersistenceBroker PersistenceBroker { get; set; }
        
        public Produto Obter(IUnitOfWork unitOfWork, int id)
        {
            return PersistenceBroker.Obter<Produto>(unitOfWork, id);
        }

        public IEnumerable<Produto> BuscarTodos(IUnitOfWork unitOfWork)
        {
            return PersistenceBroker.GetQueryable<Produto>(unitOfWork);
        }
    }
}
