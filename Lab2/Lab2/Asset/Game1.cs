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
        Camera camera;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Sprite dragonBallHero1;
        Sprite dragonBallHero;
        Sprite resume, resumeChosen, options, optionsChosen, exit, exitChosen;
        Sprite gravityUpsideDown, gravityRightsideUp;
        TileMap tileMap;

        SoundEffect jumpEffect;
        Song backgroundMusic;
        GameState gameState;
        PauseMenuChosen pauseMenuChosen = PauseMenuChosen.RESUME;

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
            camera = new Camera(GraphicsDevice.Viewport);
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
            Texture2D sample2 = Texture2D.FromStream(GraphicsDevice, File.OpenRead("Content/mario.png"));

            dragonBallHero1 = new Sprite(1f,sample2, new Vector2(150, 800), SpriteType.BOX);
            dragonBallHero = new Sprite(1f,sample, new Vector2(150, 800), SpriteType.PLAYER);

            Texture2D resumeButtonTexture = Content.Load<Texture2D>("Content/resumeButton");
            Texture2D resumeButtonChosenTexture = Content.Load<Texture2D>("Content/resumeButtonChosen");
            Texture2D optionsButtonTexture = Content.Load<Texture2D>("Content/optionsButton");
            Texture2D optionsButtonChosenTexture = Content.Load<Texture2D>("Content/optionsButtonChosen");
            Texture2D exitButtonTexture = Content.Load<Texture2D>("Content/exitButton");
            Texture2D exitButtonChosenTexture = Content.Load<Texture2D>("Content/exitButtonChosen");
            Texture2D gravityUpsideDownTexture = Content.Load<Texture2D>("Content/gravityUpsideDown");
            Texture2D gravityRightsideUpTexture = Content.Load<Texture2D>("Content/gravityRightsideUp");
            
            resume = new Sprite(1f, resumeButtonTexture, new Vector2(Window.ClientBounds.Width / 2 - 100, Window.ClientBounds.Height / 2 - 300), SpriteType.BUTTON);
            resumeChosen = new Sprite(1f, resumeButtonChosenTexture, new Vector2(Window.ClientBounds.Width / 2 - 100, Window.ClientBounds.Height / 2 - 300), SpriteType.BUTTON);
            options = new Sprite(1f, optionsButtonTexture, new Vector2(Window.ClientBounds.Width / 2 - 100, Window.ClientBounds.Height / 2 - 150), SpriteType.BUTTON);
            optionsChosen = new Sprite(1f, optionsButtonChosenTexture, new Vector2(Window.ClientBounds.Width / 2 - 100, Window.ClientBounds.Height / 2 - 150), SpriteType.BUTTON);
            exit = new Sprite(1f, exitButtonTexture, new Vector2(Window.ClientBounds.Width / 2 - 100, Window.ClientBounds.Height / 2), SpriteType.BUTTON);
            exitChosen = new Sprite(1f, exitButtonChosenTexture, new Vector2(Window.ClientBounds.Width / 2 - 100, Window.ClientBounds.Height / 2), SpriteType.BUTTON);

            gravityUpsideDown = new Sprite(1f, gravityUpsideDownTexture, new Vector2(300,900), SpriteType.GRAVITY);
            gravityRightsideUp = new Sprite(1f, gravityRightsideUpTexture, new Vector2(1400, 200), SpriteType.GRAVITY);
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
                gameState = GameState.PAUSEMENU;
            }
            if (inputManger.gameState)
            {
                gameState = GameState.GAMEPLAY;
            }

            switch (gameState)
            {
                case GameState.GAMEPLAY:
                    dragonBallHero.Update(gameTime, jumpEffect);
                    dragonBallHero1.Update(gameTime, jumpEffect);
                    gravityUpsideDown.Update(gameTime, jumpEffect);
                    gravityRightsideUp.Update(gameTime, jumpEffect);

                    isCollision = false;
                    if (dragonBallHero.boundingBox.Intersects(dragonBallHero1.boundingBox) && inputManger.action)
                    {
                        isCollision = true;
                        dragonBallHero1.setPosition(dragonBallHero.position.X, dragonBallHero.position.Y);
                    }

                    if (dragonBallHero.boundingBox.Intersects(gravityUpsideDown.boundingBox) && inputManger.action)
                    {
                        dragonBallHero.gravity = false;
                    }
                    if (dragonBallHero.boundingBox.Intersects(gravityRightsideUp.boundingBox) && inputManger.action)
                    {
                        dragonBallHero.gravity = true;
                    }
                    //if ((dragonBallHero.boundingBox.Contains(dragonBallHero1.boundingBox) == ContainmentType.Intersects || dragonBallHero.boundingSphere.Contains(dragonBallHero1.boundingSphere) == ContainmentType.Intersects) && inputManger.action)
                    //{
                    //    isCollision = true;
                    //    dragonBallHero1.setPosition(dragonBallHero.position.X, dragonBallHero.position.Y);
                    //}
                    break;
                case GameState.PAUSEMENU:
                    resume.Update(gameTime, jumpEffect);
                    resumeChosen.Update(gameTime, jumpEffect);
                    options.Update(gameTime, jumpEffect);
                    optionsChosen.Update(gameTime, jumpEffect);
                    exit.Update(gameTime, jumpEffect);
                    exitChosen.Update(gameTime, jumpEffect);
                    if(inputManger.up)
                    {
                        if(pauseMenuChosen != PauseMenuChosen.RESUME)
                        {
                            pauseMenuChosen--;
                        }
                    }
                    if(inputManger.down)
                    {
                        if (pauseMenuChosen < PauseMenuChosen.EXIT)
                        {
                            pauseMenuChosen++;
                        }
                    }
                    if(inputManger.action)
                    {
                        switch (pauseMenuChosen)
                        {
                            case PauseMenuChosen.RESUME:
                                inputManger.gameState = true;
                                break;
                            case PauseMenuChosen.OPTIONS:
                                break;
                            case PauseMenuChosen.EXIT:
                                Exit();
                                break;
                        }
                    }
                    break;
            }


            camera.Update(gameTime, dragonBallHero,tileMap);
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
                case GameState.GAMEPLAY:
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, camera.transform);
                    tileMap.Draw(spriteBatch);
                    spriteBatch.DrawString(font, (isCollision == true) ? "We stick together" : "We are apart", new Vector2(100, 20), Color.Black);
                    gravityUpsideDown.Draw(spriteBatch);
                    gravityRightsideUp.Draw(spriteBatch);
                    dragonBallHero1.Draw(spriteBatch);
                    dragonBallHero.Draw(spriteBatch);
                    
                    spriteBatch.End();
                    break;
                case GameState.PAUSEMENU:
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    spriteBatch.Begin();

                    resume.Draw(spriteBatch);
                    if(pauseMenuChosen == PauseMenuChosen.RESUME)
                    {
                        resumeChosen.Draw(spriteBatch);
                    }
                    options.Draw(spriteBatch);
                    if (pauseMenuChosen == PauseMenuChosen.OPTIONS)
                    {
                        optionsChosen.Draw(spriteBatch);
                    }
                    exit.Draw(spriteBatch);
                    if (pauseMenuChosen == PauseMenuChosen.EXIT)
                    {
                        exitChosen.Draw(spriteBatch);
                    }

                    spriteBatch.End();
                    break;
            }
            
            base.Draw(gameTime);
        }
    }
}
