using DG.Tweening;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Currency.Model;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Currency.View
{
    public class CurrencyView : MonoBehaviour
    {
        [SerializeField] private Text _currencyAmountText;

        private ICurrencyModel _currencyModel;

        [Inject]
        private void Constructor(ICurrencyModel currencyModel)
        {
            _currencyModel = currencyModel;
        }

        private void OnEnable()
        {
            RenderCurrency();
            _currencyModel.Updated += OnUpdated;
        }

        private void OnDisable()
        {
            _currencyModel.Updated -= OnUpdated;
        }

        private void OnUpdated()
        {
            RenderCurrency();
        }

        private void RenderCurrency()
        {
            _currencyAmountText.text =  $"{_currencyModel.Amount:n0}";
            _currencyAmountText.transform.DOPunchScale(new Vector3(0.1f,1f,0), 0.5f, 0).OnComplete(() => _currencyAmountText.transform.localScale = new Vector3(5, 5, 1));
        }
    }
}