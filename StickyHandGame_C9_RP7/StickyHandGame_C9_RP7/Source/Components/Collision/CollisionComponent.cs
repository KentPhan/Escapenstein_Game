using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Entities.Core;

namespace StickyHandGame_C9_RP7.Source.Entities.Components
{
    public abstract class CollisionComponent
    {
        public enum CollisionBoundaryType
        {
            Square,
            Triangle
        }

        public Entity Entity { get; protected set; }
        public Layers Layer { get; set; }
        public Tags Tag { get; set; }

        public CollisionBoundaryType BoundaryType { get; protected set; }

        public abstract void DebugDraw(GameTime gameTime);
    }
}
