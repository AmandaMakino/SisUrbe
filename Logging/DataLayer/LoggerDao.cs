using System;
using System.Data;
using Common.Logging.Context;
using System.Data.SqlClient;

namespace Common.Logging.DataLayer
{
    internal class LoggerDao
    {
        private const String REQUEST_ID_CONTEXT_KEY = "PG_COMMON_LOGGING_LOGGER_REQUEST_ID";

        public string ConnectionString {get; set;}
        public Application App { get; set; }
        public int RequestId { get; set; }

        public int LogErrorInternal(string message, ErrorType type, Exception e, HttpContextLoggerInfo wrapper)
        {
            var connection = new SqlConnection(ConnectionString);
            var cmd = new SqlCommand("dbo.LogError", connection) {
                CommandType = CommandType.StoredProcedure
            };

            var requestIdParameter = new SqlParameter("RequestId", SqlDbType.BigInt) {
                Direction = ParameterDirection.InputOutput
            };
            cmd.Parameters.Add(requestIdParameter);

            if (wrapper == null)
            {
                requestIdParameter.Value = null;
            }
            else
            {
                SetWebEnabledParameters(cmd, wrapper);
                requestIdParameter.Value = wrapper.Items[REQUEST_ID_CONTEXT_KEY];
            }

            SetStandAloneErrorParameters(message, type, e, cmd);

            ExecuteCommand(connection, cmd);
            RequestId = Convert.ToInt32(cmd.Parameters["RequestId"].Value);

            if (wrapper != null && wrapper.Items[REQUEST_ID_CONTEXT_KEY] == null)
            {
                wrapper.Items[REQUEST_ID_CONTEXT_KEY] = RequestId;
            }

            return Convert.ToInt32(cmd.Parameters["ErrorId"].Value);
        }

        public void LogSecurityEventInternal(EventType type, Result result, string description, HttpContextLoggerInfo wrapper)
        {
            var connection = new SqlConnection(ConnectionString);
            var cmd = new SqlCommand("dbo.LogSecurityEvent", connection) {
                CommandType = CommandType.StoredProcedure
            };

            var requestIdParameter = new SqlParameter("RequestId", SqlDbType.BigInt) {
                Direction = ParameterDirection.InputOutput
            };
            cmd.Parameters.Add(requestIdParameter);

            if (wrapper == null)
            {
                requestIdParameter.Value = null;
            }
            else
            {
                SetWebEnabledParameters(cmd, wrapper);
                requestIdParameter.Value = wrapper.Items[REQUEST_ID_CONTEXT_KEY];
            }

            SetStandAloneSecurityEventParameters(description, type, result, cmd);

            ExecuteCommand(connection, cmd);
            RequestId = Convert.ToInt32(cmd.Parameters["RequestId"].Value);

            if (wrapper != null && wrapper.Items[REQUEST_ID_CONTEXT_KEY] == null)
            {
                wrapper.Items[REQUEST_ID_CONTEXT_KEY] = RequestId;
            }
        }
        private static void ExecuteCommand(SqlConnection connection, SqlCommand cmd)
        {
            connection.Open();

            try
            {
                cmd.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }
        }

        private static void SetWebEnabledParameters(SqlCommand cmd, HttpContextLoggerInfo wrapper)
        {
            cmd.Parameters.Add(new SqlParameter("SessionKey", SqlDbType.NVarChar)).Value
                = wrapper.SessionID;

            cmd.Parameters.Add(new SqlParameter("Ip", SqlDbType.NChar)).Value
                = wrapper.UserHostAddress;

            cmd.Parameters.Add(new SqlParameter("UserAgent", SqlDbType.NVarChar)).Value
                = wrapper.UserAgent;

            cmd.Parameters.Add(new SqlParameter("UserId", SqlDbType.NVarChar)).Value
                = wrapper.UserName;

            cmd.Parameters.Add(new SqlParameter("Uri", SqlDbType.NVarChar)).Value
                = wrapper.AbsoluteUri;
        }

        private void SetStandAloneErrorParameters(string message, ErrorType type, Exception e, SqlCommand cmd)
        {
            cmd.Parameters.Add(new SqlParameter("AppId", SqlDbType.NChar)).Value = App.Key;
            cmd.Parameters.Add(new SqlParameter("ErrorTypeId", SqlDbType.NChar)).Value = type.Key;
            cmd.Parameters.Add(new SqlParameter("ErrorText", SqlDbType.NVarChar)).Value = message;

            var stackTraceText = BuildFullStackTraceText(e);

            if (e != null)
            {
                cmd.Parameters.Add(new SqlParameter("StackTraceText", SqlDbType.NVarChar)).Value = BuildFullStackTraceText(e);
            }
            var errorIdSqlParameter = new SqlParameter("ErrorId", SqlDbType.BigInt)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(errorIdSqlParameter);
        }

        private string BuildFullStackTraceText(Exception e)
        {
            if (e == null)
                return string.Empty;

            var thisException = e.Message + "\n" + e.StackTrace;
            var innerException = BuildFullStackTraceText(e.InnerException);
            if (!string.IsNullOrWhiteSpace(innerException))
                innerException = "\n\nINNER EXCEPTION:\n" + innerException;
            return thisException + innerException;
        }

        private void SetStandAloneSecurityEventParameters(string description, EventType type, Result result, SqlCommand cmd)
        {
            cmd.Parameters.Add(new SqlParameter("AppId", SqlDbType.NChar)).Value = App.Key;
            cmd.Parameters.Add(new SqlParameter("EventTypeId", SqlDbType.NChar)).Value = type.Key;
            cmd.Parameters.Add(new SqlParameter("Description", SqlDbType.NVarChar)).Value = description;
            cmd.Parameters.Add(new SqlParameter("ResultId", SqlDbType.NVarChar)).Value = result.Key;
        }
    }
}
