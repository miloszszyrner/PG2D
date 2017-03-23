using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToA
{
    public class Tile
    {
        Vector2 position;
        Texture2D texture;
        Rectangle sourceRectangle;

        public Tile(Vector2 position, Texture2D texture, Rectangle sourceRectangle)
        {
            this.position = position;
            this.texture = texture;
            this.sourceRectangle = sourceRectangle;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, null, sourceRectangle, null, 0f, null, null, SpriteEffects.None, 0f);
        }
    }
}
