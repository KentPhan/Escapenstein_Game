using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StickyHandGame_C9_RP7.Source.Components.Collision;
using StickyHandGame_C9_RP7.Source.Components.Render;
using StickyHandGame_C9_RP7.Source.Entities.Components;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using StickyHandGame_C9_RP7.Source.Managers;
using System;

namespace StickyHandGame_C9_RP7.Source.Entities.Classes
{
    public class PlayerEntity : Entity
    {
        int[] framNumber = RenderManager.zombieAnimatedAttribute.framNumber;
        int[] playSpeed = RenderManager.zombieAnimatedAttribute.playSpeed;
        String[] Names = RenderManager.zombieAnimatedAttribute.Names;
        AnimationComponent myAnimationComponent;

        // PhysicsEngine
        private readonly float _jumpforce = 400f;
        private readonly float _gravitationalAcceleration = 400f;
        private readonly float _runningSpeed = 300f;
        private Vector2 _velocity = new Vector2();
        private Vector2 previousposition;


        // The Hand 
        private ThrowAbleEntity HandChain;

        // States
        public enum CharacterState
        {
            Airbourne,
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

            HandChain = new ThrowAbleEntity(this.Position+HandEntry.HndPositionOffSet,this);

            // Load Content
            var texture = myAnimationComponent.LoadContent();
            this.Hide = hide;
            Width = 32f;
            Height = 32f;

            this.CollisionComponent = new BoxColliderComponent(this, this.Width, this.Height, CollisionLayers.Player);

            this.State = CharacterState.Airbourne;
        }

        public override void Draw(GameTime gameTime)
        {
            HandChain.Draw(gameTime);
            myAnimationComponent.Draw(gameTime);
        }

        public override void Reset()
        {
        }

        public override object Clone()
        {
            var cloned = new PlayerEntity(false);
            cloned.Position = this.Position;
            return cloned;
        }

        public override void UnloadContent()
        {
        }

        public override void CollisionTriggered(Tuple<CollisionComponent, Vector2, Side> collided)
        {
            //based upon the direction, react
            switch (collided.Item3)
            {
                case Side.Top:
                    this.State = CharacterState.Standing;
                    this._velocity = new Vector2(_velocity.X, 0);
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


        public override void Update(GameTime gameTime)
        {
            Vector2 deltaMovement = this.Position - this.previousposition;
            this.previousposition = this.Position;
            this.HandChain.Update(gameTime, deltaMovement);

            var kState = Keyboard.GetState();
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Based upon current state
            switch (this.State)
            {
                case CharacterState.Standing:
                    if (kState.IsKeyDown(Keys.Up))
                    {
                        _velocity += new Vector2(0, -1) * _jumpforce;
                        this.State = CharacterState.Airbourne;
                    }
                    else
                    {

                    }

                    break;
                case CharacterState.Airbourne:

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Left and right movement
            if (kState.IsKeyDown(Keys.Left))
            {
                this._velocity += new Vector2(-1, 0) * _runningSpeed * timeElapsed;
                this.myAnimationComponent.myeffect = SpriteEffects.FlipHorizontally;
            }
            else
            {
                if (this._velocity.X < 0)
                    this._velocity.X = 0;
            }
            if (kState.IsKeyDown(Keys.Right))
            {
                this._velocity += new Vector2(1, 0) * _runningSpeed * timeElapsed;
                this.myAnimationComponent.myeffect = SpriteEffects.None;
            }
            else
            {
                if (this._velocity.X > 0)
                    this._velocity.X = 0;
            }

            // Gravity
            this._velocity += new Vector2(0, 1) * _gravitationalAcceleration * timeElapsed;

            //Debug.WriteLine(this.State + $" {_velocity.X} {_velocity.Y}");
            PhysicsEngine.MoveTowards(this, _velocity, timeElapsed);
            myAnimationComponent.Update(gameTime);
        }
    }
}
