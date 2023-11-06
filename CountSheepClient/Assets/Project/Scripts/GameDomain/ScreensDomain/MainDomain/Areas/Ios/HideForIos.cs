using UnityEngine;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Ios
{
    public class HideForIos : MonoBehaviour
    {
        private void Awake()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                gameObject.SetActive(false);
            }
        }
    }
}