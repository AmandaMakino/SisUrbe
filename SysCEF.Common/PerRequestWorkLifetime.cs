using System;
using System.Web;
using DataAccess;

namespace SysCEF.Common
{
    public abstract class PerRequestWorkLifetime : IWorkLifetimeManager, IDisposable
    {
        protected abstract string Key { get; }
        protected abstract IUnitOfWork CreateUnitOfWork();

        private Func<HttpContextBase> Context { get; set; }

        protected PerRequestWorkLifetime()
        {
            Context = (() => new HttpContextWrapper(HttpContext.Current));
        }

        protected PerRequestWorkLifetime(Func<HttpContextBase> currentContext)
        {
            Context = currentContext;
        }

        public IUnitOfWork Value
        {
            get
            {
                var returnValue = GetValueFromCache();
                if (returnValue == null)
                {
                    returnValue = CreateUnitOfWork();
                    SetValueInCache(returnValue);
                }

                return returnValue;
            }
        }


        //FIXME: refactor these three methods into a null object pattern to remove repetition? 
        private IUnitOfWork GetValueFromCache()
        {
            if (Context != null)
            {
                return Context().Items[Key] as IUnitOfWork;
            }
            return null;
        }

        private void SetValueInCache(IUnitOfWork newUnitOfWork)
        {
            if (Context != null)
            {
                Context().Items[Key] = newUnitOfWork;
            }
        }

        public void Dispose()
        {
            var unitOfWork = GetValueFromCache();
            if (unitOfWork != null)
            {
                unitOfWork.Dispose();
                Context().Items.Remove(Key);
            }
        }
    }
}
