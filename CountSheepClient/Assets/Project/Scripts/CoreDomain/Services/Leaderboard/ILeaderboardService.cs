namespace Project.CoreDomain.Services.Leaderboard
{
    public interface ILeaderboardService
    {
        void Report(int score);
        void Show();
    }
}