using Core;
using DataAccess;
using SysCEF.Common.Interface;

namespace SysCEF.Common.Implementacao
{
    public class SysCEFPerRequestWorkLifetimeManager : PerRequestWorkLifetime, ISysCEFWorkLifetimeManager
    {
        protected override string Key
        {
            get { return "SYSCEF_UNIT_OF_WORK"; }
        }

        protected override IUnitOfWork CreateUnitOfWork()
        {
            return ObjectContainer.Get<IUnitOfWorkFactory>().StartSysCEFUnitOfWork();
        }

        public static readonly SysCEFPerRequestWorkLifetimeManager Instance = new SysCEFPerRequestWorkLifetimeManager();
    }
}
