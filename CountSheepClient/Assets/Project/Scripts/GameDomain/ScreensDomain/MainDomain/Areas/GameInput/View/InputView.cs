using System;
using Project.CoreDomain.Services.Screen;
using Project.Scripts.GameDomain.ScreensDomain.MainDomain.Areas.GameInput.Model;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.GameDomain.ScreensDomain.MainDomain.Areas.GameInput.View
{
    public class InputView : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Image _image;
        private IInputModelUpdater _inputModel;
        private IScreensService _screensService;

        [Inject]
        private void Constructor(IInputModelUpdater inputModel, IScreensService screensService)
        {
            _inputModel = inputModel;
            _screensService = screensService;
        }

        private void OnEnable()
        {
            _screensService.Switched += OnSwitched;
        }

        private void OnSwitched()
        {
            _image.enabled = _screensService.Current == "BattleScreen";
        }

        private void LateUpdate()
        {
            _inputModel.IsPointerDown = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _inputModel.IsPointerDown = true;
        }
    }
}