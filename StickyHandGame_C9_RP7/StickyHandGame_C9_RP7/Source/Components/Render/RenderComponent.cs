using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StickyHandGame_C9_RP7.Source.Components.Render
{
    public class RenderComponent
    {
        public String assetName;
        protected Texture2D texture;
        protected Entity e;

        public RenderComponent(String assetName, Entity entity)
        {
            this.assetName = assetName;
            this.e = entity;
        }

        public void LoadContent()
        {
            texture = GameManager.Instance.Content.Load<Texture2D>(assetName);
            Debug.Assert(texture != null, "null texture");
        }

        public virtual void Update(GameTime gameTime)
        {
            // this function do nothing since the render for a static object might not change
        }

        public virtual void Draw(GameTime gameTime)
        {
            GameManager.Instance.SpriteBatch.Draw(texture: texture, origin: new Vector2(texture.Width / 2, texture.Height / 2), position: e.position);
        }

    }
}
