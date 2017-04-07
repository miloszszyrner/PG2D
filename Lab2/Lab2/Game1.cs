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
        Sprite dragonBallHero1;
        Sprite dragonBallHero;
        TileMap tileMap;
        SoundEffect jumpEffect;
        Song backgroundMusic;
        GameState gameState;

        public TileMap TileMap
        {
            get
            {
                return tileMap;
            }
        }

        InputManager inputManger;

        public InputManager InputManager
        {
            get
            {
                return inputManger;
            }
        }

        private SpriteFont font;
        private bool isCollision = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            inputManger = new InputManager();
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
            graphics.IsFullScreen = true;
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
            Texture2D sample = Texture2D.FromStream(GraphicsDevice, File.OpenRead("Content/chodz.png"));
            dragonBallHero1 = new Sprite(1.0f,sample, new Vector2(500, 100),false);
            dragonBallHero = new Sprite(0.5f,sample, new Vector2(150, 800),true);
            font = Content.Load<SpriteFont>("Content/Tekst");
            tileMap = new TileMap("Content/level_1.tmx", "Content/spritesheet", Content);
            jumpEffect = Content.Load<SoundEffect>("Content/jump");
            backgroundMusic = Content.Load<Song>("Content/backgroundMusic");
		
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.1f;
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

            if (inputManger.back) 
            {
                Exit();
            }
            if (!inputManger.gameState)
            {
                gameState = GameState.PauseMenu;
            }
            if (inputManger.gameState)
            {
                gameState = GameState.Gameplay;
            }

            switch (gameState)
            {
                case GameState.Gameplay:
                    dragonBallHero.Update(gameTime, jumpEffect);
                    dragonBallHero1.Update(gameTime, jumpEffect);

                    isCollision = false;
                    if (dragonBallHero.boundingBox.Contains(dragonBallHero1.boundingBox) == ContainmentType.Intersects || dragonBallHero.boundingSphere.Contains(dragonBallHero1.boundingSphere) == ContainmentType.Intersects)
                    {
                        isCollision = true;
                    }
                    break;
                case GameState.PauseMenu:
                    
                    break;
            }

            

                base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            switch(gameState)
            {
                case GameState.Gameplay:
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap);
                    tileMap.Draw(spriteBatch);
                    spriteBatch.DrawString(font, (isCollision == true) ? "We stick together" : "We are apart", new Vector2(100, 20), Color.Black);
                    //dragonBallHero1.Draw(spriteBatch);
                    dragonBallHero.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
                case GameState.PauseMenu:
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    spriteBatch.Begin();
                    spriteBatch.DrawString(font,"PAUSE", new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2), Color.Black);
                    dragonBallHero1.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
            }
            
            base.Draw(gameTime);
        }
    }
}
