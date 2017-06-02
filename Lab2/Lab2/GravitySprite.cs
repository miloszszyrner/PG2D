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
    public class GravitySprite : Sprite
    {
        public GravitySprite(float scale, Texture2D texture, Vector2 position, SpriteType spriteType = SpriteType.TEST) : base(scale, texture, position, spriteType)
        {
        }
        public override void Update(GameTime pGameTime, SoundEffect effect)
        {
            base.Update(pGameTime, effect);
        }
    }
}
