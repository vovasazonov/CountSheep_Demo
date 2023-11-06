using System.IO;
using Project.CoreDomain.Services.Serialization;

namespace Project.CoreDomain.Services.FileLoader.Loaders
{
    internal class JsonFileLoader : IFileLoader
    {
        private readonly ISerializerService _serializerService;

        public JsonFileLoader(ISerializerService serializerService)
        {
            _serializerService = serializerService;
        }

        public T Load<T>(string path)
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                return _serializerService.DeserializeJson<T>(json);
            }

            return default;
        }

        public void Save<T>(T obj, string path)
        {
            string json = _serializerService.SerializeToJson(obj);
            File.WriteAllText(path, json);
        }
    }
}