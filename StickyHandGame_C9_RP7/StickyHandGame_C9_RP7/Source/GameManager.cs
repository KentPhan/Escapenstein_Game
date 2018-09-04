using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private LevelManager _levelManager;

        // High level crap
        public GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;
        public bool DebugMode = true;
        public Texture2D titleImage;
        SoundManager mysoundManager;


        // TODO TEMP
        public SpriteFont Font;

        public GameManager()
        {
            _instance = this;
            Content.RootDirectory = "Content";
            Graphics = new GraphicsDeviceManager(GameManager.Instance);
            Graphics.PreferredBackBufferWidth = 2000;
            Graphics.PreferredBackBufferHeight = 1000;
            //Graphics.IsFullScreen = true;
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
            _levelManager.Initialize();
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

            Font = Content.Load<SpriteFont>("Main");
            IsMouseVisible = true;

            mysoundManager.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO Unload content
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _levelManager.LevelUpdate(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);// TODO WTF DOES THIS DO???
            _levelManager.LevelDraw(gameTime);
            base.Draw(gameTime);
        }


    }
}
