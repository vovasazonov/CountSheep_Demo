using System.Collections.Generic;
using Osyacat.Ecs.Component.Component;
using Osyacat.Ecs.Component.Event;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Effect
{
    [Component, EventComponent]
    public class EffectComponent
    {
        public List<string> Effects;
    }
}