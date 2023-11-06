using Osyacat.Ecs.Component.Event;
using UnityEngine;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Position
{
    public class PositionListener : ComponentListener<PositionComponent>
    {
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        public override void OnChanged(PositionComponent component)
        {
            _transform.position = component.Value;
        }
    }
}