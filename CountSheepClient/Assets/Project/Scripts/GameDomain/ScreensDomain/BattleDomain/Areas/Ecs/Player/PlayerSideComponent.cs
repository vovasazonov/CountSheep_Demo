using Osyacat.Ecs.Component.Component;
using Osyacat.Ecs.Component.Event;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Player
{
    [Component, EventComponent]
    public class PlayerSideComponent
    {
        public bool IsLeft;
    }
}