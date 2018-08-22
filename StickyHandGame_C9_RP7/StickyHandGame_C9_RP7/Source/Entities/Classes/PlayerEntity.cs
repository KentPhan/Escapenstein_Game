using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Components.Collision;
using StickyHandGame_C9_RP7.Source.Components.Render;
using StickyHandGame_C9_RP7.Source.Entities.Components;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace StickyHandGame_C9_RP7.Source.Entities.Classes
{
    public class PlayerEntity : Entity
    {
        int[] framNumber = RenderManager.PlayerAnimatedAttribute.framNumber;
        int[] playSpeed = RenderManager.PlayerAnimatedAttribute.playSpeed;
        String[] Names = RenderManager.PlayerAnimatedAttribute.Names;
        AnimationComponent myAnimationComponent;


        private float speed = 100f;


        public PlayerEntity(GameManager g) : base(g)
        {
            myAnimationComponent = new AnimationComponent("Animation", g, this, 
                framNumber,
                playSpeed,
                Names,
                RenderManager.PlayerAnimatedAttribute.Width
                , RenderManager.PlayerAnimatedAttribute.Height
                , RenderManager.PlayerAnimatedAttribute.Scale);
        }

        public override void Draw(GameTime gameTime)
        {
            myAnimationComponent.Draw(gameTime);
        }

        public override void Initialize()
        {
            this.CollisionComponent = new BoxColliderComponent(this, position, (float)RenderManager.PlayerAnimatedAttribute.Width * RenderManager.PlayerAnimatedAttribute.Scale.X, (float)RenderManager.PlayerAnimatedAttribute.Height * RenderManager.PlayerAnimatedAttribute.Scale.Y, CollisionLayers.Player);
        }

        public override void LoadContent()
        {
            myAnimationComponent.LoadContent();
        }

        public override void Reset()
        {
        }

        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            var kState = Keyboard.GetState();


            if (this.CollisionComponent.HasCollided)
            {
                return;
            }
            
            if (kState.IsKeyDown(Keys.Up))
                position.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kState.IsKeyDown(Keys.Down))
                position.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kState.IsKeyDown(Keys.Left))
                position.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kState.IsKeyDown(Keys.Right))
                position.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kState.IsKeyDown(Keys.Right))
                position.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            myAnimationComponent.Update(gameTime);
        }
    }
}
