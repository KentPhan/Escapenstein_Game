using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StickyHandGame_C9_RP7.Source.Entities.Core;
using StickyHandGame_C9_RP7.Source.Managers.Classes;
using System;
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
                //List<List<Tiles>> level1 = new List<List<Tiles>>()
                //{
                //    new List<Tiles>() { Tiles.Tile_34_C, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Tile_34_C },
                //    new List<Tiles>() { Tiles.Tile_34_C, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Tile_34_C },
                //    new List<Tiles>() { Tiles.Tile_34_C, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Tile_34_C },
                //    new List<Tiles>() { Tiles.Tile_34_C, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Tile_34_C, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Tile_34_C },
                //    new List<Tiles>() { Tiles.Tile_34_C, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Tile_34_C },
                //    new List<Tiles>() { Tiles.Tile_34_C, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Tile_34_C },
                //    new List<Tiles>() { Tiles.Tile_34_C, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Tile_34_C },
                //    new List<Tiles>() { Tiles.Tile_34_C, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Nothing, Tiles.Tile_34_C },
                //    new List<Tiles>() { Tiles.Tile_34_C, Tiles.Tile_34_C, Tiles.Tile_34_C, Tiles.Tile_34_C, Tiles.Tile_34_C, Tiles.Tile_34_C, Tiles.Tile_34_C, Tiles.Tile_34_C, Tiles.Tile_34_C, Tiles.Tile_34_C },
                //    new List<Tiles>() { Tiles.Tile_34_C, Tiles.Tile_34_C, Tiles.Tile_34_C, Tiles.Tile_34_C, Tiles.Tile_34_C, Tiles.Tile_34_C, Tiles.Tile_34_C, Tiles.Tile_34_C, Tiles.Tile_34_C, Tiles.Tile_34_C },
                //    new List<Tiles>() { Tiles.Tile_34_C, Tiles.Tile_34_C, Tiles.Tile_34_C, Tiles.Tile_34_C, Tiles.Tile_34_C, Tiles.Tile_34_C, Tiles.Tile_34_C, Tiles.Tile_34_C, Tiles.Tile_34_C, Tiles.Tile_34_C },
                //};

                //NonPlayerEntityList = _levelManager.GenerateLevel(level1);
                //CollidableNonPlayerEntityList =
                //    NonPlayerEntityList.Where(i => i.CollisionComponent != null && i.CollisionComponent.Layer == CollisionLayers.Static).ToList();
                //PlayerEntity = new PlayerEntity { Position = new Vector2(200, 200) };


                string fullPath = Environment.CurrentDirectory + @"..\..\..\..\..\Content\Levels\Test_Map_RPT_Foreground.csv";
                NonPlayerEntityList = _levelManager.BuildLevelOffOfCSVFile(fullPath);
                CollidableNonPlayerEntityList =
                    NonPlayerEntityList.Where(i => i.CollisionComponent != null && ((i.CollisionComponent.Layer == CollisionLayers.Static) || (i.CollisionComponent.Layer == CollisionLayers.Trigger))).ToList();

                State = GameState.Level1;
            }
            else if (State == GameState.Level1)
            {
                CameraManager.Instance.Update();

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

            // Draw Tiles
            SpriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, CameraManager.Instance.camera.Transform);
            foreach (Entity e in NonPlayerEntityList)
            {
                e.Draw(gameTime);
            }
            SpriteBatch.End();

            // Draw Player and Chain
            SpriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, CameraManager.Instance.camera.Transform);
            PlayerEntity.Draw(gameTime);
            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
