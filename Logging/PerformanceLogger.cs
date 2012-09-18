using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Common.Logging
{
    public class PerformanceLogger : IPerformanceLogger
    {
        private string _ConnectionString;
        public string ConnectionString
        {
            get
            {
                return _ConnectionString ?? (_ConnectionString = ConfigurationManager.ConnectionStrings["logging"].ConnectionString);
            }
            set
            {
                _ConnectionString = value;
            }
        }

        #region Implementation of IPerformanceLogger

        public void LogControllerBenchmark(IControllerInfo controllerInfo)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = @"
                        INSERT INTO perf.controllerlog (keyid, controller, action, executiontime, rendertime, widgetinstanceid)
                            VALUES (@keyid, @controller, @action, @executiontime, @rendertime, @widgetinstanceid)";
                    sqlCommand.Parameters.AddWithValue("@keyid", controllerInfo.Keyid);
                    sqlCommand.Parameters.AddWithValue("@controller", controllerInfo.ControllerName);
                    sqlCommand.Parameters.AddWithValue("@action", controllerInfo.ActionName);
                    sqlCommand.Parameters.AddWithValue("@executiontime", controllerInfo.ExecutionTime);
                    sqlCommand.Parameters.AddWithValue("@rendertime", controllerInfo.RenderTime);
                    sqlCommand.Parameters.AddWithValue("@widgetinstanceid", controllerInfo.WidgetInstanceId ?? (object)DBNull.Value);
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public void LogMethodBenchmark(IMethodInfo methodInfo)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = @"
                        INSERT INTO perf.methodlog (keyid, methodname, executiontime, number_of_sites, sites, cpu, ram_mb_available)
                            VALUES (@keyid, @methodname, @executiontime, @number_of_sites, @sites, @cpu, @ram_mb_available)";
                    sqlCommand.Parameters.AddWithValue("@keyid", methodInfo.Keyid);
                    sqlCommand.Parameters.AddWithValue("@methodname", methodInfo.MethodName);
                    sqlCommand.Parameters.AddWithValue("@executiontime", methodInfo.ExecutionTime);
                    sqlCommand.Parameters.AddWithValue("@number_of_sites", methodInfo.Sites != null && methodInfo.Sites.Any() ? methodInfo.Sites.Count() : (object)DBNull.Value);
                    sqlCommand.Parameters.AddWithValue("@sites", methodInfo.Sites != null && methodInfo.Sites.Any() ? String.Join(", ", methodInfo.Sites) : (object)DBNull.Value);
                    sqlCommand.Parameters.AddWithValue("@cpu", methodInfo.CPU);
                    sqlCommand.Parameters.AddWithValue("@ram_mb_available", methodInfo.RAM);

                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        #endregion
    }
}
