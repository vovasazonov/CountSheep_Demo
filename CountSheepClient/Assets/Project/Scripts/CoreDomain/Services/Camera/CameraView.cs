using UnityEngine;

namespace Project.CoreDomain.Services.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraView : MonoBehaviour, ICamera
    {
        public UnityEngine.Camera UnityCamera { get; private set; }

        private void Awake()
        {
            UnityCamera = GetComponent<UnityEngine.Camera>();
        }
    }
}