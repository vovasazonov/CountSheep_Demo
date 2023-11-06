using System;

namespace Project.CoreDomain.Services.View
{
    public interface IDisposableView<out T> : IDisposable
    {
        T Value { get; }
    }
}