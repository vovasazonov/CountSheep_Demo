using UnityEngine;

namespace Project.CoreDomain.Services.View
{
    internal class BuiltInDisposableView<T> : DisposableView<T> where T : MonoBehaviour
    {
        public BuiltInDisposableView(T value) : base(value)
        {
        }

        public override void Dispose()
        {
            Destroy();
        }
    }
}