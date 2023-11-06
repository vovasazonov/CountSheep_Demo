using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.CoreDomain;
using Project.CoreDomain.Services.Audio.Configs.AudioPlayer;
using Project.CoreDomain.Services.Content;
using Project.CoreDomain.Services.Screen;
using UnityEngine;

namespace Project.GameDomain.ScreensDomain.MainDomain
{
    public class MainScreen : Screen<MainScreen>
    {
        private readonly List<IDomainTaskAsyncInitializable> _initializables;
        private readonly List<IDomainTaskAsyncDisposable> _domainDisposables;
        private readonly List<IPresenter> _presenters;
        private readonly IContentService _contentService;
        protected override string ScreenId => Id;

        public static string Id => "MainScreen";
        public override bool IsDisposeOnSwitch => false;

        public MainScreen(
            List<IDomainTaskAsyncInitializable> initializables,
            List<IDomainTaskAsyncDisposable> domainDisposables,
            List<IPresenter> presenters,
            IContentService contentService
        )
        {
            _initializables = initializables;
            _domainDisposables = domainDisposables;
            _presenters = presenters;
            _contentService = contentService;
        }

        public override UniTask ShowAsync()
        {
            return UniTask.CompletedTask;
        }

        public override UniTask HideAsync()
        {
            return UniTask.CompletedTask;
        }

        protected override async UniTask InitializeScreenAsync()
        {
            foreach (var initializable in _initializables)
            {
                await initializable.InitializeAsync();
            }
            
            var content = new List<UniTask>
            {
                _contentService.LoadAsync<AudioPlayerConfig>(MainScreenContentIds.BackgroundMusic),
                
                _contentService.LoadAsync<GameObject>(MainScreenContentIds.BattlePlayerPrefab),
                _contentService.LoadAsync<GameObject>(MainScreenContentIds.BattleEnemyPrefab),
                _contentService.LoadAsync<AudioPlayerConfig>(MainScreenContentIds.BattleJumpSound),
                _contentService.LoadAsync<GameObject>(MainScreenContentIds.BackgroundBattlePrefab),
                
                _contentService.LoadAsync<GameObject>(MainScreenContentIds.MergeAnimalPrefab),
                _contentService.LoadAsync<GameObject>(MainScreenContentIds.MergeBackgroundPrefab),
            };

            await UniTask.WhenAll(content);
            
            foreach (var presenter in _presenters)
            {
                await presenter.InitializeAsync();
            }
        }

        protected override async UniTask DisposeScreenAsync()
        {
            foreach (var disposable in _domainDisposables)
            {
                await disposable.DisposeAsync();
            }

            foreach (var presenter in _presenters)
            {
                await presenter.DisposeAsync();
            }
        }
    }
}