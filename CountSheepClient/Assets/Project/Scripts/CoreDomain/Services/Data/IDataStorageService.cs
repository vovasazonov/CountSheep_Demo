namespace Project.CoreDomain.Services.Data
{
    public interface IDataStorageService
    {
        void Save();
        void Load();
        bool Contains(string key);
        T Get<T>(string key) where T : class;
        T Create<T>(string key) where T : class, new();
        void Reset();
    }
}