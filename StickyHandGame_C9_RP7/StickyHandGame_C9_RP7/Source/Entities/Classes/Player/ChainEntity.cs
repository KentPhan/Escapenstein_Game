using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Components.Render;
using StickyHandGame_C9_RP7.Source.Engine;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using System;

namespace StickyHandGame_C9_RP7.Source.Entities.Classes.Arm
{
    public class ChainEntity : Entity
    {
        public RenderComponent MyrenderComponent;
        public ChainEntity(Vector2 position, float angle)
        {
            this.Position = position;
            this.MyrenderComponent = new RenderComponent("Chain", this);
            this.MyrenderComponent.Scale = HandEntry.Scale;
            //this.MyrenderComponent.Rotation = angle;
            this.MyrenderComponent.Origin = HandEntry.Ringpivot;
            this.MyrenderComponent.LoadContent();
        }

        public override void CollisionTriggered(CollisionInfo collided)
        {
            throw new NotImplementedException();
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public override void Draw(GameTime gameTime)
        {
            this.MyrenderComponent.Draw(gameTime);
        }

        public override void Reset()
        {
            throw new NotImplementedException();
        }

        public override void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
