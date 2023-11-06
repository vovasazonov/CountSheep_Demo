using Cysharp.Threading.Tasks;
using Osyacat.Ecs.Component.Event;
using Project.CoreDomain.Services.Content;
using UnityEngine;
using UnityEngine.U2D.Animation;
using Zenject;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.SpriteLibrary
{
    public class SpriteLibraryComponentListener : ComponentListener<SpriteLibraryComponent>
    {
        [SerializeField] private UnityEngine.U2D.Animation.SpriteLibrary _spriteLibrary;
        private IContentService _contentService;
        private bool _isDestroyed;
        private IContentKeeper<SpriteLibraryAsset> _spriteKeeper;

        [Inject]
        private void Constructor(IContentService contentService)
        {
            _contentService = contentService;
        }
        
        public override void OnChanged(SpriteLibraryComponent component)
        {
            _spriteKeeper?.Dispose();
            SetAssetAsync(component.AssetId);
        }

        private async UniTask SetAssetAsync(string assetId)
        {
            _spriteKeeper = await _contentService.LoadAsync<SpriteLibraryAsset>(assetId);

            if (_isDestroyed)
            {
                _spriteKeeper.Dispose();
            }
            else
            {
                _spriteLibrary.spriteLibraryAsset = _spriteKeeper.Value;
            }
        }

        private void OnDestroy()
        {
            _isDestroyed = true;
            _spriteKeeper?.Dispose();
        }
    }
}