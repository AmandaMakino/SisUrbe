using System;

namespace DataAccess
{
    public interface ISQLQuery : IHQLQuery
    {
        ISQLQuery AddExpectedModel(Type modelType);
        ISQLQuery AddExpectedModel(string modelAlias, Type modelType);
        ISQLQuery AddExpectedScalar(string columnAlias, Type scalarType);
        ISQLQuery AddJoinFetch(string modelAlias, string joinedProperty);
    }
}