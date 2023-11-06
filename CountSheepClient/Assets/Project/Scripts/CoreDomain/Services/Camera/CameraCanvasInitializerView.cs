using System;
using UnityEngine;
using Zenject;

namespace Project.CoreDomain.Services.Camera
{
    [RequireComponent(typeof(Canvas))]
    public class CameraCanvasInitializerView : MonoBehaviour
    {
        [SerializeField] private CameraType _cameraType;
        
        private ICameraService _cameraService;

        [Inject]
        private void Construct(ICameraService cameraService)
        {
            _cameraService = cameraService;
        }

        private void Start()
        {
            var canvas = GetComponent<Canvas>();
            
            switch (_cameraType)
            {
                case CameraType.Perspective:
                    canvas.worldCamera = _cameraService.Camera.UnityCamera;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private enum CameraType
        {
            Perspective
        }
    }
}