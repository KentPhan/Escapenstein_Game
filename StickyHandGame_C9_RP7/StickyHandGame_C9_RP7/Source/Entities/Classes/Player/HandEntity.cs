using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StickyHandGame_C9_RP7.Source.Cameras;
using StickyHandGame_C9_RP7.Source.Components.Collision;
using StickyHandGame_C9_RP7.Source.Components.Render;
using StickyHandGame_C9_RP7.Source.Engine;
using StickyHandGame_C9_RP7.Source.Entities.Classes.Player;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using StickyHandGame_C9_RP7.Source.Managers.Classes;
using System;

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
        public HandState CurrentState { get; set; }


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
        private Vector2 TargetDestination;
        private Vector2 TargetDirection;
        private float _returnSpeed = 50000.0f;
        private float _shootSpeed = 50000.0f;

        private float _maxDistanceOfHand = 300.0f;
        //private readonly float _gravitationalAcceleration = 100f;

        // TODO Switch this to collision Layer system instead
        public Vector2 Velocity;


        public HandEntity(PlayerEntity player)
        {
            this.CurrentState = HandState.OnPlayer;
            this._player = player;
            this.Velocity = new Vector2();

            // Render
            _renderComponent = new RenderComponent("Character_Hand_Down", this, HandEntry.HandOrigin);
            _renderComponent.LoadContent();
            _renderComponent.Scale = HandEntry.Scale;

            this.chainTexture = GameManager.Instance.Content.Load<Texture2D>("Chain");

            this.Width = 32f;
            this.Height = 32f;

            this.CollisionComponent = new BoxColliderComponent(this, this.Width / 2.0f, this.Height / 2.0f, CollisionLayers.PlayerHand);
        }



        public override void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (CurrentState)
            {
                case HandState.OnPlayer:
                    // If mouse is clicked while on player
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        this.TargetDestination = GetTargetDestination();
                        this.TargetDirection = this.TargetDestination - this.Position;
                        this.TargetDirection.Normalize();
                        
                        var test = new Vector2(-1561, 30213);
                        test.Normalize();
                        _renderComponent.Direction = this.TargetDirection;
                        //float angle = CalculateRotation(TargetDirection);
                        //this._renderComponent.Rotation = angle;

                        _player.SetFacing(this.TargetDestination);

                        this.Velocity = _player.Velocity;
                        this.Velocity = Vector2.Zero;

                        CurrentState = HandState.Shooting;
                        //SoundManager play the effect
                        SoundManager.Instance.Play(0);
                    }
                    this.Position = _player.Position;
                    break;
                case HandState.Shooting:
                    // if hand is in the middle of movement towards the target
                    if (Mouse.GetState().LeftButton != ButtonState.Pressed || ((this.Position - _player.Position).Length() >= this._maxDistanceOfHand))
                    {
                        ChangeStateToRetreating();
                    }
                    else
                    {
                        Velocity = TargetDirection * _shootSpeed * timeElapsed;
                    }

                    break;
                case HandState.Latched:
                    //if hand is latched
                    // If mouse is clicked while on player
                    Velocity = Vector2.Zero;
                    if ((this.Position - _player.Position).Length() >= _maxDistanceOfHand)
                    {
                        ChangeStateToRetreating();
                        break;
                    }
                    else if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        this._player.RappleToHand();
                    }
                    break;
                case HandState.Retreating:
                    // If hand is in the middle of returning to the player
                    if (Vector2.Dot(_player.Position - this.TargetDestination, _player.Position - this.Position) < 0)
                    {
                        this.Velocity = Vector2.Zero;

                        this.Position = _player.Position;
                        this.CurrentState = HandState.OnPlayer;
                        this.CollisionComponent.Layer = CollisionLayers.PlayerHand;
                    }
                    else
                    {
                        var directionReturning = _player.Position - this.Position;
                        directionReturning.Normalize();
                        this.Velocity = directionReturning * _returnSpeed * timeElapsed;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //// Gravity
            //this._velocity += new Vector2(0, 1) * _gravitationalAcceleration * timeElapsed;
            //Console.WriteLine(this.Velocity);
            Vector2 newPosition = PhysicsEngine.MoveTowards(this, Velocity, timeElapsed);
        }


        private void ChangeStateToRetreating()
        {

            //this._renderComponent.Rotation = angle;

            this.CurrentState = HandState.Retreating;
            this.CollisionComponent.Layer = CollisionLayers.Ghost;
        }


        public override void Draw(GameTime gameTime)
        {
            //if (this.CurrentState == HandState.Latched)
            //{
            //    _renderComponent.LoadContent("Character_Hand_Reach");
            //}
            //else
            //{
            //    _renderComponent.LoadContent("Character_Hand_Down");
            //}
            _renderComponent.Draw(gameTime);

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
            this.CurrentState = HandState.Latched;
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <returns></returns>
        private Vector2 GetTargetDestination()
        {
            Point mouseLocation = Mouse.GetState().Position;
            Vector2 worldVector = Vector2.Transform(new Vector2(mouseLocation.X, mouseLocation.Y), Matrix.Invert(Camera.Instance.Transform));
            return worldVector;
        }

        public float CalculateRotation(Vector2 direction)
        {
            return (float)Math.Atan2(direction.Y, direction.X);
        }
    }
}

