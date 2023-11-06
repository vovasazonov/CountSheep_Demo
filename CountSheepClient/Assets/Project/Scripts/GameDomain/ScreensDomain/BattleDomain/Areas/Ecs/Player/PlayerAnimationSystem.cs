using Osyacat.Ecs.System;
using Osyacat.Ecs.World;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Animation;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Jump;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Player
{
    public class PlayerAnimationSystem : IUpdateSystem
    {
        private readonly IFilter _playerFilter;
        private readonly string _downState = "Down";
        private readonly string _upState = "Up";
        private readonly string _moveState = "Move";

        public PlayerAnimationSystem(IWorld world)
        {
            _playerFilter = world.GetFilter(matcher => matcher.Has<PlayerComponent>());
        }

        public void Update()
        {
            foreach (var player in _playerFilter)
            {
                if (!player.Contains<AnimationComponent>())
                {
                    player.Replace<AnimationComponent>().State = _moveState;
                }
                
                if (player.Contains<JumpingComponent>())
                {
                    var velocity = player.Get<JumpingComponent>().Velocity;
                    if (velocity > 0 && player.Get<AnimationComponent>().State != _upState)
                    {
                        player.Replace<AnimationComponent>().State = _upState;
                    }
                    else if (velocity < 0 && player.Get<AnimationComponent>().State != _downState)
                    {
                        player.Replace<AnimationComponent>().State = _downState;
                    }
                }
                else
                {
                    if (player.Get<AnimationComponent>().State != _moveState)
                    {
                        player.Replace<AnimationComponent>().State = _moveState;
                    }
                }
            }
        }
    }
}