using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StickyHandGame_C9_RP7.Source.Cameras;
using StickyHandGame_C9_RP7.Source.Components.Collision;
using StickyHandGame_C9_RP7.Source.Entities.Classes.Player;
using StickyHandGame_C9_RP7.Source.Entities.Components;
using StickyHandGame_C9_RP7.Source.Entities.Core;
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
            Pulling,
            Retreating,
        }
        public HandState CurrentState { get; set; }


        //Render component



        /// <summary>
        /// Gets or sets the player.
        /// </summary>
        /// <value>
        /// The player.
        /// </value>
        private PlayerEntity _player { get; set; }
        private Vector2 targetDestination { get; set; }


        public HandEntity(PlayerEntity player)
        {
            this.CurrentState = HandState.OnPlayer;
            this._player = player;
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
                    if (this.CurrentState != HandEntity.HandState.OnPlayer)
                    {
                        //AimTo(Mouse.GetState().Position);
                        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                        {
                            CurrentState = HandEntity.HandState.Shooting;
                        }
                    }
                    break;
                case HandState.Shooting:
                    break;
                case HandState.Latched:
                    break;
                case HandState.Retreating:
                    break;
                case HandState.Pulling:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Reset()
        {
            throw new NotImplementedException();
        }

        public override void CollisionTriggered(Tuple<CollisionComponent, Vector2, Side> collided)
        {
            throw new NotImplementedException();
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public void AimTo()
        {
            Point mouseLocation = Mouse.GetState().Position;
            Vector2 WorldVector = Vector2.Transform(new Vector2(mouseLocation.X, mouseLocation.Y), Matrix.Invert(Camera.Instance.Transform));
            angle = CalculateRotation(WorldVector);
            this.MyrenderComponent.Rotation = angle;
        }

        public float CalculateRotation(Vector2 EndPoint)
        {
            Vector2 Direction = EndPoint - this.Position;
            Direction.Normalize();
            this.direction = Direction;
            return (float)Math.Atan2(Direction.Y, Direction.X);
        }
    }
}
