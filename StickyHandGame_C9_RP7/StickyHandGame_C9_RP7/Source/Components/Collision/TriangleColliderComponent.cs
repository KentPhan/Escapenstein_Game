using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Entities.Components;
using StickyHandGame_C9_RP7.Source.Entities.Core;

namespace StickyHandGame_C9_RP7.Source.Components.Collision
{
    public class TriangleColliderComponent : CollisionComponent
    {
        public Vector2 Point1 { get; set; }
        public Vector2 Point2 { get; set; }
        public Vector2 Point3 { get; set; }


        public TriangleColliderComponent(Entity entity, CollisionLayers layer)
        {
            this.Entity = entity;
            this.Layer = layer;
            this.BoundaryType = CollisionBoundaryType.Triangle;
        }

        public override void DebugDraw(GameTime gameTime)
        {
            throw new System.NotImplementedException();
        }
    }
}
