using Zenject;

namespace Project.GameDomain.ScreensDomain.SplashDomain
{
    public class SplashScreenInstaller : Installer<SplashScreenInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<SplashScreen>().AsSingle();
        }
    }
}