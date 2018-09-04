using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using System;
using System.Diagnostics;

namespace StickyHandGame_C9_RP7.Source.Components.Render
{
    public class RenderComponent
    {
        public String assetName;
        protected Texture2D texture;
        protected Entity entity;
        public Vector2 Scale = new Vector2(1, 1);
        public Vector2 Direction;
        public float Rotation;
        public SpriteEffects Effects;

        public RenderComponent(String assetName, Entity entity)
        {
            this.assetName = assetName;
            this.entity = entity;
            this.Rotation = 0.0f;
            this.Effects = SpriteEffects.None;
        }

        public Texture2D LoadContent(string nameOverride = null)
        {
            if (nameOverride != null)
                texture = GameManager.Instance.Content.Load<Texture2D>(nameOverride);
            else
                texture = GameManager.Instance.Content.Load<Texture2D>(assetName);
            Debug.Assert(texture != null, "null texture");
            return texture;
        }

        public virtual void Update(GameTime gameTime)
        {
            // this function do nothing since the render for a static object might not change
        }

        public virtual void Draw(GameTime gameTime, Color color)
        {
            //GameManager.Instance.SpriteBatch.Draw(texture, new Rectangle((int)entity.Position.X - 16, (int)entity.Position.Y - 16, 32, 32), color);
            GameManager.Instance.SpriteBatch.Draw(texture, new Rectangle((int)entity.Position.X, (int)entity.Position.Y, 32, 32), new Rectangle(0, 0, 32, 32), color, Rotation, new Vector2(16.0f, 16.0f), Effects, 0.0f);

            //SpriteEffects flip = (Vector2.Dot(new Vector2(1, 0), this.Direction) < 0)
            //    ? SpriteEffects.FlipHorizontally
            //    : SpriteEffects.None;

            //GameManager.Instance.SpriteBatch.Draw(texture, entity.Position, new Rectangle(0, 0, 32, 32), Color.White, 0.0f, this.Origin, 1.0f, SpriteEffects.None, 1);
        }

    }
}
