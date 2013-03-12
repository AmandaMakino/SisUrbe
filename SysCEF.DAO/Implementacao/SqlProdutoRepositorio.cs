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

        public void Salvar(IUnitOfWork unitOfWork, Produto produto)
        {
            PersistenceBroker.Salvar<Produto>(unitOfWork, produto);
        }

        public void ExcluirTudo(IUnitOfWork unitOfWork)
        {
            var itens = PersistenceBroker.GetQueryable<Produto>(unitOfWork);

            foreach (var item in itens)
            {
                var produto = Obter(unitOfWork, item.ProdutoID);
                PersistenceBroker.Excluir(unitOfWork, produto);
            }
        }
    }
}
