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
        public Vector2 Origin;
        public RenderComponent(String assetName, Entity entity)
        {
            this.assetName = assetName;
            this.entity = entity;
        }
        public RenderComponent(String assetName, Entity entity, Vector2 origin)
        {
            this.assetName = assetName;
            this.entity = entity;
            this.Origin = origin;
        }

        public Texture2D LoadContent(string nameOverride = null)
        {
            if (nameOverride != null)
                texture = GameManager.Instance.Content.Load<Texture2D>(nameOverride);
            else
                texture = GameManager.Instance.Content.Load<Texture2D>(assetName);
            Debug.Assert(texture != null, "null texture");
            if (Origin == null)
            {
                Origin = new Vector2(texture.Width, texture.Height);
            }
            return texture;
        }

        public virtual void Update(GameTime gameTime)
        {
            // this function do nothing since the render for a static object might not change
        }

        public virtual void Draw(GameTime gameTime)
        {
            GameManager.Instance.SpriteBatch.Draw(texture, new Rectangle((int)entity.Position.X - 16, (int)entity.Position.Y - 16, 32, 32), Color.White);

            //SpriteEffects flip = (Vector2.Dot(new Vector2(1, 0), this.Direction) < 0)
            //    ? SpriteEffects.FlipHorizontally
            //    : SpriteEffects.None;

            //GameManager.Instance.SpriteBatch.Draw(texture, entity.Position, new Rectangle(0, 0, 32, 32), Color.White, 0.0f, this.Origin, 1.0f, flip, 1);
        }

    }
}
