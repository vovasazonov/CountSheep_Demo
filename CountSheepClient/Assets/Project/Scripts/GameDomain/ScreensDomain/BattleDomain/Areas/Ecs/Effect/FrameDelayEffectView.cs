using UnityEngine;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Effect
{
    public class FrameDelayEffectView : EffectView
    {
        private const int _delayFrames = 1;
        private int _startedEffectFrame;
        private int _currentFrame;
        
        public override void PlayEffect()
        {
            _startedEffectFrame = Time.frameCount;
            _currentFrame = _startedEffectFrame;
        }

        public override bool IsEffecting()
        {
            _currentFrame = Time.frameCount;
            return _currentFrame - _startedEffectFrame < _delayFrames;
        }
    }
}