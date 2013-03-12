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

        public Fonte ObterPorCodigo(IUnitOfWork unitOfWork, int codigo)
        {
            return PersistenceBroker.GetQueryable<Fonte>(unitOfWork)
                .Where(p => p.Codigo == codigo)
                .Select(p => p)
                .SingleOrDefault();
        }

        public IEnumerable<Fonte> BuscarTodos(IUnitOfWork unitOfWork)
        {
            return PersistenceBroker.GetQueryable<Fonte>(unitOfWork);
        }

        public void Salvar(IUnitOfWork unitOfWork, Fonte fonte)
        {
            PersistenceBroker.Salvar<Fonte>(unitOfWork, fonte);
        }

        public void ExcluirTudo(IUnitOfWork unitOfWork)
        {
            var itens = PersistenceBroker.GetQueryable<Fonte>(unitOfWork);

            foreach (var item in itens)
            {
                var fonte = Obter(unitOfWork, item.FonteID);
                PersistenceBroker.Excluir(unitOfWork, fonte);
            }
        }
    }
}
