namespace Project.CoreDomain
{
    public interface IKeeper<out T>
    {
        T Value { get; }
    }
}