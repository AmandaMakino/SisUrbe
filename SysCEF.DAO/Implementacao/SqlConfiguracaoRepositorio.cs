using System.Linq;
using SysCEF.DAO.Interface;
using SysCEF.Model;
using DataAccess;

namespace SysCEF.DAO.Implementacao
{
    public class SqlConfiguracaoRepositorio : IConfiguracaoRepositorio
    {
        public IPersistenceBroker PersistenceBroker { get; set; }

        public void Salvar(IUnitOfWork unitOfWork, Configuracao configuracao)
        {
            PersistenceBroker.Salvar(unitOfWork, configuracao);
        }

        public Configuracao Obter(IUnitOfWork unitOfWork)
        {
            return PersistenceBroker.GetQueryable<Configuracao>(unitOfWork).SingleOrDefault();
        }
    }
}
