using System.Configuration;

namespace DataAccess
{
    public static class ConnectionStrings
    {
        private static readonly ConnectionStringSettings _SysCEFDBSettings =
            ConfigurationManager.ConnectionStrings["SysCEFConnectionString"];

        public static string SysCEFDBSettings
        {
            get { return _SysCEFDBSettings.ConnectionString; }
        }
    }
}