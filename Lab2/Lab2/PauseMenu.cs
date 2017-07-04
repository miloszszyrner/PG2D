using Lab2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Xml.Linq;

namespace ToA
{

    class PauseMenu : Display
    {

        Sprite resume, resumeChosen, options, optionsChosen, background, volumeLvlOn, volumeLvlOff, save, saveChosen, exit, exitChosen;
        private ContentManager content;
        PauseMenuChosen pauseMenuChosen = PauseMenuChosen.RESUME;

        public PauseMenu(ContentManager content)
        {
            this.content = content;
        }

        public void Load()
        {
            Texture2D backgroundTexture = content.Load<Texture2D>("Content/main_menu2");
            Texture2D resumeButtonTexture = content.Load<Texture2D>("Content/button_resume");
            Texture2D resumeButtonChosenTexture = content.Load<Texture2D>("Content/button_resume_hover");
            Texture2D optionsButtonTexture = content.Load<Texture2D>("Content/button_options");
            Texture2D optionsButtonChosenTexture = content.Load<Texture2D>("Content/button_options_hover");
            Texture2D volumeLevelOnTexture = content.Load<Texture2D>("Content/volume_level_texture_on");
            Texture2D volumeLevelOffTexture = content.Load<Texture2D>("Content/volume_level_texture_off");
            Texture2D saveTexture = content.Load<Texture2D>("Content/button_save");
            Texture2D saveChosenTexture = content.Load<Texture2D>("Content/button_save_hover");
            Texture2D exitButtonTexture = content.Load<Texture2D>("Content/button_exit");
            Texture2D exitButtonChosenTexture = content.Load<Texture2D>("Content/button_exit_hover");

            background = new Button(1f, backgroundTexture, Vector2.Zero, SpriteType.BUTTON);
            resume = new Button(1f, resumeButtonTexture, Vector2.Zero, SpriteType.BUTTON);
            resumeChosen = new Button(1f, resumeButtonChosenTexture, Vector2.Zero, SpriteType.BUTTON);
            options = new Button(1f, optionsButtonTexture, Vector2.Zero, SpriteType.BUTTON);
            optionsChosen = new Button(1f, optionsButtonChosenTexture, Vector2.Zero, SpriteType.BUTTON);
            volumeLvlOn = new Button(1f, volumeLevelOnTexture, Vector2.Zero, SpriteType.BUTTON);
            volumeLvlOff = new Button(1f, volumeLevelOffTexture, Vector2.Zero, SpriteType.BUTTON);
            save = new Button(1f, saveTexture, Vector2.Zero, SpriteType.BUTTON);
            saveChosen = new Button(1f, saveChosenTexture, Vector2.Zero, SpriteType.BUTTON);
            exit = new Button(1f, exitButtonTexture, Vector2.Zero, SpriteType.BUTTON);
            exitChosen = new Button(1f, exitButtonChosenTexture, Vector2.Zero, SpriteType.BUTTON);
        }
        public void Update()
        {
            if (Game1.Instance.InputManager.pause)
            {
                Game1.Instance.DisplayManager.gameState = GameState.GAMEPLAY;
                Game1.Instance.InputManager.pause = false;
                MediaPlayer.Play(Game1.Instance.SoundManager.Songs["BackgroundMusic"]);
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
                    if (Game1.Instance.musicVolume > 0)
                    {
                        Game1.Instance.musicVolume -= 0.1f;
                    }
                }
                if (Game1.Instance.InputManager.menuRight)
                {
                    if (Game1.Instance.musicVolume < 1.0f)
                    {
                        Game1.Instance.musicVolume += 0.1f;
                    }
                }
                MediaPlayer.Volume = Game1.Instance.musicVolume;
            }
            if (pauseMenuChosen == PauseMenuChosen.SOUNDEFFECTS_VOLUME)
            {
                if (Game1.Instance.InputManager.menuLeft)
                {
                    if (Game1.Instance.spriteEffectVolume > 0)
                    {
                        Game1.Instance.spriteEffectVolume -= 0.1f;
                    }
                    if (Game1.Instance.spriteEffectVolume < 0)
                    {
                        Game1.Instance.spriteEffectVolume = 0.0f;
                    }
                }
                if (Game1.Instance.InputManager.menuRight)
                {
                    if (Game1.Instance.spriteEffectVolume < 1.0f)
                    {
                        Game1.Instance.spriteEffectVolume += 0.1f;
                    }
                    if (Game1.Instance.spriteEffectVolume > 1.0f)
                    {
                        Game1.Instance.spriteEffectVolume = 1.0f;
                    }
                }
                SoundEffect.MasterVolume = Game1.Instance.spriteEffectVolume;
            }
            if (Game1.Instance.InputManager.menuChoose)
            {
                switch (pauseMenuChosen)
                {
                    case PauseMenuChosen.RESUME:
                        Game1.Instance.DisplayManager.gameState = GameState.GAMEPLAY;
                        MediaPlayer.Play(Game1.Instance.SoundManager.Songs["BackgroundMusic"]);
                        break;
                    case PauseMenuChosen.SAVE:
                        Game1.Instance.saveGameState();
                        break;
                    case PauseMenuChosen.EXIT:
                        Game1.Instance.DisplayManager.gameState = GameState.STARTMENU;
                        break;
                }
            }
        }

       

        public void Draw(SpriteBatch spriteBatch, GameWindow Window)
        {
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

            options.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2);
            options.Draw(spriteBatch);
            if (pauseMenuChosen == PauseMenuChosen.BACKGROUND_MUSIC_VOLUME)
            {
                optionsChosen.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2);
                optionsChosen.Draw(spriteBatch);
                for (int i = 0; i < 10; i++)
                {
                    volumeLvlOff.setPosition(Window.ClientBounds.Width / 4 + 300 + i * 50, Window.ClientBounds.Height / 2 + 25);
                    volumeLvlOff.Draw(spriteBatch);
                }
                for (int i = 0; i < (int)(Game1.Instance.musicVolume * 10); i++)
                {
                    volumeLvlOn.setPosition(Window.ClientBounds.Width / 4 + 300 + i * 50, Window.ClientBounds.Height / 2 + 25);
                    volumeLvlOn.Draw(spriteBatch);
                }

            }
            if (pauseMenuChosen == PauseMenuChosen.SOUNDEFFECTS_VOLUME)
            {
                optionsChosen.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2);
                optionsChosen.Draw(spriteBatch);
                for (int i = 0; i < 10; i++)
                {
                    volumeLvlOff.setPosition(Window.ClientBounds.Width / 4 + 300 + i * 50, Window.ClientBounds.Height / 2 + 25);
                    volumeLvlOff.Draw(spriteBatch);
                }
                for (int i = 0; i < (int)(Game1.Instance.spriteEffectVolume * 10); i++)
                {
                    volumeLvlOn.setPosition(Window.ClientBounds.Width / 4 + 300 + i * 50, Window.ClientBounds.Height / 2 + 25);
                    volumeLvlOn.Draw(spriteBatch);
                }
            }
            exit.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2 + 200);
            exit.Draw(spriteBatch);
            if (pauseMenuChosen == PauseMenuChosen.EXIT)
            {
                exitChosen.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2 + 200);
                exitChosen.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}
