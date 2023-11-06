using Zenject;

namespace Project.GameDomain.ScreensDomain.LoadingDomain
{
    public class LoadingScreenInstaller : Installer<LoadingScreenInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<LoadingScreen>().AsSingle();
        }
    }
}