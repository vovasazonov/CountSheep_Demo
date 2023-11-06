using Project.CoreDomain.Services.FileLoader.Loaders;
using Project.CoreDomain.Services.Serialization;

namespace Project.CoreDomain.Services.FileLoader
{
    public class FileLoaderService : IFileLoaderService
    {
        public IFileLoader Binary { get; }
        public IFileLoader Json { get; }
        
        public FileLoaderService(ISerializerService serializerService)
        {
            Binary = new BinaryFileLoader();
            Json = new JsonFileLoader(serializerService);
        }
    }
}