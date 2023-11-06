using Osyacat.Ecs.System;
using Osyacat.Ecs.World;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Invisible;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Player;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Rule.Presenter;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Rule
{
    public class ResumeSystem : IUpdateSystem, IInitializeSystem, IDestroySystem
    {
        private readonly IFilter _gameOverFilter;
        private readonly IFilter _playerFilter;
        private readonly IRuleModel _ruleModel;
        private bool _isResumed;

        public ResumeSystem(IWorld world, IRuleModel ruleModel)
        {
            _gameOverFilter = world.GetFilter(matcher => matcher.Has<GameoverComponent>());
            _playerFilter = world.GetFilter(matcher => matcher.Has<PlayerComponent>());
            _ruleModel = ruleModel;
        }

        public void Update()
        {
            if (_isResumed)
            {
                _isResumed = false;

                foreach (var player in _playerFilter)
                {
                    player.Replace<InvisibleComponent>().Seconds = 3;
                }
                
                foreach (var gameOver in _gameOverFilter.GetEntities())
                {
                    gameOver.Destroy();
                }
            }
        }

        public void Initialize()
        {
            _ruleModel.Resumed += OnResumed;
        }

        public void Destroy()
        {
            _ruleModel.Resumed -= OnResumed;
        }

        private void OnResumed()
        {
            _isResumed = true;
        }
    }
}