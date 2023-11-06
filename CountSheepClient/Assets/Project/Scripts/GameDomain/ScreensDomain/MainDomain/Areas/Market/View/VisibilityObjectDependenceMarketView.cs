using System;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Market.Model;
using UnityEngine;
using Zenject;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Market.View
{
    public class VisibilityObjectDependenceMarketView : MonoBehaviour
    {
        private IMarketModel _marketModel;

        [Inject]
        private void Constructor(IMarketModel marketModel)
        {
            _marketModel = marketModel;
        }

        private void Awake()
        {
            _marketModel.VisibleUpdated += OnVisiblityUpdated;
        }

        private void OnDestroy()
        {
            _marketModel.VisibleUpdated -= OnVisiblityUpdated;
        }

        private void OnVisiblityUpdated()
        {
            gameObject.SetActive(!_marketModel.IsOpen);
        }
    }
}