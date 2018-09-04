using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using StickyHandGame_C9_RP7.Source.Managers.Core;
using System.Collections.Generic;
using StickyHandGame_C9_RP7.Source.Cameras;

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
            _currentLevel = Level.Start();
            this.CurrentCamera = new Camera(GameManager.Instance.GraphicsDevice.Viewport, CameraType.Screen);
        }

        /// <summary>
        /// The current level
        /// </summary>
        private Level _currentLevel;

        public Camera CurrentCamera { get; private set; }

        private KeyboardState _previousState;
        private KeyboardState _currentState;

        /// <summary>
        /// Level Update
        ///
        /// TODO ON SWITCHING LEVELS NEED TO UNLOAD RESOURCES
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public void LevelUpdate(GameTime gameTime)
        {
            if (_currentState != null)
                _previousState = _currentState;
            else
                _previousState = Keyboard.GetState();
            _currentState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                GameManager.Instance.Exit();

            switch (_currentLevel.Enum)
            {
                case LevelEnum.Start:
                    {
                        if (_currentState.IsKeyUp(Keys.Enter) && _previousState.IsKeyDown(Keys.Enter))
                        {
                            NextLevel();
                        }
                        break;
                    }
                case LevelEnum.Credits:
                    {
                        if (_currentState.IsKeyUp(Keys.Enter) && _previousState.IsKeyDown(Keys.Enter))
                            NextLevel();
                        break;
                    }
                default:
                    {
                        if (_currentState.IsKeyDown(Keys.OemTilde))
                            SwitchToLevel(LevelEnum.Start);

                        if (_currentState.IsKeyDown(Keys.D1))
                            SwitchToLevel(LevelEnum.Level1);

                        if (_currentState.IsKeyDown(Keys.D2))
                            SwitchToLevel(LevelEnum.Level2);

                        if (_currentState.IsKeyDown(Keys.D3))
                            SwitchToLevel(LevelEnum.Credits);

                        _currentLevel.Update(gameTime);
                        break;
                    }
            }
            CurrentCamera.UpdateCamera(GameManager.Instance.GraphicsDevice.Viewport);
        }

        public void LevelDraw(GameTime gameTime)
        {
            _currentLevel.Draw(gameTime);
        }


        /// <summary>
        /// Switches to level.
        /// </summary>
        /// <param name="level">The level.</param>
        private void SwitchToLevel(LevelEnum level)
        {
            switch (level)
            {
                case LevelEnum.Start:
                    this.CurrentCamera = new Camera(GameManager.Instance.GraphicsDevice.Viewport, CameraType.Screen);
                    _currentLevel = Level.Start();
                    break;
                case LevelEnum.Level1:
                    this.CurrentCamera = new Camera(GameManager.Instance.GraphicsDevice.Viewport, CameraType.Follow);
                    _currentLevel = Level.Level_1();
                    break;
                case LevelEnum.Level2:
                    this.CurrentCamera = new Camera(GameManager.Instance.GraphicsDevice.Viewport, CameraType.Follow);
                    _currentLevel = Level.Level_2();
                    break;
                case LevelEnum.Credits:
                    this.CurrentCamera = new Camera(GameManager.Instance.GraphicsDevice.Viewport, CameraType.Screen);
                    _currentLevel = Level.Credits();
                    break;
            }
        }


        #region Public accessors and Triggers        

        /// <summary>
        /// Go to next level
        /// </summary>
        public void NextLevel()
        {
            switch (_currentLevel.Enum)
            {
                case LevelEnum.Start:
                    SwitchToLevel(LevelEnum.Level1);
                    break;
                case LevelEnum.Level1:
                    SwitchToLevel(LevelEnum.Level2);
                    break;
                case LevelEnum.Level2:
                    SwitchToLevel(LevelEnum.Credits);
                    break;
                case LevelEnum.Credits:
                    SwitchToLevel(LevelEnum.Start);
                    break;
            }
        }

        /// <summary>
        /// Triggers the credits.
        /// </summary>
        public void TriggerCredits()
        {
            SwitchToLevel(LevelEnum.Credits);
        }

        /// <summary>
        /// Restarts the game.
        /// </summary>
        public void RestartGame()
        {
            SwitchToLevel(LevelEnum.Start);
        }

        /// <summary>
        /// Gets the current physical world for the physics/collision system.
        /// </summary>
        /// <returns></returns>
        public List<Entity> GetCurrentPhysicalLevel()
        {
            return _currentLevel.ForegroundEntities;
        }


        /// <summary>
        /// Gets the current player in level.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Spawns the temporary entity.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="entity">The entity.</param>
        public void SpawnTempEntity(int id, Entity entity)
        {
            _currentLevel.TempEntities.Add(id, entity);
        }

        /// <summary>
        /// Destroys the temporary entity.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void DestroyTempEntity(Entity entity)
        {
            _currentLevel.ToDelete.Add(entity);
        }

        #endregion

    }
}