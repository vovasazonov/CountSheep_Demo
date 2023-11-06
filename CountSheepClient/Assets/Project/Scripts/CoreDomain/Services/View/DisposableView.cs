using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Project.CoreDomain.Services.View
{
    internal abstract class DisposableView<T> : IDisposableView<T> where T : MonoBehaviour
    {
        private bool _isDestroyed;
        
        public T Value { get; private set; }

        protected DisposableView(T value)
        {
            Value = value;
        }

        public abstract void Dispose();

        protected void Destroy()
        {
            if (!_isDestroyed)
            {
                if (Value is IDisposable disposable)
                {
                    disposable.Dispose();
                }
                else
                {
                    Object.Destroy(Value.gameObject);
                }

                _isDestroyed = true;
                Value = null;
            }
        }
    }
}