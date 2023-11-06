using System.Collections.Generic;
using Osyacat.Ecs.Entity;
using Osyacat.Ecs.System;
using Osyacat.Ecs.World;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Input;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Position;
using UnityEngine;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Jump
{
    public class JumpSystem : IUpdateSystem
    {
        private readonly IFilter _jumpableFilter;
        private readonly IFilter _inputFilter;
        private readonly List<IEntity> _buffer = new();
        private readonly float _gravity = -9.81f;
        private readonly IFilter _jumpingFilter;
        private readonly float _landY = -1.15f;
        private bool _isFirstInputPast;

        public JumpSystem(IWorld world)
        {
            _jumpableFilter = world.GetFilter(matcher => matcher.Has<JumpableComponent>().Has<PositionComponent>().Has<JumpForceComponent>());
            _jumpingFilter = world.GetFilter(matcher => matcher.Has<JumpingComponent>().Has<PositionComponent>().Has<JumpForceComponent>());
            _inputFilter = world.GetFilter(matcher => matcher.Has<InputComponent>());
        }

        public void Update()
        {
            _jumpableFilter.UpdateBuffer(_buffer);

            foreach (var _ in _inputFilter)
            {
                if (!_isFirstInputPast)
                {
                    _isFirstInputPast = true;
                    return;
                }
                
                foreach (var jump in _buffer)
                {
                    if (!jump.Contains<JumpingComponent>())
                    {
                        jump.Replace<JumpingComponent>().Velocity = jump.Get<JumpForceComponent>().Value;
                    }
                }
            }

            _jumpingFilter.UpdateBuffer(_buffer);

            foreach (var jump in _buffer)
            {
                var velocity = jump.Get<JumpingComponent>().Velocity + _gravity * Time.deltaTime;
                jump.Replace<JumpingComponent>().Velocity = velocity;
                    
                var position = jump.Get<PositionComponent>().Value;
                var y = position.y + velocity * Time.deltaTime;
                position.y = y <= _landY ? _landY : y;
                jump.Replace<PositionComponent>().Value = position;

                if (y <= _landY)
                {
                    jump.Remove<JumpingComponent>();
                }
            }
        }
    }
}