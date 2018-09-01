using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StickyHandGame_C9_RP7.Source.Components.Collision;
using StickyHandGame_C9_RP7.Source.Components.Render;
using StickyHandGame_C9_RP7.Source.Engine;
using StickyHandGame_C9_RP7.Source.Entities.Classes.Arm;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using System;
using System.Collections.Generic;

namespace StickyHandGame_C9_RP7.Source.Entities.Classes.Player
{
    public class PlayerEntity : Entity
    {
        int[] framNumber = RenderManager.zombieAnimatedAttribute.framNumber;
        int[] playSpeed = RenderManager.zombieAnimatedAttribute.playSpeed;
        String[] Names = RenderManager.zombieAnimatedAttribute.Names;
        AnimationComponent myAnimationComponent;

        // PhysicsEngine
        private const float _jumpforce = 100f;
        //private const float _rappleAcceleration = 1000f;
        private const float _rappleSpeed = 3000f;

        private const float _velocityCap = 1000;
        private const float _runningSpeed = 500f;
        public Vector2 Velocity;
        private Vector2 previousposition;
        private bool _runningJumpingEnabled = false;

        // The Hand 
        //private ThrowAbleEntity HandChain;
        private readonly HandEntity _hand;
        private readonly HandEntity _hand2;


        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerEntity"/> class.
        /// </summary>
        /// <param name="hide">if set to <c>true</c> [hide].</param>
        public PlayerEntity(bool hide = false) : base()
        {
            //this.CurrentState = CharacterState.Standard;
            this.Velocity = new Vector2();


            myAnimationComponent = new AnimationComponent("Idle", this,
                framNumber,
                playSpeed,
                Names,
                RenderManager.zombieAnimatedAttribute.Width
                , RenderManager.zombieAnimatedAttribute.Height
                , RenderManager.zombieAnimatedAttribute.Scale);

            // Hand Logic
            _hand = new HandEntity(this, HandEntity.HandID.First);
            _hand2 = new HandEntity(this, HandEntity.HandID.Second);
            this.Anchors = new List<Entity>() { _hand, _hand2 };

            // Load Content
            var texture = myAnimationComponent.LoadContent();
            this.Hide = hide;
            Width = 32f;
            Height = 32f;

            this.CollisionComponent = new BoxColliderComponent(this, this.Width, this.Height, CollisionLayers.Player);
        }

        /// <summary>
        /// Draws the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public override void Draw(GameTime gameTime)
        {

            myAnimationComponent.Draw(gameTime);
            _hand.Draw(gameTime);
            _hand2.Draw(gameTime);

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

        }

        /// <summary>
        /// Applies a velocity to the player.
        /// </summary>
        /// <param name="velocity">The velocity.</param>
        public void HoldRappleToHand(HandEntity.HandID handId)
        {

        }

        private void AcceleratePlayerToEntity(Entity entity, float speed, float timeElapsed)
        {
            var direction = (entity.Position - this.Position);
            direction.Normalize();
            this.Velocity += direction * speed * timeElapsed;
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
            this._hand2.Update(gameTime);

            var kState = Keyboard.GetState();

            // For Testing Purposes
            if (kState.IsKeyDown(Keys.J))
                _runningJumpingEnabled = true;
            if (kState.IsKeyDown(Keys.K))
                _runningJumpingEnabled = false;

            if (_hand.CurrentState == HandEntity.HandState.Latched)
            {
                if (!Keyboard.GetState().IsKeyDown(Keys.A))
                {

                }
                else
                {
                    AcceleratePlayerToEntity(_hand, _rappleSpeed, timeElapsed);
                }
            }


            if (_hand2.CurrentState == HandEntity.HandState.Latched)
            {
                if (!Keyboard.GetState().IsKeyDown(Keys.S))
                {

                }
                else
                {
                    AcceleratePlayerToEntity(_hand2, _rappleSpeed, timeElapsed);
                }
            }


            // Velocity Cap
            if (this.Velocity.Length() > _velocityCap)
            {
                var direction = new Vector2(Velocity.X, Velocity.Y);
                direction.Normalize();
                this.Velocity = direction * _velocityCap;
            }

            this.Velocity = PhysicsEngine.MoveTowards(this, this.Velocity, timeElapsed);
            myAnimationComponent.Update(gameTime);
        }


        ///// <summary>
        ///// Jumps the check.
        ///// </summary>
        ///// <param name="kState">State of the k.</param>
        //private void JumpCheck(KeyboardState kState)
        //{
        //    if (kState.IsKeyDown(Keys.W))
        //    {
        //        this.Velocity += new Vector2(0, -1) * _jumpforce;
        //        this.CurrentState = CharacterState.Airbourne;
        //    }
        //}

        /// <summary>
        /// Updates the left and right movement.
        /// </summary>
        /// <param name="timeElapsed">The time elapsed.</param>
        //private void UpdateLeftAndRightMovement(float timeElapsed)
        //{
        //    var kState = Keyboard.GetState();

        //    if (kState.IsKeyDown(Keys.A))
        //    {
        //        this.Velocity += new Vector2(-1, 0) * _runningSpeed * timeElapsed;
        //        this.myAnimationComponent.myeffect = SpriteEffects.FlipHorizontally;
        //    }

        //    if (kState.IsKeyDown(Keys.D))
        //    {
        //        this.Velocity += new Vector2(1, 0) * _runningSpeed * timeElapsed;
        //        this.myAnimationComponent.myeffect = SpriteEffects.None;
        //    }
        //}
    }
}
