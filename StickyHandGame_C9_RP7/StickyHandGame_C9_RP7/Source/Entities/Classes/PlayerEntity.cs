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
        int[] framNumber = RenderManager.zombieAnimatedAttribute.framNumber;
        int[] playSpeed = RenderManager.zombieAnimatedAttribute.playSpeed;
        String[] Names = RenderManager.zombieAnimatedAttribute.Names;
        AnimationComponent myAnimationComponent;


        

        private readonly float _jumpforce = 300f;
        private readonly float _gravitationalAcceleration = 300f;
        private readonly float _runningSpeed = 300f;
        private readonly float _rejectionSpeed = 200f;
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

            myAnimationComponent = new AnimationComponent("Idle", this,
                framNumber,
                playSpeed,
                Names,
                RenderManager.zombieAnimatedAttribute.Width
                , RenderManager.zombieAnimatedAttribute.Height
                , RenderManager.zombieAnimatedAttribute.Scale);

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
            if (this.CollisionComponent.CollidedWith.Count > 0)
            {
                // Move user in opposite direction of collision
                foreach (var item in this.CollisionComponent.CollidedWith)
                {
                    this.Position += _rejectionSpeed * item.Item2 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                return;
            }
            else
            {

            }

            // If its moving, do predictive collisions
            this.CollisionComponent.CheckWillCollide(GameManager.Instance.NonPlayerEntityList, _velocity);
            if (this.CollisionComponent.WillCollideWith.Count > 0)
            {
                // based upon the direction, react
                foreach (var item in this.CollisionComponent.WillCollideWith)
                {
                    bool thereIsSomethingBelowMe = false;
                    switch (item.Item3)
                    {
                        case Side.Top:
                            
                            thereIsSomethingBelowMe = true;
                            this._velocity = new Vector2(_velocity.X, 0);
                            break;
                        case Side.Left:
                            if(_velocity.X < 0)
                                this._velocity = new Vector2(0, _velocity.Y);
                            break;
                        case Side.Right:
                            if (_velocity.X > 0)
                                this._velocity = new Vector2(0, _velocity.Y);
                            break;
                        case Side.Bottom:
                            if(_velocity.Y < 0)
                                this._velocity = new Vector2(_velocity.X, 0);
                            break;
                        case Side.None:
                            break;
                    }

                    this.State = !thereIsSomethingBelowMe ? CharacterState.AirBourne : CharacterState.Standing;
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
                this._velocity += new Vector2(-1, 0) * _runningSpeed * timeElapsed;
                //if (Math.Abs(this._velocity.X) > this._runningTerminal)
                //    this._velocity.X = this._runningTerminal * -1;
            }
            else
            {
                if (this._velocity.X < 0)
                    this._velocity.X = 0;
            }
            if (kState.IsKeyDown(Keys.Right))
            {
                this._velocity += new Vector2(1, 0) * _runningSpeed * timeElapsed;
                //if (Math.Abs(this._velocity.X) > this._runningTerminal)
                //    this._velocity.X = this._runningTerminal;
            }
            else
            {
                if (this._velocity.X > 0)
                    this._velocity.X = 0;
            }
            
            this.Position += _velocity * timeElapsed;

            Debug.WriteLine(this.State);

            myAnimationComponent.Update(gameTime);
        }
    }
}
