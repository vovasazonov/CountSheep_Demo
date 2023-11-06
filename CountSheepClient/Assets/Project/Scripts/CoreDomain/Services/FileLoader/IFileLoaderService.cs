namespace Project.CoreDomain.Services.FileLoader
{
    public interface IFileLoaderService
    {
        IFileLoader Binary { get; }
        IFileLoader Json { get; }
    }
}