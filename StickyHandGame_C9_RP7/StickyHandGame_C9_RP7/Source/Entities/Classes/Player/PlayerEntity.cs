using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StickyHandGame_C9_RP7.Source.Components.Collision;
using StickyHandGame_C9_RP7.Source.Components.Render;
using StickyHandGame_C9_RP7.Source.Engine;
using StickyHandGame_C9_RP7.Source.Entities.Classes.Arm;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using System;

namespace StickyHandGame_C9_RP7.Source.Entities.Classes.Player
{
    public class PlayerEntity : Entity
    {
        int[] framNumber = RenderManager.zombieAnimatedAttribute.framNumber;
        int[] playSpeed = RenderManager.zombieAnimatedAttribute.playSpeed;
        String[] Names = RenderManager.zombieAnimatedAttribute.Names;
        AnimationComponent myAnimationComponent;

        // PhysicsEngine
        private readonly float _jumpforce = 400f;
        private readonly float _rappleAcceleration = 400f;
        private readonly float _rappleVelocityCap = 1000;
        private readonly float _gravitationalAcceleration = 100f;
        private readonly float _runningSpeed = 100f;
        private readonly float _drag = 2.0f;
        private Vector2 _velocity = new Vector2();
        private Vector2 previousposition;

        // The Hand 
        //private ThrowAbleEntity HandChain;
        private HandEntity _hand;
        private Vector2 _originalPosition;


        // States
        public enum CharacterState
        {
            Airbourne,
            Standing,
            Rappling,

        }
        public CharacterState CurrentState { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerEntity"/> class.
        /// </summary>
        /// <param name="hide">if set to <c>true</c> [hide].</param>
        public PlayerEntity(bool hide = false) : base()
        {
            this.CurrentState = CharacterState.Standing;

            myAnimationComponent = new AnimationComponent("Idle", this,
                framNumber,
                playSpeed,
                Names,
                RenderManager.zombieAnimatedAttribute.Width
                , RenderManager.zombieAnimatedAttribute.Height
                , RenderManager.zombieAnimatedAttribute.Scale);

            // Hand Logic
            //HandChain = new ThrowAbleEntity(this.Position + HandEntry.HndPositionOffSet, this);
            _hand = new HandEntity(this);

            // Load Content
            var texture = myAnimationComponent.LoadContent();
            this.Hide = hide;
            Width = 32f;
            Height = 32f;

            this.CollisionComponent = new BoxColliderComponent(this, this.Width, this.Height, CollisionLayers.Player);

            this.CurrentState = CharacterState.Airbourne;
        }

        /// <summary>
        /// Draws the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public override void Draw(GameTime gameTime)
        {
            _hand.Draw(gameTime);
            myAnimationComponent.Draw(gameTime);
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public override void Reset()
        {
            _velocity = Vector2.Zero;
        }


        public void SetFacing(Vector2 targetLooking)
        {
            Vector2 directionLooking = targetLooking - this.Position;
            Vector2 right = new Vector2(1, 0);
            if (Vector2.Dot(right, directionLooking) > 0)
                this.myAnimationComponent.myeffect = SpriteEffects.None;
            else
                this.myAnimationComponent.myeffect = SpriteEffects.FlipHorizontally;

        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            var cloned = new PlayerEntity(false);
            cloned.Position = this.Position;
            return cloned;
        }

        /// <summary>
        /// Unloads the content.
        /// </summary>
        public override void UnloadContent()
        {
        }


        /// <summary>
        /// Collisions triggered.
        /// </summary>
        /// <param name="collided">The collided.</param>
        public override void CollisionTriggered(CollisionInfo collided)
        {
            //based upon the direction, react
            if (this.CurrentState == CharacterState.Rappling)
                return;

            switch (collided.Side)
            {
                case Side.Top:
                    this.CurrentState = CharacterState.Standing;
                    this._velocity = new Vector2(0, 0);
                    break;
                case Side.Left:
                    if (_velocity.X < 0)
                        this._velocity = new Vector2(0, _velocity.Y);
                    break;
                case Side.Right:
                    if (_velocity.X > 0)
                        this._velocity = new Vector2(0, _velocity.Y);
                    break;
                case Side.Bottom:
                    if (_velocity.Y < 0)
                        this._velocity = new Vector2(_velocity.X, 0);
                    break;
                case Side.None:
                    break;
            }
        }


        /// <summary>
        /// Applies a velocity to the player.
        /// </summary>
        /// <param name="velocity">The velocity.</param>
        public void RappleToHand()
        {
            if (this.CurrentState != CharacterState.Rappling)
            {
                this._originalPosition = this.Position;
                this.CurrentState = CharacterState.Rappling;
            }
        }


        /// <summary>
        /// Update
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public override void Update(GameTime gameTime)
        {
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Hand
            //Vector2 deltaMovement = this.Position - this.previousposition;
            //this.previousposition = this.Position;
            //this.HandChain.Update(gameTime, deltaMovement);
            this._hand.Update(gameTime);

            var kState = Keyboard.GetState();

            // Based upon current state
            switch (this.CurrentState)
            {
                case CharacterState.Standing:
                    //if (kState.IsKeyDown(Keys.Up))
                    //{
                    //    _velocity += new Vector2(0, -1) * _jumpforce;
                    //    this.CurrentState = CharacterState.Airbourne;
                    //}
                    break;
                case CharacterState.Airbourne:
                    // Left and right movement
                    //if (kState.IsKeyDown(Keys.Left))
                    //{
                    //    this._velocity += new Vector2(-1, 0) * _runningSpeed * timeElapsed;
                    //    this.myAnimationComponent.myeffect = SpriteEffects.FlipHorizontally;
                    //}
                    //else
                    //{
                    //    if (this._velocity.X < 0)
                    //        this._velocity.X = 0;
                    //}
                    //if (kState.IsKeyDown(Keys.Right))
                    //{
                    //    this._velocity += new Vector2(1, 0) * _runningSpeed * timeElapsed;
                    //    this.myAnimationComponent.myeffect = SpriteEffects.None;
                    //}
                    //else
                    //{
                    //    if (this._velocity.X > 0)
                    //        this._velocity.X = 0;
                    //}
                    break;
                case CharacterState.Rappling:
                    if (Mouse.GetState().RightButton == ButtonState.Released || Vector2.Dot(_hand.Position - this._originalPosition, _hand.Position - this.Position) <= 0)
                    {
                        _hand.CurrentState = HandEntity.HandState.OnPlayer;
                        this.CurrentState = CharacterState.Airbourne;
                    }
                    else
                    {
                        var direction = (_hand.Position - this.Position);
                        direction.Normalize();
                        _velocity += direction * _rappleAcceleration * timeElapsed;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Horizontal drag
            if (Math.Abs(_velocity.X) > 0)
            {
                Vector2 dragVector = _velocity * -1;
                dragVector.Normalize();
                _velocity.X += dragVector.X * _drag;

            }

            // Gravity
            this._velocity += new Vector2(0, 1) * _gravitationalAcceleration * timeElapsed;

            PhysicsEngine.MoveTowards(this, _velocity, timeElapsed);
            myAnimationComponent.Update(gameTime);
        }
    }
}
