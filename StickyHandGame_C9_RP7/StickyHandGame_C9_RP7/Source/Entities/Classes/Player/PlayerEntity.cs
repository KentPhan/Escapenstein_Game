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
        private readonly float _jumpforce = 100f;
        private readonly float _rappleAcceleration = 400f;
        private readonly float _velocityCap = 1000;
        private readonly float _gravitationalAcceleration = 100f;
        private readonly float _runningSpeed = 500f;
        private readonly float _drag = 2.0f;
        public Vector2 Velocity;
        private Vector2 previousposition;
        private bool _runningJumpingEnabled = false;

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
            Holding

        }
        public CharacterState CurrentState { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerEntity"/> class.
        /// </summary>
        /// <param name="hide">if set to <c>true</c> [hide].</param>
        public PlayerEntity(bool hide = false) : base()
        {
            this.CurrentState = CharacterState.Standing;
            this.Velocity = new Vector2();


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

            if (GameManager.Instance.DebugMode)
            {
                this.CollisionComponent?.DebugDraw(gameTime);
            }
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public override void Reset()
        {
            this.Velocity = Vector2.Zero;
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

            // Top collided with other
            if (collided.NormalVector.Y == 1.0f)
            {
                if (this.Velocity.Y < 0)
                    this.Velocity = new Vector2(this.Velocity.X, 0);
                //this.Position += new Vector2(0, 1.0f);
            }
            // Bottom collided with other
            else if (collided.NormalVector.Y == -1.0f)
            {
                this.CurrentState = CharacterState.Standing;
                this.Velocity = new Vector2(this.Velocity.X, 0);
            }
            // Left collided with other
            else if (collided.NormalVector.X == 1.0f)
            {
                if (this.Velocity.X > 0)
                    this.Velocity = new Vector2(0, this.Velocity.Y);
                //this.Position += new Vector2(1.0f, 0);
            }
            // Right collided with other
            else if (collided.NormalVector.X == -1.0f)
            {
                if (this.Velocity.X < 0)
                    this.Velocity = new Vector2(0, this.Velocity.Y);
                //this.Position += new Vector2(-1.0f, 0);
            }
            else
            {
                throw new NotImplementedException("Did not implement that collision TRigger");
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
            this._hand.Update(gameTime);

            var kState = Keyboard.GetState();


            // For Testing Purposes
            if (kState.IsKeyDown(Keys.J))
                _runningJumpingEnabled = true;
            if (kState.IsKeyDown(Keys.K))
                _runningJumpingEnabled = false;


            // Based upon current state
            switch (this.CurrentState)
            {
                case CharacterState.Standing:
                    if (_runningJumpingEnabled)
                    {
                        if (kState.IsKeyDown(Keys.Up))
                        {
                            this.Velocity += new Vector2(0, -1) * _jumpforce;
                            this.CurrentState = CharacterState.Airbourne;
                        }
                        UpdateLeftAndRightMovement(timeElapsed);
                    }

                    break;
                case CharacterState.Airbourne:
                    // Left and right movement
                    if (_runningJumpingEnabled)
                    {
                        UpdateLeftAndRightMovement(timeElapsed);
                    }
                    break;
                case CharacterState.Rappling:
                    // For releasing on reaching destination
                    //if (Mouse.GetState().RightButton == ButtonState.Released || Vector2.Dot(_hand.Position - this._originalPosition, _hand.Position - this.Position) <= 0)
                    if (Mouse.GetState().RightButton == ButtonState.Released)
                    {
                        _hand.CurrentState = HandEntity.HandState.OnPlayer;
                        this.CurrentState = CharacterState.Airbourne;
                    }
                    else
                    {
                        var direction = (_hand.Position - this.Position);
                        direction.Normalize();
                        this.Velocity += direction * _rappleAcceleration * timeElapsed;
                    }
                    break;
                case CharacterState.Holding:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Horizontal drag
            if (Math.Abs(this.Velocity.X) > 0)
            {
                Vector2 dragVector = this.Velocity * -1;
                dragVector.Normalize();
                this.Velocity.X += dragVector.X * _drag;

            }

            // Gravity
            this.Velocity += new Vector2(0, 1) * _gravitationalAcceleration * timeElapsed;

            if (this.Velocity.Length() > _velocityCap)
            {
                var direction = new Vector2(Velocity.X, Velocity.Y);
                direction.Normalize();
                this.Velocity = direction * _velocityCap;
            }

            Console.WriteLine(this.CurrentState);


            this.Velocity = PhysicsEngine.MoveTowards(this, this.Velocity, timeElapsed);
            myAnimationComponent.Update(gameTime);
        }

        /// <summary>
        /// Updates the left and right movement.
        /// </summary>
        /// <param name="timeElapsed">The time elapsed.</param>
        private void UpdateLeftAndRightMovement(float timeElapsed)
        {
            var kState = Keyboard.GetState();

            if (kState.IsKeyDown(Keys.Left))
            {
                this.Velocity += new Vector2(-1, 0) * _runningSpeed * timeElapsed;
                this.myAnimationComponent.myeffect = SpriteEffects.FlipHorizontally;
            }

            if (kState.IsKeyDown(Keys.Right))
            {
                this.Velocity += new Vector2(1, 0) * _runningSpeed * timeElapsed;
                this.myAnimationComponent.myeffect = SpriteEffects.None;
            }
        }
    }
}
