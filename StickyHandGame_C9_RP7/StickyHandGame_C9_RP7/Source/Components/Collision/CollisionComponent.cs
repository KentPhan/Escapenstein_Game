using StickyHandGame_C9_RP7.Source.Entities.Core;

namespace StickyHandGame_C9_RP7.Source.Entities.Components
{
    public abstract class CollisionComponent
    {
        public Entity Entity { get; protected set; }
        public CollisionLayers Layer { get; set; }
    }
}
