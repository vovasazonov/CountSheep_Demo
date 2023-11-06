namespace Project.GameDomain.ScreensDomain.MainDomain
{
    public static class MainScreenContentIds
    {
        // PlayerDomain
        public static string PlayerSpriteLibrary => "Content/GameDomain/PlayerDomain/Players/{0}/SpriteLibrary/Library.spriteLib";
        public static string PlayerIdleSprite => "Content/GameDomain/PlayerDomain/Players/{0}/Sprites/idle.png";

        // MainDomain
        public static string MainCanvas => "Content/GameDomain/ScreensDomain/MainDomain/Prefabs/MainCanvas.prefab";
        public static string AchievementsConfig => "Content/GameDomain/ScreensDomain/MainDomain/Configs/Achievements.asset";
        public static string BackgroundMusic => "Content/GameDomain/ScreensDomain/MainDomain/AudioConfigs/MusicBackground.asset";
        public static string FarmConfig => $"Content/GameDomain/PlayerDomain/Configs/FarmConfig.json";
        
        // BattleDomain
        public static string BattleDomain => "Content/GameDomain/ScreensDomain/BattleDomain";
        public static string BattlePlayerPrefab => $"{BattleDomain}/Prefabs/PlayerEntity.prefab";
        public static string BattleEnemyPrefab => $"{BattleDomain}/Prefabs/EnemyEntity.prefab";
        public static string BattleJumpSound => $"{BattleDomain}/AudioConfigs/Jump.asset";
        public static string BackgroundBattlePrefab => $"{BattleDomain}/Prefabs/Battle.prefab";
        
        // MergeDomain
        private static string MergeDomain => "Content/GameDomain/ScreensDomain/MergeDomain";
        public static string MergeBackgroundPrefab => $"{MergeDomain}/Prefabs/Merge.prefab";
        public static string MergeAnimalPrefab => $"{MergeDomain}/Prefabs/Animal.prefab";
    }
}