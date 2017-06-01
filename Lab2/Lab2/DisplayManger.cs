using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Lab2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace ToA
{
    public class DisplayManger
    {
        private GraphicsDevice graphicsDevice;
        private ContentManager content;
        private GameWindow Window;
        Sprite resume, resumeChosen, options, optionsChosen, exit, exitChosen;
        GameState gameState;
        LevelManager manager;
        PauseMenuChosen pauseMenuChosen = PauseMenuChosen.RESUME;
        SoundEffect jumpEffect;
        Song backgroundMusic;
        private float musicVolume = 0.1f;

        public LevelManager Manager
        {
            get
            {
                return manager;
            }
        }

        public DisplayManger(GraphicsDevice graphicsDevice, ContentManager content, GameWindow Window)
        {
            this.graphicsDevice = graphicsDevice;
            this.content = content;
            this.Window = Window;
            manager = new LevelManager("Content/content.xml", graphicsDevice, content);
        }

        public void Load()
        {
            Texture2D resumeButtonTexture = content.Load<Texture2D>("Content/resumeButton");
            Texture2D resumeButtonChosenTexture = content.Load<Texture2D>("Content/resumeButtonChosen");
            Texture2D optionsButtonTexture = content.Load<Texture2D>("Content/optionsButton");
            Texture2D optionsButtonChosenTexture = content.Load<Texture2D>("Content/optionsButtonChosen");
            Texture2D exitButtonTexture = content.Load<Texture2D>("Content/exitButton");
            Texture2D exitButtonChosenTexture = content.Load<Texture2D>("Content/exitButtonChosen");

            resume = new Sprite(1f, resumeButtonTexture, new Vector2(Window.ClientBounds.Width / 2 - 100, Window.ClientBounds.Height / 2 - 300), SpriteType.BUTTON);
            resumeChosen = new Sprite(1f, resumeButtonChosenTexture, new Vector2(Window.ClientBounds.Width / 2 - 100, Window.ClientBounds.Height / 2 - 300), SpriteType.BUTTON);
            options = new Sprite(1f, optionsButtonTexture, new Vector2(Window.ClientBounds.Width / 2 - 100, Window.ClientBounds.Height / 2 - 150), SpriteType.BUTTON);
            optionsChosen = new Sprite(1f, optionsButtonChosenTexture, new Vector2(Window.ClientBounds.Width / 2 - 100, Window.ClientBounds.Height / 2 - 150), SpriteType.BUTTON);
            exit = new Sprite(1f, exitButtonTexture, new Vector2(Window.ClientBounds.Width / 2 - 100, Window.ClientBounds.Height / 2), SpriteType.BUTTON);
            exitChosen = new Sprite(1f, exitButtonChosenTexture, new Vector2(Window.ClientBounds.Width / 2 - 100, Window.ClientBounds.Height / 2), SpriteType.BUTTON);
        }

        public void Update(GameTime gameTime)
        {
            //Game1.Instance.InputManager.Update();

            if (Game1.Instance.InputManager.back)
            {
                Game1.Instance.Quit();
            }
            if (!Game1.Instance.InputManager.gameState)
            {
                gameState = GameState.PAUSEMENU;
            }
            if (Game1.Instance.InputManager.gameState)
            {
                gameState = GameState.GAMEPLAY;
            }

            switch (gameState)
            {
                case GameState.GAMEPLAY:
                    manager.Update(gameTime);
                    break;
                case GameState.PAUSEMENU:
                    resume.Update(gameTime, jumpEffect);
                    resumeChosen.Update(gameTime, jumpEffect);
                    options.Update(gameTime, jumpEffect);
                    optionsChosen.Update(gameTime, jumpEffect);
                    exit.Update(gameTime, jumpEffect);
                    exitChosen.Update(gameTime, jumpEffect);
                    if (Game1.Instance.InputManager.menuUp)
                    {
                        if (pauseMenuChosen != PauseMenuChosen.RESUME)
                        {
                            pauseMenuChosen--;
                        }
                    }
                    if (Game1.Instance.InputManager.menuDown)
                    {
                        if (pauseMenuChosen < PauseMenuChosen.EXIT)
                        {
                            pauseMenuChosen++;
                        }
                    }
                    if (pauseMenuChosen == PauseMenuChosen.BACKGROUND_MUSIC_VOLUME)
                    {
                        if (Game1.Instance.InputManager.menuLeft)
                        {
                            if (musicVolume > 0)
                            {
                                musicVolume -= 0.1f;
                            }
                        }
                        if (Game1.Instance.InputManager.menuRight)
                        {
                            if (musicVolume < 1.0f)
                            {
                                musicVolume += 0.1f;
                            }
                        }
                        MediaPlayer.Volume = musicVolume;
                    }
                    /*if (pauseMenuChosen == PauseMenuChosen.SOUNDEFFECTS_VOLUME)
                    {
                        if (Game1.Instance.InputManager.menuLeft)
                        {
                            if (dragonBallHero.soundEffectVolume > 0)
                            {
                                dragonBallHero.soundEffectVolume -= 0.1f;
                            }
                        }
                        if (Game1.Instance.InputManager.menuRight)
                        {
                            if (dragonBallHero.soundEffectVolume < 1.0f)
                            {
                                dragonBallHero.soundEffectVolume += 0.1f;
                            }
                        }
                        SoundEffect.MasterVolume = dragonBallHero.soundEffectVolume;
                    }*/
                    if (Game1.Instance.InputManager.menuChoose)
                    {
                        switch (pauseMenuChosen)
                        {
                            case PauseMenuChosen.RESUME:
                                Game1.Instance.InputManager.gameState = true;
                                break;
                            case PauseMenuChosen.OPTIONS:
                                break;
                            case PauseMenuChosen.EXIT:
                                Game1.Instance.Quit();
                                break;
                        }
                    }
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (gameState)
            {
                case GameState.GAMEPLAY:
                    graphicsDevice.Clear(Color.CornflowerBlue);
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, manager.Camera.transform);
                    manager.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
                case GameState.PAUSEMENU:
                    graphicsDevice.Clear(Color.CornflowerBlue);
                    spriteBatch.Begin();

                    options.setPosition(Window.ClientBounds.Width / 2 - 100, Window.ClientBounds.Height / 2 - 150);
                    optionsChosen.setPosition(Window.ClientBounds.Width / 2 - 100, Window.ClientBounds.Height / 2 - 150);

                    resume.Draw(spriteBatch);
                    if (pauseMenuChosen == PauseMenuChosen.RESUME)
                    {
                        resumeChosen.Draw(spriteBatch);
                    }
                    options.Draw(spriteBatch);
                    if (pauseMenuChosen == PauseMenuChosen.OPTIONS)
                    {
                        optionsChosen.Draw(spriteBatch);
                    }
                    if (pauseMenuChosen == PauseMenuChosen.BACKGROUND_MUSIC_VOLUME)
                    {
                        options.setPosition(Window.ClientBounds.Width / 2 - 70, Window.ClientBounds.Height / 2 - 150);
                        optionsChosen.setPosition(Window.ClientBounds.Width / 2 - 70, Window.ClientBounds.Height / 2 - 150);
                        optionsChosen.Draw(spriteBatch);

                    }
                    if (pauseMenuChosen == PauseMenuChosen.SOUNDEFFECTS_VOLUME)
                    {
                        options.setPosition(Window.ClientBounds.Width / 2 - 70, Window.ClientBounds.Height / 2 - 150);
                        optionsChosen.setPosition(Window.ClientBounds.Width / 2 - 70, Window.ClientBounds.Height / 2 - 150);
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
        }
    }
}
