namespace Osyacat.Ecs.System
{
    public interface IBeforeUpdateSystem : ISystem
    {
        void BeforeUpdate();
    }
}