using Project.CoreDomain.Services.Analytics;
using Project.CoreDomain.Services.Data;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Model;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Rule.Presenter;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Tutorials
{
    public class ToMergeAreaTutorialView : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Image _image;
        private IDataStorageService _dataStorageService;
        private IRuleModel _ruleModel;
        private IFarmModel _farmModel;
        private PrimitiveData<bool> _isTutorialCompletedData;

        [Inject]
        private void Constructor(IDataStorageService dataStorageService, IRuleModel ruleModel, IFarmModel farmModel)
        {
            _dataStorageService = dataStorageService;
            _ruleModel = ruleModel;
            _farmModel = farmModel;
        }

        private void OnEnable()
        {
            _image.raycastTarget = false;
            var dataKey = "to_merge_area_tutorial";
            _isTutorialCompletedData = _dataStorageService.Contains(dataKey) ? _dataStorageService.Get<PrimitiveData<bool>>(dataKey) : _dataStorageService.Create<PrimitiveData<bool>>(dataKey);

            _ruleModel.Prepared += OnBattlePrepared;
            if (_isTutorialCompletedData.Value)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            _ruleModel.Prepared -= OnBattlePrepared;
        }

        private void OnBattlePrepared()
        {
            if (!_isTutorialCompletedData.Value && _farmModel.Animals.Count > 0)
            {
                StartTutorial();
            }
        }

        public void CompleteTutorial()
        {
            if (!_isTutorialCompletedData.Value)
            {
                AnalyticService.Instance.Track("finish_tutorial_to_merge_area");
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
            AnalyticService.Instance.Track("start_tutorial_to_merge_area");
            _image.raycastTarget = true;
            _animator.Rebind();
            _animator.Update(0f);
            _animator.Play("ToMergeTutorial");
        }
    }
}