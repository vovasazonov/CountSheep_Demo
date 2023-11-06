using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.CoreDomain.Services.Achievement
{
    [CreateAssetMenu(fileName = "GooglePlayAchievement", menuName = "Configs/GooglePlayAchievement", order = 0)]
    public class GoogleplayAchievementConfig : ScriptableObject
    {
        public List<GooglePlayIdByAchievementId> Achievements;
        
        [Serializable]
        public struct GooglePlayIdByAchievementId
        {
            public string GameAchievementId;
            public string GooglePlayAchievementId;
        }
    }
}