using Cysharp.Threading.Tasks;
using Project.CoreDomain;
using Project.CoreDomain.Services.Content;
using Project.CoreDomain.Services.Serialization;
using UnityEngine;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Config
{
    public class FarmConfigKeeper : IFarmConfigKeeper, IDomainTaskAsyncInitializable, IDomainTaskAsyncDisposable
    {
        private readonly IContentService _contentService;
        private readonly ISerializerService _serializerService;

        public FarmConfig FarmConfig { get; private set; }

        public FarmConfigKeeper(IContentService contentService, ISerializerService serializerService)
        {
            _contentService = contentService;
            _serializerService = serializerService;
        }

        public async UniTask InitializeAsync()
        {
            var textKeeper = await _contentService.LoadAsync<TextAsset>(MainScreenContentIds.FarmConfig);
            FarmConfig = _serializerService.DeserializeJson<FarmConfig>(textKeeper.Value.text);
            textKeeper.Dispose();
        }

        public UniTask DisposeAsync()
        {
            return UniTask.CompletedTask;
        }
    }
}