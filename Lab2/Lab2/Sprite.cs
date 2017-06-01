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


        public Rectangle sourceRectangle;
        public Rectangle destinationRectangle;


        public int animationX;
        public int animationY;
        public int currentFrame = 0;
        public int totalFrames;

        public float elapsed;
        public float delay = 100f;



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

        public Sprite (int animationX, int animationY, int totalFrames, float scale, Texture2D texture, Vector2 position, SpriteType spriteType = SpriteType.TEST)
        {
            this.ScaleFactor = scale;
            this.texture = texture;
            this.position = position;
            this.spriteType = spriteType;
            this.gravity = true;
            this.animationX = animationX;
            this.animationY = animationY;
            this.totalFrames = totalFrames;
        }
        public Sprite(float scale, Texture2D texture, Vector2 position, SpriteType spriteType = SpriteType.TEST)
        {
            this.ScaleFactor = scale;
            this.texture = texture;
            this.position = position;
            this.spriteType = spriteType;
            this.gravity = true;
        }
        public void UpdateBoundingBox()
        {
            boundingBox = new Rectangle((int)position.X, (int)position.Y, animationX, animationY);
        }

        public virtual void Update(GameTime pGameTime, SoundEffect effect)
        {
            UpdateBoundingBox();
            checkCollisions();
        }
        public virtual void Draw(SpriteBatch sp)
        {
            sp.Draw(texture, destinationRectangle, sourceRectangle, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
        }
        public virtual void checkCollisions()
        {

           
        }
        
        public void Animate(GameTime pGameTime, int row)
        {
            elapsed += (float)pGameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsed >= delay)
            {
                if (currentFrame >= totalFrames)
                    currentFrame = 0;
                else
                    currentFrame++;
                elapsed = 0;
            }
            sourceRectangle = new Rectangle(animationX * currentFrame, row * animationY, animationX, animationY);

        }
        
    }
}
