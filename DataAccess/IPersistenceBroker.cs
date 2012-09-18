using System.Data;
using System.Linq;

namespace DataAccess
{
    /// <summary>
    ///   Exposes the execution interface of the unit of work.
    /// </summary>
    public interface IPersistenceBroker
    {
        object Salvar<T>(IUnitOfWork unitOfWork, T entity);

        T Obter<T>(IUnitOfWork unitOfWork, object id);
        T Obter<T>(IUnitOfWork unitOfWork, T transientModel);
        IHQLQuery CreateHQLQuery(IUnitOfWork unitOfWork, string queryString);
        IQueryable<T> GetQueryable<T>(IUnitOfWork unitOfWork);
        ISQLQuery CreateSQLQuery(IUnitOfWork unitOfWork, string queryString);
        IADOQuery CreateADOQuery(IUnitOfWork unitOfWork, string queryString);

        void Atualizar(IUnitOfWork unitOfWork, object model);

        void Excluir(IUnitOfWork unitOfWork, object model);

        IDbCommand CreateDBCommand(IUnitOfWork unitOfWork);
    }
}