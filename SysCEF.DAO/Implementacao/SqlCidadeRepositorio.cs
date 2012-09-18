using System.Collections.Generic;
using System.Linq;
using SysCEF.DAO.Interface;
using SysCEF.Model;
using DataAccess;

namespace SysCEF.DAO.Implementacao
{
    public class SqlCidadeRepositorio : ICidadeRepositorio
    {
        public IPersistenceBroker PersistenceBroker { get; set; }
        
        public Cidade Obter(IUnitOfWork unitOfWork, int id)
        {
            return PersistenceBroker.Obter<Cidade>(unitOfWork, id);
        }

        public IEnumerable<Estado> BuscarTodos(IUnitOfWork unitOfWork)
        {
            return PersistenceBroker.GetQueryable<Estado>(unitOfWork);
        }

        public Cidade ObterPorNomeUF(IUnitOfWork unitOfWork, string nome, string uf)
        {
            return PersistenceBroker.GetQueryable<Cidade>(unitOfWork)
                .Where(c => c.Nome.Equals(nome) && c.Estado.Sigla == uf)
                .Select(c => c)
                .SingleOrDefault();
        }

        public IEnumerable<Cidade> BuscarTodasEstado(IUnitOfWork unitOfWork, string uf)
        {
            return PersistenceBroker.GetQueryable<Cidade>(unitOfWork)
                .Where(c => c.Estado.Sigla == uf)
                .Select(c => c);
        }
    }
}
