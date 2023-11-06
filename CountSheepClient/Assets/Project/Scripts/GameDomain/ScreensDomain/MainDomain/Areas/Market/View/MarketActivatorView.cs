using Project.GameDomain.ScreensDomain.MainDomain.Areas.Market.Model;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Market.View
{
    public class MarketActivatorView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private bool _isOpenMarket;
        private IMarketModel _marketModel;

        [Inject]
        private void Constructor(IMarketModel marketModel)
        {
            _marketModel = marketModel;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isOpenMarket)
            {
                _marketModel.Open();
            }
            else
            {
                _marketModel.Close();
            }
        }
    }
}