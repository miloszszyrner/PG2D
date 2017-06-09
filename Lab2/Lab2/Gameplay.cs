using Lab2;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

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
            }
        }
    }
}
