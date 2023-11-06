using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Project.CoreDomain.Services.FileLoader.Loaders
{
    internal class BinaryFileLoader : IFileLoader
    {
        public T Load<T>(string path)
        {
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using FileStream stream = new FileStream(path, FileMode.Open);
                
                return (T)formatter.Deserialize(stream);
            }

            return default;
        }

        public void Save<T>(T obj, string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using FileStream stream = new FileStream(path, FileMode.Create);
            
            formatter.Serialize(stream, obj);
        }
    }
}