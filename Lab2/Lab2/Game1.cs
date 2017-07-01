using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Xml.Linq;
using ToA;


namespace Lab2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 
    public class Game1 : Game
    {
        private static Game1 instance;
        public static Game1 Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Game1();
                }
                return instance;
            }
        }
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public TileMap TileMap;
        public int levelNumber { get; set; } = 1;
        public int NumberOfLevels { get; set; } = 0;

        InputManager inputManger;
        DisplayManger displayManager;
        SoundManager soundManager;
        public float musicVolume = 0.1f;
        public float spriteEffectVolume = 0.1f;

        public InputManager InputManager
        {
            get
            {
                return inputManger;
            }
        }
        public DisplayManger DisplayManager
        {
            get
            {
                return displayManager;
            }
        }

        public SoundManager SoundManager
        {
            get
            {
                return soundManager;
            }
        }

        public bool isFinishing = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            inputManger = new InputManager();
            soundManager = new SoundManager();
            
        }

        public void Quit()
        {
            this.Exit();
        }
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            
            base.Initialize();
        }

        

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
          
            spriteBatch = new SpriteBatch(GraphicsDevice);

            displayManager = new DisplayManger(GraphicsDevice, Content, Window );
            displayManager.Load();

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            inputManger.Update();

            displayManager.Update(gameTime);
            base.Update(gameTime);
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            displayManager.Draw(spriteBatch);
            
            base.Draw(gameTime);
        }
    }
}
