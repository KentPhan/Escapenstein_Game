using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Components.Collision;
using StickyHandGame_C9_RP7.Source.Components.Render;
using StickyHandGame_C9_RP7.Source.Engine;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using StickyHandGame_C9_RP7.Source.Managers.Classes;
using System;

namespace StickyHandGame_C9_RP7.Source.Entities.Classes
{
    public class TriggerEntity : Entity
    {
        public enum TriggerType
        {
            Restart
        }


        public RenderComponent renderComponent;
        private TriggerType _type;

        public TriggerEntity(string tileName, Vector2 position, TriggerType type) : base()
        {
            renderComponent = new RenderComponent(tileName, this);
            this.renderComponent.LoadContent();
            this.Position = position;
            Width = 32;
            Height = 32;

            this._type = type;

            this.CollisionComponent = new BoxColliderComponent(this, Width, Height, CollisionLayers.Trigger);
        }

        public override void UnloadContent()
        {
            return;
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
            if (this._type == TriggerType.Restart && collided.CollisionComponent.Layer == CollisionLayers.Player)
                LevelManager.Instance.ResetPlayerPosition();
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
