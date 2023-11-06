using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.CoreDomain.Services.Screen;
using Project.GameDomain.ScreensDomain;
using Project.GameDomain.ScreensDomain.LoadingDomain;
using Project.GameDomain.ScreensDomain.MainDomain;
using Project.GameDomain.ScreensDomain.SplashDomain;
using UnityEngine;
using Zenject;

namespace Project.GameDomain
{
    public class Entry : MonoBehaviour
    {
        [SerializeField] [Dropdown(nameof(GetScreens) + "()")]
        private string _startScreen;

        [SerializeField] [Space] private GameObject _tempBeforeSplash;

        private IScreenInitializable _screenInitializable;
        private IScreensService _screensService;

        public List<string> GetScreens() => ScreensIds.Ids;

        [Inject]
        private void Construct(
            IScreenInitializable screenInitializable,
            IScreensService screensService
        )
        {
            _screenInitializable = screenInitializable;
            _screensService = screensService;
        }

        private async void Start()
        {
            _screenInitializable.SetSplashScreen(SplashScreen.Id);
            _screenInitializable.SetLoadingScreen(LoadingScreen.Id);
            await _screensService.SwitchAsync(SplashScreen.Id);
            Destroy(_tempBeforeSplash);
            await _screensService.SwitchAsync(MainScreen.Id);
            _screensService.SwitchAsync(_startScreen).Forget();
        }
    }
}