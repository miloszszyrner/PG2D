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
    public class Button : Sprite
    {
        public Button(float scale, Texture2D texture, Vector2 position, SpriteType spriteType = SpriteType.TEST) : base(scale, texture, position, spriteType)
        {
        }
        public override void Update(GameTime pGameTime, SoundEffect effect)
        {
            base.Update(pGameTime, effect);
            sourceRectangle = new Rectangle(0, 0, 200, 100);
            destinationRectangle = new Rectangle((int)position.X, (int)position.Y, 200, 100);
        }
    }
}
