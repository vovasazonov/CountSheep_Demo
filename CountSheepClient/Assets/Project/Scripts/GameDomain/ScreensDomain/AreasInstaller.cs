using Project.GameDomain.ScreensDomain.MainDomain.Areas.Achievements;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Currency.Model;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Config;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Model;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Market.Model;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Rate;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Rule.Presenter;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Score.Model;
using Project.Scripts.GameDomain.ScreensDomain.MainDomain.Areas.GameInput.Model;
using Zenject;

namespace Project.GameDomain.ScreensDomain
{
    public class AreasInstaller : Installer<AreasInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<RuleModel>().AsSingle();
            Container.BindInterfacesTo<ScoreModel>().AsSingle();
            Container.BindInterfacesTo<InputModel>().AsSingle();
            Container.BindInterfacesTo<FarmConfigKeeper>().AsSingle();
            Container.BindInterfacesTo<FarmModel>().AsSingle();
            Container.BindInterfacesTo<CurrencyModel>().AsSingle();
            Container.BindInterfacesTo<SheepNewCollectedAchievementProcess>().AsSingle();
            Container.BindInterfacesTo<ScoreAchievementProcess>().AsSingle();
            Container.BindInterfacesTo<RateModel>().AsSingle();
            Container.BindInterfacesTo<MarketModel>().AsSingle();
        }
    }
}