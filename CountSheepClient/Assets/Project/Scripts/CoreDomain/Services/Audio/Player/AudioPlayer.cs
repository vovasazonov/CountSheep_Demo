using System;
using Project.CoreDomain.Services.Audio.Configs.AudioPlayer;
using Project.CoreDomain.Services.Audio.Fade;
using Project.CoreDomain.Services.Audio.Source;
using Project.CoreDomain.Services.Content;
using Project.CoreDomain.Services.Time;
using Project.CoreDomain.Services.View;
using UnityEngine;

namespace Project.CoreDomain.Services.Audio.Player
{
    internal class AudioPlayer : IAudioPlayer, IDisposable
    {
        private readonly IViewService _viewService;
        private readonly AudioFade _fade;
        private AudioSource _source;
        private float _originalVolume = 1f;
        private IDisposableView<AudioSourceView> _audioSourceKeeper;

        public string Id { get; }
        public bool IsStopped => !_source.isPlaying;

        public float Volume
        {
            get => _originalVolume * (1f - _fade.PercentFade);
            set
            {
                _originalVolume = value;
                _source.volume = _originalVolume * (1f - _fade.PercentFade);
            }
        }

        public bool IsMuted
        {
            get => _source.mute;
            set => _source.mute = value;
        }

        public AudioPlayer(IContentKeeper<IAudioPlayerConfig> configKeeper, IDisposableView<AudioSourceView> audioSourceKeeper, ITimeService time)
        {
            Id = configKeeper.Value.Id;
            _audioSourceKeeper = audioSourceKeeper;
            _source = audioSourceKeeper.Value.AudioSource;
            _source.clip = configKeeper.Value.Clip;
            _source.loop = configKeeper.Value.IsLoop;
            _fade = new AudioFade(configKeeper.Value.FadeSeconds, _source, time);
            configKeeper.Dispose();
        }
        
        public AudioPlayer(IAudioPlayerConfig config, IDisposableView<AudioSourceView> audioSourceKeeper, ITimeService time)
        {
            Id = config.Id;
            _audioSourceKeeper = audioSourceKeeper;
            _source = audioSourceKeeper.Value.AudioSource;
            _source.clip = config.Clip;
            _source.loop = config.IsLoop;
            _fade = new AudioFade(config.FadeSeconds, _source, time);
        }

        public void Play()
        {
            _source.Play();
        }

        public void Stop()
        {
            if (_audioSourceKeeper != null)
            {
                _source.Stop();
            }
        }

        public void Update()
        {
            _fade.Update();
            Volume = _originalVolume;
        }

        public void Dispose()
        {
            _audioSourceKeeper?.Dispose();
            _audioSourceKeeper = null;
            _source = null;
        }
    }
}