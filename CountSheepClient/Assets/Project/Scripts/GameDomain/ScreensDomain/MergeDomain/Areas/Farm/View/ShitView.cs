using Cysharp.Threading.Tasks;
using Project.CoreDomain.Services.Audio;
using Project.CoreDomain.Services.Audio.Configs.AudioPlayer;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Currency.Model;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Model;
using UnityEngine;
using Zenject;

namespace Project.GameDomain.ScreensDomain.MergeDomain.Areas.Farm.View
{
    public class ShitView : MonoBehaviour
    {
        [SerializeField] private int _destroyAfterSeconds;
        [SerializeField] private Animator _animator;
        [SerializeField] private AudioPlayerConfig _collectSound;
        [SerializeField] private TextMesh _coinsAmountText;
        private int _sheepId;
        private bool _isDestroyed;
        private bool _isGotMoney;
        private ICurrencyModel _currencyModel;
        private IAudioService _audioService;
        private IFarmModel _farmModel;

        [Inject]
        private void Constructor(
            ICurrencyModel currencyModel,
            IAudioService audioService,
            IFarmModel farmModel
        )
        {
            _currencyModel = currencyModel;
            _audioService = audioService;
            _farmModel = farmModel;
        }

        public void Initialize(int sheepId)
        {
            _sheepId = sheepId;
        }

        private void OnMouseUpAsButton()
        {
            DestroyAsync();
        }

        private void OnEnable()
        {
            StartDestroy();
        }

        private void OnDestroy()
        {
            _isDestroyed = true;
        }

        private async UniTask DestroyAsync()
        {
            if (!_isDestroyed)
            {
                if (!_isGotMoney)
                {
                    _isGotMoney = true;
                    
                    if (_farmModel.Animals.ContainsKey(_sheepId))
                    {
                        var sheep = _farmModel.Animals[_sheepId];
                        var coins = _farmModel.GetCoinsPerClick(sheep);
                        _currencyModel.Amount += coins;
                        _coinsAmountText.text = "+" + coins;
                    }
                    
                    _animator.Rebind();
                    _animator.Update(0f);
                    _animator.Play("ShitDisappear");
                    _audioService.Sound.PlayImmediately(_collectSound);
                }

                await UniTask.Delay(2000);
                if (!_isDestroyed)
                {
                    Destroy(gameObject);
                }
            }
        }

        private async UniTask StartDestroy()
        {
            await UniTask.Delay(_destroyAfterSeconds * 1000);
            await DestroyAsync();
        }
    }
}