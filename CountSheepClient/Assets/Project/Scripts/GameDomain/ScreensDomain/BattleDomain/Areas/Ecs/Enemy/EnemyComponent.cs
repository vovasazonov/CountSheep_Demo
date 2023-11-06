using Osyacat.Ecs.Component.Component;
using Osyacat.Ecs.Component.Event;
using UnityEngine;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Enemy
{
    [Component, EventComponent]
    public class EnemyComponent
    {
        public Vector3 LeftPoint;
        public Vector3 RightPoint;
    }
}