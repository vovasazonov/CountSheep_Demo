using System;
using System.Collections.Generic;
using Osyacat.Ecs.Entity;
using Osyacat.Ecs.Matcher;
using Osyacat.Ecs.System;
using Osyacat.Ecs.World;
using Project.CoreDomain.Services.View;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.View
{
    public class ViewSystem : IReactSystem, IDestroySystem
    {
        private readonly IViewService _viewService;
        private readonly List<IDisposable> _disposables = new();
        private readonly IFilter _viewFilter;
        private readonly List<IEntity> _buffer = new();

        public Func<IEntryMatcher, IMatcher> Matcher => matcher => matcher.Has<ViewRequestComponent>();

        public ViewSystem(IWorld world, IViewService viewService)
        {
            _viewService = viewService;
            _viewFilter = world.GetFilter(matcher => matcher.Has<ViewComponent>());
        }

        public void React(List<IEntity> entities)
        {
            foreach (var entity in entities)
            {
                var id = entity.Get<ViewRequestComponent>().Id;
                var disposableView = _viewService.CreateAsync<EntityView>(id).GetAwaiter().GetResult();
                disposableView.Value.Initialize(entity);
                _disposables.Add(disposableView);
                entity.Replace<ViewComponent>().Value = disposableView.Value;
            }
        }

        public void Destroy()
        {
            _viewFilter.UpdateBuffer(_buffer);
            
            foreach (var view in _buffer)
            {
                view.Get<ViewComponent>().Value.UnInitialize();
                view.Destroy();
            }
            
            _buffer.Clear();
            
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }

            _disposables.Clear();
        }
    }
}