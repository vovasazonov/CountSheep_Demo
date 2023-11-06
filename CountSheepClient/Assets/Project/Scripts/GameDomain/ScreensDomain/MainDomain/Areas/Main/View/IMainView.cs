using Project.GameDomain.ScreensDomain.MainDomain.Areas.Collection.View;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Market.View;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Pause.View;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Rule.View;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Score.View;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Main.View
{
    public interface IMainView
    {
        IGameOverView GameOver { get; }
        IScoreInBattleView ScoreInBattle { get; }
        IPauseView Pause { get; }
        ICollectionView Collection { get; }
        IMarketView Market { get; }

        void SetActiveTutorialHand(bool isActive);
        void ShowBattle();
        void ShowMerge();
        void SetVisibleToMergeButton(bool isVisible);
    }
}