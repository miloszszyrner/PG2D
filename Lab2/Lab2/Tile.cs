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
        public float ScaleFactor;
        public Rectangle Size;
        public TileProperty property { get; }
        public BoundingBox getBoundingBox
        {
            get
            {   
                if( property == TileProperty.PLATFORM_CENTER || property == TileProperty.PLATFORM_LEFT || property == TileProperty.PLATFORM_RIGHT)
                    return new BoundingBox(new Vector3(position, 0), new Vector3(position.X + (texture.Width * ScaleFactor), position.Y + (25 * ScaleFactor), 0));
                return new BoundingBox(new Vector3(position, 0), new Vector3(position.X + (texture.Width * ScaleFactor), position.Y + (texture.Height * ScaleFactor), 0));
            }
        }
        public float scale
        {
            get { return ScaleFactor; }
            set
            {
                ScaleFactor = value;
                Size = new Rectangle(0, 0, (int)(texture.Width * scale), (int)(texture.Height * scale));
            }
        }
        public Tile(float ScaleFactor, Vector2 position, Texture2D texture, Rectangle sourceRectangle, TileProperty property)
        {
            this.ScaleFactor = ScaleFactor;
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
