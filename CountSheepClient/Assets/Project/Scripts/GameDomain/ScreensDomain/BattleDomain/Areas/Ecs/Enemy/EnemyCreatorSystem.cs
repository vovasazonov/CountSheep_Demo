using Osyacat.Ecs.System;
using Osyacat.Ecs.World;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Physics;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Position;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.View;
using UnityEngine;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Enemy
{
    public class EnemyCreatorSystem : IInitializeSystem, IDestroySystem
    {
        private readonly IWorld _world;
        private readonly IFilter _enemyFilter;

        public EnemyCreatorSystem(IWorld world)
        {
            _world = world;
            _enemyFilter = world.GetFilter(matcher => matcher.Has<EnemyComponent>());
        }

        public void Initialize()
        {
            var entity = _world.CreateEntity();
            entity.Replace<EnemyComponent>();
            entity.Replace<PositionComponent>().Value = new Vector3(0, -1.39f);
            entity.Replace<ViewRequestComponent>().Id = BattleScreenContentIds.EnemyPrefab;
        }

        public void Destroy()
        {
            foreach (var entity in _enemyFilter.GetEntities())
            {
                entity.Destroy();
            }
        }
    }
}