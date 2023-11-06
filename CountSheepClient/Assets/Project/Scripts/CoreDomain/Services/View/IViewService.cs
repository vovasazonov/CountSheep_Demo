using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.CoreDomain.Services.View
{
    public interface IViewService
    {
        UniTask<IDisposableView<T>> CreateAsync<T>(string assetId, string screenId = null) where T : MonoBehaviour;
        IDisposableView<T> Create<T>(T asset, string screenId = null) where T : MonoBehaviour;
    }
}