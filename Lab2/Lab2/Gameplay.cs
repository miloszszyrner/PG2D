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

        private int position = 0;
        private SpriteFont font;
        private String[] credits = { "Koniec Gry", "Tworcy", "Programista - Milosz Szyrner", "Projektant Gry - Michal Kaminski", "Grafik - Patrycja Ignaczak", "Tester - Katarzyna Koziel" };

        public Gameplay(ContentManager content, GameWindow Window)
        {
            position = Window.ClientBounds.Height;
            font = content.Load<SpriteFont>("Content/Tekst");
        }

        public void Draw(SpriteBatch spriteBatch, GameWindow Window)
        {
            if (Game1.Instance.levelNumber == 3)
            {
                for (int i = 0; i < credits.Length; i++)
                {
                    spriteBatch.DrawString(font, credits[i], new Vector2(Window.ClientBounds.Width / 2 - font.MeasureString(credits[i]).Length() / 2, position + i * 20), Color.White);
                }
            }
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
            if (Game1.Instance.levelNumber == 3)
            {
                position--;
                if (position < 0)
                {
                    Game1.Instance.DisplayManager.gameState = GameState.STARTMENU;
                    //Tutaj ustawic powrot do pierwszego poziomu
                }
            }
        }
    }
}
