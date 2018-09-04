using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using StickyHandGame_C9_RP7.Source.Managers.Core;
using System.Collections.Generic;

namespace StickyHandGame_C9_RP7.Source.Managers.Classes
{


    public class LevelManager
    {
        /// <summary>
        /// The instance
        /// </summary>
        private static LevelManager _instance;
        public static LevelManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LevelManager();
                }
                return _instance;
            }
        }
        private LevelManager()
        {

        }

        public void Initialize()
        {
            _currentLevel = Level.Start();
        }

        /// <summary>
        /// The current level
        /// </summary>
        private Level _currentLevel;

        /// <summary>
        /// Level Update
        ///
        /// TODO ON SWITCHING LEVELS NEED TO UNLOAD RESOURCES
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void LevelUpdate(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                GameManager.Instance.Exit();

            if (_currentLevel.Enum == LevelEnum.Start)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    _currentLevel = Level.Level_1();
                }
            }
            else if (_currentLevel.Enum == LevelEnum.Credits)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                    _currentLevel = Level.Start();
            }
            else
            {
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                    _currentLevel = Level.Start();

                if (Keyboard.GetState().IsKeyDown(Keys.D1))
                    _currentLevel = Level.Level_1();

                if (Keyboard.GetState().IsKeyDown(Keys.D2))
                    _currentLevel = Level.Level_2();

                _currentLevel.Update(gameTime);
            }


            CameraManager.Instance.Update();
        }

        public void LevelDraw(GameTime gameTime)
        {
            _currentLevel.Draw(gameTime);
        }


        public void RestartGame()
        {
            this._currentLevel = Level.Start();
        }

        /// <summary>
        /// Gets the current physical world for the physics/collision system.
        /// </summary>
        /// <returns></returns>
        public List<Entity> GetCurrentPhysicalLevel()
        {
            return _currentLevel.ForegroundEntities;
        }


        public Entity GetCurrentPlayerInLevel()
        {
            return _currentLevel.PlayerEntity;
        }

        /// <summary>
        /// Resets the player position.
        /// </summary>
        public void ResetCurrentPlayerPosition()
        {
            _currentLevel.MovePlayerToStartPosition();
        }

    }
}

