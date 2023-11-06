using Cysharp.Threading.Tasks;
using Project.CoreDomain;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Rule.Presenter;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Score.Model;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Score.View;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Score.Presenter
{
    public class ScoreInBattlePresenter : IPresenter
    {
        private readonly IScoreModel _scoreModel;
        private readonly IRuleModel _ruleModel;
        private readonly IScoreInBattleView _view;

        public ScoreInBattlePresenter(
            IScoreModel scoreModel,
            IRuleModel ruleModel,
            IScoreInBattleView view
        )
        {
            _scoreModel = scoreModel;
            _ruleModel = ruleModel;
            _view = view;
        }

        public UniTask InitializeAsync()
        {
            _scoreModel.Updated += OnUpdated;
            _ruleModel.Finished += OnFinished;
            _ruleModel.Prepared += OnPrepared;
            _ruleModel.Resumed += OnResumed;
            return UniTask.CompletedTask;
        }

        public UniTask DisposeAsync()
        {
            _scoreModel.Updated -= OnUpdated;
            _ruleModel.Finished -= OnFinished;
            _ruleModel.Prepared -= OnPrepared;
            _ruleModel.Resumed -= OnResumed;
            return UniTask.CompletedTask;
        }

        private void OnResumed()
        {
            _view.Show();
        }

        private void OnPrepared()
        {
            _view.Show();
        }

        private void OnFinished()
        {
            _view.Hide();
        }

        private void OnUpdated()
        {
            _view.Score = _scoreModel.CurrentScore;
        }
    }
}