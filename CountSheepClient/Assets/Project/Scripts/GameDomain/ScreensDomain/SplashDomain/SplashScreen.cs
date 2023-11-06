using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.CoreDomain;
using Project.CoreDomain.Services.Screen;
using Project.CoreDomain.Services.View;
using Project.GameDomain.ScreensDomain.SplashDomain.Splash.View;
using UnityEngine;

namespace Project.GameDomain.ScreensDomain.SplashDomain
{
    public class SplashScreen : Screen<SplashScreen>
    {
        private readonly List<ITaskAsyncInitializable> _initializables;
        private readonly IViewService _viewService;
        private IDisposableView<SplashView> _view;

        protected override string ScreenId => Id;

        public static string Id => "SplashScreen";
        public override bool IsDisposeOnSwitch => false;

        public SplashScreen(List<ITaskAsyncInitializable> initializables, IViewService viewService)
        {
            _initializables = initializables;
            _viewService = viewService;
        }

        public override UniTask ShowAsync()
        {
            return UniTask.CompletedTask;
        }

        public override UniTask HideAsync()
        {
            _view.Value.Hide().ContinueWith(_view.Dispose);
            return UniTask.CompletedTask;
        }

        protected override async UniTask InitializeScreenAsync()
        {
            _view = await _viewService.CreateAsync<SplashView>(SplashScreenContentIds.Splash);
            
            Application.targetFrameRate = 120;
            
            foreach (var initializable in _initializables)
            {
                await initializable.InitializeAsync();
            }
        }

        protected override async UniTask DisposeScreenAsync()
        {
        }
    }
}