using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StickyHandGame_C9_RP7.Source.Entities.Classes;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using System;
using System.Collections.Generic;
using StickyHandGame_C9_RP7.Source.Managers;
using StickyHandGame_C9_RP7.Source.Managers.Classes;

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
        private CollisionManager _collisionManager;

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
        public Entity PlayerEntity { get; private set; }
        public List<Entity> NonPlayerEntityList { get; private set; }


        // For Drawing Crap
        public GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;

        public GameManager()
        {
            _instance = this;
            NonPlayerEntityList = new List<Entity>();
            Content.RootDirectory = "Content";
            Graphics = new GraphicsDeviceManager(GameManager.Instance);
            Graphics.PreferredBackBufferWidth = 1000;
            Graphics.PreferredBackBufferHeight = 1000;
            //Graphics.IsFullScreen = true;
            this.State = GameState.Start;
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

            _collisionManager = CollisionManager.Instance;
            _collisionManager.Initialize(NonPlayerEntityList);

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
            //
            IsMouseVisible = true;
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


            if (State == GameState.Start)
            {
                PlatformEntity plat = new PlatformEntity();
                NonPlayerEntityList.Add(plat);
                plat.Position = new Vector2(600, 800);

                PlatformEntity plat2 = new PlatformEntity();
                NonPlayerEntityList.Add(plat2);
                plat2.Position = new Vector2(200, 1000);

                PlayerEntity = new PlayerEntity {Position = new Vector2(200, 200)};
                CollisionManager.Instance.AssignPlayerEntity(PlayerEntity);


                State = GameState.Level1;
            }
            else if (State == GameState.Level1)
            {
                CameraManager.Instance.Update();
                // Collision Updates
                _collisionManager.Update(gameTime);

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

            // TODO: Add your drawing code here
            // Changed the begin for camera
            SpriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, CameraManager.Instance.camaer.Transform);
           // SpriteBatch.Begin();
            PlayerEntity.Draw(gameTime);
            foreach (Entity e in NonPlayerEntityList)
            {
                e.Draw(gameTime);
            }

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
