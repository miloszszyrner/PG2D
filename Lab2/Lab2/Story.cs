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
    class Story : Display
    {
        private SpriteFont font;
        private String[] story = { "historia", "naszego" , "bohatera" };
        private String skip = "press enter to skip";
        private int position = 0;

        public Story(ContentManager content, GameWindow Window)
        {
            position = Window.ClientBounds.Height;
            font = content.Load<SpriteFont>("Content/CreditsFont");
        }
        public void Draw(SpriteBatch spriteBatch, GameWindow Window)
        {
            spriteBatch.Begin();
            for (int i = 0; i < story.Length; i++)
            {
                spriteBatch.DrawString(font, story[i], new Vector2(Window.ClientBounds.Width / 2 - font.MeasureString(story[i]).Length() / 2, position + i * font.MeasureString(story[0]).Y), Color.White);
            }
            spriteBatch.DrawString(font, skip, new Vector2(Window.ClientBounds.Width - font.MeasureString(skip).Length(), Window.ClientBounds.Height - font.MeasureString(skip).Y), Color.White);
            spriteBatch.End();
        }

        public void Load()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            position--;
            if (position + story.Length * font.MeasureString(story[0]).Y < 0 || Game1.Instance.InputManager.menuChoose)
            {
                Game1.Instance.DisplayManager.gameState = GameState.GAMEPLAY;
            }
        }
    }
}
