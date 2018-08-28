using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Components.Collision;
using StickyHandGame_C9_RP7.Source.Components.Render;
using StickyHandGame_C9_RP7.Source.Entities.Components;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using System;

namespace StickyHandGame_C9_RP7.Source.Entities.Classes
{
    public class TriggerEntity : Entity
    {
        public RenderComponent renderComponent;

        public TriggerEntity(string tileName, Vector2 position) : base()
        {
            renderComponent = new RenderComponent(tileName, this);
            this.renderComponent.LoadContent();
            this.Position = position;


            Width = 32;
            Height = 32;

            this.CollisionComponent = new BoxColliderComponent(this, Width, Height, CollisionLayers.Trigger);
        }

        public override void UnloadContent()
        {
            throw new NotImplementedException();
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

        public override void CollisionTriggered(Tuple<CollisionComponent, Vector2, Side> collided)
        {
            throw new NotImplementedException();
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
