using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StickyHandGame_C9_RP7.Source.Components.Collision;
using StickyHandGame_C9_RP7.Source.Components.Render;
using StickyHandGame_C9_RP7.Source.Engine;
using StickyHandGame_C9_RP7.Source.Entities.Classes.Player;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using StickyHandGame_C9_RP7.Source.Managers;
using StickyHandGame_C9_RP7.Source.Managers.Classes;
using System;
using System.Collections.Generic;

namespace StickyHandGame_C9_RP7.Source.Entities.Classes.Arm
{
    public class HandEntity : Entity
    {
        public enum HandState
        {
            OnPlayer,
            Shooting,
            Latched,
            Retreating,
        }

        public enum HandID
        {
            First,
            Second
        }

        public HandState CurrentState { get; set; }
        public HandID HandId { get; set; }


        //Render component
        private readonly RenderComponent _renderComponent;
        private readonly RenderComponent _renderChainComponents;
        private Texture2D chainTexture;

        /// <summary>
        /// Gets or sets the player.
        /// </summary>
        /// <value>
        /// The player.
        /// </value>
        private PlayerEntity _player { get; set; }


        // Physics
        private Vector2 TargetDirection;
        private float _returnSpeed = 1000.0f;
        private float _shootSpeed = 1000.0f;

        private float _maxDistanceOfHand = 350.0f;
        //private readonly float _gravitationalAcceleration = 100f;

        // TODO Switch this to collision Layer system instead
        public Vector2 Velocity;


        public HandEntity(PlayerEntity player, HandID handId)
        {
            this.CurrentState = HandState.OnPlayer;
            this.HandId = handId;
            this._player = player;
            this.Velocity = new Vector2();

            // Render
            _renderComponent = new RenderComponent("Character_Hand_Down", this);
            _renderComponent.LoadContent();
            _renderComponent.Scale = new Vector2(1.0f, 1.0f);



            this.chainTexture = GameManager.Instance.Content.Load<Texture2D>("Chain");

            this.Width = 32f;
            this.Height = 32f;

            this.CollisionComponent = new BoxColliderComponent(this, this.Width / 2.0f, this.Height / 2.0f, Layers.NonStatic, Tags.PlayerHand);
        }



        public override void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            switch (CurrentState)
            {
                case HandState.OnPlayer:
                    // If mouse is clicked while on player
                    if (GetShootTrigger())
                    {
                        this.TargetDirection = GetShootDirection();
                        // Check if Zero vector.
                        if (this.TargetDirection == Vector2.Zero)
                            break;
                        this.TargetDirection.Normalize();

                        _renderComponent.Direction = this.TargetDirection;

                        _player.SetFacing(GetShootDirection());

                        this.Velocity = _player.Velocity;
                        this.Velocity = Vector2.Zero;

                        CurrentState = HandState.Shooting;

                        SoundManager.Instance.Play(0);
                    }

                    this.Position = _player.Position;
                    break;
                case HandState.Shooting:
                    // if hand is in the middle of movement towards the target
                    //TODO Get Controller direction point and first trigger press
                    if (!GetShootTrigger() || ((this.Position - _player.Position).Length() >= this._maxDistanceOfHand))
                    {
                        ChangeStateToRetreating();
                    }
                    else
                    {
                        Velocity = TargetDirection * _shootSpeed;
                    }

                    break;
                case HandState.Latched:
                    //if hand is latched
                    // If mouse is clicked while on player
                    Velocity = Vector2.Zero;
                    if (!GetShootTrigger())
                    {
                        ChangeStateToRetreating();
                        break;
                    }
                    else if (((this.Position - _player.Position).Length() >= _maxDistanceOfHand))
                    {
                        //Velocity ropeTension
                    }
                    break;
                case HandState.Retreating:
                    // If hand is in the middle of returning to the player
                    if (Vector2.Dot(this.TargetDirection, this.Position - _player.Position) < 0)
                    {
                        this.Velocity = Vector2.Zero;

                        this.Position = _player.Position;
                        this.CurrentState = HandState.OnPlayer;
                        this.CollisionComponent.Layer = Layers.NonStatic;
                    }
                    else
                    {
                        var directionReturning = _player.Position - this.Position;
                        if (directionReturning == Vector2.Zero)
                            directionReturning = TargetDirection * -1;
                        directionReturning.Normalize();
                        this.Velocity = directionReturning * _returnSpeed;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            PhysicsEngine.MoveTowards(this, Velocity, gameTime, new List<Layers>() { });
        }


        /// <summary>
        /// Changes the state to retreating.
        /// </summary>
        private void ChangeStateToRetreating()
        {

            //this._renderComponent.Rotation = angle;
            this.IsActiveAnchor = false;
            this.AnchorDistance = 0.0f;
            this.CurrentState = HandState.Retreating;
            this.CollisionComponent.Layer = Layers.Ghost;
        }



        public override void Draw(GameTime gameTime)
        {
            if (this.CurrentState == HandState.Latched)
            {
                _renderComponent.LoadContent("Character_Hand_Down");
                _renderComponent.Draw(gameTime, Color.White);
            }
            else if (this.CurrentState == HandState.Shooting || this.CurrentState == HandState.Retreating)
            {
                _renderComponent.LoadContent("Character_Hand_Open");
                _renderComponent.Rotation = CalculateRotation(this.TargetDirection);
                _renderComponent.Draw(gameTime, Color.White);
            }
            else
            {
                // dont draw
            }



            //if (HandId == HandID.First)
            //{
            //    _renderComponent.Draw(gameTime, Color.Red);
            //}
            //else
            //{
            //    _renderComponent.Draw(gameTime, Color.Azure);
            //}

            // Draw chain
            Vector2 startPosition = _player.Position;
            Vector2 endPosition = this.Position;
            Vector2 direction = endPosition - startPosition;
            direction.Normalize();
            float scale = 2.5f;
            Vector2 currentDrawPosition = startPosition;
            while (Vector2.Dot(direction, (endPosition - currentDrawPosition)) > 0)
            {

                Color color = Color.Lerp(Color.White, Color.Red, ((startPosition - endPosition).Length() / _maxDistanceOfHand));
                GameManager.Instance.SpriteBatch.Draw(chainTexture, new Rectangle((int)currentDrawPosition.X - 16, (int)currentDrawPosition.Y - 16, 32, 32), color);
                currentDrawPosition += direction * scale;
            }

            if (GameManager.Instance.DebugMode)
            {
                this.CollisionComponent?.DebugDraw(gameTime);
            }
        }

        public override void Reset()
        {
            throw new NotImplementedException();
        }



        public override void CollisionTriggered(CollisionInfo collided)
        {

            if (collided.CollisionComponent.Tag == Tags.Hazard || collided.CollisionComponent.Tag == Tags.CantLatch)
            {
                ChangeStateToRetreating();
            }
            else
            {
                this.CurrentState = HandState.Latched;
                this.IsActiveAnchor = true;
                this.AnchorDistance = (_player.Position - this.Position).Length();
                this._renderComponent.Rotation = CalculateRotation(collided.NormalVector * -1) - ((float)Math.PI / 2);
            }
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the pressed input.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private bool GetShootTrigger()
        {
            switch (this.HandId)
            {
                case HandID.First:
                    if (InputManager.Instance.GetLeftShootTrigger())
                        return true;
                    break;
                case HandID.Second:
                    if (InputManager.Instance.GetRightShootTrigger())
                        return true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return false;
        }

        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <returns></returns>
        private Vector2 GetShootDirection()
        {
            switch (this.HandId)
            {
                case HandID.First:
                    return InputManager.Instance.GetLeftDirection();
                case HandID.Second:
                    return InputManager.Instance.GetRightDirection();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        public float CalculateRotation(Vector2 direction)
        {
            return (float)Math.Atan2(direction.Y, direction.X);
        }
    }
}

