﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Lab2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace ToA
{
    public class DisplayManger
    {
        private GraphicsDevice graphicsDevice;
        private ContentManager content;
        private GameWindow Window;
        public GameState gameState = GameState.STARTMENU;
        LevelManager manager;
        
        Gameplay gameplay;
        PauseMenu pauseMenu;
        StartMenu startMenu;
        Story story;
        EndGame endGame;

        public LevelManager Manager
        {
            get
            {
                return this.manager;
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
            gameplay = new Gameplay();
            pauseMenu = new PauseMenu(content);
            pauseMenu.Load();
            startMenu = new StartMenu(content);
            startMenu.Load();
            story = new Story(content, Window);
            endGame = new EndGame(content, Window);
            MediaPlayer.Play(Game1.Instance.SoundManager.Songs["GameMenuMusic"]);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.1f;

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
                    gameplay.Update();
                    break;
                case GameState.PAUSEMENU:
                    pauseMenu.Update();
                    break;
                case GameState.STARTMENU:
                    startMenu.Update();
                    break;
                case GameState.STORY:
                    story.Update();
                    break;
                case GameState.ENDGAME:
                    endGame.Update();
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
                    pauseMenu.Draw(spriteBatch, Window);
                    spriteBatch.End();
                    break;
                case GameState.STARTMENU:
                    graphicsDevice.Clear(Color.CornflowerBlue);
                    startMenu.Draw(spriteBatch, Window);
                    spriteBatch.End();
                    break;
                case GameState.STORY:
                    graphicsDevice.Clear(Color.Black);
                    story.Draw(spriteBatch, Window);
                    spriteBatch.End();
                    break;
                case GameState.ENDGAME:
                    graphicsDevice.Clear(Color.Black);
                    endGame.Draw(spriteBatch, Window);
                    spriteBatch.End();
                    break;
            }
        }
    }
}
