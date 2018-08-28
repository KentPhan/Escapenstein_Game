using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Components.Render;
using StickyHandGame_C9_RP7.Source.Engine;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using System;

namespace StickyHandGame_C9_RP7.Source.Entities.Classes
{
    public class EmptyEntity : Entity
    {
        public RenderComponent renderComponent;

        public EmptyEntity(string tileName, Vector2 position) : base()
        {
            renderComponent = new RenderComponent(tileName, this);
            this.renderComponent.LoadContent();
            this.Position = position;


            Width = 32;
            Height = 32;
        }

        public override void UnloadContent()
        {
            //throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            return;
        }

        public override void Draw(GameTime gameTime)
        {
            if (!Hide)
                this.renderComponent.Draw(gameTime);
        }

        public override void Reset()
        {
            throw new NotImplementedException();
        }

        public override void CollisionTriggered(CollisionInfo collided)
        {
            throw new NotImplementedException();
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
