﻿using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Components.Collision;
using StickyHandGame_C9_RP7.Source.Components.Render;
using StickyHandGame_C9_RP7.Source.Entities.Components;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using System;

namespace StickyHandGame_C9_RP7.Source.Entities.Classes
{
    public class PlatformEntity : Entity
    {
        public RenderComponent renderComponent;
        private string _tileName;


        public PlatformEntity(string tileName, Vector2 position, bool hide = false) : base()
        {
            this._tileName = tileName;
            renderComponent = new RenderComponent(tileName, this);
            this.Position = position;

            // Load Content

            var texture = this.renderComponent.LoadContent();
            this.Hide = hide;
            Width = 32;
            Height = 32;

            this.CollisionComponent = new BoxColliderComponent(this, Width, Height, CollisionLayers.Static);
        }
        public override void Draw(GameTime gameTime)
        {
            if (!Hide)
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

        public override void CollisionTriggered(Tuple<CollisionComponent, Vector2, Side> collided)
        {
            throw new NotImplementedException();
        }

        public override object Clone()
        {
            var cloned = new PlatformEntity(this._tileName, this.Position, true);
            cloned.Position = this.Position;
            return cloned;
        }
    }
}
