using UnityEngine;

namespace Project.CoreDomain.Services.Camera
{
    public interface ICameraService
    {
        ICamera Camera { get; }
        
        Vector2 ConvertScreenToWorldPosition(Vector2 screenPosition);
    }
}