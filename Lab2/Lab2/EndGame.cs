using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Lab2;

namespace ToA
{
    class EndGame : Display
    {
        private int position = 0;
        private SpriteFont font;
        private String[] credits = { "Koniec Gry", "Tworcy", "Programista - Milosz Szyrner", "Projektant Gry - Michal Kaminski", "Grafik - Patrycja Ignaczak", "Tester - Katarzyna Koziel" };
        GameWindow Window;

        public EndGame(ContentManager content, GameWindow Window)
        {
            this.Window = Window;
            position = Window.ClientBounds.Height;
            font = content.Load<SpriteFont>("Content/CreditsFont");
        }

        public void Draw(SpriteBatch spriteBatch, GameWindow Window)
        {
            spriteBatch.Begin();
            for (int i = 0; i < credits.Length; i++)
            {
                spriteBatch.DrawString(font, credits[i], new Vector2(Window.ClientBounds.Width / 2 - font.MeasureString(credits[i]).Length() / 2, position + i * font.MeasureString(credits[0]).Y), Color.White);
            }
            spriteBatch.End();
        }

        public void Load()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            position--;
            if (position < 0 || (position < Window.ClientBounds.Height - 10 && Game1.Instance.InputManager.menuChoose))
            {

                Game1.Instance.DisplayManager.gameState = GameState.STARTMENU;
                Game1.Instance.levelNumber = 1;
                Game1.Instance.DisplayManager.Manager.loadLevel(Game1.Instance.levelNumber);
            }
        }
    }
}
