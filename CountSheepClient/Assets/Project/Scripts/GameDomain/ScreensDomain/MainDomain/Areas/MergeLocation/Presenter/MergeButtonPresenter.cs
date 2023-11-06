using Cysharp.Threading.Tasks;
using Project.CoreDomain;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Model;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Main.View;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Rule.Presenter;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.MergeLocation.Presenter
{
    public class MergeButtonPresenter : IPresenter
    {
        private readonly IMainView _mainView;
        private readonly IRuleModel _ruleModel;
        private readonly IFarmModel _farmModel;

        public MergeButtonPresenter(IMainView mainView, IRuleModel ruleModel, IFarmModel farmModel)
        {
            _mainView = mainView;
            _ruleModel = ruleModel;
            _farmModel = farmModel;
        }

        public UniTask InitializeAsync()
        {
            _ruleModel.Prepared += OnPrepared;
            _ruleModel.Started += OnStarted;
            return UniTask.CompletedTask;
        }

        public UniTask DisposeAsync()
        {
            _ruleModel.Prepared -= OnPrepared;
            _ruleModel.Started -= OnStarted;
            return UniTask.CompletedTask;
        }

        private void OnStarted()
        {
            _mainView.SetVisibleToMergeButton(false);
        }

        private void OnPrepared()
        {
            _mainView.SetVisibleToMergeButton(_farmModel.Animals.Count != 0);
        }
    }
}