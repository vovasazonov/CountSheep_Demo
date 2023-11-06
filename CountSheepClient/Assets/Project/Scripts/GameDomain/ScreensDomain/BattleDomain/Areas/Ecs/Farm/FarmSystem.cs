using System;
using System.Collections.Generic;
using Osyacat.Ecs.Entity;
using Osyacat.Ecs.Matcher;
using Osyacat.Ecs.System;
using Osyacat.Ecs.World;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Player;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Rule;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Model;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Farm
{
    public class FarmSystem : IReactSystem
    {
        private readonly IFarmModel _farmModel;
        private readonly IFilter _playerFilter;

        public Func<IEntryMatcher, IMatcher> Matcher => matcher => matcher.Has<GameoverComponent>();

        public FarmSystem(IWorld world, IFarmModel farmModel)
        {
            _farmModel = farmModel;
            _playerFilter = world.GetFilter(matcher => matcher.Has<PlayerComponent>());
        }

        public void React(List<IEntity> entities)
        {
            foreach (var player in _playerFilter)
            {
                if (_farmModel.HasPlaceInFarm())
                {
                    _farmModel.Add(player.Get<PlayerComponent>().Id);
                }
            }
        }
    }
}