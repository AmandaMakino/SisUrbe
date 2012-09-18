using Core;
using DataAccess.NHibernate;

namespace DataAccess
{
    public class DependencyConfigurator
    {
        private static bool _IsConfigured;

        public static void ConfigureDependencies()
        {
            if (_IsConfigured)
                return;

            ObjectContainer.AddSingletonDefinition(typeof (IObjectContainer), typeof (ObjectContainer));
            ObjectContainer.AddSingletonDefinition(typeof (IUnitOfWorkFactory), typeof (UnitOfWorkFactory));
            ObjectContainer.AddPrototypeDefinition(typeof (IADOQuery), typeof (ADOQuery));
            ObjectContainer.AddPrototypeDefinition(typeof (IHQLQuery), typeof (HQLQuery));
            ObjectContainer.AddSingletonDefinition(typeof (IPersistenceBroker), typeof (PersistenceBroker));
            ObjectContainer.AddPrototypeDefinition(typeof (ISQLQuery), typeof (SQLQuery));
            ObjectContainer.AddSingletonDefinition(typeof (IUnitOfWorkFactory), typeof (UnitOfWorkFactory));

            _IsConfigured = true;
        }
    }
}