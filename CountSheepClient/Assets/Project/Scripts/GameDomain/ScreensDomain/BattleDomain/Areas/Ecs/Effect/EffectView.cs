using UnityEngine;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Effect
{
    public abstract class EffectView : MonoBehaviour
    {
        public string Id;
        
        public abstract void PlayEffect();
        public abstract bool IsEffecting();
    }
}