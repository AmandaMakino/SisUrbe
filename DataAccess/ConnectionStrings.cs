using System.Configuration;

namespace DataAccess
{
    public static class ConnectionStrings
    {
        private static readonly ConnectionStringSettings _SysCEFDBSettings =
            ConfigurationManager.ConnectionStrings["SisUrbeConnectionString"];

        public static string SysCEFDBSettings
        {
            get { return _SysCEFDBSettings.ConnectionString; }
        }
    }
}