using Cysharp.Threading.Tasks;
using Project.CoreDomain.Services.Content;
using Project.CoreDomain.Services.Di;
using Project.CoreDomain.Services.Screen;
using UnityEngine;

namespace Project.CoreDomain.Services.View
{
    public class ViewService : IViewService
    {
        private readonly IContentService _contentService;
        private readonly IScreensService _screensService;
        private readonly IDiService _diService;

        public ViewService(IContentService contentService, IScreensService screensService, IDiService diService)
        {
            _contentService = contentService;
            _screensService = screensService;
            _diService = diService;
        }

        public async UniTask<IDisposableView<T>> CreateAsync<T>(string assetId, string screenId = null) where T : MonoBehaviour
        {
            var gameObjectKeeper = await _contentService.LoadAsync<GameObject>(assetId);
            
            var component = gameObjectKeeper.Value;
            var view = _diService.ContainerByScreenId[screenId ?? _screensService.Current].InstantiatePrefab(component);
            
            return new ContentDisposableView<T>(view.GetComponent<T>(), gameObjectKeeper);
        }

        public IDisposableView<T> Create<T>(T asset, string screenId = null) where T : MonoBehaviour
        {
            var view = _diService.ContainerByScreenId[screenId ?? _screensService.Current].InstantiatePrefab(asset);
            
            return new BuiltInDisposableView<T>(view.GetComponent<T>());
        }
    }
}