using Project.CoreDomain.Services.Audio;
using Project.CoreDomain.Services.Audio.Configs.AudioPlayer;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Currency.Model;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Model;
using UnityEngine;
using Zenject;

namespace Project.GameDomain.ScreensDomain.MergeDomain.Areas.AreaTablo
{
    public class AreaTabloView : MonoBehaviour
    {
        [SerializeField] private TextMesh _animalsAmountText;
        [SerializeField] private GameObject _expandButton;
        [SerializeField] private TextMesh _costText;
        [SerializeField] private AudioPlayerConfig _buySoundConfig;
        private IFarmModel _farmModel;
        private IAudioService _audioService;
        private ICurrencyModel _currencyModel;

        [Inject]
        private void Constructor(IFarmModel farmModel, IAudioService audioService, ICurrencyModel currencyModel)
        {
            _farmModel = farmModel;
            _audioService = audioService;
            _currencyModel = currencyModel;
        }

        private void OnEnable()
        {
            _farmModel.Added += OnAmountSheepUpdated;
            _farmModel.Removed += OnAmountSheepUpdated;
            _farmModel.AreaUpdated += OnAreaUpdated;
            _currencyModel.Updated += OnCurrencyUpdated;

            UpdateVisibilityTabloButton();
            UpdateAmountAnimals();
        }

        private void OnDisable()
        {
            _farmModel.Added -= OnAmountSheepUpdated;
            _farmModel.Removed -= OnAmountSheepUpdated;
            _farmModel.AreaUpdated -= OnAreaUpdated;
            _currencyModel.Updated -= OnCurrencyUpdated;
        }

        private void OnCurrencyUpdated()
        {
            UpdateVisibilityTabloButton();
        }

        private void OnAreaUpdated()
        {
            UpdateAmountAnimals();
            UpdateVisibilityTabloButton();
        }

        private void OnAmountSheepUpdated(int animalId)
        {
            UpdateAmountAnimals();
            UpdateVisibilityTabloButton();
        }

        private void UpdateAmountAnimals()
        {
            _animalsAmountText.text = $"{_farmModel.Animals.Count}/{_farmModel.GetMaxAnimalInFarm()}";
        }

        private void UpdateVisibilityTabloButton()
        {
            _expandButton.SetActive(CanBuyExtraArea());
            _costText.text = _farmModel.GetCostNextArea().ToString();
        }

        private void OnMouseUpAsButton()
        {
            if (CanBuyExtraArea())
            {
                _audioService.Sound.PlayImmediately(_buySoundConfig);
                _currencyModel.Amount -= _farmModel.GetCostNextArea();
                _farmModel.IncreaseLevelArea();
            }
        }

        private bool CanBuyExtraArea()
        {
            return _farmModel.HasIncreaseAreaLevel() && _farmModel.Animals.Count / (float)_farmModel.GetMaxAnimalInFarm() > 0.5f && _farmModel.GetCostNextArea() <= _currencyModel.Amount;
        }
    }
}