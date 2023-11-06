using UnityEngine;
using UnityEngine.UI;

namespace Project.GameDomain.ScreensDomain.MergeDomain.Areas.Shop.View
{
    public class ThanksShopSheepWatchAdView : MonoBehaviour
    {
        [SerializeField] private Text _messageText;
        private string _textFormat = "You received one {0} for watching the video";
        
        public void SetNameOfSheep(string sheepName)
        {
            _messageText.text = string.Format(_textFormat, sheepName);
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}