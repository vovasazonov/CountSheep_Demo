using System;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Score.Model
{
    public interface IScoreModel
    {
        event Action Updated;
        
        int MaxScore { get; }
        bool IsNewMaxScore { get; }
        int CurrentScore { get; set; }
    }
}