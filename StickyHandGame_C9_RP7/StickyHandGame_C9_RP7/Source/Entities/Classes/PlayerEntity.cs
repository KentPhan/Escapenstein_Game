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

namespace StickyHandGame_C9_RP7.Source.Entities.Classes
{
    public class PlayerEntity : Entity
    {
        int[] framNumber = RenderManager.PlayerAnimatedAttribute.framNumber;
        int[] playSpeed = RenderManager.PlayerAnimatedAttribute.playSpeed;
        String[] Names = RenderManager.PlayerAnimatedAttribute.Names;
        AnimationComponent myAnimationComponent;
        public PlayerEntity(Game1 g) : base(g)
        {
            myAnimationComponent = new AnimationComponent("Animation", g, this, 
                framNumber,
                playSpeed,
                Names,
                RenderManager.PlayerAnimatedAttribute.Width
                , RenderManager.PlayerAnimatedAttribute.Hight
                , RenderManager.PlayerAnimatedAttribute.Scale);
        }

        public override void Draw(GameTime gameTime)
        {
            myAnimationComponent.Draw(gameTime);
        }

        public override void Initialize()
        {
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
            myAnimationComponent.Update(gameTime);
        }
    }
}
