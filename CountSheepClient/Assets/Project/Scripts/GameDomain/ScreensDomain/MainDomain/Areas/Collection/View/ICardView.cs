using UnityEngine;

namespace Project.GameDomain.ScreensDomain.MainDomain.Areas.Collection.View
{
    public interface ICardView
    {
        bool IsVisible { set; }
        Sprite Sprite { set; }
        string Name { set; }
    }
}