using System;
using UnityEngine;
using UnityEngine.UI;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Market.View
{
    public class MarketItemView : MonoBehaviour, IMarketItemView
    {
        [SerializeField] private Button _buyButton;
        [SerializeField] private GameObject _loadingPriceCircle;
        [SerializeField] private Text _priceText;
        [SerializeField] private Text _amountCoinsText;
        [SerializeField] private GameObject _adsOffer;

        public event Action BuyClicked;

        public bool IsIncludeAds
        {
            set
            {
                if (_adsOffer == null)
                {
                    return;
                }
                _adsOffer.SetActive(value);
            }
        }

        public int AmountCoins
        {
            set => _amountCoinsText.text = $"{value:n0}";
        }

        public string Price
        {
            set => _priceText.text = value;
        }

        public bool IsAvailable
        {
            set
            {
                _buyButton.interactable = value;
                _loadingPriceCircle.SetActive(!value);
                _priceText.gameObject.SetActive(value);
            }
        }

        private void OnEnable()
        {
            _buyButton.onClick.AddListener(OnBuyClicked);
        }

        private void OnDisable()
        {
            _buyButton.onClick.RemoveListener(OnBuyClicked);
        }

        private void OnBuyClicked()
        {
            BuyClicked?.Invoke();
        }
    }
}