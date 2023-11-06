using Osyacat.Ecs.Component.Event;
using UnityEngine;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Animation
{
    public class AnimationComponentListener : ComponentListener<AnimationComponent>
    {
        [SerializeField] private Animator _animator;
        
        public override void OnChanged(AnimationComponent component)
        {
            _animator.Play(component.State);
        }
    }
}