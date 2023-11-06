using Project.CoreDomain.Services.Screen;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.MergeLocation
{
    public class MergeLocationButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private string _screenName;
        private IScreensService _screensService;

        [Inject]
        private void Constructor(IScreensService screensService)
        {
            _screensService = screensService;
        }
        
        private void OnEnable()
        {
            _button.onClick.AddListener(SwitchLocations);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(SwitchLocations);
        }

        private void SwitchLocations()
        {
            _screensService.SwitchAsync(_screenName);
        }
    }
}