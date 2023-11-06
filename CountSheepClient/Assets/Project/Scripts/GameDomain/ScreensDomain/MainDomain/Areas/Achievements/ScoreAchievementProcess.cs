using Cysharp.Threading.Tasks;
using Project.CoreDomain;
using Project.CoreDomain.Services.Achievement;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Score.Model;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Achievements
{
    public class ScoreAchievementProcess : IDomainTaskAsyncInitializable, IDomainTaskAsyncDisposable
    {
        private readonly IScoreModel _scoreModel;
        private readonly IAchievementService _achievementService;

        public ScoreAchievementProcess(IScoreModel scoreModel, IAchievementService achievementService)
        {
            _scoreModel = scoreModel;
            _achievementService = achievementService;
        }

        public UniTask InitializeAsync()
        {
            _scoreModel.Updated += OnScoreUpdated;
            return UniTask.CompletedTask;
        }

        public UniTask DisposeAsync()
        {
            _scoreModel.Updated -= OnScoreUpdated;
            return UniTask.CompletedTask;
        }

        private void OnScoreUpdated()
        {
            if (_scoreModel.CurrentScore > 15)
            {
                _achievementService.Unlock("score_15");
            }
            
            if (_scoreModel.CurrentScore > 50)
            {
                _achievementService.Unlock("score_50");
            }
            
            if (_scoreModel.CurrentScore > 150)
            {
                _achievementService.Unlock("score_150");
            }
        }
    }
}