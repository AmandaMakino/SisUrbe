using System;
using System.Data.SqlClient;
using Common.Logging.Context;

namespace Common.Logging.DataLayer
{
    internal class LoggerDao2
    {
        public string ConnectionString { get; set; }
        public Application App { get; set; }

        public Int32 OpenSession(string sessionKey, string ip, string userId, string userAgent)
        {
            if (string.IsNullOrEmpty(sessionKey))
                throw new ArgumentException();

            var connection = new SqlConnection(ConnectionString);
            var cmd = new SqlCommand("dbo.OpenSession", connection)
            {CommandType = System.Data.CommandType.StoredProcedure};

            cmd.Parameters.Add(new SqlParameter("SessionKey", System.Data.SqlDbType.NVarChar)).Value = sessionKey;
            cmd.Parameters.Add(new SqlParameter("Ip", System.Data.SqlDbType.NChar)).Value = ip;
            cmd.Parameters.Add(new SqlParameter("UserId", System.Data.SqlDbType.NVarChar)).Value = userId;
            cmd.Parameters.Add(new SqlParameter("AppId", System.Data.SqlDbType.NChar)).Value = App.Key;
            cmd.Parameters.Add(new SqlParameter("UserAgent", System.Data.SqlDbType.NVarChar)).Value = userAgent;

            ExecuteCommand(connection, cmd);

            return Convert.ToInt32(cmd.Parameters["SessionId"].Value);
        }

        public Int32 OpenRequest(string sessionKey, string uri)
        {
            if (string.IsNullOrEmpty(sessionKey))
                throw new ArgumentException();

            var connection = new SqlConnection(ConnectionString);
            var cmd = new SqlCommand("dbo.OpenRequest", connection)
            {CommandType = System.Data.CommandType.StoredProcedure};

            cmd.Parameters.Add(new SqlParameter("SessionKey", System.Data.SqlDbType.NVarChar)).Value = sessionKey;
            cmd.Parameters.Add(new SqlParameter("URI", System.Data.SqlDbType.NVarChar)).Value = uri;

            ExecuteCommand(connection, cmd);

            return Convert.ToInt32(cmd.Parameters["RequestId"].Value);
        }

        public Int32 LogError(Int32? requestId, string message, ErrorType type, Exception e)
        {
            var connection = new SqlConnection(ConnectionString);
            var cmd = new SqlCommand("dbo.LogError2", connection)
            {CommandType = System.Data.CommandType.StoredProcedure};

            if (requestId != null)
                cmd.Parameters.Add(new SqlParameter("RequestId", System.Data.SqlDbType.BigInt)).Value = requestId;
            //cmd.Parameters.Add(new SqlParameter("AppId", System.Data.SqlDbType.NChar)).Value = this.App.Key;
            cmd.Parameters.Add(new SqlParameter("ErrorTypeId", System.Data.SqlDbType.NChar)).Value = type.Key;
            cmd.Parameters.Add(new SqlParameter("ErrorText", System.Data.SqlDbType.NVarChar)).Value = message;

            var erroridparam = new SqlParameter("ErrorId", System.Data.SqlDbType.BigInt)
            {Direction = System.Data.ParameterDirection.Output};
            cmd.Parameters.Add(erroridparam);

            if (e != null)
            {
                cmd.Parameters.Add(new SqlParameter("StackTraceText", System.Data.SqlDbType.NVarChar)).Value = e.Message + "\n" + e.StackTrace;
            }

            ExecuteCommand(connection, cmd);

            var errorId = Convert.ToInt32(erroridparam.Value);

            while (e.InnerException != null)
            {
                e = e.InnerException;
                const string insertExtraStackTraceSql = "insert into dbo.stacktrace (errorid, stacktracetext) values (@ErrorId, @StackTraceText)";
                var insertExtraStackTrace = new SqlCommand(insertExtraStackTraceSql, connection)
                {CommandType = System.Data.CommandType.Text};
                insertExtraStackTrace.Parameters.Add(new SqlParameter("ErrorId", System.Data.SqlDbType.BigInt)).Value = errorId;
                insertExtraStackTrace.Parameters.Add(new SqlParameter("StackTraceText", System.Data.SqlDbType.NVarChar)).Value = e.Message + "\n" + e.StackTrace;
                ExecuteCommand(connection, insertExtraStackTrace);
            }

            return errorId;
        }

        public Int32 LogSecurityEvent(Int32? requestId, EventType type, Result result, string description)
        {
            var connection = new SqlConnection(ConnectionString);
            var cmd = new SqlCommand("dbo.LogSecurityEvent2", connection)
            {CommandType = System.Data.CommandType.StoredProcedure};

            if (requestId != null)
                cmd.Parameters.Add(new SqlParameter("RequestId", System.Data.SqlDbType.BigInt)).Value = requestId;
            cmd.Parameters.Add(new SqlParameter("EventTypeId", System.Data.SqlDbType.NChar)).Value = type.Key;
            cmd.Parameters.Add(new SqlParameter("Description", System.Data.SqlDbType.NVarChar)).Value = description;
            cmd.Parameters.Add(new SqlParameter("ResultId", System.Data.SqlDbType.NVarChar)).Value = result.Key;

            ExecuteCommand(connection, cmd);

            return Convert.ToInt32(cmd.Parameters["SecurityEventId"].Value);
        }

        public Int32 LogActivity(Int32? requestId, ActivityType type, string description)
        {
            var connection = new SqlConnection(ConnectionString);
            var cmd = new SqlCommand("dbo.LogActivity2", connection)
            {CommandType = System.Data.CommandType.StoredProcedure};

            if (requestId != null)
                cmd.Parameters.Add(new SqlParameter("RequestId", System.Data.SqlDbType.BigInt)).Value = requestId;
            cmd.Parameters.Add(new SqlParameter("ActivityTypeId", System.Data.SqlDbType.NChar)).Value = type.Key;
            cmd.Parameters.Add(new SqlParameter("Description", System.Data.SqlDbType.NVarChar)).Value = description;

            ExecuteCommand(connection, cmd);

            return Convert.ToInt32(cmd.Parameters["ActivityId"].Value);
        }

        public void CloseRequest(Int32? requestId, RequestStatus status)
        {
            if (requestId == null)
                throw new ArgumentException();

            var connection = new SqlConnection(ConnectionString);
            var cmd = new SqlCommand("dbo.CloseRequest", connection)
            {CommandType = System.Data.CommandType.StoredProcedure};

            cmd.Parameters.Add(new SqlParameter("RequestId", System.Data.SqlDbType.NVarChar)).Value = requestId;
            cmd.Parameters.Add(new SqlParameter("Status", System.Data.SqlDbType.NChar)).Value = status.Key;

            ExecuteCommand(connection, cmd);
        }

        public void CloseSession(string sessionKey, SessionStatus status)
        {
            if (string.IsNullOrEmpty(sessionKey))
                throw new ArgumentException();

            var connection = new SqlConnection(ConnectionString);
            var cmd = new SqlCommand("dbo.CloseSession", connection)
            {CommandType = System.Data.CommandType.StoredProcedure};

            cmd.Parameters.Add(new SqlParameter("SessionKey", System.Data.SqlDbType.NVarChar)).Value = sessionKey;
            cmd.Parameters.Add(new SqlParameter("Status", System.Data.SqlDbType.NChar)).Value = status.Key;

            ExecuteCommand(connection, cmd);
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
    }
}
