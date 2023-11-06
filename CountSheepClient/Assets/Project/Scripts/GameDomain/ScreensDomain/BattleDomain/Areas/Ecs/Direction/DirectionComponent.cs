using Osyacat.Ecs.Component.Component;
using Osyacat.Ecs.Component.Event;
using UnityEngine;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Direction
{
    [Component, EventComponent]
    public class DirectionComponent
    {
        public Vector3 Value;
    }
}