﻿using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Engine;

namespace StickyHandGame_C9_RP7.Source.Entities.Classes.Collidables
{
    public class DestructablePlatformEntity : PlatformEntity
    {
        public DestructablePlatformEntity(string tileName, Vector2 position, bool hide = false) : base(tileName, position, hide)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            if (!Hide)
                this.renderComponent.Draw(gameTime);

            if (GameManager.Instance.DebugMode)
            {
                this.CollisionComponent?.DebugDraw(gameTime);
            }
        }

        public override void Update(GameTime gameTime)
        {
            this.renderComponent.Update(gameTime);
        }

        public override void CollisionTriggered(CollisionInfo collided)
        {
            return;
        }
    }
}
