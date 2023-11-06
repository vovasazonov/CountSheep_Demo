using Project.CoreDomain.Services.View;
using UnityEngine;

namespace Project.GameDomain.ScreensDomain.MergeDomain.Areas.Farm.View
{
    public class ShitController
    {
        private readonly int _sheepId;
        private readonly ShitView _shitView;
        private readonly IViewService _viewService;
        private readonly AnimalView _animalView;
        private float _createPerSeconds = 20;
        private float _leftSeconds;

        public ShitController(int sheepId, ShitView shitView, IViewService viewService, AnimalView animalView)
        {
            _sheepId = sheepId;
            _shitView = shitView;
            _viewService = viewService;
            _animalView = animalView;
            _leftSeconds = Random.Range(6f, _createPerSeconds);
        }

        public void Update()
        {
            _leftSeconds -= Time.deltaTime;
            
            if (_leftSeconds < 0f)
            {
                _leftSeconds = Random.Range(6f, _createPerSeconds);
                Create();
            }
        }

        public void Create()
        {
            var shitView = _viewService.Create(_shitView);
            shitView.Value.transform.position = _animalView.transform.position;
            shitView.Value.Initialize(_sheepId);
        }
    }
}