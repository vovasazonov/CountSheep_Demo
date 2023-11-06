namespace Project.CoreDomain.Services.View
{
    public interface IViewFactory<out T>
    {
        IDisposableView<T> Create();
    }
}