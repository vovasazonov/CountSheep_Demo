using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Osyacat.Ecs.System;
using Project.CoreDomain;
using Project.CoreDomain.Services.Analytics;
using Project.CoreDomain.Services.Engine;
using Project.CoreDomain.Services.Screen;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Rule.Presenter;

namespace Project.GameDomain.ScreensDomain.BattleDomain
{
    public class BattleScreen : Screen<BattleScreen>
    {
        private readonly ISystems _systems;
        private readonly IEngineService _engineService;
        private readonly IRuleModel _ruleModel;
        private readonly List<IPresenter> _presenters;
        protected override string ScreenId => Id;
        private static bool _isContentInitialized;

        public static string Id => "BattleScreen";
        public override bool IsDisposeOnSwitch => true;

        public BattleScreen(
            ISystems systems,
            IEngineService engineService, 
            IRuleModel ruleModel,
            List<IPresenter> presenters
        )
        {
            _systems = systems;
            _engineService = engineService;
            _ruleModel = ruleModel;
            _presenters = presenters;
        }

        public override UniTask ShowAsync()
        {
            AddListeners();
            _systems.Initialize();
            _ruleModel.PrepareToStart();

            return UniTask.CompletedTask;
        }

        public override UniTask HideAsync()
        {
            RemoveListeners();
            _systems.Destroy();
            
            return UniTask.CompletedTask;
        }

        protected override async UniTask InitializeScreenAsync()
        {
            if (!_isContentInitialized)
            {
                _isContentInitialized = true;

                var content = new List<UniTask>
                {
                };

                await UniTask.WhenAll(content);

                AnalyticService.Instance.Track("first_scene_opened");
            }

            foreach (var presenter in _presenters)
            {
                await presenter.InitializeAsync();
            }
        }

        protected override async UniTask DisposeScreenAsync()
        {
            foreach (var presenter in _presenters)
            {
                await presenter.DisposeAsync();
            }
        }

        private void AddListeners()
        {
            _engineService.Updating += OnUpdating;
            _engineService.FixedUpdating += OnFixedUpdating;
        }

        private void RemoveListeners()
        {
            _engineService.Updating -= OnUpdating;
            _engineService.FixedUpdating -= OnFixedUpdating;
        }

        private void OnUpdating()
        {
            _systems.BeforeUpdate();
            _systems.Update();
            _systems.LateUpdate();
        }

        private void OnFixedUpdating()
        {
            _systems.FixedUpdate();
        }
    }
}