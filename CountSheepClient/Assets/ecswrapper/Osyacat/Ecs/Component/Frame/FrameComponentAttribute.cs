using System;

namespace Osyacat.Ecs.Component.Frame
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FrameComponentAttribute : Attribute
    {
        public bool IsDestroyEntity { get; }
        public bool IsClearBeforeUpdate { get; }
        
        public FrameComponentAttribute() { }
        
        public FrameComponentAttribute(bool isDestroyEntity = false, bool isClearBeforeUpdate = false)
        {
            IsDestroyEntity = isDestroyEntity;
            IsClearBeforeUpdate = isClearBeforeUpdate;
        }
    }
}