using Project.CoreDomain;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Main.Presenter;
using Zenject;

namespace Project.GameDomain.ScreensDomain.MainDomain
{
    public class MainScreenInstaller : Installer<MainScreenInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<MainScreen>().AsSingle();

            BindConfigs();
            BindModels();
            BindPresenters();
        }

        private void BindConfigs()
        {
        }

        private void BindModels()
        {
        }

        private void BindPresenters()
        {
            Container.Bind<IPresenter>().To<MainPresenter>().AsSingle();
        }
    }
}