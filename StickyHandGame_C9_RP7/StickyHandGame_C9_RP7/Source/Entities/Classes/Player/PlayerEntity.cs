using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StickyHandGame_C9_RP7.Source.Components.Collision;
using StickyHandGame_C9_RP7.Source.Components.Render;
using StickyHandGame_C9_RP7.Source.Engine;
using StickyHandGame_C9_RP7.Source.Entities.Classes.Arm;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using StickyHandGame_C9_RP7.Source.Managers;
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


        private const float _rappleAcceleration = 200f;
        private const float _velocityCap = 500;
        public Vector2 Velocity;
        private Vector2 previousposition;

        // The Hand 
        private readonly HandEntity _hand;
        //private readonly HandEntity _hand2;


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
            //_hand2 = new HandEntity(this, HandEntity.HandID.Second);
            this.Anchors = new List<Entity>() { _hand };

            // Load Content
            var texture = myAnimationComponent.LoadContent();
            this.Hide = hide;
            Width = 32f;
            Height = 32f;

            this.CollisionComponent = new BoxColliderComponent(this, this.Width, this.Height, Layers.NonStatic, Tags.Player);
        }

        /// <summary>
        /// Draws the specified game time.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public override void Draw(GameTime gameTime)
        {

            myAnimationComponent.Draw(gameTime);
            _hand.Draw(gameTime);
            //_hand2.Draw(gameTime);

            InputManager.Instance.DrawAssist(gameTime);

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

        private void AcceleratePlayerToEntity(Entity entity, float speed)
        {
            var direction = (entity.Position - this.Position);
            direction.Normalize();
            this.Velocity += direction * speed;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public override void Update(GameTime gameTime)
        {
            // Hand
            this._hand.Update(gameTime);
            //this._hand2.Update(gameTime);


            if (_hand.CurrentState == HandEntity.HandState.Latched)
            {
                if (InputManager.Instance.GetLeftReelTrigger())
                {
                    _hand.IsActiveAnchor = false;
                    //_hand2.IsActiveAnchor = false;
                    AcceleratePlayerToEntity(_hand, _rappleAcceleration);
                    _hand.AnchorDistance = (this.Position - _hand.Position).Length();
                    //_hand2.AnchorDistance = (this.Position - _hand2.Position).Length();
                }
                else
                {
                    _hand.IsActiveAnchor = true;
                }
            }


            //if (_hand2.CurrentState == HandEntity.HandState.Latched)
            //{
            //    if (InputManager.Instance.GetRightReelTrigger())
            //    {
            //        _hand.IsActiveAnchor = false;
            //        _hand2.IsActiveAnchor = false;
            //        AcceleratePlayerToEntity(_hand2, _rappleAcceleration);
            //        _hand.AnchorDistance = (this.Position - _hand.Position).Length();
            //        _hand2.AnchorDistance = (this.Position - _hand2.Position).Length();
            //    }
            //    else
            //    {
            //        if (!InputManager.Instance.GetLeftReelTrigger())
            //        {
            //            _hand2.IsActiveAnchor = true;
            //        }
            //    }
            //}

            // Velocity Cap
            if (this.Velocity.Length() > _velocityCap)
            {
                var direction = new Vector2(Velocity.X, Velocity.Y);
                direction.Normalize();
                this.Velocity = direction * _velocityCap;
            }

            this.Velocity = PhysicsEngine.MoveTowards(this, this.Velocity, gameTime, new List<Layers>() { Layers.HandOnlyStatic });
            myAnimationComponent.Update(gameTime);
        }
    }
}
