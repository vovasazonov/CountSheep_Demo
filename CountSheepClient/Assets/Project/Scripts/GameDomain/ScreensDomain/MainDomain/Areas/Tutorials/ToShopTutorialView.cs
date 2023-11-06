using Project.CoreDomain.Services.Analytics;
using Project.CoreDomain.Services.Data;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Model;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Rule.Presenter;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Tutorials
{
    public class ToShopTutorialView : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Image _image;
        private IDataStorageService _dataStorageService;
        private IFarmModel _farmModel;
        private PrimitiveData<bool> _isTutorialCompletedData;

        [Inject]
        private void Constructor(IDataStorageService dataStorageService, IFarmModel farmModel)
        {
            _dataStorageService = dataStorageService;
            _farmModel = farmModel;
        }

        private void OnEnable()
        {
            _image.raycastTarget = false;
            var dataKey = "to_shop_tutorial";
            _isTutorialCompletedData = _dataStorageService.Contains(dataKey) ? _dataStorageService.Get<PrimitiveData<bool>>(dataKey) : _dataStorageService.Create<PrimitiveData<bool>>(dataKey);

            if (_isTutorialCompletedData.Value)
            {
                gameObject.SetActive(false);
            }
            else
            {
                StartTutorial();
            }
        }

        public void CompleteTutorial()
        {
            if (!_isTutorialCompletedData.Value)
            {
                AnalyticService.Instance.Track("finish_tutorial_open_shop");
                _animator.Rebind();
                _animator.Update(0f);
                _animator.Play("Idle");
                _isTutorialCompletedData.Value = true;
                _image.raycastTarget = false;
                gameObject.SetActive(false);
            }
        }

        private void StartTutorial()
        {
            AnalyticService.Instance.Track("start_tutorial_open_shop");
            _image.raycastTarget = true;
            _animator.Rebind();
            _animator.Update(0f);
            _animator.Play("ToMergeTutorial");
        }
    }
}