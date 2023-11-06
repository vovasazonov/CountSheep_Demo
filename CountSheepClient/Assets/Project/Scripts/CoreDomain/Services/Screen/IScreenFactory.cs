namespace Project.CoreDomain.Services.Screen
{
    public interface IScreenFactory
    {
        string Id { get; }
        
        IScreen Create();
    }
}