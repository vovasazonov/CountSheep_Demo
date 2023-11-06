using Osyacat.Ecs.Component.Component;
using Osyacat.Ecs.Component.Event;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Animation
{
    [Component, EventComponent]
    public class AnimationComponent
    {
        public string State;
    }
}