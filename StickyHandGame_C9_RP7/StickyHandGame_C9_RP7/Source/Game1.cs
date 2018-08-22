using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StickyHandGame_C9_RP7.Source.Entities.Classes;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using System;
using System.Collections.Generic;

namespace StickyHandGame_C9_RP7
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        
        private Texture2D textureBall;
        private Vector2 ballPosition;
        float ballSpeed;
        
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        // test the basic rendercomponent;
        public PlatformEntity test;
        private List<Entity> entityList;
        public Game1()
        {
            entityList = new List<Entity>();
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            test = new PlatformEntity(this, "Ball");
            entityList.Add(test);
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
            ballPosition = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
            ballSpeed = 100f;
            foreach (Entity e in entityList) {
                e.Initialize();
            }

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            textureBall = Content.Load<Texture2D>("ball");

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

            if (kState.IsKeyDown(Keys.Up))
                ballPosition.Y -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kState.IsKeyDown(Keys.Down))
                ballPosition.Y += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kState.IsKeyDown(Keys.Left))
                ballPosition.X -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kState.IsKeyDown(Keys.Right))
                ballPosition.X += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kState.IsKeyDown(Keys.Right))
                ballPosition.X += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            ballPosition.X = Math.Min(Math.Max(textureBall.Width / 2, ballPosition.X), graphics.PreferredBackBufferWidth - textureBall.Width / 2);
            ballPosition.Y = Math.Min(Math.Max(textureBall.Height / 2, ballPosition.Y), graphics.PreferredBackBufferHeight - textureBall.Height / 2);

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
            spriteBatch.Begin();
            spriteBatch.Draw(textureBall, ballPosition, null, Color.White, 0f, new Vector2(textureBall.Width / 2, textureBall.Height / 2), Vector2.One, SpriteEffects.None, 0f);

            foreach (Entity e in entityList)
            {
                e.Draw(gameTime);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
