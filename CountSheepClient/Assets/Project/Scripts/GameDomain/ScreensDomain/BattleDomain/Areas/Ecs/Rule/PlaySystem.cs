using System.Collections.Generic;
using Osyacat.Ecs.Entity;
using Osyacat.Ecs.System;
using Osyacat.Ecs.World;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Input;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Move;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Player;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Rule.Presenter;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Rule
{
    public class PlaySystem : IUpdateSystem
    {
        private readonly IRuleModel _ruleModel;
        private readonly IFilter _playerFilter;
        private readonly IFilter _inputFilter;
        private readonly List<IEntity> _playerBuffer = new();

        public PlaySystem(IWorld world, IRuleModel ruleModel)
        {
            _playerFilter = world.GetFilter(matcher => matcher.Has<PlayerComponent>());
            _inputFilter = world.GetFilter(matcher => matcher.Has<InputComponent>());
            _ruleModel = ruleModel;
        }

        public void Update()
        {
            _playerFilter.UpdateBuffer(_playerBuffer);

            foreach (var _ in _inputFilter)
            {
                if (_ruleModel.IsReadyToPlay)
                {
                    _ruleModel.Start();
                }
            }

            foreach (var player in _playerBuffer)
            {
                if (_ruleModel.IsPlaying)
                {
                    if (!player.Contains<MovableComponent>())
                    {
                        player.Replace<MovableComponent>();
                    }
                }
                else
                {
                    if (player.Contains<MovableComponent>())
                    {
                        player.Remove<MovableComponent>();
                    }
                }
            }
        }
    }
}