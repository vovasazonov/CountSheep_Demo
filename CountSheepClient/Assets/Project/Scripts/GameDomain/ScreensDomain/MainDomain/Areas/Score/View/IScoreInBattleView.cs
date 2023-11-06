namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Score.View
{
    public interface IScoreInBattleView
    {
        int Score { set; }
        
        void Show();
        void Hide();
    }
}