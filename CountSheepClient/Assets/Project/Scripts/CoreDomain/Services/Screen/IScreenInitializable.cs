namespace Project.CoreDomain.Services.Screen
{
    public interface IScreenInitializable
    {
        void SetLoadingScreen(string id);
        void SetSplashScreen(string id);
    }
}