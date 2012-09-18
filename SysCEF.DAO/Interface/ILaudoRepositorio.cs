using System.Collections.Generic;
using SysCEF.Model;
using DataAccess;

namespace SysCEF.DAO.Interface
{
    public interface ILaudoRepositorio
    {
        void Salvar(IUnitOfWork unitOfWork, Laudo laudo);
        void Atualizar(IUnitOfWork unitOfWork, Laudo laudo);
        void Excluir(IUnitOfWork unitOfWork, int laudoId);
        Laudo Obter(IUnitOfWork unitOfWork, int laudoId);
        Laudo ObterPorReferencia(IUnitOfWork unitOfWork, string referencia);
        IEnumerable<Laudo> BuscarTodos(IUnitOfWork unitOfWork);
        IEnumerable<Laudo> BuscarLaudosPorStatus(IUnitOfWork unitOfWork, EnumStatusLaudo enumStatusLaudo);
    }
}
