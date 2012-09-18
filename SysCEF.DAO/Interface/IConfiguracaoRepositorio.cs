using SysCEF.Model;
using DataAccess;

namespace SysCEF.DAO.Interface
{
    public interface IConfiguracaoRepositorio
    {
        Configuracao Obter(IUnitOfWork unitOfWork);
        void Salvar(IUnitOfWork unitOfWork, Configuracao configuracao);
    }
}
