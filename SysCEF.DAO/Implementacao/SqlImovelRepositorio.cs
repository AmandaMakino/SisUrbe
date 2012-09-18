using System.Collections.Generic;
using System.Linq;
using SysCEF.DAO.Interface;
using SysCEF.Model;
using DataAccess;

namespace SysCEF.DAO.Implementacao
{
    public class SqlImovelRepositorio : IImovelRepositorio
    {
        public IPersistenceBroker PersistenceBroker { get; set; }

        public void Salvar(IUnitOfWork unitOfWork, Imovel imovel)
        {
            PersistenceBroker.Salvar(unitOfWork, imovel);
        }

        public void Atualizar(IUnitOfWork unitOfWork, Imovel imovel)
        {
            PersistenceBroker.Atualizar(unitOfWork, imovel);
        }

        public void Excluir(IUnitOfWork unitOfWork, int imovelID)
        {
            var Imovel = Obter(unitOfWork, imovelID);
            if (Imovel != null)
                PersistenceBroker.Excluir(unitOfWork, Imovel);
        }

        public Imovel Obter(IUnitOfWork unitOfWork, int imovelID)
        {
            return PersistenceBroker.GetQueryable<Imovel>(unitOfWork)
                .Where(p => p.ImovelID == imovelID)
                .Select(p => p)
                .SingleOrDefault();
        }

        public IEnumerable<Imovel> BuscarTodos(IUnitOfWork unitOfWork)
        {
            return PersistenceBroker.GetQueryable<Imovel>(unitOfWork);
        }
    }
}
