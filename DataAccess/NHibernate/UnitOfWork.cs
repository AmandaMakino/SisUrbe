using System;
using System.Data;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using Core;
using DataAccess.Exceptions;

namespace DataAccess.NHibernate
{
    public class UnitOfWork : IUnitOfWork
    {
        private ITransaction transaction;

        public UnitOfWork(ISession session)
        {
            Session = session;
        }

        public UnitOfWork(IStatelessSession statelessSession)
        {
            StatelessSession = statelessSession;
        }

        public IObjectContainer ObjectContainer { get; set; }
        protected ISession Session { get; set; }
        protected IStatelessSession StatelessSession { get; set; }

        #region IUnitOfWork Members

        public void Flush()
        {
            if (Session != null)
            {
                try
                {
                    Session.Flush();
                }
                catch (StaleStateException e)
                {
                    throw new StaleModelException("Flush failed", e);
                }
            }
            else
            {
                throw new InvalidOperationException("Flush not allowed on a global::NHibernate.IStatelessSession.");
            }
        }

        public void Refresh(object model)
        {
            if (Session != null)
            {
                try
                {
                    Session.Refresh(model);
                }
                catch (Exception e)
                {
                    throw new RepositoryException("Refresh failed", e);
                }
            }
            else
            {
                throw new InvalidOperationException("Refresh not allowed on a global::NHibernate.IStatelessSession.");
            }
        }

        public void Clear()
        {
            if (Session != null)
            {
                try
                {
                    Session.Clear();
                }
                catch (Exception e)
                {
                    throw new RepositoryException("Clear failed", e);
                }
            }
            else
            {
                throw new InvalidOperationException("Clear not allowed on a global::NHibernate.IStatelessSession.");
            }
        }

        public void Evict(object model)
        {
            if (Session != null)
            {
                try
                {
                    Session.Evict(model);
                }
                catch (Exception e)
                {
                    throw new RepositoryException("Evict failed", e);
                }
            }
            else
            {
                throw new InvalidOperationException("Evict not allowed on a global::NHibernate.IStatelessSession.");
            }
        }

        public void BeginTransaction(IsolationLevel il)
        {
            try
            {
                if (Session != null)
                {
                    transaction = Session.BeginTransaction(il);
                }
                else
                {
                    // note that isolation level is not supported.
                    transaction = StatelessSession.BeginTransaction();
                }
            }
            catch (Exception e)
            {
                throw new RepositoryException("Begin Transaction failed", e);
            }
        }

        public void Commit()
        {
            try
            {
                if (transaction != null)
                {
                    transaction.Commit();
                    transaction = null;
                }
            }
            catch (StaleStateException e)
            {
                throw new StaleModelException("Commit failed", e);
            }
            catch (Exception e)
            {
                throw new RepositoryException("Commit failed", e);
            }
        }

        public void Rollback()
        {
            try
            {
                if (transaction != null)
                    transaction.Rollback();
                transaction = null;
            }
            catch (Exception e)
            {
                throw new RepositoryException("Rollback failed", e);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Rollback();

                if (Session != null)
                {
                    Session.Dispose();
                }
                else
                {
                    // note that isolation level is not supported.
                    StatelessSession.Dispose();
                }
            }
        }

        // INTERNAL METHODS TO BE ONLY USED BY PersistenceBroker.
        internal object Salvar<T>(T entity)
        {
            if (Session != null)
            {
                return Session.Save(entity);
            }
            else
            {
                return StatelessSession.Insert(entity);
            }
        }

        internal T Obter<T>(object id)
        {
            if (Session != null)
            {
                return Session.Get<T>(id);
            }
            else
            {
                return StatelessSession.Get<T>(id);
            }
        }

        internal T Obter<T>(T transientModel)
        {
            if (Session != null)
            {
                return Session.Get<T>(transientModel);
            }
            else
            {
                return StatelessSession.Get<T>(transientModel);
            }
        }

        internal IHQLQuery CreateHQLQuery(string queryString)
        {
            if (Session != null)
            {
                return ObjectContainer.Get<IHQLQuery>(Session.CreateQuery(queryString));
            }
            else
            {
                return ObjectContainer.Get<IHQLQuery>(StatelessSession.CreateQuery(queryString));
            }
        }

        internal IQueryable<T> GetQueryable<T>()
        {
            if (Session != null)
            {
                return Session.Query<T>();
            }
            else
            {
                throw new NotSupportedException("Stateless Sessions are not supported by LinqToNhibernate");
            }
        }

        internal ISQLQuery CreateSQLQuery(string queryString)
        {
            if (Session != null)
            {
                return ObjectContainer.Get<ISQLQuery>(Session.CreateSQLQuery(queryString));
            }
            else
            {
                return ObjectContainer.Get<ISQLQuery>(StatelessSession.CreateSQLQuery(queryString));
            }
        }

        internal void Atualizar(object model)
        {
            try
            {
                if (Session != null)
                {
                    Session.Update(model);
                }
                else
                {
                    StatelessSession.Update(model);
                }
            }
            catch (StaleStateException e)
            {
                throw new StaleModelException("Atualizar failed", e);
            }
            catch (Exception e)
            {
                throw new RepositoryException("Atualizar failed", e);
            }
        }

        internal void Excluir(object model)
        {
            try
            {
                if (Session != null)
                {
                    Session.Delete(model);
                }
                else
                {
                    StatelessSession.Delete(model);
                }
            }
            catch (StaleStateException e)
            {
                throw new StaleModelException("Excluir failed", e);
            }
            catch (Exception e)
            {
                throw new RepositoryException("Atualizar failed", e);
            }
        }

        internal IDbCommand CreateDbCommand()
        {
            IDbCommand dbCommand;

            if (Session != null)
            {
                dbCommand = Session.Connection.CreateCommand();
            }
            else
            {
                dbCommand = StatelessSession.Connection.CreateCommand();
            }

            dbCommand.CommandTimeout = 600;

            if (transaction != null)
            {
                transaction.Enlist(dbCommand);
            }

            return dbCommand;
        }
    }
}