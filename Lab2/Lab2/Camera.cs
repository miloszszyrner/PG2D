using Lab2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToA
{
    public class Camera
    {
        public Matrix transform;
        Viewport view;
        Vector2 centre;
        public float Zoom { get; private set; }

        public Camera(Viewport view)
        {
            this.view = view;
            Zoom = 1.0f;
        }
        public void Update(GameTime gameTime, Sprite hero, TileMap tileMap)
        {
            centre = new Vector2(hero.position.X + (hero.Size.Width / 2) - 900, hero.position.Y / 2);
            float cameraX = centre.X;
            float cameraY = centre.Y;

            float cameraWidth = view.Width;
            float cameraHeight = view.Height;

            float worldWidth = tileMap.mapWidth * tileMap.tilewidth;
            float worldHeight = tileMap.mapHeight * tileMap.tilewidth;

            if (cameraX < 0)
                cameraX = 0;
            else if (cameraX + cameraWidth > worldWidth)
                cameraX = worldWidth - cameraWidth;

            if (cameraY < 0)
                cameraY = 0;
            else if (cameraY + cameraHeight > worldHeight)
                cameraY = worldHeight - cameraHeight;

            transform = Matrix.CreateTranslation(new Vector3(-cameraX, -cameraY, 0)) * Matrix.CreateScale(new Vector3(Zoom, Zoom, 1));

        }
        public void AdjustZoom(float amount)
        {
            Zoom += amount;
            if (Zoom < 0.25f)
            {
                Zoom = 0.25f;
            }
        }
    }
}
