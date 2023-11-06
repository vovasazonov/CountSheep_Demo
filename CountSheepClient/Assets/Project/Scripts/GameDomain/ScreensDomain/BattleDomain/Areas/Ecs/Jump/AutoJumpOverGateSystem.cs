using Osyacat.Ecs.System;
using Osyacat.Ecs.World;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Enemy;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Player;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Position;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Jump
{
    public class AutoJumpOverGateSystem : IUpdateSystem
    {
        private readonly IFilter _playerFilter;
        private readonly IFilter _enemyFilter;
        private readonly IFilter _autoJumpFilter;

        public AutoJumpOverGateSystem(IWorld world)
        {
            _playerFilter = world.GetFilter(i => i.Has<PlayerComponent>());
            _enemyFilter = world.GetFilter(matcher => matcher.Has<EnemyComponent>());
            _autoJumpFilter = world.GetFilter(matcher => matcher.Has<AutoJumpComponent>());
        }

        public void Update()
        {
            foreach (var _ in _autoJumpFilter)
            {
                foreach (var enemy in _enemyFilter)
                {
                    var enemyComponent = enemy.Get<EnemyComponent>();
                    var left = enemyComponent.LeftPoint;
                    var right = enemyComponent.RightPoint;

                    foreach (var player in _playerFilter)
                    {
                        var isPlayerLeft = player.Get<PlayerSideComponent>().IsLeft;
                        var isPlayerRight = !isPlayerLeft;
                        var playerPosition = player.Get<PositionComponent>().Value;
                        // var isPlayerGameOver = (isPlayerLeft && playerPosition.x > right.x || isPlayerRight && playerPosition.x < left.x);
                        var isPlayerGameOver = (isPlayerLeft && playerPosition.x > left.x || isPlayerRight && playerPosition.x < right.x);

                        if (isPlayerGameOver && !player.Contains<JumpingComponent>())
                        {
                            player.Replace<JumpingComponent>().Velocity = player.Get<JumpForceComponent>().Value;
                        }
                    }
                }
            }
        }
    }
}