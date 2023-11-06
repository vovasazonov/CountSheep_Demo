using System.Linq;
using Cysharp.Threading.Tasks;
using GooglePlayGames;
using Project.CoreDomain.Services.Content;
using Project.CoreDomain.Services.GoogplayGames;
using UnityEngine;

namespace Project.CoreDomain.Services.Achievement
{
    public class GooglePlayAchievementService : IAchievementService, IDomainTaskAsyncInitializable
    {
        private readonly IGoogleplayGamesService _googleplayGamesService;
        private readonly IContentService _contentService;
        private IContentKeeper<GoogleplayAchievementConfig> _googleplayAchievementConfig;

        public GooglePlayAchievementService(IGoogleplayGamesService googleplayGamesService, IContentService contentService)
        {
            _googleplayGamesService = googleplayGamesService;
            _contentService = contentService;
        }

        public async UniTask InitializeAsync()
        {
            _googleplayAchievementConfig = await _contentService.LoadAsync<GoogleplayAchievementConfig>("Content/GameDomain/ScreensDomain/MainDomain/Configs/GooglePlayAchievement.asset");
        }

        public void Unlock(string id)
        {
            var achievementGoogleId = _googleplayAchievementConfig.Value.Achievements.FirstOrDefault(i => id == i.GameAchievementId).GooglePlayAchievementId;
            if (achievementGoogleId != null)
            {
                PlayGamesPlatform.Instance.UnlockAchievement(achievementGoogleId);
                OsyaLogger.Log($"Achievement {id} unlocked");
            }
            else
            {
                OsyaLogger.LogError($"There is no such achievement {id}");
            }
        }

        public void Show()
        {
            if (!_googleplayGamesService.IsAuthenticated)
            {
                _googleplayGamesService.SignIn(Social.ShowAchievementsUI);
            }
            else
            {
                Social.ShowAchievementsUI();
            }
        }
    }
}