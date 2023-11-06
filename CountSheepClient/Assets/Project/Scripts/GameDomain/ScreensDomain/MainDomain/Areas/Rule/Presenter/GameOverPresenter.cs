using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.CoreDomain;
using Project.CoreDomain.Services.Analytics;
using Project.CoreDomain.Services.Audio;
using Project.CoreDomain.Services.Audio.Player;
using Project.CoreDomain.Services.Data;
using Project.CoreDomain.Services.Leaderboard;
using Project.CoreDomain.Services.Screen;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Rule.View;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Score.Model;
using Project.Scripts.CoreDomain.Services.Ads;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Rule.Presenter
{
    public class GameOverPresenter : IPresenter
    {
        private readonly IRuleModel _ruleModel;
        private readonly IGameOverView _gameOverView;
        private readonly IScreensService _screensService;
        private readonly IDataStorageService _dataStorageService;
        private readonly IScoreModel _scoreModel;
        private readonly IAudioService _audioService;
        private readonly IAdsService _adsService;
        private readonly ILeaderboardService _leaderboardService;
        private IAudioStopper _backgroundMusic;
        private int _showInternADAfterCountGame;
        private int _gamesOverCount;
        private PrimitiveData<bool> _isOldPlayerData;

        public GameOverPresenter(
            IRuleModel ruleModel,
            IGameOverView gameOverView,
            IScreensService screensService,
            IDataStorageService dataStorageService,
            IScoreModel scoreModel,
            IAudioService audioService,
            IAdsService adsService,
            ILeaderboardService leaderboardService
        )
        {
            _ruleModel = ruleModel;
            _gameOverView = gameOverView;
            _screensService = screensService;
            _dataStorageService = dataStorageService;
            _scoreModel = scoreModel;
            _audioService = audioService;
            _adsService = adsService;
            _leaderboardService = leaderboardService;
        }

        public UniTask InitializeAsync()
        {
            _gameOverView.HideGameOver();
            _ruleModel.Started += OnStarted;
            _ruleModel.Finished += OnFinished;
            _ruleModel.Resumed += OnResumed;
            _ruleModel.Prepared += OnPrepared;
            _gameOverView.PlayChoose += OnPlayChoose;
            _gameOverView.LeaderBoardChoose += OnLeaderBoardChoose;

            var isOldPlayerDataKey = "is_game_over_old_player";
            _isOldPlayerData = _dataStorageService.Contains(isOldPlayerDataKey)
                ? _dataStorageService.Get<PrimitiveData<bool>>(isOldPlayerDataKey)
                : _dataStorageService.Create<PrimitiveData<bool>>(isOldPlayerDataKey);
            _showInternADAfterCountGame = _isOldPlayerData.Value ? 2 : 3;
            
            return UniTask.CompletedTask;
        }

        public UniTask DisposeAsync()
        {
            _ruleModel.Started -= OnStarted;
            _ruleModel.Finished -= OnFinished;
            _ruleModel.Resumed -= OnResumed;
            _ruleModel.Prepared -= OnPrepared;
            _gameOverView.PlayChoose -= OnPlayChoose;
            _gameOverView.LeaderBoardChoose -= OnLeaderBoardChoose;
            return UniTask.CompletedTask;
        }

        private void OnStarted()
        {
            AnalyticService.Instance.Track("battle_start");
        }

        private void OnLeaderBoardChoose()
        {
            AnalyticService.Instance.Track("leaderboard_finishgame_clicked");
            _leaderboardService.Show();
        }

        private void OnPrepared()
        {
            // _backgroundMusic ??= _audioService.Music.PlayImmediately(MainScreenContentIds.BackgroundMusic);
            _gameOverView.HideGameOver();
        }

        private void OnPlayChoose()
        {
            AnalyticService.Instance.Track("play_button_clicked");
            _screensService.SwitchAsync("BattleScreen");
        }

        private void OnResumed()
        {
            _gameOverView.HideGameOver();
        }

        private void OnFinished()
        {
            _backgroundMusic?.Stop();
            _backgroundMusic = null;
            FinishAsync();
        }

        private async UniTask FinishAsync()
        {
            AnalyticService.Instance.Track("battle_finish", new Dictionary<string, object>()
            {
                {
                    "current_score", _scoreModel.CurrentScore
                }
            });

            _gameOverView.SetCurrentScore(_scoreModel.CurrentScore);
            _gameOverView.SetMaxScore(_scoreModel.MaxScore);
            _gameOverView.SetVisibleNewBestScore(_scoreModel.IsNewMaxScore);

            _leaderboardService.Report(_scoreModel.MaxScore);
            
            await _gameOverView.ShowGameOver();
            
            _dataStorageService.Save();

            if (_gamesOverCount >= _showInternADAfterCountGame && _adsService.Inter.IsReady)
            {
                _isOldPlayerData.Value = true;
                _gamesOverCount = 0;
                _adsService.Inter.Watch();
            }
            else
            {
                _gamesOverCount++;
            }
        }
    }
}