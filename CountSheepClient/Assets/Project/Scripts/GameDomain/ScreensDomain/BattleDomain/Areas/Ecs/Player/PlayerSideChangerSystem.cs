using System;
using System.Collections.Generic;
using Osyacat.Ecs.Entity;
using Osyacat.Ecs.Matcher;
using Osyacat.Ecs.System;
using Osyacat.Ecs.World;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Direction;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Enemy;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Invisible;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Jump;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Position;
using UnityEngine;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Player
{
    public class PlayerSideChangerSystem : IUpdateSystem//, IReactSystem
    {
        private readonly IFilter _enemyFilter;
        private readonly IFilter _playerFilter;
        private const float _changeSideHeightTrigger = -0.3f;
        // private readonly Dictionary<IEntity, Vector3> _positionByJumpingPlayer = new();

        // public Func<IEntryMatcher, IMatcher> Matcher => matcher => matcher.Has<JumpingComponent>();

        public PlayerSideChangerSystem(IWorld world)
        {
            _enemyFilter = world.GetFilter(matcher => matcher.Has<EnemyComponent>());
            _playerFilter = world.GetFilter(matcher => matcher.Has<PlayerComponent>());
        }

        public void Update()
        {
            foreach (var enemy in _enemyFilter)
            {
                var leftFailTarget = enemy.Get<EnemyComponent>().LeftPoint;
                var rightFailTarget = enemy.Get<EnemyComponent>().RightPoint;

                foreach (var player in _playerFilter)
                {
                    var playerPosition = player.Get<PositionComponent>().Value;
                    var isPlayerSafe = playerPosition.x >= leftFailTarget.x && 
                                       playerPosition.x <= rightFailTarget.x && 
                                       (player.Contains<JumpingComponent>() && playerPosition.y >= _changeSideHeightTrigger || player.Contains<InvisibleComponent>());
                    var direction = player.Get<DirectionComponent>().Value;

                    if (isPlayerSafe)
                    {
                        var isSideLeft = direction == Vector3.left;
                        if (player.Get<PlayerSideComponent>().IsLeft != isSideLeft)
                        {
                            player.Replace<PlayerSideComponent>().IsLeft = isSideLeft;
                        }
                    }

                    var isPlayerFarFromEnemy = direction == Vector3.right && playerPosition.x < leftFailTarget.x || direction == Vector3.left && playerPosition.x > rightFailTarget.x;
                    if (isPlayerFarFromEnemy)
                    {
                        if (playerPosition.x < leftFailTarget.x && !player.Get<PlayerSideComponent>().IsLeft)
                        {
                            if (!player.Get<PlayerSideComponent>().IsLeft)
                            {
                                player.Replace<PlayerSideComponent>().IsLeft = true;
                            }
                        }
                        else if (playerPosition.x > rightFailTarget.x && player.Get<PlayerSideComponent>().IsLeft)
                        {
                            if (player.Get<PlayerSideComponent>().IsLeft)
                            {
                                player.Replace<PlayerSideComponent>().IsLeft = false;
                            }
                        }
                    }
                }
            }
        }

        // public void React(List<IEntity> entities)
        // {
        //     foreach (var player in entities)
        //     {
        //         var isSideLeft = player.Get<PlayerSideComponent>().IsLeft;
        //         var direction = player.Get<DirectionComponent>().Value;
        //         var isNeedJumpOverGate = isSideLeft && direction == Vector3.right || !isSideLeft && direction == Vector3.left;
        //
        //         if (player.Contains<PlayerComponent>() && !_positionByJumpingPlayer.ContainsKey(player) && isNeedJumpOverGate)
        //         {
        //             var position = player.Get<PositionComponent>().Value;
        //             _positionByJumpingPlayer.Add(player, position);
        //         }
        //     }
        // }
    }
}