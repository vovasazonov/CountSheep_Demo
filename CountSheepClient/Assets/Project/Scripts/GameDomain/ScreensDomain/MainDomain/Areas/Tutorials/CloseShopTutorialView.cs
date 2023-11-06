using Project.CoreDomain.Services.Analytics;
using Project.CoreDomain.Services.Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Tutorials
{
    public class CloseShopTutorialView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject _button;
        private IDataStorageService _dataStorageService;
        private PrimitiveData<bool> _isTutorialCompletedData;

        public static string DataKey => "close_shop_tutorial";

        [Inject]
        private void Constructor(IDataStorageService dataStorageService)
        {
            _dataStorageService = dataStorageService;
        }

        private void OnEnable()
        {
            _image.raycastTarget = false;
            _isTutorialCompletedData = _dataStorageService.Contains(DataKey) ? _dataStorageService.Get<PrimitiveData<bool>>(DataKey) : _dataStorageService.Create<PrimitiveData<bool>>(DataKey);

            if (_isTutorialCompletedData.Value)
            {
                gameObject.SetActive(false);
            }
        }

        public void CompleteTutorial()
        {
            if (!_isTutorialCompletedData.Value)
            {
                AnalyticService.Instance.Track("finish_tutorial_close_shop");
                _animator.Rebind();
                _animator.Update(0f);
                _animator.Play("Idle");
                _isTutorialCompletedData.Value = true;
                _image.raycastTarget = false;
                gameObject.SetActive(false);
            }
        }

        public void StartTutorial()
        {
            if (!_isTutorialCompletedData.Value)
            {
                AnalyticService.Instance.Track("start_tutorial_close_shop");
                _button.transform.SetSiblingIndex(transform.GetSiblingIndex());
                _image.raycastTarget = true;
                _animator.Rebind();
                _animator.Update(0f);
                _animator.Play("ToMergeTutorial");
            }
        }
    }
}