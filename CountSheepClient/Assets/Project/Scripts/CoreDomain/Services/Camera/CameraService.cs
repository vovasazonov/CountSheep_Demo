using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.CoreDomain.Services.Camera
{
    public class CameraService : ICameraService, ITaskAsyncInitializable
    {
        public ICamera Camera { get; private set; }

        public Vector2 ConvertScreenToWorldPosition(Vector2 screenPosition)
        {
            return Camera.UnityCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, -1 * Camera.UnityCamera.transform.position.z));
        }

        public UniTask InitializeAsync()
        {
            if (UnityEngine.Camera.main != null)
            {
                Camera = UnityEngine.Camera.main.GetComponent<CameraView>();
            }
            else
            {
                throw new NullReferenceException();
            }
            
            return UniTask.CompletedTask;
        }
    }
}