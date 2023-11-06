namespace Osyacat.Ecs.Entity
{
    public interface IEntityView
    {
        void Initialize(IEntity entity);
        void UnInitialize();
    }
}