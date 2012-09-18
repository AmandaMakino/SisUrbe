using System.Linq;
using System.Reflection;
using Core;

namespace SysCEF.DAO
{
    public static class DependencyConfigurator
    {
        private static bool _IsSqlConfigured;

        /// <summary>
        /// Loop through all of the classes in the Implementation namespace and
        /// map those implementations to their associated interfaces and adds
        /// those mappings to the IoC container
        /// </summary>
        public static void ConfigureSqlRepositories()
        {
            if (_IsSqlConfigured)
                return;

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(p => p.Namespace == "SysCEF.DAO.Implementacao"))
                foreach (var iface in type.GetInterfaces().Where(p => p.Namespace == "SysCEF.DAO.Interface"))
                    ObjectContainer.AddSingletonDefinition(iface, type);

            _IsSqlConfigured = true;
        }
    }
}
