using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NHibernate;
using NHibernate.ByteCode.Castle;
using NHibernate.Cfg;
using Core;

namespace DataAccess.NHibernate
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private static ISessionFactory _SysCEFDBSessionFactory;

        private static readonly object _LockTarget = new object();

        public IObjectContainer ObjectContainer { get; set; }

        public IUnitOfWork StartSysCEFUnitOfWork(params UnitOfWorkOption[] options)
        {
            InitializeSessionFactory(
                ref _SysCEFDBSessionFactory,
                "NHibernate.Dialect.MsSql2005Dialect",
                ConnectionStrings.SysCEFDBSettings,
                "NHibernate.Driver.SqlClientDriver",
                Assembly.Load("SysCEF.DAO")); 

            return StartUnitOfWork(_SysCEFDBSessionFactory, options);
        }

        private static void InitializeSessionFactory(
            ref ISessionFactory sessionFactory,
            string databaseDialect,
            string connectionString,
            string driverClass,
            Assembly businessObjectAssembly)
        {
            if (sessionFactory != null)
                return;

            lock (_LockTarget)
            {
                var nhConfigs = new Configuration();
                nhConfigs.SetProperty("dialect", databaseDialect);
                nhConfigs.SetProperty("connection.connection_string", connectionString);
                nhConfigs.SetProperty("connection.driver_class", driverClass);
                nhConfigs.SetProperty("proxyfactory.factory_class",
                                      "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
                nhConfigs.SetProperty("command_timeout", "600");
                nhConfigs.SetProperty("hbm2ddl.keywords", "auto-quote");
                nhConfigs.SetProperty("connection.isolation", "ReadUncommitted");

                //L2 Cache
                //nhConfigs.SetProperty("cache.use_second_level_cache", "true");
                //nhConfigs.SetProperty("cache.use_query_cache", "true");
                //nhConfigs.SetProperty("cache.provider_class", "NHibernate.Caches.SysCache.SysCacheProvider, NHibernate.Caches.SysCache");

#if DEBUG
                nhConfigs.SetProperty("generate_statistics", "true");
                nhConfigs.SetProperty("show_sql", "false");
                nhConfigs.SetProperty("format_sql", "true");
#endif

                nhConfigs.AddAssembly(businessObjectAssembly);

                sessionFactory = nhConfigs.BuildSessionFactory();
            }
        }

        private static IUnitOfWork StartUnitOfWork(ISessionFactory sessionFactory, IEnumerable<UnitOfWorkOption> options)
        {
            if (options != null && options.Contains(UnitOfWorkOption.Stateless))
            {
                return new UnitOfWork(sessionFactory.OpenStatelessSession());
            }

            var session = sessionFactory.OpenSession();
            session.FlushMode = FlushMode.Commit;
            return new UnitOfWork(session) {ObjectContainer = Core.ObjectContainer.Get<IObjectContainer>()};
        }

        #region Nested type: HackClassToLoadBytecodeReference

        private static class HackClassToLoadBytecodeReference
        {
            private static void MakeReferenceSoMSBuildLoadsDLL()
            {
                //This is never called, but it forces the compiler to include the ByteCode DLL
                var x = new ProxyFactory();
            }
        }

        #endregion
    }
}
