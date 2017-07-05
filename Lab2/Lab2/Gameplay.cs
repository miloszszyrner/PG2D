using Lab2;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

namespace ToA
{
    class Gameplay : Display
    {
        public void Draw(SpriteBatch spriteBatch, GameWindow Window)
        {
            throw new NotImplementedException();
        }

        public void Load()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            if (Game1.Instance.InputManager.pause)
            {
                Game1.Instance.DisplayManager.gameState = GameState.PAUSEMENU;
                Game1.Instance.InputManager.pause = false;
                MediaPlayer.Play(Game1.Instance.SoundManager.Songs["GameMenuMusic"]);
            }
            if (Game1.Instance.levelNumber == Game1.Instance.NumberOfLevels && Game1.Instance.isFinishing)
            {
                Game1.Instance.DisplayManager.gameState = GameState.ENDGAME;
            }
        }
    }
}
