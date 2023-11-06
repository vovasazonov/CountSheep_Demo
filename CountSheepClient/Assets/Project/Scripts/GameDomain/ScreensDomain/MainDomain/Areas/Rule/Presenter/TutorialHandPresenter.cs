using Cysharp.Threading.Tasks;
using Project.CoreDomain;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Main.View;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Rule.Presenter
{
    public class TutorialHandPresenter : IPresenter
    {
        private readonly IMainView _mainView;
        private readonly IRuleModel _ruleModel;

        public TutorialHandPresenter(IMainView mainView, IRuleModel ruleModel)
        {
            _mainView = mainView;
            _ruleModel = ruleModel;
        }

        public UniTask InitializeAsync()
        {
            _ruleModel.Prepared += OnPrepared;
            _ruleModel.Started += OnStarted;
            
            _mainView.SetActiveTutorialHand(false);

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
            _mainView.SetActiveTutorialHand(false);
        }

        private void OnPrepared()
        {
            _mainView.SetActiveTutorialHand(true);
        }
    }
}