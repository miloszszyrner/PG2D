using Lab2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace ToA
{
    public class Box : Sprite
    {
        public Box(int animationX, int animationY, int totalFrames, float scale, Texture2D texture, Vector2 position, SpriteType spriteType = SpriteType.TEST) : base(animationX, animationY, totalFrames, scale, texture, position, spriteType)
        {
        }
        public override void Update(GameTime pGameTime, SoundEffect effect)
        {
            base.Update(pGameTime, effect);
            sourceRectangle = new Rectangle(0, 0, animationX, animationY);
            destinationRectangle = new Rectangle((int)position.X, (int)position.Y, animationX, animationY);
        }
    }
}
