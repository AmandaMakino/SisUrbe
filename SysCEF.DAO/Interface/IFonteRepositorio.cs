using System.Collections.Generic;
using SysCEF.Model;
using DataAccess;

namespace SysCEF.DAO.Interface
{
    public interface IFonteRepositorio
    {
        Fonte Obter(IUnitOfWork iUnitOfWork, int id);
        IEnumerable<Fonte> BuscarTodos(IUnitOfWork unitOfWork);
        void Salvar(IUnitOfWork unitOfWork, Fonte fonte);
        void ExcluirTudo(IUnitOfWork unitOfWork);
    }
}
