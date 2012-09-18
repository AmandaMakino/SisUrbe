using System;
using System.Data;
using DataAccess.Exceptions;

namespace DataAccess.NHibernate
{
    public class ADOQuery : IADOQuery
    {
        public ADOQuery(IDbCommand cmd, string queryString)
        {
            Command = cmd;
            Command.CommandText = queryString;

            Command.Parameters.Clear();
        }

        #region IADOQuery Members

        public IDbCommand Command { get; set; }

        public IADOQuery SetParameter<T>(string parameterName, T parameterValue)
        {
            AddParameter(Command, parameterName, parameterValue);
            return this;
        }

        public int ExecuteNonQuery()
        {
            try
            {
                return Command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new RepositoryException("ADO Execute NON query failed.", e);
            }
        }

        public object ExecuteScalar()
        {
            try
            {
                return Command.ExecuteScalar();
            }
            catch (Exception e)
            {
                throw new RepositoryException("ADO Execute scalar failed.", e);
            }
        }

        public IDataReader ExecuteReader()
        {
            try
            {
                return Command.ExecuteReader();
            }
            catch (Exception e)
            {
                throw new RepositoryException("ADO Execute reader failed.", e);
            }
        }

        public IADOQuery SetCommandType(CommandType commandType)
        {
            Command.CommandType = commandType;
            return this;
        }

        #endregion

        public static void AddParameter<T>(IDbCommand cmd, string parameterName, T parameterValue)
        {
            var param = CreateParameter(cmd, parameterValue);

            param.ParameterName = parameterName;

            cmd.Parameters.Add(param);
        }

        public static IDbDataParameter CreateParameter<T>(IDbCommand cmd, T parameterValue)
        {
            var param = cmd.CreateParameter();

            if (typeof (T) != typeof (string) &&
                typeof (T) != typeof (char) &&
                typeof (T) != typeof (char?) &&
                typeof (T) != typeof (int) &&
                typeof (T) != typeof (int?) &&
                typeof (T) != typeof (long) &&
                typeof (T) != typeof (long?) &&
                typeof (T) != typeof (short) &&
                typeof (T) != typeof (short?) &&
                typeof (T) != typeof (DateTime) &&
                typeof (T) != typeof (DateTime?))
            {
                throw new Exception("Disallowed type of '" + typeof (T) + "' Create ADO parameter.");
            }

            if (parameterValue == null)
            {
                if (typeof (T) == typeof (int?))
                {
                    param.DbType = DbType.Int32;
                }
                else if (typeof (T) == typeof (short?))
                {
                    param.DbType = DbType.Int16;
                }
                else
                {
                    param.DbType = DbType.String;
                }

                param.Value = null;
            }
            else
            {
                param.Value = parameterValue;
            }

            return param;
        }

        public static void AddParameter<T>(IDbCommand cmd, T parameterValue)
        {
            cmd.Parameters.Add(CreateParameter(cmd, parameterValue));
        }
    }
}