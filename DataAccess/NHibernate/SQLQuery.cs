using System;
using NHibernate;
using NHibernate.Type;

namespace DataAccess.NHibernate
{
    public class SQLQuery : HQLQuery, ISQLQuery
    {
        private readonly global::NHibernate.ISQLQuery nhQuery;

        public SQLQuery(global::NHibernate.ISQLQuery nhQuery) : base(nhQuery)
        {
            this.nhQuery = nhQuery;
        }

        #region ISQLQuery Members

        public ISQLQuery AddExpectedModel(Type modelType)
        {
            nhQuery.AddEntity(modelType);
            return this;
        }

        public ISQLQuery AddExpectedModel(string modelAlias, Type modelType)
        {
            nhQuery.AddEntity(modelAlias, modelType);
            return this;
        }

        public ISQLQuery AddExpectedScalar(string columnAlias, Type scalarType)
        {
            IType nhType;

            if (scalarType == typeof (int))
                nhType = NHibernateUtil.Int32;
            else if (scalarType == typeof (string))
                nhType = NHibernateUtil.String;
            else if (scalarType == typeof (DateTime))
                nhType = NHibernateUtil.DateTime;
            else
                throw new NotImplementedException("The following type is not supported: " + scalarType.FullName);

            nhQuery.AddScalar(columnAlias, nhType);
            return this;
        }

        public ISQLQuery AddJoinFetch(string modelAlias, string joinedProperty)
        {
            nhQuery.AddJoin(modelAlias, joinedProperty);
            return this;
        }

        #endregion
    }
}