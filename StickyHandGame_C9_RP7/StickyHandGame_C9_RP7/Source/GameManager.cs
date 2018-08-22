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
        
        //private Texture2D textureBall;
        //private Vector2 ballPosition;
        //float ballSpeed;

        //Managers
        private CollisionManager _collisionManager;
        
        public GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;

        
        // test the basic rendercomponent;
        public PlatformEntity test;
        public PlayerEntity playertest;
        private List<Entity> entityList;
        public GameManager()
        {
            entityList = new List<Entity>();
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            test = new PlatformEntity(this, "Ball");
            this.playertest = new PlayerEntity(this); 
            entityList.Add(test);
            entityList.Add(playertest);
            playertest.position = new Vector2(100, 100);
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
            foreach (Entity e in entityList) {
                e.Initialize();
            }
            
            
            _collisionManager = CollisionManager.Instance;
            _collisionManager.Initialize(entityList);
            

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

            //// TODO: use this.Content to load your game content here
            //textureBall = Content.Load<Texture2D>("ball");

            foreach (Entity e in entityList)
            {
                e.LoadContent();
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            foreach (Entity e in entityList)
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

            // TODO: Add your update logic here
            var kState = Keyboard.GetState();
            
            // Collisions
            _collisionManager.Update(gameTime);
            
            foreach (Entity e in entityList)
            {
                e.Update(gameTime);
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
            //spriteBatch.Draw(textureBall, ballPosition, null, Color.White, 0f, new Vector2(textureBall.Width / 2, textureBall.Height / 2), Vector2.One, SpriteEffects.None, 0f);

            foreach (Entity e in entityList)
            {
                e.Draw(gameTime);
            }

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
