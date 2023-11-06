namespace Osyacat.Ecs.Matcher
{
    public interface IMatcher
    {
        IMatcher Has<TComponent>() where TComponent : class;
        IMatcher None<TComponent>() where TComponent : class;
    }
}