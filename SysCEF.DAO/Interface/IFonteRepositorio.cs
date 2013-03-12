using System.Collections.Generic;
using SysCEF.Model;
using DataAccess;

namespace SysCEF.DAO.Interface
{
    public interface IFonteRepositorio
    {
        Fonte Obter(IUnitOfWork iUnitOfWork, int id);
        Fonte ObterPorCodigo(IUnitOfWork iUnitOfWork, int codgo);
        IEnumerable<Fonte> BuscarTodos(IUnitOfWork unitOfWork);
        void Salvar(IUnitOfWork unitOfWork, Fonte fonte);
        void ExcluirTudo(IUnitOfWork unitOfWork);
    }
}
