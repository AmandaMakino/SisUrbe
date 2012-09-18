namespace Core
{
    public interface IObjectContainer
    {
        T Get<T>(params object[] constructorArgs) where T : class;
    }
}