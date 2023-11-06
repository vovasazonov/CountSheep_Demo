using System;
using UnityEngine;

namespace Project.CoreDomain.Services.View
{
    internal class ContentDisposableView<T> : DisposableView<T> where T : MonoBehaviour
    {
        private readonly IDisposable _contentKeeper;

        public ContentDisposableView(T value, IDisposable contentKeeper) : base(value)
        {
            _contentKeeper = contentKeeper;
        }

        public override void Dispose()
        {
            Destroy();
            _contentKeeper.Dispose();
        }
    }
}