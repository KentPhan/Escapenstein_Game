using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Components.Collision;
using StickyHandGame_C9_RP7.Source.Components.Render;
using StickyHandGame_C9_RP7.Source.Entities.Components;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using StickyHandGame_C9_RP7.Source.Managers;

namespace StickyHandGame_C9_RP7.Source.Entities.Classes
{
    public class PlayerEntity : Entity
    {
        int[] framNumber = RenderManager.PlayerAnimatedAttribute.framNumber;
        int[] playSpeed = RenderManager.PlayerAnimatedAttribute.playSpeed;
        String[] Names = RenderManager.PlayerAnimatedAttribute.Names;
        AnimationComponent myAnimationComponent;


        

        private readonly float _jumpforce = 200f;
        private readonly float _gravitationalAcceleration = 100f;
        private readonly float _runningAcceleration = 100f;
        private readonly float _runningTerminal = 1000f;
        private Vector2 _velocity = new Vector2();

        public enum CharacterState
        {
            AirBourne,
            Standing
        }
        public CharacterState State { get; private set; }

        public PlayerEntity(bool hide = false) : base()
        {
            this.State = CharacterState.Standing;

            myAnimationComponent = new AnimationComponent("Animation", this,
                framNumber,
                playSpeed,
                Names,
                RenderManager.PlayerAnimatedAttribute.Width
                , RenderManager.PlayerAnimatedAttribute.Height
                , RenderManager.PlayerAnimatedAttribute.Scale);

            this.CollisionComponent = new BoxColliderComponent(this, CollisionLayers.Player);

            // Load Content
            var texture = myAnimationComponent.LoadContent();
            this.Hide = hide;
            Width = texture.Width;
            Height = texture.Height;
            this.State = CharacterState.AirBourne;
        }

        public override void Draw(GameTime gameTime)
        {
            myAnimationComponent.Draw(gameTime);
        }

        public override void Reset()
        {
        }

        public override object Clone()
        {
            var cloned = new PlayerEntity(true);
            cloned.Position = this.Position;
            return cloned;
        }

        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            var kState = Keyboard.GetState();
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Collision Checking
            //if (this.CollisionComponent.CollidedWith.Count > 0)
            //{
            //    // Move user in opposite direction of collision
            //    foreach (var item in this.CollisionComponent.CollidedWith)
            //    {
            //        this.Position += _speed * item.Item2 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //    }

            //    return;
            //}
            //else
            //{

            //}

            // If its moving, do predictive collisions
            if (_velocity.X > 0 || _velocity.Y > 0)
            {
                this.CollisionComponent.CheckWillCollide(GameManager.Instance.NonPlayerEntityList, _velocity);
                if (this.CollisionComponent.WillCollideWith.Count > 0)
                {
                    // based upon the direction, react
                    bool onPlatform = false;
                    foreach (var item in this.CollisionComponent.WillCollideWith)
                    {
                        Vector2 upVector = new Vector2(0, -1);
                        if (Vector2.Dot((upVector), item.Item2) > 0)
                        {
                            // if below
                            onPlatform = true;
                        }
                    }

                    if (onPlatform)
                    {
                        this.State = CharacterState.Standing;
                        this._velocity = new Vector2(0, 0);
                    }
                    else
                    {
                        this.State = CharacterState.AirBourne;
                    }

                }
            }
            

            switch (this.State)
            {
                case CharacterState.Standing:
                    if (kState.IsKeyDown(Keys.Up))
                    {
                        _velocity +=  new Vector2(0,-1) * _jumpforce;
                        this.State = CharacterState.AirBourne;
                    }
                    break;
                case CharacterState.AirBourne:
                    // Gravity
                    this._velocity +=  new Vector2(0,1) * _gravitationalAcceleration * timeElapsed;
                    break;
            }


            if (kState.IsKeyDown(Keys.Left))
            {
                this._velocity += new Vector2(-1, 0) * _runningAcceleration * timeElapsed;
                if (Math.Abs(this._velocity.X) > this._runningTerminal)
                    this._velocity.X = this._runningTerminal * -1;

            }
            if (kState.IsKeyDown(Keys.Right))
            {
                this._velocity += new Vector2(1, 0) * _runningAcceleration * timeElapsed;
                if (Math.Abs(this._velocity.X) > this._runningTerminal)
                    this._velocity.X = this._runningTerminal;
            }
            
            this.Position += _velocity * timeElapsed;

            Debug.WriteLine(this.State);

            myAnimationComponent.Update(gameTime);
        }
    }
}
