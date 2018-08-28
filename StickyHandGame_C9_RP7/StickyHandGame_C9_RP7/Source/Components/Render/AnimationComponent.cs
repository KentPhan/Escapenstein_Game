using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace StickyHandGame_C9_RP7.Source.Components.Render
{
    //TODO make this more Data driven
    //TODO make the rotation to save efforts
    public class AnimationComponent : RenderComponent
    {
        Dictionary<String, Rectangle[]> Animation = new Dictionary<string, Rectangle[]>();
        Dictionary<String, int> AnimationOrder = new Dictionary<string, int>();
        private int FrameX = 36;
        private int FrameY = 36;// test with 36*36
        int[] framNumber;
        // the play speed is the lasting time of each frame
        int[] playSpeed;
        String[] Names;
        String currentAnimation = "";
        int currentFrame = 0;
        int cummulattime = 0;// int in Milliseconds the return type is int.
        int currentPlaySpeed = 0; // int in Millionseconds
        int currentlength = 0;
        Vector2 scale;
        public SpriteEffects myeffect;
        //Vector2 origin;
        public AnimationComponent(string assetName, Entity entity, int[] framNumber, int[] playSpeed, String[] Names, int FrameX, int FrameY, Vector2 scale) : base(assetName, entity)
        {
            this.scale = scale;
            this.FrameX = FrameX;
            this.FrameY = FrameY;
            this.framNumber = framNumber;
            this.playSpeed = playSpeed;
            this.Names = Names;
            Debug.Assert(framNumber.Length == playSpeed.Length && framNumber.Length == Names.Length, "inconsistent length in animation");
            this.BuildDictionary();
            SetAnimation(Names[0], 0);
            myeffect = SpriteEffects.None;
        }

        public override void Update(GameTime gameTime)
        {
            cummulattime += gameTime.ElapsedGameTime.Milliseconds;
            if (cummulattime > currentPlaySpeed)
            {
                cummulattime = 0;
                loopFrameUpdate();
            }
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            GameManager.Instance.SpriteBatch.Draw(scale: this.scale, texture: texture, position: entity.Position, sourceRectangle: GetFrame(currentAnimation, currentFrame), origin: new Vector2(FrameX / 2, FrameY / 2), effects: myeffect);
        }
        private void BuildDictionary()
        {
            // for every row
            for (int i = 0; i < framNumber.Length; i++)
            {
                Rectangle[] array = new Rectangle[framNumber[i]];
                // for every frame
                for (int j = 0; j < framNumber[i]; j++)
                {
                    array[j] = RenderManager.CutFrame(FrameX, FrameY, i, j);
                }
                Animation[Names[i]] = array;
                AnimationOrder[Names[i]] = i;
            }
        }
        //this function use to play animation
        public Rectangle GetFrame(String Name, int Frame)
        {
            return Animation[Name][Frame];
        }
        public void SetAnimation(String Name, int StartFrame)
        {
            cummulattime = 0;
            currentAnimation = Name;
            currentFrame = StartFrame;
            currentlength = framNumber[AnimationOrder[Name]];
            currentPlaySpeed = playSpeed[AnimationOrder[Name]];
        }
        private void loopFrameUpdate()
        {
            currentFrame = (currentFrame + 1) % currentlength;
        }
    }
}
