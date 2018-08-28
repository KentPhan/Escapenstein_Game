using StickyHandGame_C9_RP7.Source.Components.Collision;
using StickyHandGame_C9_RP7.Source.Entities.Components;

namespace StickyHandGame_C9_RP7.Source.Engine
{
    public class CollisionInfo
    {
        public CollisionComponent CollisionComponent { get; set; }
        public Side Side { get; set; }

        public CollisionInfo(CollisionComponent collisionComponent, Side side)
        {
            CollisionComponent = collisionComponent;
            Side = side;
        }
    }
}
