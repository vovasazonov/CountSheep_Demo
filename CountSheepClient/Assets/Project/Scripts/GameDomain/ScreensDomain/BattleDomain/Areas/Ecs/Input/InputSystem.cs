using Osyacat.Ecs.System;
using Osyacat.Ecs.World;
using Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Rule;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Rule.Presenter;
using Project.Scripts.GameDomain.ScreensDomain.MainDomain.Areas.GameInput.Model;
using UnityEngine;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Input
{
    public class InputSystem : IUpdateSystem
    {
        private readonly IWorld _world;
        private readonly IRuleModel _ruleModel;
        private readonly IInputModel _inputModel;
        private readonly IFilter _gameOverFilter;

        public InputSystem(IWorld world, IRuleModel ruleModel, IInputModel inputModel)
        {
            _world = world;
            _ruleModel = ruleModel;
            _inputModel = inputModel;
            _gameOverFilter = _world.GetFilter(matcher => matcher.Has<GameoverComponent>());
        }

        public void Update()
        {
            var isPlayMode = _ruleModel.IsPlaying || _ruleModel.IsReadyToPlay;
            
            if (!isPlayMode)
            {
                return;
            }
            
            foreach (var _ in _gameOverFilter)
            {
                return;
            }

            if (!_inputModel.IsPointerDown)
            {
                return;
            }

            if (UnityEngine.Input.GetMouseButtonDown(0) ||
                UnityEngine.Input.touchCount > 0 && UnityEngine.Input.GetTouch(0).phase == TouchPhase.Began
            )
            {
                _world.CreateEntity().Replace<InputComponent>();
            }
        }
    }
}