namespace Osyacat.Ecs.System
{
    public interface ILateUpdateSystem : ISystem
    {
        void LateUpdate();
    }
}