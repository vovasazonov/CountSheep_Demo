using Cysharp.Threading.Tasks;

namespace Project.CoreDomain
{
    public interface ITaskAsyncInitializable
    {
        UniTask InitializeAsync();
    }
}