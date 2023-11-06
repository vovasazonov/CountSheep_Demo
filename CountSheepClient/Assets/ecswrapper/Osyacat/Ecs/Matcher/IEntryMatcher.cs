namespace Osyacat.Ecs.Matcher
{
    public interface IEntryMatcher
    {
        IMatcher Has<TComponent>() where TComponent : class;
    }
}