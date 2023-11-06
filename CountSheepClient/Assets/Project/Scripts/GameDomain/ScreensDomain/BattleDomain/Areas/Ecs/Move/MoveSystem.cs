using Osyacat.Ecs.System;
using Osyacat.Ecs.World;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Direction;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Position;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Speed;
using UnityEngine;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Move
{
    public class MoveSystem : IUpdateSystem
    {
        private readonly IFilter _movableFilter;

        public MoveSystem(IWorld world)
        {
            _movableFilter = world.GetFilter(matcher => matcher.Has<MovableComponent>().Has<PositionComponent>().Has<DirectionComponent>().Has<SpeedComponent>());
        }

        public void Update()
        {
            foreach (var movable in _movableFilter)
            {
                var position = movable.Get<PositionComponent>().Value;
                var direction = movable.Get<DirectionComponent>().Value;
                var speed = movable.Get<SpeedComponent>().Value;

                movable.Replace<PositionComponent>().Value = position + direction * speed * Time.deltaTime;
            }
        }
    }
}