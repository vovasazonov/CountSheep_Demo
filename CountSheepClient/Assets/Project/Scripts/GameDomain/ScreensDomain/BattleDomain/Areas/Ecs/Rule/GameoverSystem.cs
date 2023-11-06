using Osyacat.Ecs.System;
using Osyacat.Ecs.World;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Enemy;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Invisible;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Player;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Position;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Rule.Presenter;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Rule
{
    public class GameoverSystem : IUpdateSystem, IDestroySystem
    {
        private readonly IWorld _world;
        private readonly IRuleModel _ruleModel;
        private readonly IFilter _enemyFilter;
        private readonly IFilter _gameoverFilter;
        private readonly IFilter _playerFilter;

        public GameoverSystem(IWorld world, IRuleModel ruleModel)
        {
            _world = world;
            _ruleModel = ruleModel;
            _enemyFilter = world.GetFilter(matcher => matcher.Has<EnemyComponent>());
            _playerFilter = world.GetFilter(matcher => matcher.Has<PlayerComponent>());
            _gameoverFilter = world.GetFilter(matcher => matcher.Has<GameoverComponent>());
        }

        public void Update()
        {
            foreach (var _ in _gameoverFilter)
            {
                return;
            }
            
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
                    var isInvisible = player.Contains<InvisibleComponent>();
                    var isPlayerGameOver = (isPlayerLeft && playerPosition.x > right.x || isPlayerRight && playerPosition.x < left.x) && !isInvisible;

                    if (isPlayerGameOver)
                    {
                        _world.CreateEntity().Replace<GameoverComponent>();
                        _ruleModel.Finish();
                    }
                }
            }
        }

        public void Destroy()
        {
            foreach (var gameover in _gameoverFilter.GetEntities())
            {
                gameover.Destroy();
            }
        }
    }
}