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
        public TileProperty property { get; }
        public Rectangle getBoundingBox
        {
            get
            {   
                if( property == TileProperty.PLATFORM_CENTER || property == TileProperty.PLATFORM_LEFT || property == TileProperty.PLATFORM_RIGHT)
                    return new Rectangle((int)position.X, (int)position.Y, sourceRectangle.Width, 25);
                return new Rectangle((int)position.X, (int)position.Y, sourceRectangle.Width, sourceRectangle.Height);

            }
        }

        public Tile(Vector2 position, Texture2D texture, Rectangle sourceRectangle, TileProperty property)
        {
            this.position = position;
            this.texture = texture;
            this.sourceRectangle = sourceRectangle;
            this.property = property;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, null, sourceRectangle, null, 0f, null, null, SpriteEffects.None, 0f);
        }
    }
}
