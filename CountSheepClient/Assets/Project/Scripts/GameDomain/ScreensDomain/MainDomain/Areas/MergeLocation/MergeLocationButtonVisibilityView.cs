using System;
using Project.CoreDomain.Services.Screen;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Farm.Model;
using Project.GameDomain.ScreensDomain.MainDomain.Areas.Rule.Presenter;
using UnityEngine;
using Zenject;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.MergeLocation
{
    public class MergeLocationButtonVisibilityView : MonoBehaviour
    {
        [SerializeField] private GameObject _importantSign;
        private IScreensService _screensService;
        private IFarmModel _farmModel;
        private IRuleModel _ruleModel;

        [Inject]
        private void Constructor(IScreensService screensService, IFarmModel farmModel, IRuleModel ruleModel)
        {
            _screensService = screensService;
            _farmModel = farmModel;
            _ruleModel = ruleModel;
        }

        private void Start()
        {
            _farmModel.Added += OnAnimalAdded;
        }

        private void OnAnimalAdded(int obj)
        {
            if (_screensService.Current == "BattleScreen")
            {
                _importantSign.SetActive(true);
            }
        }
    }
}