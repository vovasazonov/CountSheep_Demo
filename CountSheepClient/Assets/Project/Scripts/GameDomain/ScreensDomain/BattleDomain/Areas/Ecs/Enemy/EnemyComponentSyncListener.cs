using Osyacat.Ecs.Component.Event;
using UnityEngine;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Enemy
{
    public class EnemyComponentSyncListener : ComponentListener<EnemyComponent>
    {
        [SerializeField] private Transform _leftPoint;
        [SerializeField] private Transform _rightPoint;
        
        public override void OnChanged(EnemyComponent component)
        {
            component.LeftPoint = _leftPoint.position;
            component.RightPoint = _rightPoint.position;
        }
    }
}