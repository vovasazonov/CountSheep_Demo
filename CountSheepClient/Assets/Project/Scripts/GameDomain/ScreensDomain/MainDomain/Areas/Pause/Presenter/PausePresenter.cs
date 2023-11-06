using Cysharp.Threading.Tasks;
using Project.CoreDomain;
using Project.CoreDomain.Services.Audio;
using Project.CoreDomain.Services.Data;
using Project.CoreDomain.Services.Screen;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Pause.View;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Rule.Presenter;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Pause.Presenter
{
    public class PausePresenter : IPresenter
    {
        private readonly IPauseView _view;
        private readonly IRuleModel _ruleModel;
        private readonly IDataStorageService _dataStorageService;
        private readonly IAudioService _audioService;
        private readonly IScreensService _screensService;
        private PrimitiveData<bool> _isMusicOnData;
        private PrimitiveData<bool> _isSoundOnData;

        public PausePresenter(
            IPauseView view,
            IRuleModel ruleModel,
            IDataStorageService dataStorageService,
            IAudioService audioService,
            IScreensService screensService
        )
        {
            _view = view;
            _ruleModel = ruleModel;
            _dataStorageService = dataStorageService;
            _audioService = audioService;
            _screensService = screensService;
        }

        public UniTask InitializeAsync()
        {
            var isMusicOnDataKey = "is_music_on";
            _isMusicOnData = _dataStorageService.Contains(isMusicOnDataKey)
                ? _dataStorageService.Get<PrimitiveData<bool>>(isMusicOnDataKey)
                : _dataStorageService.Create<PrimitiveData<bool>>(isMusicOnDataKey);
            
            var isSoundOnDataKey = "is_sound_on";
            _isSoundOnData = _dataStorageService.Contains(isSoundOnDataKey)
                ? _dataStorageService.Get<PrimitiveData<bool>>(isSoundOnDataKey)
                : _dataStorageService.Create<PrimitiveData<bool>>(isSoundOnDataKey);
            
            UpdateAudio();
            
            _view.IsMusicOn = _isMusicOnData.Value;
            _view.IsSoundOn = _isSoundOnData.Value;
            
            _view.Paused += OnPaused;
            _view.Restarted += OnRestarted;
            _view.Resumed += OnResumed;
            _view.MusicStatusChanged += OnMusicStatusChanged;
            _view.SoundStatusChanged += OnSoundStatusChanged;

            _ruleModel.Finished += OnFinished;
            _ruleModel.Started += OnStarted;

            _view.ShowPauseButton();
            _view.HidePopup();

            return UniTask.CompletedTask;
        }

        public UniTask DisposeAsync()
        {
            _view.Paused -= OnPaused;
            _view.Restarted -= OnRestarted;
            _view.Resumed -= OnResumed;
            _view.MusicStatusChanged -= OnMusicStatusChanged;
            _view.SoundStatusChanged -= OnSoundStatusChanged;
            
            _ruleModel.Finished -= OnFinished;
            _ruleModel.Started -= OnStarted;

            return UniTask.CompletedTask;
        }

        private void OnStarted()
        {
            _view.ShowPauseButton();
        }

        private void OnFinished()
        {
            _view.HidePauseButton();
        }

        private void OnMusicStatusChanged()
        {
            _isMusicOnData.Value = !_isMusicOnData.Value;
            UpdateAudio();
        }   
        private void OnSoundStatusChanged()
        {
            _isSoundOnData.Value = !_isSoundOnData.Value;
            UpdateAudio();
        }

        private void UpdateAudio()
        {
            _audioService.Music.IsMuted = _isMusicOnData.Value;
            _audioService.Sound.IsMuted = _isSoundOnData.Value;
            _dataStorageService.Save();
        }

        private void OnResumed()
        {
            _view.ShowPauseButton();
            _view.HidePopup();
            _ruleModel.Resume();
        }

        private void OnRestarted()
        {
            _view.ShowPauseButton();
            _view.HidePopup();
            _screensService.SwitchAsync("BattleScreen");
        }

        private void OnPaused()
        {
            _view.HidePauseButton();
            _view.ShowPopup();
            _ruleModel.Pause();
        }
    }
}