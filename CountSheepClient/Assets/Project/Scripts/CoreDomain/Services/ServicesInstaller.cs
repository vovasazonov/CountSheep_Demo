using Project.CoreDomain.Services.Achievement;
using Project.CoreDomain.Services.Analytics;
using Project.CoreDomain.Services.Audio;
using Project.CoreDomain.Services.Camera;
using Project.CoreDomain.Services.Content;
using Project.CoreDomain.Services.Data;
using Project.CoreDomain.Services.Di;
using Project.CoreDomain.Services.Engine;
using Project.CoreDomain.Services.FileLoader;
using Project.CoreDomain.Services.GoogplayGames;
using Project.CoreDomain.Services.InAppUpdate;
using Project.CoreDomain.Services.Leaderboard;
using Project.CoreDomain.Services.Logger;
using Project.CoreDomain.Services.Purchase;
using Project.CoreDomain.Services.Rate;
using Project.CoreDomain.Services.Screen;
using Project.CoreDomain.Services.Serialization;
using Project.CoreDomain.Services.Time;
using Project.CoreDomain.Services.View;
using Project.Scripts.CoreDomain.Services.AdsImplemantations.UnityAds;
using Zenject;

namespace Project.CoreDomain.Services
{
    public class ServicesInstaller : Installer<ServicesInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<LoggerService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<DataStorageService>().AsSingle();
            Container.BindInterfacesTo<FileLoaderService>().AsSingle();
            Container.BindInterfacesTo<AudioService>().AsSingle();
            Container.BindInterfacesTo<SerializerService>().AsSingle();
            Container.BindInterfacesTo<ContentService>().AsSingle();
            Container.BindInterfacesTo<ScreensService>().AsSingle();
            Container.BindInterfacesTo<AnalyticService>().AsSingle();
            Container.BindInterfacesTo<EngineService>().AsSingle();
            Container.BindInterfacesTo<CameraService>().AsSingle();
            Container.BindInterfacesTo<DiService>().AsSingle();
            Container.BindInterfacesTo<ViewService>().AsSingle();
            Container.BindInterfacesTo<TimeService>().AsSingle();
            Container.BindInterfacesTo<PurchaseService>().AsSingle();
            Container.BindInterfacesTo<RateService>().AsSingle();
            Container.BindInterfacesTo<UnityAdsService>().AsSingle();
#if UNITY_ANDROID
            Container.BindInterfacesTo<GoogleplayGamesService>().AsSingle();
            Container.BindInterfacesTo<GooglePlayLeaderboardService>().AsSingle();
            Container.BindInterfacesTo<GooglePlayAchievementService>().AsSingle();
            Container.BindInterfacesTo<GooglePlayInAppUpdateService>().AsSingle();
            #else
            Container.BindInterfacesTo<FakeLeaderboardService>().AsSingle();
            Container.BindInterfacesTo<FakeAchievementService>().AsSingle();
#endif
        }
    }
}