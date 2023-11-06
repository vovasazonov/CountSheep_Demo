using Osyacat.Ecs.Component.Event;
using UnityEngine;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Player
{
    public class OrderPlayerComponentListener : ComponentListener<PlayerComponent>
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private static int _counter;
        
        public override void OnChanged(PlayerComponent component)
        {
            _spriteRenderer.sortingOrder = _counter;
            _counter++;
        }
    }
}