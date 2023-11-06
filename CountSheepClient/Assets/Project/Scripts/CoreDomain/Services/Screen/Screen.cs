using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.CoreDomain.Services.Di;
using Project.CoreDomain.Services.Logger;
using UnityEngine;
using Zenject;

namespace Project.CoreDomain.Services.Screen
{
    public abstract class Screen<TSpecificScreen> : IScreen where TSpecificScreen : Screen<TSpecificScreen>
    {
        private IDiService _diService;
        private DiContainer _diContainer;
        
        protected readonly Stack<IDisposable> _disposables = new();
        protected abstract string ScreenId { get; }
        
        public static string ContextPostfix => "Context";

        public abstract bool IsDisposeOnSwitch { get; }

        [Inject]
        private void Construct(IDiService diService, DiContainer diContainer)
        {
            _diService = diService;
            _diContainer = diContainer;
        }
        
        public abstract UniTask ShowAsync();

        public abstract UniTask HideAsync();

        public async UniTask InitializeAsync()
        {
            OsyaLogger.Log($"Initializing {ScreenId} screen");

            _diService.ContainerByScreenId.Add(ScreenId, _diContainer);
            await InitializeScreenAsync();
            
            OsyaLogger.Log($"Initialized {ScreenId} screen");
        }

        protected abstract UniTask InitializeScreenAsync();

        protected abstract UniTask DisposeScreenAsync();

        public async UniTask DisposeAsync()
        {
            OsyaLogger.Log($"Disposing {ScreenId} screen");

            await DisposeScreenAsync();
            UnityEngine.Object.Destroy(GameObject.Find(ScreenId + ContextPostfix));
            while (_disposables.Count > 0)
            {
                _disposables.Pop().Dispose();
            }

            _diService.ContainerByScreenId.Remove(ScreenId);
            
            OsyaLogger.Log($"Disposed {ScreenId} screen");
        }

        public class ZenjectFactory : PlaceholderFactory<TSpecificScreen>
        {
        }

        public class Factory : IScreenFactory
        {
            private readonly ZenjectFactory _zenjectFactory;

            public string Id { get; }

            public Factory(string id, ZenjectFactory zenjectFactory)
            {
                _zenjectFactory = zenjectFactory;
                Id = id;
            }

            public IScreen Create()
            {
                return _zenjectFactory.Create();
            }
        }
    }
}