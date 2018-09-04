using Microsoft.Xna.Framework;
using StickyHandGame_C9_RP7.Source.Components.Render;
using StickyHandGame_C9_RP7.Source.Engine;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using System;
using StickyHandGame_C9_RP7.Source.Managers.Classes;

namespace StickyHandGame_C9_RP7.Source.Entities.Classes
{
    public class EmptyEntity : Entity
    {
        int[] framNumber = RenderManager.zombieAnimatedAttribute.framNumber;
        String[] Names = RenderManager.zombieAnimatedAttribute.Names;

        public RenderComponent renderComponent;
        private readonly AnimationComponent _animationComponent;
        private bool isAnimation;
        private float _lifeSpan;
        private float _currentLifeSpan;
        private bool _hasLifeSpan;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyEntity"/> class.
        ///
        /// Used for Background Crap
        /// </summary>
        /// <param name="tileName">Name of the tile.</param>
        /// <param name="position">The position.</param>
        public EmptyEntity(string tileName, Vector2 position) : base()
        {
            renderComponent = new RenderComponent(tileName, this);
            this.renderComponent.LoadContent();
            this.Position = position;
            isAnimation = false;

            Width = 32;
            Height = 32;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyEntity"/> class.
        /// Used for Temp animations
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="animationName">Name of the animation.</param>
        /// <param name="lifeSpan">The life span.</param>
        /// <param name="dieOnEnd">if set to <c>true</c> [die on end].</param>
        public EmptyEntity(Vector2 position, string animationName, float lifeSpan, bool dieOnEnd) : base()
        {
            this.Position = position;


            _animationComponent = new AnimationComponent(animationName, this,
                new int[] { 6 },
                new int[] { 100 },
                new string[] { animationName },
                32,
                32,
                new Vector2(1, 1));
            _animationComponent.LoadContent();
            _lifeSpan = lifeSpan;
            _currentLifeSpan = 0.0f;
            _hasLifeSpan = dieOnEnd;
            isAnimation = true;

            Width = 32;
            Height = 32;
        }

        public override void UnloadContent()
        {
            //throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            if (isAnimation && _hasLifeSpan)
            {
                _animationComponent.Update(gameTime);
                _currentLifeSpan += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_currentLifeSpan > _lifeSpan)
                {
                    LevelManager.Instance.DestroyTempEntity(this);
                }
            }

            return;
        }

        public override void Draw(GameTime gameTime)
        {
            if (!Hide)
            {
                if (isAnimation)
                {
                    this._animationComponent.Draw(gameTime);
                }
                else
                {
                    this.renderComponent.Draw(gameTime, Color.White);
                }
            }

        }

        public override void Reset()
        {
            throw new NotImplementedException();
        }

        public override void CollisionTriggered(CollisionInfo collided)
        {
            throw new NotImplementedException();
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
