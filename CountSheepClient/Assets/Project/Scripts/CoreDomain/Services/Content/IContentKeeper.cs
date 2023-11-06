using System;

namespace Project.CoreDomain.Services.Content
{
    public interface IContentKeeper<out T> : IDisposable
    {
        T Value { get; }
    }
}