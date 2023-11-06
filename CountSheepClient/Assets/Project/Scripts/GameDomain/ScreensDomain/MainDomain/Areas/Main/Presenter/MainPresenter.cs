using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.CoreDomain;
using Project.CoreDomain.Services.Audio;
using Project.CoreDomain.Services.Content;
using Project.CoreDomain.Services.Data;
using Project.CoreDomain.Services.Leaderboard;
using Project.CoreDomain.Services.Screen;
using Project.CoreDomain.Services.View;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Collection.Presenter;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Config;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Model;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Main.View;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Market.Model;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Market.Presenter;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.MergeLocation.Presenter;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Pause.Presenter;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Rule.Presenter;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Score.Model;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Score.Presenter;
using Project.Scripts.CoreDomain.Services.Ads;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Main.Presenter
{
    public class MainPresenter : IPresenter
    {
        private readonly IViewService _viewService;
        private readonly IRuleModel _ruleModel;
        private readonly IScreensService _screensService;
        private readonly IScoreModel _scoreModel;
        private readonly IDataStorageService _dataStorageService;
        private readonly IAudioService _audioService;
        private readonly IAdsService _adsService;
        private readonly IFarmModel _farmModel;
        private readonly IFarmConfigKeeper _farmConfigKeeper;
        private readonly IContentService _contentService;
        private readonly ILeaderboardService _leaderboardService;
        private readonly IMarketModel _marketModel;
        private IDisposableView<IMainView> _view;
        private readonly List<ITaskAsyncDisposable> _disposables = new();

        public MainPresenter(
            IViewService viewService,
            IRuleModel ruleModel,
            IScreensService screensService,
            IScoreModel scoreModel,
            IDataStorageService dataStorageService,
            IAudioService audioService,
            IAdsService adsService,
            IFarmModel farmModel,
            IFarmConfigKeeper farmConfigKeeper,
            IContentService contentService, 
            ILeaderboardService leaderboardService,
            IMarketModel marketModel
        )
        {
            _viewService = viewService;
            _ruleModel = ruleModel;
            _screensService = screensService;
            _scoreModel = scoreModel;
            _dataStorageService = dataStorageService;
            _audioService = audioService;
            _adsService = adsService;
            _farmModel = farmModel;
            _farmConfigKeeper = farmConfigKeeper;
            _contentService = contentService;
            _leaderboardService = leaderboardService;
            _marketModel = marketModel;
        }

        public async UniTask InitializeAsync()
        {
            _view = await _viewService.CreateAsync<MainView>(MainScreenContentIds.MainCanvas);
            await InitializePresenter(new GameOverPresenter(_ruleModel, _view.Value.GameOver, _screensService, _dataStorageService, _scoreModel, _audioService, _adsService, _leaderboardService));
            await InitializePresenter(new TutorialHandPresenter(_view.Value, _ruleModel));
            await InitializePresenter(new ScoreInBattlePresenter(_scoreModel, _ruleModel, _view.Value.ScoreInBattle));
            await InitializePresenter(new PausePresenter(_view.Value.Pause, _ruleModel, _dataStorageService, _audioService, _screensService));
            await InitializePresenter(new MergeButtonPresenter(_view.Value, _ruleModel, _farmModel));
            await InitializePresenter(new CollectionPresenter(_view.Value.Collection, _farmModel, _farmConfigKeeper, _contentService));
            await InitializePresenter(new MarketPresenter(_view.Value.Market, _marketModel));
            
            _screensService.Switched += OnScreenSwitched;
        }

        private async UniTask InitializePresenter(IPresenter presenter)
        {
            await presenter.InitializeAsync();
            _disposables.Add(presenter);
        }

        public async UniTask DisposeAsync()
        {
            foreach (var disposable in _disposables)
            {
                await disposable.DisposeAsync();
            }

            _screensService.Switched -= OnScreenSwitched;
            _view.Dispose();
            _disposables.Clear();
        }

        private void OnScreenSwitched()
        {
            switch (_screensService.Current)
            {
                case "BattleScreen":
                    _view.Value.ShowBattle();
                    break;
                case "MergeScreen":
                    _view.Value.ShowMerge();
                    break;
                default:
                    break;
            }
        }
    }
}