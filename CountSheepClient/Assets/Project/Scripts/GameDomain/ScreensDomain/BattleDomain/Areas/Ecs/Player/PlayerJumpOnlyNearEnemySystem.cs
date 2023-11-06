using System.Collections.Generic;
using Osyacat.Ecs.Entity;
using Osyacat.Ecs.System;
using Osyacat.Ecs.World;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Direction;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Enemy;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Jump;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Position;
using UnityEngine;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Player
{
    public class PlayerJumpOnlyNearEnemySystem : IUpdateSystem
    {
        private readonly List<IEntity> _buffer = new();
        private readonly IFilter _playerFilter;
        private readonly IFilter _enemyFilter;

        public PlayerJumpOnlyNearEnemySystem(IWorld world)
        {
            _playerFilter = world.GetFilter(matcher => matcher.Has<PlayerComponent>().Has<PositionComponent>().Has<DirectionComponent>());
            _enemyFilter = world.GetFilter(matcher => matcher.Has<EnemyComponent>().Has<PositionComponent>());
        }

        public void Update()
        {
            foreach (var enemy in _enemyFilter)
            {
                var enemyPosition = enemy.Get<PositionComponent>().Value;

                _playerFilter.UpdateBuffer(_buffer);

                IEntity candidatePlayer = null;

                foreach (var player in _buffer)
                {
                    if (!player.Contains<JumpingComponent>())
                    {
                        var playerPosition = player.Get<PositionComponent>().Value;
                        var playerDirection = player.Get<DirectionComponent>().Value;
                        var isPlayerLookToEnemy = playerDirection == Vector3.left && enemyPosition.x < playerPosition.x || playerDirection == Vector3.right && enemyPosition.x > playerPosition.x;
                        
                        if (candidatePlayer == null && isPlayerLookToEnemy)
                        {
                            candidatePlayer = player;
                        }
                        else if(candidatePlayer != null)
                        {
                            var candidatePosition = candidatePlayer.Get<PositionComponent>().Value;
                            var isPlayerCloseThenCandidate = Vector3.Distance(enemyPosition, candidatePosition) > Vector3.Distance(enemyPosition, playerPosition);

                            if (isPlayerLookToEnemy && isPlayerCloseThenCandidate)
                            {
                                candidatePlayer = player;
                            }
                        }
                    }
                }

                foreach (var player in _buffer)
                {
                    if (player == candidatePlayer)
                    {
                        if (!player.Contains<JumpableComponent>())
                        {
                            player.Replace<JumpableComponent>();
                        }
                    }
                    else
                    {
                        if (player.Contains<JumpableComponent>())
                        {
                            player.Remove<JumpableComponent>();
                        }
                    }
                }
            }
        }
    }
}