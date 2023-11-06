using Osyacat.Ecs.Entity;
using UnityEngine;

namespace Osyacat.Ecs.Component.Event
{
    public abstract class ComponentListener<TComponent> : MonoBehaviour, IComponentListener<TComponent> where TComponent : class
    {
        protected IEntity _entity;
        
        public abstract void OnChanged(TComponent component);

        public void Register(IEntity entity)
        {
            _entity = entity;
            entity.RegisterListener(this);
            if (entity.Contains<TComponent>())
            {
                OnChanged(entity.Get<TComponent>());
            }
        }

        public void Unregister()
        {
            _entity.UnregisterListener(this);
            _entity = null;
        }
    }
}