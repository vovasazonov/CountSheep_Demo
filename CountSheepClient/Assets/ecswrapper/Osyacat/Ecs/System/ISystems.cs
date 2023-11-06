namespace Osyacat.Ecs.System
{
    public interface  ISystems : IInitializeSystem, IUpdateSystem, ILateUpdateSystem, IDestroySystem, IFixedUpdateSystem, IBeforeUpdateSystem
    {
        void Add<TSystem>(TSystem system) where TSystem : ISystem;
    }
}