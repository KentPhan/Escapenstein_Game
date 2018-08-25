using StickyHandGame_C9_RP7.Source.Entities.Components;
using StickyHandGame_C9_RP7.Source.Entities.Core;

namespace StickyHandGame_C9_RP7.Source.Components.Collision
{


    public enum Side
    {
        Left,
        Right,
        Top,
        Bottom,
        None,
    }


    public class BoxColliderComponent : CollisionComponent
    {
        public float Width { get; private set; }
        public float Height { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoxColliderComponent"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="layer">The layer.</param>
        public BoxColliderComponent(Entity entity, float width, float height, CollisionLayers layer)
        {
            this.Entity = entity;
            this.Width = width;
            this.Height = height;
            this.Layer = layer;
        }
    }
}
