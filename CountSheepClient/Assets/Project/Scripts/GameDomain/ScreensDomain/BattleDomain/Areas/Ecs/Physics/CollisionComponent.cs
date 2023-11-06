using System.Collections.Generic;
using Osyacat.Ecs.Component.Component;
using Osyacat.Ecs.Component.Frame;
using Osyacat.Ecs.Entity;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Physics
{
    [Component, FrameComponent]
    public class CollisionComponent
    {
        public HashSet<IEntity> Others;
    }
}