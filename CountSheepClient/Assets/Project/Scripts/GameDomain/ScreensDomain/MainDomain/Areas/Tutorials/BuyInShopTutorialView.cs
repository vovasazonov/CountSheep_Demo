using Project.CoreDomain.Services.Analytics;
using Project.CoreDomain.Services.Data;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Currency.Model;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Model;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Tutorials
{
    public class BuyInShopTutorialView : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Image _image;
        [SerializeField] private CloseShopTutorialView _closeShopTutorialView;
        private IDataStorageService _dataStorageService;
        private IFarmModel _farmModel;
        private PrimitiveData<bool> _isTutorialCompletedData;
        private ICurrencyModel _currencyModel;

        public static string TutorialDataKey => "buy_in_shop_tutorial";

        [Inject]
        private void Constructor(IDataStorageService dataStorageService, IFarmModel farmModel, ICurrencyModel currencyModel)
        {
            _currencyModel = currencyModel;
            _dataStorageService = dataStorageService;
            _farmModel = farmModel;
        }

        private void OnEnable()
        {
            _image.raycastTarget = false;
            _isTutorialCompletedData = _dataStorageService.Contains(TutorialDataKey)
                ? _dataStorageService.Get<PrimitiveData<bool>>(TutorialDataKey)
                : _dataStorageService.Create<PrimitiveData<bool>>(TutorialDataKey);

            _farmModel.Added += OnAdded;

            if (_isTutorialCompletedData.Value)
            {
                gameObject.SetActive(false);
            }
            else
            {
                StartTutorial();
            }
        }

        private void OnDisable()
        {
            _farmModel.Added -= OnAdded;
        }

        private void OnAdded(int obj)
        {
            CompleteTutorial();
        }

        public void CompleteTutorial()
        {
            if (!_isTutorialCompletedData.Value)
            {
                AnalyticService.Instance.Track("finish_tutorial_buy_in_shop");
                _animator.Rebind();
                _animator.Update(0f);
                _animator.Play("Idle");
                _isTutorialCompletedData.Value = true;
                _image.raycastTarget = false;
                gameObject.SetActive(false);
                _closeShopTutorialView.StartTutorial();
            }
        }

        private void StartTutorial()
        {
            AnalyticService.Instance.Track("start_tutorial_buy_in_shop");
            _currencyModel.Amount += 100;
            _image.raycastTarget = true;
            _animator.Rebind();
            _animator.Update(0f);
            _animator.Play("ToMergeTutorial");
        }
    }
}