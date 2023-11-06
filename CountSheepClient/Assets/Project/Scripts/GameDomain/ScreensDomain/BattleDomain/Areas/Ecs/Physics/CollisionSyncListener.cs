using System.Collections.Generic;
using Osyacat.Ecs.Component.Event;
using Osyacat.Ecs.Entity;
using UnityEngine;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Physics
{
    public class CollisionSyncListener : ComponentListener<CollisionSyncComponent>
    {
        public override void OnChanged(CollisionSyncComponent component)
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            ReplaceCollisionComponent(other.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            ReplaceCollisionComponent(other.gameObject);
        }

        private void ReplaceCollisionComponent(GameObject other)
        {
            if (_entity != null && _entity.Contains<CollisionSyncComponent>())
            {
                var colliderViews = other.gameObject.GetComponents<CollisionSyncListener>();

                foreach (var colliderView in colliderViews)
                {
                    if (_entity.Contains<CollisionComponent>())
                    {
                        var collisions = _entity.Get<CollisionComponent>().Others;
                        _entity.Get<CollisionComponent>().Others = null;

                        if (!collisions.Contains(colliderView._entity))
                        {
                            collisions.Add(colliderView._entity);
                        }

                        _entity.Replace<CollisionComponent>().Others = collisions;
                    }
                    else
                    {
                        var collisionComponent = _entity.Replace<CollisionComponent>();
                        collisionComponent.Others ??= new HashSet<IEntity>();
                        collisionComponent.Others.Clear();
                        collisionComponent.Others.Add(colliderView._entity);
                    }
                }
            }
        }
    }
}