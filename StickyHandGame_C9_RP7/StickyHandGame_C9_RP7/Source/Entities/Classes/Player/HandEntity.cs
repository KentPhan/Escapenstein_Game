using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StickyHandGame_C9_RP7.Source.Cameras;
using StickyHandGame_C9_RP7.Source.Components.Collision;
using StickyHandGame_C9_RP7.Source.Components.Render;
using StickyHandGame_C9_RP7.Source.Entities.Classes.Player;
using StickyHandGame_C9_RP7.Source.Entities.Components;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using System;
using System.Diagnostics;
using StickyHandGame_C9_RP7.Source.Engine;

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


        /// <summary>
        /// Gets or sets the player.
        /// </summary>
        /// <value>
        /// The player.
        /// </value>
        private PlayerEntity _player { get; set; }


        // Physics
        private Vector2 OriginalPosition { get; set; }
        private Vector2 TargetDestination { get; set; }
        private float _speed = 10.0f;

        private float _maxDistanceOfHand = 1000.0f;

        // TODO Switch this to collision Layer system instead
        public float HandCatchDistance => 32.0f;
        private Vector2 _velocity = new Vector2();


        public HandEntity(PlayerEntity player)
        {
            this.CurrentState = HandState.OnPlayer;
            this._player = player;

            // Render
            _renderComponent = new RenderComponent("Character_Hand_Down", this, HandEntry.HandOrigin);
            _renderComponent.LoadContent();
            _renderComponent.Scale = HandEntry.Scale;
            this.Width = 32f;
            this.Height = 32f;

            this.CollisionComponent = new BoxColliderComponent(this, this.Width, this.Height, CollisionLayers.Player);
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
                        this.OriginalPosition = this.Position;
                        this.TargetDestination = GetTarget();

                        float angle = CalculateRotation(this.TargetDestination, this.Position);
                        this._renderComponent.Rotation = angle;

                        CurrentState = HandState.Shooting;
                    }
                    this.Position = _player.Position;
                    break;
                case HandState.Shooting:
                    // if hand is in the middle of movement towards the target
                    if (Mouse.GetState().RightButton == ButtonState.Pressed || ((this.Position - this.OriginalPosition).Length() >= this._maxDistanceOfHand))
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
                    if (Mouse.GetState().RightButton == ButtonState.Pressed)
                    {
                        this._player.RappleToHand();

                    }
                    break;
                case HandState.Retreating:
                    // If hand is in the middle of returning to the player
                    if ((_player.Position - this.Position).Length() <= this.HandCatchDistance)
                    {
                        this._velocity = Vector2.Zero;

                        this.Position = _player.Position;
                        this.CurrentState = HandState.OnPlayer;
                        this.CollisionComponent.Layer = CollisionLayers.Player;
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
        }

        public override void Reset()
        {
            throw new NotImplementedException();
        }

        public override void CollisionTriggered(Tuple<CollisionComponent, Vector2, Side> collided)
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
