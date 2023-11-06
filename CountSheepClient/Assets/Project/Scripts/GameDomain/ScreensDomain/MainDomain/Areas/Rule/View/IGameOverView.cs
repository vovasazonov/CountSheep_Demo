using System;
using Cysharp.Threading.Tasks;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Rule.View
{
    public interface IGameOverView
    {
        event Action PlayChoose;
        event Action LeaderBoardChoose;

        UniTask ShowGameOver();
        void HideGameOver();
        void SetMaxScore(int score);
        void SetCurrentScore(int score);
        void SetVisibleNewBestScore(bool isActive);
    }
}