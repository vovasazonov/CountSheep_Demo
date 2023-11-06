namespace Project.CoreDomain.Services.FileLoader
{
    public interface IFileLoader
    {
        T Load<T>(string path);
        void Save<T>(T obj, string path);
    }
}