namespace Project.CoreDomain.Services.Achievement
{
    public interface IAchievementService
    {
        void Unlock(string achievementId);
        void Show();
    }
}