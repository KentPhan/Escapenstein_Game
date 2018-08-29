using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using StickyHandGame_C9_RP7.Source.Managers.Classes;
using System.Collections.Generic;
using System.Linq;

namespace StickyHandGame_C9_RP7
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameManager : Game
    {
        /// <summary>
        /// The instance
        /// </summary>
        private static GameManager _instance;
        public static GameManager Instance
        {
            get { return _instance ?? null; }
        }

        //Managers
        private LevelManager _levelManager;

        /// <summary>
        /// Game State
        /// </summary>
        public enum GameState
        {
            Start,
            Level1,
            End
        }
        public GameState State { get; private set; }

        // For keeping track of current entities
        public Entity PlayerEntity { get; set; }
        public List<Entity> NonPlayerEntityList { get; private set; }

        public List<Entity> CollidableNonPlayerEntityList { get; private set; }


        // For Drawing Crap
        public GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;
        public SpriteFont font;
        public bool DebugMode = true;
        public Texture2D titleImage;
        //For Play Backgroud Music
        SoundManager mysoundManager;

        public GameManager()
        {
            _instance = this;
            NonPlayerEntityList = new List<Entity>();
            Content.RootDirectory = "Content";
            Graphics = new GraphicsDeviceManager(GameManager.Instance);
            Graphics.PreferredBackBufferWidth = 2000;
            Graphics.PreferredBackBufferHeight = 1000;
            //Graphics.IsFullScreen = true;
            this.State = GameState.Start;
            mysoundManager = SoundManager.Instance;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _levelManager = LevelManager.Instance;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            CameraManager.Instance.LoadContent();

            font = Content.Load<SpriteFont>("Main");
            IsMouseVisible = true;

            mysoundManager.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            foreach (Entity e in NonPlayerEntityList)
            {
                e.UnloadContent();
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.R))
                RestartGame();

            CameraManager.Instance.Update();
            if (State == GameState.Start)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    string fullPath = Levels.Level1.Path;
                    NonPlayerEntityList = _levelManager.BuildLevelOffOfCSVFile(fullPath);
                    CollidableNonPlayerEntityList =
                        NonPlayerEntityList.Where(i => i.CollisionComponent != null && ((i.CollisionComponent.Layer == CollisionLayers.Static) || (i.CollisionComponent.Layer == CollisionLayers.Trigger))).ToList();

                    State = GameState.Level1;
                }
            }
            else if (State == GameState.Level1)
            {
                // Entity Updates
                PlayerEntity.Update(gameTime);
                foreach (Entity e in NonPlayerEntityList)
                {
                    e.Update(gameTime);
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (State != GameState.Start)
            {
                // Draw Tiles

                foreach (Entity e in NonPlayerEntityList)
                {
                    SpriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, CameraManager.Instance.camera.Transform);
                    e.Draw(gameTime);
                    SpriteBatch.End();
                }






                // Draw Player and Chain
                SpriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, CameraManager.Instance.camera.Transform);
                PlayerEntity.Draw(gameTime);
                SpriteBatch.End();
            }
            else
            {

                SpriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, CameraManager.Instance.camera.Transform);

                SpriteBatch.DrawString(font, "Press Enter To Start", new Vector2(-80, 0), Color.Black);

                SpriteBatch.End();

            }
            base.Draw(gameTime);
        }

        public void RestartGame()
        {

            this.State = GameState.Start;
        }
    }
}
