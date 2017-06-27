using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToA;
using System.Threading;

namespace Lab2
{
    public abstract class Sprite
    {
        public Texture2D texture { get; }
        public Vector2 position;
        public Rectangle boundingBox { get; set; }

        public float ScaleFactor;
        public Rectangle Size;

        public SpriteType spriteType;
        public bool gravity;

        public float scale
        {
            get { return ScaleFactor; }
            set
            {
                ScaleFactor = value;
                Size = new Rectangle(0, 0, (int)(texture.Width * scale), (int)(texture.Height * scale));
            }
        }
        public void setPosition(float x, float y)
        {
            position = new Vector2(x, y);
        }
        public Sprite(float scale, Texture2D texture, Vector2 position, SpriteType spriteType = SpriteType.TEST)
        {
            this.ScaleFactor = scale;
            this.texture = texture;
            this.position = position;
            this.spriteType = spriteType;
            this.gravity = true;
        }
        public virtual void UpdateBoundingBox()
        {
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public virtual void Update(GameTime pGameTime)
        {
            UpdateBoundingBox();
        }
        public virtual void Draw(SpriteBatch sp)
        {
            sp.Draw(texture, position, null, Color.White);
        }
    }
}
