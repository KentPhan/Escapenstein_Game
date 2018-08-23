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
        public PlatformEntity(String assetName) : base()
        {
            renderComponent = new RenderComponent(assetName, this);
            this.CollisionComponent = new BoxColliderComponent(this, position, (float)RenderManager.PlayerAnimatedAttribute.Width, (float)RenderManager.PlayerAnimatedAttribute.Height, CollisionLayers.Platform);

            // Load Content
            this.renderComponent.LoadContent();
        }
        public override void Draw(GameTime gameTime)
        {
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
    }
}
