using Osyacat.Ecs.System;
using Osyacat.Ecs.World;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Direction;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Position;
using UnityEngine;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Move
{
    public class SwitchMoveDirectionSystem : IUpdateSystem
    {
        private readonly IFilter _movableFilter;
        private float _leftBorderX = -3f;
        private float _rightBorderX = 3f;

        public SwitchMoveDirectionSystem(IWorld world)
        {
            _movableFilter = world.GetFilter(matcher => matcher.Has<MovableComponent>().Has<PositionComponent>().Has<DirectionComponent>());
        }

        public void Update()
        {
            foreach (var movable in _movableFilter)
            {
                var position = movable.Get<PositionComponent>().Value;
                var direction = movable.Get<DirectionComponent>().Value;
                
                if (_leftBorderX > position.x && direction != Vector3.right)
                {
                    movable.Replace<DirectionComponent>().Value = Vector3.right;
                }
                else if (_rightBorderX < position.x && direction != Vector3.left)
                {
                    movable.Replace<DirectionComponent>().Value = Vector3.left;
                }
            }
        }
    }
}