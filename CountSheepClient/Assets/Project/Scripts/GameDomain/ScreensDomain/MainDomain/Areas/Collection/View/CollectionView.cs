using System;
using Project.CoreDomain.Services.Analytics;
using Project.CoreDomain.Services.View;
using UnityEngine;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Collection.View
{
    public class CollectionView : MonoBehaviour, ICollectionView
    {
        [SerializeField] private CardView _cardPrefab;
        [SerializeField] private Transform _cardsContainer;
        private IViewService _viewService;

        public ICardView Create()
        {
            return Instantiate(_cardPrefab, _cardsContainer);
        }

        private void OnEnable()
        {
            AnalyticService.Instance.Track("collection_open");
        }

        private void OnDisable()
        {
            AnalyticService.Instance.Track("collection_close");
        }
    }
}