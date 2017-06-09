using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToA
{
    public interface Display
    {
        void Load();

        void Update();

        void Draw(SpriteBatch spriteBatch, GameWindow Window);
    }
}
