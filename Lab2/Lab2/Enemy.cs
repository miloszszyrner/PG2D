using Lab2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Audio;

namespace ToA
{
    public class Enemy : MovingSprite
    {
        private Vector2 sourcePosition;

        float deviation = 200f;
        public Enemy(int animationX, int animationY, int totalFrames, float scale, Texture2D texture, Vector2 position, SpriteType spriteType = SpriteType.TEST) : base(animationX, animationY, totalFrames, scale, texture, position, spriteType)
        {
            this.sourcePosition = position;
            velocity.X = 1f;
        }
        public override void Update(GameTime pGameTime, SoundEffect effect)
        {
            position += velocity;
            base.Update(pGameTime, effect);
            checkGravitation();
            
            if (position.X > sourcePosition.X + deviation)
            {
                velocity.X = -velocity.X;
            }
            else if (position.X < sourcePosition.X - deviation)
            {
                velocity.X = -velocity.X;
            }
            sourceRectangle = new Rectangle(0, 0, animationX, animationY);
            destinationRectangle = new Rectangle((int)position.X, (int)position.Y, animationX, animationY);
        }
        
    }
}