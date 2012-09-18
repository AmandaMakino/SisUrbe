using System.Data;

namespace DataAccess
{
    public interface IADOQuery
    {
        IDbCommand Command { get; set; }

        IADOQuery SetParameter<T>(string parameterName, T parameterValue);

        int ExecuteNonQuery();

        object ExecuteScalar();

        IDataReader ExecuteReader();
        IADOQuery SetCommandType(CommandType commandType);
    }
}