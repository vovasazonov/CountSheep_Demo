using System;
using Cysharp.Threading.Tasks;
using Project.CoreDomain;
using Project.CoreDomain.Services.Data;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Score.Data;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Score.Model
{
    public class ScoreModel : IScoreModel, IDomainTaskAsyncInitializable
    {
        private readonly IDataStorageService _dataStorageService;
        public event Action Updated;

        private int _currentScore;
        private ScoreData _data;

        public int MaxScore
        {
            get => _data.MaxScore;
            private set => _data.MaxScore = value;
        }

        public bool IsNewMaxScore { get; private set; }

        public int CurrentScore
        {
            get => _currentScore;
            set
            {
                _currentScore = value;

                IsNewMaxScore = _currentScore > MaxScore;
                
                if (IsNewMaxScore)
                {
                    MaxScore = _currentScore;
                }

                Updated?.Invoke();
            }
        }

        public ScoreModel(IDataStorageService dataStorageService)
        {
            _dataStorageService = dataStorageService;
        }

        public UniTask InitializeAsync()
        {
            var dataKey = "score";
            _data = _dataStorageService.Contains(dataKey) ? _dataStorageService.Get<ScoreData>(dataKey) : _dataStorageService.Create<ScoreData>(dataKey);
            return UniTask.CompletedTask;
        }
    }
}