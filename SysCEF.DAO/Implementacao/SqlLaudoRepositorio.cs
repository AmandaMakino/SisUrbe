using System.Collections.Generic;
using System.Linq;
using SysCEF.DAO.Interface;
using SysCEF.Model;
using DataAccess;

namespace SysCEF.DAO.Implementacao
{
    public class SqlLaudoRepositorio : ILaudoRepositorio
    {
        public IPersistenceBroker PersistenceBroker { get; set; }

        public void Salvar(IUnitOfWork unitOfWork, Laudo laudo)
        {
            PersistenceBroker.Salvar(unitOfWork, laudo);
        }

        public void Atualizar(IUnitOfWork unitOfWork, Laudo laudo)
        {
            PersistenceBroker.Atualizar(unitOfWork, laudo);
        }

        public void Excluir(IUnitOfWork unitOfWork, int laudoId)
        {
            var laudo = Obter(unitOfWork, laudoId);
            if (laudo != null)
                PersistenceBroker.Excluir(unitOfWork, laudo);
        }

        public Laudo Obter(IUnitOfWork unitOfWork, int laudoId)
        {
            return PersistenceBroker.GetQueryable<Laudo>(unitOfWork)
                .Where(p => p.LaudoID == laudoId)
                .Select(p => p)
                .SingleOrDefault();
        }

        public Laudo ObterPorReferencia(IUnitOfWork unitOfWork, string referencia)
        {
            return PersistenceBroker.GetQueryable<Laudo>(unitOfWork)
                .Where(p => p.Referencia.Equals(referencia))
                .Select(p => p)
                .SingleOrDefault();
        }

        public IEnumerable<Laudo> BuscarTodos(IUnitOfWork unitOfWork)
        {
            return PersistenceBroker.GetQueryable<Laudo>(unitOfWork);
        }
        
        public IEnumerable<Laudo> BuscarLaudosPorStatus(IUnitOfWork unitOfWork, EnumStatusLaudo enumStatusLaudo)
        {
            return PersistenceBroker.GetQueryable<Laudo>(unitOfWork)
                .Where(p => p.Status == (int) enumStatusLaudo)
                .Select(p => p);
        }
    }
}
