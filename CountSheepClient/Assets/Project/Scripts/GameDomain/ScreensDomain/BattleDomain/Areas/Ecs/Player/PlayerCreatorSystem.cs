using Osyacat.Ecs.System;
using Osyacat.Ecs.World;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Direction;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Effect;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Jump;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Physics;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Position;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Rule;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Speed;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.SpriteLibrary;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.View;
using Project.GameDomain.ScreensDomain.MainDomain;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Score.Model;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Player
{
    public class PlayerCreatorSystem : IInitializeSystem, IDestroySystem, IUpdateSystem
    {
        private readonly IWorld _world;
        private readonly IScoreModel _scoreModel;
        private readonly IFilter _playerFilter;
        private readonly float _landY = -1.15f;
        private readonly IFilter _gameoverFilter;

        public PlayerCreatorSystem(IWorld world, IScoreModel scoreModel)
        {
            _world = world;
            _scoreModel = scoreModel;
            _playerFilter = world.GetFilter(matcher => matcher.Has<PlayerComponent>());
            _gameoverFilter = world.GetFilter(matcher => matcher.Has<GameoverComponent>());
        }

        public void Initialize()
        {
            CreatePlayers();
        }

        public void Update()
        {
            foreach (var _ in _gameoverFilter)
            {
                return;
            }

            CreatePlayers();
        }

        private void CreatePlayers()
        {
            var amount = GetAmountPlayers();
            var score = _scoreModel.CurrentScore;
            var positionX = Random.Range(-15f, -5.64f);

            if (amount == 0)
            {
                CreatePlayer("Sheep", new Vector3(-5f, _landY), 1.5f);
            }
            else if (score == 2 && amount == 1 || score == 6 && amount == 2)
            {
                CreatePlayer("Sheep", new Vector3(positionX, _landY), 1.5f);
            }
            else if (score == 15 && amount == 3 || score == 30 && amount == 4 || score == 45 && amount == 5)
            {
                CreatePlayer("PinkSheep", new Vector3(positionX, _landY), 3f);
            }
            else if (score == 60 && amount == 6 || score == 90 && amount == 7 || score == 150 && amount == 8)
            {
                CreatePlayer("BrownSheep", new Vector3(positionX, _landY), 1.5f);
            }
            else if (score == 999)
            {
                CreatePlayer("Sheep", new Vector3(positionX, _landY), 1f);
            }
        }

        private void CreatePlayer(string playerId, Vector3 position, float speed)
        {
            var entity = _world.CreateEntity();
            entity.Replace<EffectComponent>().Effects = new();
            entity.Replace<PlayerComponent>().Id = playerId;
            entity.Replace<JumpForceComponent>().Value = 5f;
            entity.Replace<DirectionComponent>().Value = Vector3.right;
            entity.Replace<PositionComponent>().Value = position;
            entity.Replace<PlayerSideComponent>().IsLeft = true;
            entity.Replace<SpeedComponent>().Value = speed;
            entity.Replace<CollisionSyncComponent>();
            entity.Replace<ViewRequestComponent>().Id = BattleScreenContentIds.PlayerPrefab;
            entity.Replace<SpriteLibraryComponent>().AssetId = string.Format(MainScreenContentIds.PlayerSpriteLibrary, playerId);
        }

        public void Destroy()
        {
            foreach (var entity in _playerFilter.GetEntities())
            {
                entity.Destroy();
            }
        }

        private int GetAmountPlayers()
        {
            int amount = 0;
            foreach (var _ in _playerFilter)
            {
                amount++;
            }

            return amount;
        }
    }
}