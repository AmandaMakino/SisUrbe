using System.Collections.Generic;

namespace DataAccess
{
    public interface IHQLQuery
    {
        IHQLQuery SetMaxResults(int maxResultsCount);
        IHQLQuery SetParameter<T>(string parameterName, T parameterValue);
        IHQLQuery SetNullAsEmptyStringParameter(string parameterName, string parameterValue);

        IList<T> List<T>();
        T UniqueResult<T>();
    }
}