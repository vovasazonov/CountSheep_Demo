using Cysharp.Threading.Tasks;

namespace Project.CoreDomain
{
    public interface IDomainTaskAsyncDisposable
    {
        UniTask DisposeAsync();
    }
}