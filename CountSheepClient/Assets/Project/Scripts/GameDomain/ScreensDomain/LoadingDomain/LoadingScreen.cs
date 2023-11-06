using Cysharp.Threading.Tasks;
using Project.CoreDomain.Services.Screen;

namespace Project.GameDomain.ScreensDomain.LoadingDomain
{
    public class LoadingScreen : Screen<LoadingScreen>
    {
        protected override string ScreenId => Id;

        public static string Id => "LoadingScreen";
        public override bool IsDisposeOnSwitch => false;

        public override UniTask ShowAsync()
        {
            return UniTask.CompletedTask;
        }

        public override UniTask HideAsync()
        {
            return UniTask.CompletedTask;
        }

        protected override UniTask InitializeScreenAsync()
        {
            return UniTask.CompletedTask;
        }

        protected override UniTask DisposeScreenAsync()
        {
            return UniTask.CompletedTask;
        }
    }
}