using Osyacat.Ecs.Component.Event;

namespace Osyacat.Ecs.Entity
{
    public interface IEntity
    {
        TComponent Get<TComponent>() where TComponent : class;
        TComponent Replace<TComponent>() where TComponent : class, new();
        void Remove<TComponent>() where TComponent : class;
        bool Contains<TComponent>() where TComponent : class;
        void RegisterListener<TComponent>(IComponentListener<TComponent> listener) where TComponent : class;
        void UnregisterListener<TComponent>(IComponentListener<TComponent> listener) where TComponent : class;
        void Destroy();
    }
}