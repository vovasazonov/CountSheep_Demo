using Osyacat.Ecs.Component.Event;
using UnityEngine;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Player
{
    public class PlayerSideComponentListener : ComponentListener<PlayerSideComponent>
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private string _leftSideLayer = "LeftSide";
        private string _rightSideLayer = "RightSide";
        
        public override void OnChanged(PlayerSideComponent component)
        {
            _spriteRenderer.sortingLayerName = component.IsLeft ? _leftSideLayer : _rightSideLayer;
        }
    }
}