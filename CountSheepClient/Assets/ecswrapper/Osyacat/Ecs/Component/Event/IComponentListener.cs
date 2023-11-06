using Osyacat.Ecs.Entity;

namespace Osyacat.Ecs.Component.Event
{
    public interface IComponentListener<TComponent> : IComponentListener where TComponent : class
    {
        void OnChanged(TComponent component);
    }
    
    public interface IComponentListener
    {
        void Register(IEntity entity);
        void Unregister();
    }
}