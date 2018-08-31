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
            Restart,
            Victory
        }


        private bool _isAnimation;
        public RenderComponent renderComponent;
        public AnimationComponent animationComponent;
        private TriggerType _type;

        public TriggerEntity(string tileName, Vector2 position, TriggerType type, bool isAnimation = false) : base()
        {
            this._isAnimation = isAnimation;
            if (isAnimation)
            {
                animationComponent = new AnimationComponent("Tile_65_NC_K", this,
                    new int[] { 6 },
                    new int[] { 60 },
                    new string[] { "Tile_65_NC_K" },
                    32
                    , 32
                    , new Vector2(1, 1));
                animationComponent.LoadContent();
            }
            else
            {
                renderComponent = new RenderComponent(tileName, this);
                this.renderComponent.LoadContent();
            }



            this.Position = position;
            Width = 32;
            Height = 32;

            this._type = type;
            _isAnimation = isAnimation;

            this.CollisionComponent = new BoxColliderComponent(this, Width, Height, CollisionLayers.Trigger);
        }

        public override void UnloadContent()
        {
            return;
        }

        public override void Update(GameTime gameTime)
        {
            animationComponent?.Update(gameTime);
            return;
        }

        public override void Draw(GameTime gameTime)
        {
            if (!Hide)
            {
                if (this._isAnimation)
                    this.animationComponent.Draw(gameTime);
                else
                    this.renderComponent.Draw(gameTime);
            }

        }

        public override void Reset()
        {
            throw new NotImplementedException();
        }

        public override void CollisionTriggered(CollisionInfo collided)
        {
            if (this._type == TriggerType.Restart && collided.CollisionComponent.Layer == CollisionLayers.Player)
                LevelManager.Instance.ResetPlayerPosition();
            if (this._type == TriggerType.Victory && collided.CollisionComponent.Layer == CollisionLayers.Player)
                GameManager.Instance.RestartGame();
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }

}
