using Cysharp.Threading.Tasks;

namespace Project.CoreDomain
{
    public interface ITaskAsyncDisposable
    {
        UniTask DisposeAsync();
    }
}