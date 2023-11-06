using Osyacat.Ecs.Component.Event;
using UnityEngine;

namespace Osyacat.Ecs.Entity
{
    public class EntityView : MonoBehaviour, IEntityView
    {
        protected IEntity _entity;
        
        public void Initialize(IEntity entity)
        {
            _entity = entity;
            _entity.Replace<ViewComponent>().Value = this;
            
            foreach (var listener in GetComponents<IComponentListener>())
            {
                listener.Register(_entity);
            }
        }

        public void UnInitialize()
        {
            foreach (var listener in GetComponents<IComponentListener>())
            {
                listener.Unregister();
            }

            _entity.Remove<ViewComponent>();
            _entity = null;
        }
    }
}