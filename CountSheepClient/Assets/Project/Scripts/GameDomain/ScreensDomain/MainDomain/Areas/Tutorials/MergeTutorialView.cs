using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Project.CoreDomain.Services.Analytics;
using Project.CoreDomain.Services.Data;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Model;
using UnityEngine;
using Zenject;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Tutorials
{
    public class MergeTutorialView : MonoBehaviour
    {
        private IDataStorageService _dataStorageService;
        private IFarmModel _farmModel;
        private PrimitiveData<bool> _isTutorialCompletedData;
        private bool _isActive;
        private TweenerCore<Vector3, Vector3, VectorOptions> _tween;

        [Inject]
        private void Constructor(IDataStorageService dataStorageService, IFarmModel farmModel)
        {
            _dataStorageService = dataStorageService;
            _farmModel = farmModel;
        }

        private void OnEnable()
        {
            var dataKey = "merge_tutorial";
            _isTutorialCompletedData = _dataStorageService.Contains(dataKey) ? _dataStorageService.Get<PrimitiveData<bool>>(dataKey) : _dataStorageService.Create<PrimitiveData<bool>>(dataKey);

            _farmModel.Updated += OnSomeAnimalUpdated;

            if (_isTutorialCompletedData.Value)
            {
                gameObject.SetActive(false);
            }
            else
            {
                transform.position = new Vector3(999, 999);
            }
        }

        private void Update()
        {
            if (!_isActive && !_isTutorialCompletedData.Value)
            {
                var isShopTutorialFinished = _dataStorageService.Contains(CloseShopTutorialView.DataKey) && _dataStorageService.Get<PrimitiveData<bool>>(CloseShopTutorialView.DataKey).Value;

                if (isShopTutorialFinished)
                {
                    StartTutorial();
                }
            }
        }

        private void OnDisable()
        {
            _farmModel.Updated -= OnSomeAnimalUpdated;
        }

        private void OnSomeAnimalUpdated(int obj)
        {
            CompleteTutorial();
        }

        private void CompleteTutorial()
        {
            if (!_isTutorialCompletedData.Value)
            {
                AnalyticService.Instance.Track("finish_tutorial_merge_animal");
                _isTutorialCompletedData.Value = true;
                _tween?.Kill();
                gameObject.SetActive(false);
            }
        }

        private void StartTutorial()
        {
            if (!_isTutorialCompletedData.Value)
            {
                AnalyticService.Instance.Track("start_tutorial_merge_animal");
                _isActive = true;
                StartTween();
            }
        }

        private void StartTween()
        {
            var players = GameObject.FindGameObjectsWithTag("Player");
            transform.position = players[0].transform.position;
            _tween = transform.DOMove(players[1].transform.position, 2);
            _tween.OnComplete(StartTween);
        }
    }
}