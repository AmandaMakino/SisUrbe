namespace DataAccess
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork StartSysCEFUnitOfWork(params UnitOfWorkOption[] options);
    }
}
