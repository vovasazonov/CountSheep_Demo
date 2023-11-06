using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.CoreDomain.Services.Audio.Collection;
using Project.CoreDomain.Services.Content;
using Project.CoreDomain.Services.Engine;
using Project.CoreDomain.Services.Time;
using Project.CoreDomain.Services.View;
using UnityEngine;

namespace Project.CoreDomain.Services.Audio
{
    public class AudioService : IAudioService, ITaskAsyncInitializable, ITaskAsyncDisposable
    {
        private readonly IContentService _contentService;
        private readonly List<AudioCollection> _collections;

        public IAudioCollection Sound { get; }
        public IAudioCollection Music { get; }

        public AudioService(
            IEngineService engineService,
            ITimeService timeService,
            IContentService contentService,
            IViewService viewService
        )
        {
            _contentService = contentService;
            _collections = new List<AudioCollection>(2);
            var audioKeeperScreenId = "MainScreen";

            Sound = InitializeUpdateable(new AudioCollection(engineService, timeService, contentService, viewService, audioKeeperScreenId));
            Music = InitializeUpdateable(new AudioCollection(engineService, timeService, contentService, viewService, audioKeeperScreenId));
            Music.Volume = 0.7f;
        }

        private AudioCollection InitializeUpdateable(AudioCollection audioCollection)
        {
            _collections.Add(audioCollection);
            return audioCollection;
        }

        public async UniTask InitializeAsync()
        {
            await _contentService.LoadAsync<GameObject>(AudioContentId.AudioSourceId);
            
            foreach (var audioCollection in _collections)
            {
                await audioCollection.InitializeAsync();
            }
        }

        public async UniTask DisposeAsync()
        {
            foreach (var audioCollection in _collections)
            {
                await audioCollection.DisposeAsync();
            }
        }
    }
}