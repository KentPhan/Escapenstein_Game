using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StickyHandGame_C9_RP7.Source.Entities.Components;
using StickyHandGame_C9_RP7.Source.Entities.Core;

namespace StickyHandGame_C9_RP7.Source.Components.Collision
{

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="StickyHandGame_C9_RP7.Source.Entities.Components.CollisionComponent" />
    public class BoxColliderComponent : CollisionComponent
    {
        public float Width { get; private set; }
        public float Height { get; private set; }

        private Texture2D _debugLines;

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
            this.BoundaryType = CollisionBoundaryType.Square;

            _debugLines = new Texture2D(GameManager.Instance.GraphicsDevice, 1, 1);
            _debugLines.SetData<Color>(new Color[] { Color.White });
        }

        public override void DebugDraw(GameTime gameTime)
        {

            GameManager.Instance.SpriteBatch.Draw(_debugLines,
                new Rectangle(
                    (int)Entity.Position.X - (int)this.Width / 2,
                    (int)Entity.Position.Y - (int)this.Height / 2,
                    (int)this.Width,
                    1),
                    null,
                Color.Red, 0, Vector2.Zero, SpriteEffects.None, 0);
            GameManager.Instance.SpriteBatch.Draw(_debugLines,
                new Rectangle(
                    (int)Entity.Position.X - (int)this.Width / 2,
                    (int)Entity.Position.Y + (int)this.Height / 2,
                    (int)this.Width,
                    1),
                null,
                Color.Red, 0, Vector2.Zero, SpriteEffects.None, 0);

            GameManager.Instance.SpriteBatch.Draw(_debugLines,
                new Rectangle(
                    (int)Entity.Position.X - (int)this.Width / 2,
                    (int)Entity.Position.Y - (int)this.Height / 2,
                    (int)this.Width,
                    1),
                null,
                Color.Red, 1.57f, Vector2.Zero, SpriteEffects.None, 0);

            GameManager.Instance.SpriteBatch.Draw(_debugLines,
                new Rectangle(
                    (int)Entity.Position.X + (int)this.Width / 2,
                    (int)Entity.Position.Y - (int)this.Height / 2,
                    (int)this.Width,
                    1),
                null,
                Color.Red, 1.57f, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
