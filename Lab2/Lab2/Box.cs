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
    public class Box : MovingSprite
    {
        public Box(int animationX, int animationY, int totalFrames, float scale, Texture2D texture, Vector2 position, SpriteType spriteType = SpriteType.TEST) : base(animationX, animationY, totalFrames, scale, texture, position, spriteType)
        {
            velocity.X = 0f;
        }

        public override void Update(GameTime pGameTime)
        {
            objectPreviousPosition = position;
            position += velocity;
            base.Update(pGameTime);
        }
        public override void checkCollisions()
        {
            base.checkCollisions();
            for (int i = 0; i < Game1.Instance.DisplayManager.Manager.TileMap.mapWidth; i++)
                for (int j = 0; j < Game1.Instance.DisplayManager.Manager.TileMap.mapHeight; j++)
                {
                    if (Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).property == TileProperty.TRAP && boundingBox.Intersects(Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).getBoundingBox))  //wpada w polapke
                    {
                        hasJumped = false;
                    }
                }
        }
    }
}
