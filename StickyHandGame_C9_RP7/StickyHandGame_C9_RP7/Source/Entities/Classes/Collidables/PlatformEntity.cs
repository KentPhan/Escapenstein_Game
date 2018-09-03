using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Components.Collision;
using StickyHandGame_C9_RP7.Source.Components.Render;
using StickyHandGame_C9_RP7.Source.Engine;
using StickyHandGame_C9_RP7.Source.Entities.Core;

namespace StickyHandGame_C9_RP7.Source.Entities.Classes
{
    public class PlatformEntity : Entity
    {
        public RenderComponent renderComponent;
        private string _tileName;


        public PlatformEntity(string tileName, Vector2 position, Layers layer = Layers.Static, Tags tag = Tags.None, bool hide = false) : base()
        {
            this._tileName = tileName;
            renderComponent = new RenderComponent(tileName, this);
            this.Position = position;

            // Load Content

            var texture = this.renderComponent.LoadContent();
            this.Hide = hide;
            Width = 32;
            Height = 32;

            this.CollisionComponent = new BoxColliderComponent(this, Width, Height, layer, tag);
        }
        public PlatformEntity(string tileName, Vector2 position, TriangleColliderComponent.Oritation oritation, bool hide = false) : base()
        {
            this._tileName = tileName;
            renderComponent = new RenderComponent(tileName, this);
            this.Position = position;

            // Load Content

            var texture = this.renderComponent.LoadContent();
            this.Hide = hide;
            Width = 32;
            Height = 32;

            this.CollisionComponent = new TriangleColliderComponent(this, Width, Height, Layers.Static, oritation);
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

        public override void CollisionTriggered(CollisionInfo collided)
        {
            return;
        }

        public override object Clone()
        {
            var cloned = new PlatformEntity(this._tileName, this.Position, this.CollisionComponent.Layer, this.CollisionComponent.Tag, true);
            cloned.Position = this.Position;
            return cloned;
        }
    }
}
