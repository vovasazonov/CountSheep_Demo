using UnityEngine;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Effect
{
    public class AnimationEffectView : EffectView
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private string _state;
        [SerializeField] private int _layer;
        [SerializeField] private float _waitAnimationPercent = 1f;

        public override void PlayEffect()
        {
            _animator.Play(_state, _layer, 0f);
        }

        public override bool IsEffecting()
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(_layer);
            return stateInfo.IsName(_state) && stateInfo.normalizedTime < _waitAnimationPercent;
        }
    }
}