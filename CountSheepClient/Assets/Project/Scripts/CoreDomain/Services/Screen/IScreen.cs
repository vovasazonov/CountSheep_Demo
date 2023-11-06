using Cysharp.Threading.Tasks;

namespace Project.CoreDomain.Services.Screen
{
    public interface IScreen : ITaskAsyncDisposable, ITaskAsyncInitializable
    {
        bool IsDisposeOnSwitch { get; }

        UniTask ShowAsync();
        UniTask HideAsync();
    }
}