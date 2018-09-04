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
        private bool _isAnimation;
        public RenderComponent renderComponent;
        public AnimationComponent animationComponent;
        private bool _alreadyTriggered = false;

        public TriggerEntity(string tileName, Vector2 position, Tags type, bool isAnimation = false) : base()
        {
            this._isAnimation = isAnimation;
            if (isAnimation)
            {
                animationComponent = new AnimationComponent(tileName, this,
                    new int[] { 6 },
                    new int[] { 60 },
                    new string[] { tileName },
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

            _isAnimation = isAnimation;
            _alreadyTriggered = false;

            this.CollisionComponent = new BoxColliderComponent(this, Width, Height, Layers.Static, type);
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
                    this.renderComponent.Draw(gameTime, Color.White);
            }

        }

        public override void Reset()
        {
            throw new NotImplementedException();
        }

        public override void CollisionTriggered(CollisionInfo collided)
        {
            if (!_alreadyTriggered)
            {
                if (this.CollisionComponent.Tag == Tags.Hazard && collided.CollisionComponent.Tag == Tags.Player)
                {
                    LevelManager.Instance.ResetCurrentPlayerPosition();
                    _alreadyTriggered = true;
                }
                else if (this.CollisionComponent.Tag == Tags.Goal && collided.CollisionComponent.Tag == Tags.Player)
                {
                    LevelManager.Instance.NextLevel();
                    _alreadyTriggered = true;
                }
            }

        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }

}
