using UnityEngine;
using Object = UnityEngine.Object;

namespace Project.CoreDomain.Services.View
{
    public class BuiltInViewFactory<T> : IViewFactory<T> where T : MonoBehaviour
    {
        private readonly T _prefab;
        private readonly Transform _parent;

        public BuiltInViewFactory(T prefab, Transform parent)
        {
            _prefab = prefab;
            _parent = parent;
        }

        public BuiltInViewFactory(T prefab)
        {
            _prefab = prefab;
        }

        public IDisposableView<T> Create()
        {
            T view = _parent == null ? Object.Instantiate(_prefab) : Object.Instantiate(_prefab, _parent);
            return new BuiltInDisposableView<T>(view);
        }
    }
}