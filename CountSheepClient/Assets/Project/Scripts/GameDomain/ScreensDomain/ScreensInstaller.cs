using Project.GameDomain.ScreensDomain.BattleDomain;
using Project.GameDomain.ScreensDomain.LoadingDomain;
using Project.GameDomain.ScreensDomain.MainDomain;
using Project.GameDomain.ScreensDomain.MergeDomain;
using Project.GameDomain.ScreensDomain.SplashDomain;
using Zenject;

namespace Project.GameDomain.ScreensDomain
{
    public class ScreensInstaller : Installer<ScreensInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindFactory<SplashScreen, SplashScreen.ZenjectFactory>().FromSubContainerResolve().ByNewGameObjectInstaller<SplashScreenInstaller>().WithGameObjectName(SplashScreen.Id + SplashScreen.ContextPostfix);
            Container.BindInterfacesTo<SplashScreen.Factory>().AsSingle();
            Container.BindInstance(SplashScreen.Id).WhenInjectedInto<SplashScreen.Factory>();
            
            Container.BindFactory<LoadingScreen, LoadingScreen.ZenjectFactory>().FromSubContainerResolve().ByNewGameObjectInstaller<LoadingScreenInstaller>().WithGameObjectName(LoadingScreen.Id + LoadingScreen.ContextPostfix);
            Container.BindInterfacesTo<LoadingScreen.Factory>().AsSingle();
            Container.BindInstance(LoadingScreen.Id).WhenInjectedInto<LoadingScreen.Factory>();

            Container.BindFactory<MainScreen, MainScreen.ZenjectFactory>().FromSubContainerResolve().ByNewGameObjectInstaller<MainScreenInstaller>().WithGameObjectName(MainScreen.Id + MainScreen.ContextPostfix);
            Container.BindInterfacesTo<MainScreen.Factory>().AsSingle();
            Container.BindInstance(MainScreen.Id).WhenInjectedInto<MainScreen.Factory>();
            
            Container.BindFactory<BattleScreen, BattleScreen.ZenjectFactory>().FromSubContainerResolve().ByNewGameObjectInstaller<BattleScreenInstaller>().WithGameObjectName(BattleScreen.Id + BattleScreen.ContextPostfix);
            Container.BindInterfacesTo<BattleScreen.Factory>().AsSingle();
            Container.BindInstance(BattleScreen.Id).WhenInjectedInto<BattleScreen.Factory>();
            
            Container.BindFactory<MergeScreen, MergeScreen.ZenjectFactory>().FromSubContainerResolve().ByNewGameObjectInstaller<MergeScreenInstaller>().WithGameObjectName(MergeScreen.Id + MergeScreen.ContextPostfix);
            Container.BindInterfacesTo<MergeScreen.Factory>().AsSingle();
            Container.BindInstance(MergeScreen.Id).WhenInjectedInto<MergeScreen.Factory>();
        }
    }}