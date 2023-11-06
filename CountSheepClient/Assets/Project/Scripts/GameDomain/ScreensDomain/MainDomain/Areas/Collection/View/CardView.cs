using UnityEngine;
using UnityEngine.UI;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Collection.View
{
    public class CardView : MonoBehaviour, ICardView
    {
        [SerializeField] private Image _image;
        [SerializeField] private Text _nameText;

        public bool IsVisible
        {
            set
            {
                _image.color = value ? Color.white : Color.black;
                _nameText.text = value ? Name.ToUpper() : "???";
            }
        }

        public Sprite Sprite
        {
            set => _image.sprite = value;
        }

        public string Name
        {
            get;
            set;
        }
    }
}