using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Entities.Components;

namespace StickyHandGame_C9_RP7.Source.Engine
{
    public class CollisionInfo
    {
        public CollisionComponent CollisionComponent { get; set; }
        public Vector2 NormalVector { get; set; }
        public Vector2 PointOfCollision { get; set; }

        public CollisionInfo(CollisionComponent collisionComponent, Vector2 pointOfCollision, Vector2 normalVector)
        {
            CollisionComponent = collisionComponent;
            this.PointOfCollision = pointOfCollision;
            this.NormalVector = normalVector;
        }
    }
}
