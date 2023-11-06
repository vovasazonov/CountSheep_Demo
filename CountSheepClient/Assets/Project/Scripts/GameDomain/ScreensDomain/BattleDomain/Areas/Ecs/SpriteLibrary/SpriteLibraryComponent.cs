using Osyacat.Ecs.Component.Component;
using Osyacat.Ecs.Component.Event;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.SpriteLibrary
{
    [Component, EventComponent]
    public class SpriteLibraryComponent
    {
        public string AssetId;
    }
}