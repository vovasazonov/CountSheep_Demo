using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.GameDomain.ScreensDomain.SplashDomain.Splash.View
{
    public class SplashView : MonoBehaviour
    {
        [SerializeField] private string _progressFormat;
        [SerializeField] private Text _progressText;
        [SerializeField] private Animator _animator;
        private int _progress;

        private void Update()
        {
            if (_progress != 100)
            {
                _progress++;
                _progressText.text = string.Format(_progressFormat, _progress);
            }
        }

        public async UniTask Hide()
        {
            while (_progress != 100)
            {
                await UniTask.DelayFrame(1);
            }
            
            _animator.Play("HideSplash");
            await UniTask.Delay(1500);
        }
    }
}