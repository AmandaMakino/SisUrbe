namespace DataAccess
{
    public interface IWorkLifetimeManager
    {
        IUnitOfWork Value { get; }
    }
}