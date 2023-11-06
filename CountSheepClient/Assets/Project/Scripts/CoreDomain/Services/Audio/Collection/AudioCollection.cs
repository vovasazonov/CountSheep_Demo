using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.CoreDomain.Services.Audio.Configs.AudioPlayer;
using Project.CoreDomain.Services.Audio.Player;
using Project.CoreDomain.Services.Audio.Source;
using Project.CoreDomain.Services.Content;
using Project.CoreDomain.Services.Engine;
using Project.CoreDomain.Services.Time;
using Project.CoreDomain.Services.View;

namespace Project.CoreDomain.Services.Audio.Collection
{
    internal class AudioCollection : IAudioCollection, ITaskAsyncInitializable, ITaskAsyncDisposable
    {
        private readonly IEngineService _engineService;
        private readonly ITimeService _timeService;
        private readonly IContentService _contentService;
        private readonly IViewService _viewService;
        private readonly string _audioKeeperScreenId;
        private readonly HashSet<AudioPlayer> _players = new();
        private readonly List<AudioPlayer> _toRemove = new();
        private float _volume = 1f;
        private bool _isMuted;

        public float Volume
        {
            get => _volume;
            set
            {
                _volume = value;

                foreach (var player in _players)
                {
                    player.Volume = _volume;
                }
            }
        }

        public bool IsMuted
        {
            get => _isMuted;
            set
            {
                _isMuted = value;

                foreach (var player in _players)
                {
                    player.IsMuted = _isMuted;
                }
            }
        }

        public AudioCollection(
            IEngineService engineService,
            ITimeService timeService,
            IContentService contentService,
            IViewService viewService,
            string audioKeeperScreenId
        )
        {
            _engineService = engineService;
            _timeService = timeService;
            _contentService = contentService;
            _viewService = viewService;
            _audioKeeperScreenId = audioKeeperScreenId;
        }

        public UniTask InitializeAsync()
        {
            _engineService.Updating += OnUpdating;

            return UniTask.CompletedTask;
        }

        public UniTask DisposeAsync()
        {
            _engineService.Updating -= OnUpdating;

            return UniTask.CompletedTask;
        }

        private void OnUpdating()
        {
            foreach (var player in _players)
            {
                if (player.IsStopped)
                {
                    _toRemove.Add(player);
                }
                else
                {
                    player.Update();
                }
            }

            foreach (var player in _toRemove)
            {
                _players.Remove(player);
                player.Dispose();
            }

            _toRemove.Clear();
        }

        public async UniTask<IAudioStopper> Play(string playerId)
        {
            var configKeeper = await _contentService.LoadAsync<AudioPlayerConfig>(playerId);
            var audioSourceKeeper = await _viewService.CreateAsync<AudioSourceView>(AudioContentId.AudioSourceId, _audioKeeperScreenId);
            var player = new AudioPlayer(configKeeper, audioSourceKeeper, _timeService);
            _players.Add(player);
            player.Volume = _volume;
            player.IsMuted = _isMuted;
            player.Play();
            return player;
        }

        public IAudioStopper PlayImmediately(string playerId)
        {
            var configKeeper = _contentService.LoadAsync<AudioPlayerConfig>(playerId).GetAwaiter().GetResult();
            var audioSourceKeeper = _viewService.CreateAsync<AudioSourceView>(AudioContentId.AudioSourceId, _audioKeeperScreenId).GetAwaiter().GetResult();
            var player = new AudioPlayer(configKeeper, audioSourceKeeper, _timeService);
            _players.Add(player);
            player.Volume = _volume;
            player.IsMuted = _isMuted;
            player.Play();
            return player;
        }

        public IAudioStopper PlayImmediately(AudioPlayerConfig config)
        {
            var audioSourceKeeper = _viewService.CreateAsync<AudioSourceView>(AudioContentId.AudioSourceId, _audioKeeperScreenId).GetAwaiter().GetResult();
            var player = new AudioPlayer(config, audioSourceKeeper, _timeService);
            _players.Add(player);
            player.Volume = _volume;
            player.IsMuted = _isMuted;
            player.Play();
            return player;
        }
    }
}