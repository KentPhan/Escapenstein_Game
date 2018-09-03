using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Engine;
using StickyHandGame_C9_RP7.Source.Entities.Core;

namespace StickyHandGame_C9_RP7.Source.Entities.Classes.Collidables
{
    public class DestructablePlatformEntity : PlatformEntity
    {
        public DestructablePlatformEntity(string tileName, Vector2 position, Layers layer = Layers.Static, Tags tag = Tags.None, bool hide = false) : base(tileName, position, layer, tag, hide)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            if (!Hide)
                this.renderComponent.Draw(gameTime, Color.White);

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
