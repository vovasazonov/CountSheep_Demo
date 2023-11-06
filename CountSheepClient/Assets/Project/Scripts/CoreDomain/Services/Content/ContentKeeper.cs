using UnityEngine;

namespace Project.CoreDomain.Services.Content
{
    internal class ContentKeeper<T> : IContentKeeper<T>
    {
        private readonly ContentService _contentService;
        private readonly string _contentId;
        private bool _isDisposed;

        public T Value { get; }

        public ContentKeeper(ContentService contentService, string contentId, T value)
        {
            _contentService = contentService;
            _contentId = contentId;
            Value = value;
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _contentService.Unload(_contentId);
                _isDisposed = true;
            }
        }

#if UNITY_EDITOR
        ~ContentKeeper()
        {
            if (Application.isPlaying)
            {
                if (!_isDisposed)
                {
                    Debug.LogError($"{nameof(ContentKeeper<T>)}: Content must to be disposed when it is not in using anymore. Id content: {_contentId}");
                }
            }
        }
#endif
    }
}