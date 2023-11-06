using Cysharp.Threading.Tasks;
using Project.CoreDomain;
using Project.CoreDomain.Services.View;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Battle.View;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Battle.Presenter
{
    public class BattlePresenter : IPresenter
    {
        private readonly IViewService _viewService;
        private IDisposableView<BattleView> _backgroundView;

        public BattlePresenter(IViewService viewService)
        {
            _viewService = viewService;
        }

        public async UniTask InitializeAsync()
        {
            _backgroundView = await _viewService.CreateAsync<BattleView>(BattleScreenContentIds.BattlePrefab);
        }

        public UniTask DisposeAsync()
        {
            _backgroundView?.Dispose();
            _backgroundView = null;
            
            return UniTask.CompletedTask;
        }
    }
}