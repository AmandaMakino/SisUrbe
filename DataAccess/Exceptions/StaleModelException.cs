using System;

namespace DataAccess.Exceptions
{
    public class StaleModelException : RepositoryException
    {
        public StaleModelException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}