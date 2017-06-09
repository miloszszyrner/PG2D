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
        Sprite resume, resumeChosen, options, optionsChosen, quit, quitChosen, background, volumeLvlOn, volumeLvlOff, save, saveChosen, load, loadChosen, start, startChosen, exit, exitChosen;
        GameState gameState = GameState.STARTMENU;
        LevelManager manager;
        PauseMenuChosen pauseMenuChosen = PauseMenuChosen.RESUME;
        StartMenuChosen startMenuChosen = StartMenuChosen.START;
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
            Texture2D backgroundTexture = content.Load<Texture2D>("Content/main_menu2");
            Texture2D resumeButtonTexture = content.Load<Texture2D>("Content/button_resume");
            Texture2D resumeButtonChosenTexture = content.Load<Texture2D>("Content/button_resume_hover");
            Texture2D optionsButtonTexture = content.Load<Texture2D>("Content/button_options");
            Texture2D optionsButtonChosenTexture = content.Load<Texture2D>("Content/button_options_hover");
            Texture2D quitButtonTexture = content.Load<Texture2D>("Content/button_quit");
            Texture2D quitButtonChosenTexture = content.Load<Texture2D>("Content/button_quit_hover");
            Texture2D volumeLevelOnTexture = content.Load<Texture2D>("Content/volume_level_texture_on");
            Texture2D volumeLevelOffTexture = content.Load<Texture2D>("Content/volume_level_texture_off");
            Texture2D saveTexture = content.Load<Texture2D>("Content/button_save");
            Texture2D saveChosenTexture = content.Load<Texture2D>("Content/button_save_hover");
            Texture2D loadTexture = content.Load<Texture2D>("Content/button_load");
            Texture2D loadChosenTexture = content.Load<Texture2D>("Content/button_load_hover");
            Texture2D startTexture = content.Load<Texture2D>("Content/button_start");
            Texture2D startChosenTexture = content.Load<Texture2D>("Content/button_start_hover");
            Texture2D exitButtonTexture = content.Load<Texture2D>("Content/button_exit");
            Texture2D exitButtonChosenTexture = content.Load<Texture2D>("Content/button_exit_hover");

            background = new Button(1f, backgroundTexture, Vector2.Zero , SpriteType.BUTTON);
            resume = new Button(1f, resumeButtonTexture, Vector2.Zero, SpriteType.BUTTON);
            resumeChosen = new Button(1f, resumeButtonChosenTexture, Vector2.Zero, SpriteType.BUTTON);
            options = new Button(1f, optionsButtonTexture, Vector2.Zero, SpriteType.BUTTON);
            optionsChosen = new Button(1f, optionsButtonChosenTexture, Vector2.Zero, SpriteType.BUTTON);
            quit = new Button(1f, quitButtonTexture, Vector2.Zero, SpriteType.BUTTON);
            quitChosen = new Button(1f, quitButtonChosenTexture, Vector2.Zero, SpriteType.BUTTON);
            volumeLvlOn = new Button(1f, volumeLevelOnTexture, Vector2.Zero, SpriteType.BUTTON);
            volumeLvlOff = new Button(1f, volumeLevelOffTexture, Vector2.Zero, SpriteType.BUTTON);
            save = new Button(1f, saveTexture, Vector2.Zero, SpriteType.BUTTON);
            saveChosen = new Button(1f, saveChosenTexture, Vector2.Zero, SpriteType.BUTTON);
            load = new Button(1f, loadTexture, Vector2.Zero, SpriteType.BUTTON);
            loadChosen = new Button(1f, loadChosenTexture, Vector2.Zero, SpriteType.BUTTON);
            start = new Button(1f, startTexture, Vector2.Zero, SpriteType.BUTTON);
            startChosen = new Button(1f, startChosenTexture, Vector2.Zero, SpriteType.BUTTON);
            exit = new Button(1f, exitButtonTexture, Vector2.Zero, SpriteType.BUTTON);
            exitChosen = new Button(1f, exitButtonChosenTexture, Vector2.Zero, SpriteType.BUTTON);
        }

        public void Update(GameTime gameTime)
        {
            if (Game1.Instance.InputManager.back)
            {
                Game1.Instance.Quit();
            }
            switch (gameState)
            {
                case GameState.GAMEPLAY:
                    manager.Update(gameTime);
                    if (Game1.Instance.InputManager.pause)
                    {
                        gameState = GameState.PAUSEMENU;
                        Game1.Instance.InputManager.pause = false;
                    }
                    break;
                case GameState.PAUSEMENU:
                    if (Game1.Instance.InputManager.pause)
                    {
                        gameState = GameState.GAMEPLAY;
                        Game1.Instance.InputManager.pause = false;
                    }
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
                                gameState = GameState.GAMEPLAY;
                                break;
                            case PauseMenuChosen.SAVE:
                                break;
                            case PauseMenuChosen.EXIT:
                                gameState = GameState.STARTMENU;
                                break;
                        }
                    }
                    break;
                case GameState.STARTMENU:
                    if (Game1.Instance.InputManager.menuUp)
                    {
                        if (startMenuChosen != StartMenuChosen.START)
                        {
                            startMenuChosen--;
                        }
                    }
                    if (Game1.Instance.InputManager.menuDown)
                    {
                        if (startMenuChosen < StartMenuChosen.QUIT)
                        {
                            startMenuChosen++;
                        }
                    }
                    if (startMenuChosen == StartMenuChosen.BACKGROUND_MUSIC_VOLUME)
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
                    if (Game1.Instance.InputManager.menuChoose)
                    {
                        switch (startMenuChosen)
                        {
                            case StartMenuChosen.START:
                                gameState = GameState.GAMEPLAY;
                                break;
                            case StartMenuChosen.LOAD:
                                break;
                            case StartMenuChosen.QUIT:
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

                    background.Draw(spriteBatch);

                    resume.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2 - 400);
                    resume.Draw(spriteBatch);
                    if (pauseMenuChosen == PauseMenuChosen.RESUME)
                    {
                        resumeChosen.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2 - 400);
                        resumeChosen.Draw(spriteBatch);
                    }

                    save.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2 - 200);
                    save.Draw(spriteBatch);
                    if (pauseMenuChosen == PauseMenuChosen.SAVE)
                    {
                        saveChosen.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2 - 200);
                        saveChosen.Draw(spriteBatch);
                    }

                    options.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2 );
                    options.Draw(spriteBatch);
                    if (pauseMenuChosen == PauseMenuChosen.BACKGROUND_MUSIC_VOLUME)
                    {
                        optionsChosen.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2 );
                        optionsChosen.Draw(spriteBatch);
                        for(int i = 0; i < 10; i++)
                        {
                            volumeLvlOff.setPosition(Window.ClientBounds.Width / 4 + 300 + i * 50, Window.ClientBounds.Height / 2 + 25);
                            volumeLvlOff.Draw(spriteBatch);
                        }
                        for (int i = 0; i < (int) (musicVolume * 10); i++)
                        {
                            volumeLvlOn.setPosition(Window.ClientBounds.Width / 4 + 300 + i * 50, Window.ClientBounds.Height / 2 + 25);
                            volumeLvlOn.Draw(spriteBatch);
                        }

                    }
                    if (pauseMenuChosen == PauseMenuChosen.SOUNDEFFECTS_VOLUME)
                    {
                        optionsChosen.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2 );
                        optionsChosen.Draw(spriteBatch);
                    }
                    exit.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2 + 200);
                    exit.Draw(spriteBatch);
                    if (pauseMenuChosen == PauseMenuChosen.EXIT)
                    {
                        exitChosen.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2 + 200);
                        exitChosen.Draw(spriteBatch);
                    }

                    spriteBatch.End();
                    break;
                case GameState.STARTMENU:
                    graphicsDevice.Clear(Color.CornflowerBlue);
                    spriteBatch.Begin();

                    background.Draw(spriteBatch);

                    start.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2 - 400);
                    start.Draw(spriteBatch);
                    if (startMenuChosen == StartMenuChosen.START)
                    {
                        startChosen.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2 - 400);
                        startChosen.Draw(spriteBatch);
                    }

                    load.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2 - 200);
                    load.Draw(spriteBatch);
                    if (startMenuChosen == StartMenuChosen.LOAD)
                    {
                        loadChosen.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2 - 200);
                        loadChosen.Draw(spriteBatch);
                    }

                    options.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2);
                    options.Draw(spriteBatch);
                    if (startMenuChosen == StartMenuChosen.BACKGROUND_MUSIC_VOLUME)
                    {
                        optionsChosen.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2);
                        optionsChosen.Draw(spriteBatch);
                        for (int i = 0; i < 10; i++)
                        {
                            volumeLvlOff.setPosition(Window.ClientBounds.Width / 4 + 300 + i * 50, Window.ClientBounds.Height / 2 + 25);
                            volumeLvlOff.Draw(spriteBatch);
                        }
                        for (int i = 0; i < (int)(musicVolume * 10); i++)
                        {
                            volumeLvlOn.setPosition(Window.ClientBounds.Width / 4 + 300 + i * 50, Window.ClientBounds.Height / 2 + 25);
                            volumeLvlOn.Draw(spriteBatch);
                        }

                    }
                    if (startMenuChosen == StartMenuChosen.SOUNDEFFECTS_VOLUME)
                    {
                        optionsChosen.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2);
                        optionsChosen.Draw(spriteBatch);
                    }
                    quit.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2 + 200);
                    quit.Draw(spriteBatch);
                    if (startMenuChosen == StartMenuChosen.QUIT)
                    {
                        quitChosen.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2 + 200);
                        quitChosen.Draw(spriteBatch);
                    }

                    spriteBatch.End();
                    break;
            }
        }
    }
}
