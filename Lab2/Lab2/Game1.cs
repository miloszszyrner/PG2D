using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace Lab2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Sprite dragonBallHero1;
        Sprite dragonBallHero;
        private SpriteFont font;
        private bool isCollision = false;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //Content.RootDirectory = "Content";
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
            Texture2D sample = Texture2D.FromStream(GraphicsDevice, File.OpenRead("Content/dragonball.png"));
            dragonBallHero1 = new Sprite(sample, new Vector2(500, 100));
            dragonBallHero = new Sprite(sample, new Vector2(50, 50));
            font = Content.Load<SpriteFont>("Content/Tekst");
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var keyboardState = Keyboard.GetState();

            if(keyboardState.IsKeyDown(Keys.W))
            {
                dragonBallHero.position.Y -= 100 * gameTime.ElapsedGameTime.Milliseconds / 1000F;
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                dragonBallHero.position.Y += 100 * gameTime.ElapsedGameTime.Milliseconds / 1000F;
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                dragonBallHero.position.X -= 100 * gameTime.ElapsedGameTime.Milliseconds / 1000F;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                dragonBallHero.position.X += 100 * gameTime.ElapsedGameTime.Milliseconds / 1000F;
            }
            dragonBallHero.Update(gameTime);
            dragonBallHero1.Update(gameTime);

            isCollision = false;
            if (dragonBallHero.boundingBox.Contains(dragonBallHero1.boundingBox) == ContainmentType.Intersects || dragonBallHero.boundingSphere.Contains(dragonBallHero1.boundingSphere) == ContainmentType.Intersects)
            {
                isCollision = true;
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

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap);
            spriteBatch.DrawString(font, (isCollision == true) ? "We stick together" : "We are apart", new Vector2(100, 20), Color.Black);
            dragonBallHero1.Draw(spriteBatch);
            dragonBallHero.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
