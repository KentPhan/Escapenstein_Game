using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StickyHandGame_C9_RP7.Source.Entities.Classes;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using System;
using System.Collections.Generic;
using StickyHandGame_C9_RP7.Source.Managers;

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
        private readonly List<Entity> currentEntityList;


        // For Drawing Crap
        public GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;

        public GameManager()
        {
            _instance = this;
            currentEntityList = new List<Entity>();
            Content.RootDirectory = "Content";
            Graphics = new GraphicsDeviceManager(GameManager.Instance);
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
            _collisionManager.Initialize(currentEntityList);

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
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            foreach (Entity e in currentEntityList)
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
                PlatformEntity plat = new PlatformEntity("platform");
                currentEntityList.Add(plat);
                plat.Position = new Vector2(200, 400);

                PlayerEntity player = new PlayerEntity();
                currentEntityList.Add(player);
                player.Position = new Vector2(200, 200);


                State = GameState.Level1;
            }
            else if (State == GameState.Level1)
            {

                // Collision Updates
                _collisionManager.Update(gameTime);

                // Entity Updates
                foreach (Entity e in currentEntityList)
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
            SpriteBatch.Begin();

            foreach (Entity e in currentEntityList)
            {
                e.Draw(gameTime);
            }

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
