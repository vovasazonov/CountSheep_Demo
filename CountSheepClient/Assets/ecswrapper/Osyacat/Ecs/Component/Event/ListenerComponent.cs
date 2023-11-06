using System.Collections.Generic;

namespace Osyacat.Ecs.Component.Event
{
    public class ListenerComponent<TComponent> where TComponent : class
    {
        public List<IComponentListener<TComponent>> Value;
    }
}