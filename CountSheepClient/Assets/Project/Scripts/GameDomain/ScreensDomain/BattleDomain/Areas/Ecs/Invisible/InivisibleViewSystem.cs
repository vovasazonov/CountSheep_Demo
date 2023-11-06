using System.Collections.Generic;
using Osyacat.Ecs.Entity;
using Osyacat.Ecs.System;
using Osyacat.Ecs.World;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Player;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Invisible
{
    public class InivisibleViewSystem : IUpdateSystem
    {
        private readonly HashSet<IEntity> _appliedEffect = new();
        private readonly IFilter _playerFilter;

        public InivisibleViewSystem(IWorld world)
        {
            _playerFilter = world.GetFilter(matcher => matcher.Has<PlayerComponent>());
        }

        public void Update()
        {
            foreach (var player in _playerFilter)
            {
                if (player.Contains<InvisibleComponent>() && !_appliedEffect.Contains(player))
                {
                    _appliedEffect.Add(player);
                    player.Replace<InvisibleViewComponent>();
                }

                if (!player.Contains<InvisibleComponent>() &&_appliedEffect.Contains(player))
                {
                    _appliedEffect.Remove(player);
                }
            }
        }
    }
}