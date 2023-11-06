using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Project.CoreDomain.Services.Audio;
using Project.CoreDomain.Services.Audio.Configs.AudioPlayer;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Rule.View
{
    public class GameOverView : MonoBehaviour, IGameOverView
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _leaderBoardButton;
        [SerializeField] private GameObject _gameOverGameObject;
        [SerializeField] private Text _maxScoreText;
        [SerializeField] private Text _currentScoreText;
        [SerializeField] private GameObject _newBestScoreGameObject;
        [SerializeField] private Animator _animator;
        [SerializeField] private AudioPlayerConfig _appearSound;
        [SerializeField] private AudioPlayerConfig _flashSound;
        [SerializeField] private AudioPlayerConfig _buttonAppearSound;
        private float _secondsTimer;
        private bool _isWaitingToContinue;
        private IAudioService _audioService;

        public event Action PlayChoose;
        public event Action LeaderBoardChoose;

        [Inject]
        private void Constructor(IAudioService audioService)
        {
            _audioService = audioService;
        }
        
        private void OnEnable()
        {
            _leaderBoardButton.onClick.AddListener(OnLeaderBoardClicked);
            _playButton.onClick.AddListener(OnPlayClick);
        }

        private void OnDisable()
        {
            _leaderBoardButton.onClick.RemoveListener(OnLeaderBoardClicked);
            _playButton.onClick.RemoveListener(OnPlayClick);
        }

        private void OnLeaderBoardClicked()
        {
            LeaderBoardChoose?.Invoke();
        }

        private void OnPlayClick()
        {
            PlayChoose?.Invoke();
        }

        public async UniTask ShowGameOver()
        {
            _gameOverGameObject.SetActive(true);
            _animator.Rebind();
            _animator.Update(0f);
            _animator.Play("GameOver");
            await UniTask.Delay(3000);
        }

        public void HideGameOver()
        {
            _gameOverGameObject.SetActive(false);
        }

        public void SetMaxScore(int score)
        {
            _maxScoreText.text = score.ToString();
        }

        public void SetCurrentScore(int score)
        {
            _currentScoreText.text = score.ToString();
        }

        public void SetVisibleNewBestScore(bool isActive)
        {
            _newBestScoreGameObject.SetActive(isActive);
        }

        [UsedImplicitly]
        public void PlayWindowAppearSound()
        {
            _audioService.Sound.PlayImmediately(_appearSound);
        }
        
        [UsedImplicitly]
        public void PlayFlashSound()
        {
            _audioService.Sound.PlayImmediately(_flashSound);
        }
        
        [UsedImplicitly]
        public void PlayButtonAppearSound()
        {
            _audioService.Sound.PlayImmediately(_buttonAppearSound);
        }
    }
}