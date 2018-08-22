using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StickyHandGame_C9_RP7.Source.Entities.Core;

namespace StickyHandGame_C9_RP7.Source.Components.Render
{
    public class AnimationComponent : RenderComponent
    {
        Dictionary<String, Rectangle[]> Animation = new Dictionary<string, Rectangle[]>();
        Dictionary<String, int> AnimationOrder = new Dictionary<string, int>();
        private int FrameX = 36;
        private int FrameY = 36;// test with 36*36
        int[] framNumber;
        int[] playSpeed;
        String[] Names;
        String currentAnimation = "";
        int currentFrame = 0;
        int cummulattime = 0;// int in Milliseconds the return type is int.
        int currentPlaySpeed = 0; // int in Millionseconds
        int currentlength = 0;
        public AnimationComponent(string assetName, Game1 g, Entity entity,int[] framNumber,int[] playSpeed,String[] Names) : base(assetName, g, entity)
        {
            this.framNumber = framNumber;
            this.playSpeed = playSpeed;
            this.Names = Names;
            Debug.Assert(framNumber.Length == playSpeed.Length&& framNumber.Length == Names.Length, "inconsistent length in animation");

        }

        public override void Update(GameTime gameTime)
        {
            cummulattime += gameTime.ElapsedGameTime.Milliseconds;
            
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            g.spriteBatch.Draw(texture: texture, origin: new Vector2(FrameX / 2, FrameY / 2), position: e.position,sourceRectangle:GetFrame(CurrentAnimation,CurrentFrame));
        }
        private void BuildDictionary() {
            // for every row
            for (int i = 0; i < framNumber.Length; i++) {
                Rectangle[] array = new Rectangle[framNumber[i]];
                // for every frame
                for (int j = 0; j < framNumber[i]; j++) {
                    array[j] = RenderManager.CutFrame(FrameX, FrameY, i, j);
                }
                Animation[Names[i]] = array;
                AnimationOrder[Names[i]] = i;
            }
        }
        //this function use to play animation
        public Rectangle GetFrame(String Name, int Frame) {
            return Animation[Name][Frame];
        }
        public void SetAnimation(String Name, int StartFrame) {
            currentAnimation = Name;
            currentFrame = StartFrame;
            currentlength = framNumber[AnimationOrder[Name]];
            currentPlaySpeed = playSpeed[AnimationOrder[Name]];
        }
        private void loopFrameUpdate() {
            currentFrame = (currentFrame + 1) % currentlength;
        }
    }
}
