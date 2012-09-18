using System;
using System.Collections.Generic;
using NHibernate;
using DataAccess.Exceptions;

namespace DataAccess.NHibernate
{
    public class HQLQuery : IHQLQuery
    {
        private readonly IQuery nhQuery;

        public HQLQuery(IQuery nhQuery)
        {
            this.nhQuery = nhQuery;
        }

        #region IHQLQuery Members

        public IHQLQuery SetParameter<T>(string parameterName, T parameterValue)
        {
            nhQuery.SetParameter(parameterName, parameterValue);
            return this;
        }

        public IHQLQuery SetMaxResults(int maxResultsCount)
        {
            nhQuery.SetMaxResults(maxResultsCount);
            return this;
        }

        public IList<T> List<T>()
        {
            try
            {
                return nhQuery.List<T>();
            }
            catch (Exception e)
            {
                throw new RepositoryException("HQL List failed.", e);
            }
        }

        public T UniqueResult<T>()
        {
            try
            {
                return nhQuery.UniqueResult<T>();
            }
            catch (Exception e)
            {
                throw new RepositoryException("HQL Unique result failed.", e);
            }
        }

        public IHQLQuery SetNullAsEmptyStringParameter(string parameterName, string parameterValue)
        {
            if (parameterValue == null)
            {
                nhQuery.SetParameter(parameterName, "");
            }
            else
            {
                nhQuery.SetParameter(parameterName, parameterValue);
            }
            return this;
        }

        #endregion
    }
}