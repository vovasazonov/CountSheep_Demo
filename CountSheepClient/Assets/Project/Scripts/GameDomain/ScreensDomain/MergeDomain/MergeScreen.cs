using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.CoreDomain;
using Project.CoreDomain.Services.Analytics;
using Project.CoreDomain.Services.Screen;

namespace Project.GameDomain.ScreensDomain.MergeDomain
{
    public class MergeScreen : Screen<MergeScreen>
    {
        private readonly List<IPresenter> _presenters;
        protected override string ScreenId => Id;
        private static bool _isContentInitialized;

        public static string Id => "MergeScreen";
        public override bool IsDisposeOnSwitch => true;

        public MergeScreen(
            List<IPresenter> presenters
        )
        {
            _presenters = presenters;
        }

        public override UniTask ShowAsync()
        {
            AnalyticService.Instance.Track("merge_start");
            return UniTask.CompletedTask;
        }

        public override UniTask HideAsync()
        {
            AnalyticService.Instance.Track("merge_finish");
            return UniTask.CompletedTask;
        }

        protected override async UniTask InitializeScreenAsync()
        {
            foreach (var presenter in _presenters)
            {
                await presenter.InitializeAsync();
            }
        }

        protected override async UniTask DisposeScreenAsync()
        {
            foreach (var presenter in _presenters)
            {
                await presenter.DisposeAsync();
            }
        }
    }
}