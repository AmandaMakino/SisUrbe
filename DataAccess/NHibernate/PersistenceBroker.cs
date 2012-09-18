using System.Data;
using System.Linq;
using Core;

namespace DataAccess.NHibernate
{
    public class PersistenceBroker : IPersistenceBroker
    {
        public IObjectContainer ObjectContainer { get; set; }

        #region IPersistenceBroker Members

        public object Salvar<T>(IUnitOfWork unitOfWork, T entity)
        {
            return ((UnitOfWork) unitOfWork).Salvar(entity);
        }

        public T Obter<T>(IUnitOfWork unitOfWork, object id)
        {
            return ((UnitOfWork) unitOfWork).Obter<T>(id);
        }

        public T Obter<T>(IUnitOfWork unitOfWork, T transientModel)
        {
            return ((UnitOfWork) unitOfWork).Obter(transientModel);
        }

        public IHQLQuery CreateHQLQuery(IUnitOfWork unitOfWork, string queryString)
        {
            return ((UnitOfWork) unitOfWork).CreateHQLQuery(queryString);
        }

        public IQueryable<T> GetQueryable<T>(IUnitOfWork unitOfWork)
        {
            return ((UnitOfWork) unitOfWork).GetQueryable<T>();
        }

        public ISQLQuery CreateSQLQuery(IUnitOfWork unitOfWork, string queryString)
        {
            return ((UnitOfWork) unitOfWork).CreateSQLQuery(queryString);
        }

        public void Atualizar(IUnitOfWork unitOfWork, object model)
        {
            ((UnitOfWork) unitOfWork).Atualizar(model);
        }

        public void Excluir(IUnitOfWork unitOfWork, object model)
        {
            ((UnitOfWork) unitOfWork).Excluir(model);
        }

        public IDbCommand CreateDBCommand(IUnitOfWork unitOfWork)
        {
            return ((UnitOfWork) unitOfWork).CreateDbCommand();
        }

        public IADOQuery CreateADOQuery(IUnitOfWork unitOfWork, string queryString)
        {
            var cmd = ((UnitOfWork) unitOfWork).CreateDbCommand();

            return new ADOQuery(cmd, queryString);
        }

        #endregion
    }
}