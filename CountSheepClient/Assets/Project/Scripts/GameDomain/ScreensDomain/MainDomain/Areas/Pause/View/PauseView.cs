using System;
using Project.CoreDomain.Services.Achievement;
using Project.CoreDomain.Services.Leaderboard;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Pause.View
{
    public class PauseView : MonoBehaviour, IPauseView
    {
        public event Action Paused;
        public event Action Resumed;
        public event Action Restarted;
        public event Action MusicStatusChanged;
        public event Action SoundStatusChanged;

        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _resumedButton;
        [SerializeField] private GameObject _popup;
        [SerializeField] private Toggle _musicToggle;
        [SerializeField] private Toggle _soundToggle;
        [SerializeField] private Text _versionText;
        private ILeaderboardService _leaderboardService;
        private IAchievementService _achievementService;

        [Inject]
        private void Constructor(
            ILeaderboardService leaderboardService,
            IAchievementService achievementService
        )
        {
            _leaderboardService = leaderboardService;
            _achievementService = achievementService;
        }

        private void Awake()
        {
            _versionText.text = $"{Application.version}";
        }

        public bool IsMusicOn
        {
            set => _musicToggle.isOn = value;
        }

        public bool IsSoundOn
        {
            set => _soundToggle.isOn = value;
        }

        public void ShowPauseButton()
        {
            _pauseButton.gameObject.SetActive(true);
        }

        public void HidePauseButton()
        {
            _pauseButton.gameObject.SetActive(false);
        }

        public void ShowPopup()
        {
            _popup.SetActive(true);
        }

        public void HidePopup()
        {
            _popup.SetActive(false);
        }

        private void OnEnable()
        {
            _musicToggle.onValueChanged.AddListener(OnMusicStatusChanged);
            _soundToggle.onValueChanged.AddListener(OnSoundStatusChanged);
            _pauseButton.onClick.AddListener(OnPaused);
            _resumedButton.onClick.AddListener(OnResumed);
            _restartButton.onClick.AddListener(OnRestarted);
        }

        private void OnDisable()
        {
            _musicToggle.onValueChanged.RemoveListener(OnMusicStatusChanged);
            _soundToggle.onValueChanged.RemoveListener(OnSoundStatusChanged);
            _pauseButton.onClick.RemoveListener(OnPaused);
            _resumedButton.onClick.RemoveListener(OnResumed);
            _restartButton.onClick.RemoveListener(OnRestarted);
        }

        private void OnMusicStatusChanged(bool isOn)
        {
            MusicStatusChanged?.Invoke();
        }

        private void OnSoundStatusChanged(bool isOn)
        {
            SoundStatusChanged?.Invoke();
        }

        private void OnRestarted()
        {
            Restarted?.Invoke();
        }

        private void OnResumed()
        {
            Resumed?.Invoke();
        }

        private void OnPaused()
        {
            Paused?.Invoke();
        }

        public void ShowLeaderBoardUi()
        {
            _leaderboardService.Show();
        }

        public void ShowAchievementBoardUi()
        {
            _achievementService.Show();
        }
    }
}