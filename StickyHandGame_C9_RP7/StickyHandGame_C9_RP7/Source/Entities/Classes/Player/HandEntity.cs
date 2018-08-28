using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StickyHandGame_C9_RP7.Source.Cameras;
using StickyHandGame_C9_RP7.Source.Components.Collision;
using StickyHandGame_C9_RP7.Source.Components.Render;
using StickyHandGame_C9_RP7.Source.Engine;
using StickyHandGame_C9_RP7.Source.Entities.Classes.Player;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using System;
using System.Diagnostics;

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
        private Vector2 TargetDestination { get; set; }
        private float _speed = 200.0f;

        private float _maxDistanceOfHand = 500.0f;

        // TODO Switch this to collision Layer system instead
        private Vector2 _velocity = new Vector2();


        public HandEntity(PlayerEntity player)
        {
            this.CurrentState = HandState.OnPlayer;
            this._player = player;

            // Render
            _renderComponent = new RenderComponent("Character_Hand_Down", this, HandEntry.HandOrigin);
            _renderComponent.LoadContent();
            _renderComponent.Scale = HandEntry.Scale;

            this.chainTexture = GameManager.Instance.Content.Load<Texture2D>("Chain");


            this.Width = 32f;
            this.Height = 32f;

            this.CollisionComponent = new BoxColliderComponent(this, this.Width, this.Height, CollisionLayers.PlayerHand);
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
                        this.TargetDestination = GetTarget();

                        float angle = CalculateRotation(this.TargetDestination, this.Position);
                        this._renderComponent.Rotation = angle;

                        CurrentState = HandState.Shooting;
                    }
                    this.Position = _player.Position;
                    break;
                case HandState.Shooting:
                    // if hand is in the middle of movement towards the target
                    if (Mouse.GetState().RightButton == ButtonState.Pressed || ((this.Position - _player.Position).Length() >= this._maxDistanceOfHand))
                    {
                        ChangeStateToRetreating();
                    }
                    else
                    {
                        var directionShooting = this.TargetDestination - this.Position;
                        directionShooting.Normalize();
                        _velocity += directionShooting * _speed;
                    }

                    break;
                case HandState.Latched:
                    //if hand is latched
                    // If mouse is clicked while on player
                    _velocity = Vector2.Zero;
                    if ((this.Position - _player.Position).Length() >= _maxDistanceOfHand)
                    {
                        ChangeStateToRetreating();
                        break;
                    }
                    if (Mouse.GetState().RightButton == ButtonState.Pressed)
                    {
                        this._player.RappleToHand();

                    }
                    break;
                case HandState.Retreating:
                    // If hand is in the middle of returning to the player
                    if (Vector2.Dot(_player.Position - this.TargetDestination, _player.Position - this.Position) < 0)
                    {
                        this._velocity = Vector2.Zero;

                        this.Position = _player.Position;
                        this.CurrentState = HandState.OnPlayer;
                        this.CollisionComponent.Layer = CollisionLayers.PlayerHand;
                    }
                    else
                    {
                        var directionReturning = _player.Position - this.Position;
                        directionReturning.Normalize();

                        this.Position += directionReturning * _speed;
                        //this._velocity += directionReturning * _speed;
                        return;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            Debug.WriteLine(this.CurrentState + " " + this.CollisionComponent.Layer);
            Vector2 newPostion = PhysicsEngine.MoveTowards(this, _velocity, timeElapsed);

        }


        private void ChangeStateToRetreating()
        {
            float angle = CalculateRotation(_player.Position, this.Position);
            this._renderComponent.Rotation = angle;

            this.CurrentState = HandState.Retreating;
            this.CollisionComponent.Layer = CollisionLayers.Ghost;
        }


        public override void Draw(GameTime gameTime)
        {
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
        private Vector2 GetTarget()
        {
            Point mouseLocation = Mouse.GetState().Position;
            Vector2 worldVector = Vector2.Transform(new Vector2(mouseLocation.X, mouseLocation.Y), Matrix.Invert(Camera.Instance.Transform));
            return worldVector;
        }

        public float CalculateRotation(Vector2 destination, Vector2 position)
        {
            Vector2 Direction = destination - position;
            Direction.Normalize();
            return (float)Math.Atan2(Direction.Y, Direction.X);
        }
    }
}
