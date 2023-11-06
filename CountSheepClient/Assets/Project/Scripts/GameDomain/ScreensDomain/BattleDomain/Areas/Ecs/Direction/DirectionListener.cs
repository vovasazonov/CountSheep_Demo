using Osyacat.Ecs.Component.Event;
using UnityEngine;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Direction
{
    public class DirectionListener : ComponentListener<DirectionComponent>
    {
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        public override void OnChanged(DirectionComponent component)
        {
            var rotation = _transform.rotation;
            var flipAngle = component.Value == Vector3.right ? 0f : 180f;
            rotation.eulerAngles = new Vector3(0, flipAngle, 0);
            _transform.rotation = rotation;
        }
    }
}