using System;
using System.Data;

namespace DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        void Flush();
        void Refresh(object model);
        void Clear();
        void Evict(object model);

        void BeginTransaction(IsolationLevel il);
        void Commit();
        void Rollback();
    }
}