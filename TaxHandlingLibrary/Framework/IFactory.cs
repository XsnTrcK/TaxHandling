namespace TaxHandlingLibrary.Framework
{
    public interface IFactory<T>
    {
        T Build(string key);
    }
}
