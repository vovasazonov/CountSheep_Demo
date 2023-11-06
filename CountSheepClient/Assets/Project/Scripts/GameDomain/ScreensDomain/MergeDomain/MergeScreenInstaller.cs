using Project.CoreDomain;
using Project.GameDomain.ScreensDomain.MergeDomain.Areas.Farm.Presenter;
using Project.GameDomain.ScreensDomain.MergeDomain.Areas.Merge.Presenter;
using Zenject;

namespace Project.GameDomain.ScreensDomain.MergeDomain
{
    public class MergeScreenInstaller : Installer<MergeScreenInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<MergeScreen>().AsSingle();

            BindPresenters();
        }

        private void BindPresenters()
        {
            Container.Bind<IPresenter>().To<MergePresenter>().AsSingle();
            Container.Bind<IPresenter>().To<FarmPresenter>().AsSingle();
        }
    }
}