using Osyacat.Ecs.Component.Component;
using Osyacat.Ecs.Component.Event;
using UnityEngine;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Position
{
    [Component, EventComponent]
    public class PositionComponent
    {
        public Vector3 Value;
    }
}