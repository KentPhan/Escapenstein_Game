using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Components.Render;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StickyHandGame_C9_RP7.Source.Components.Collision;

namespace StickyHandGame_C9_RP7.Source.Entities.Classes
{
    public class PlatformEntity : Entity
    {
        public RenderComponent renderComponent;
        public PlatformEntity(bool hide = false) : base()
        {
            renderComponent = new RenderComponent("platform", this);
            this.CollisionComponent = new BoxColliderComponent(this, CollisionLayers.Platform);

            // Load Content
            
            var texture = this.renderComponent.LoadContent();
            this.Hide = hide;
            Width = texture.Width;
            Height = texture.Height;
        }
        public override void Draw(GameTime gameTime)
        {
            if(!Hide)
                this.renderComponent.Draw(gameTime);
        }

        public override void Reset()
        {
            //throw new NotImplementedException();
        }

        public override void UnloadContent()
        {
            //throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            this.renderComponent.Update(gameTime);
        }

        public override object Clone()
        {
            var cloned = new PlatformEntity(true);
            cloned.Position = this.Position;
            return cloned;
        }
    }
}
