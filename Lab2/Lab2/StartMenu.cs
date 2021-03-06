﻿using Lab2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ToA
{
    class StartMenu : Display
    {
        Effect logotypeEffect;
        Sprite options, optionsChosen, quit, quitChosen, background, volumeLvlOn, volumeLvlOff, load, loadChosen, start, startChosen, logoType;
        private ContentManager content;
        StartMenuChosen startMenuChosen = StartMenuChosen.START;
        public StartMenu(ContentManager content)
        {
            this.content = content;
        }
        public void Load()
        {
            Effect logotypeEffect = content.Load<Effect>("Content/logotypeEffect"); //tutorialowy efekt do nałożenia na logo gry
            Texture2D backgroundTexture = content.Load<Texture2D>("Content/main_menu2");
            Texture2D logoTypeTexture = content.Load<Texture2D>("Content/logotype");
            Texture2D optionsButtonTexture = content.Load<Texture2D>("Content/button_options");
            Texture2D optionsButtonChosenTexture = content.Load<Texture2D>("Content/button_options_hover");
            Texture2D quitButtonTexture = content.Load<Texture2D>("Content/button_quit");
            Texture2D quitButtonChosenTexture = content.Load<Texture2D>("Content/button_quit_hover");
            Texture2D volumeLevelOnTexture = content.Load<Texture2D>("Content/volume_level_texture_on");
            Texture2D volumeLevelOffTexture = content.Load<Texture2D>("Content/volume_level_texture_off");
            Texture2D loadTexture = content.Load<Texture2D>("Content/button_load");
            Texture2D loadChosenTexture = content.Load<Texture2D>("Content/button_load_hover");
            Texture2D startTexture = content.Load<Texture2D>("Content/button_start");
            Texture2D startChosenTexture = content.Load<Texture2D>("Content/button_start_hover");

            background = new Button(1f, backgroundTexture, Vector2.Zero, SpriteType.BUTTON);
            logoType = new Button(1f, logoTypeTexture, Vector2.Zero, SpriteType.BUTTON);
            options = new Button(1f, optionsButtonTexture, Vector2.Zero, SpriteType.BUTTON);
            optionsChosen = new Button(1f, optionsButtonChosenTexture, Vector2.Zero, SpriteType.BUTTON);
            quit = new Button(1f, quitButtonTexture, Vector2.Zero, SpriteType.BUTTON);
            quitChosen = new Button(1f, quitButtonChosenTexture, Vector2.Zero, SpriteType.BUTTON);
            volumeLvlOn = new Button(1f, volumeLevelOnTexture, Vector2.Zero, SpriteType.BUTTON);
            volumeLvlOff = new Button(1f, volumeLevelOffTexture, Vector2.Zero, SpriteType.BUTTON);
            load = new Button(1f, loadTexture, Vector2.Zero, SpriteType.BUTTON);
            loadChosen = new Button(1f, loadChosenTexture, Vector2.Zero, SpriteType.BUTTON);
            start = new Button(1f, startTexture, Vector2.Zero, SpriteType.BUTTON);
            startChosen = new Button(1f, startChosenTexture, Vector2.Zero, SpriteType.BUTTON);

            
        }
        public void Update()
        {
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
            if (startMenuChosen == StartMenuChosen.SOUNDEFFECTS_VOLUME)
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
                switch (startMenuChosen)
                {
                    case StartMenuChosen.START:
                        Game1.Instance.DisplayManager.gameState = GameState.STORY;
                        MediaPlayer.Play(Game1.Instance.SoundManager.Songs["BackgroundMusic"]);
                        break;
                    case StartMenuChosen.LOAD:
                        loadGameState();
                        break;
                    case StartMenuChosen.QUIT:
                        Game1.Instance.Quit();
                        break;
                }
            }
        }

        private static void loadGameState()
        {
            if (File.Exists("save.xml"))
            {
                XElement xDoc = XElement.Load("save.xml");
                IEnumerable<XElement> game = xDoc.Elements();
                int i = 0;
                foreach (var save in game)
                {
                    Game1.Instance.DisplayManager.Manager.loadLevel(Convert.ToInt32(save.Element("LevelId").Value));
                    foreach (var sprite in save.Descendants("Sprite"))
                    {
                        Game1.Instance.DisplayManager.Manager.spriteList[i].setPosition(float.Parse(sprite.Element("X").Value, CultureInfo.InvariantCulture), float.Parse(sprite.Element("Y").Value, CultureInfo.InvariantCulture));
                        i++;
                    }
                }
                i = 0;
                Game1.Instance.DisplayManager.gameState = GameState.GAMEPLAY;
                MediaPlayer.Play(Game1.Instance.SoundManager.Songs["BackgroundMusic"]);
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameWindow Window)
        {
           

            spriteBatch.Begin();
            background.Draw(spriteBatch);
        
            start.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2 - 270);
            start.Draw(spriteBatch);
            if (startMenuChosen == StartMenuChosen.START)
            {
                startChosen.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2 - 270);
                startChosen.Draw(spriteBatch);
            }

            load.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2 - 100);
            load.Draw(spriteBatch);
            if (startMenuChosen == StartMenuChosen.LOAD)
            {
                loadChosen.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2 - 100);
                loadChosen.Draw(spriteBatch);
            }

            options.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2 + 70);
            options.Draw(spriteBatch);
            if (startMenuChosen == StartMenuChosen.BACKGROUND_MUSIC_VOLUME)
            {
                optionsChosen.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2 + 70);
                optionsChosen.Draw(spriteBatch);
                for (int i = 0; i < 10; i++)
                {
                    volumeLvlOff.setPosition(Window.ClientBounds.Width / 4 + 300 + i * 50, Window.ClientBounds.Height / 2 + 95);
                    volumeLvlOff.Draw(spriteBatch);
                }
                for (int i = 0; i < (int)(Game1.Instance.musicVolume * 10); i++)
                {
                    volumeLvlOn.setPosition(Window.ClientBounds.Width / 4 + 300 + i * 50, Window.ClientBounds.Height / 2 + 95);
                    volumeLvlOn.Draw(spriteBatch);
                }

            }
            if (startMenuChosen == StartMenuChosen.SOUNDEFFECTS_VOLUME)
            {
                optionsChosen.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2 + 70);
                optionsChosen.Draw(spriteBatch);
                for (int i = 0; i < 10; i++)
                {
                    volumeLvlOff.setPosition(Window.ClientBounds.Width / 4 + 300 + i * 50, Window.ClientBounds.Height / 2 + 95);
                    volumeLvlOff.Draw(spriteBatch);
                }
                for (int i = 0; i < (int)(Game1.Instance.spriteEffectVolume * 10); i++)
                {
                    volumeLvlOn.setPosition(Window.ClientBounds.Width / 4 + 300 + i * 50, Window.ClientBounds.Height / 2 + 95);
                    volumeLvlOn.Draw(spriteBatch);
                }
            }
            quit.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2 + 240);
            quit.Draw(spriteBatch);
            if (startMenuChosen == StartMenuChosen.QUIT)
            {
                quitChosen.setPosition(Window.ClientBounds.Width / 4 - 100, Window.ClientBounds.Height / 2 + 240);
                quitChosen.Draw(spriteBatch);
            }
            spriteBatch.End();

            spriteBatch.Begin(0, null, null, null, null, logotypeEffect);
            //logoType.setPosition(Window.ClientBounds.Width / 4 - 120, Window.ClientBounds.Height / 2 - 500);
            spriteBatch.Draw(content.Load<Texture2D>("Content/logotype"),new Vector2(350,20), new Rectangle(0, 0, 400, 250), new Color(255, 255, 255, 50));
            spriteBatch.End();
        }

    }
}
