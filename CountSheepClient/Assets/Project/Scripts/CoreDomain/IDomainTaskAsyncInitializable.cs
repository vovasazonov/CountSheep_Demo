using Cysharp.Threading.Tasks;

namespace Project.CoreDomain
{
    public interface IDomainTaskAsyncInitializable
    {
         UniTask InitializeAsync();
    }
}