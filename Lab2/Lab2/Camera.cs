using Lab2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
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
            centre = new Vector2(hero.position.X - (view.Bounds.Width / 2), hero.position.Y / 2);
            float cameraX = centre.X;
            float cameraY = centre.Y;

            float worldWidth = tileMap.mapWidth * tileMap.tilewidth;
            float worldHeight = tileMap.mapHeight * tileMap.tilewidth;

            if (cameraX < 0)
                cameraX = 0;
            else if (cameraX + view.Width > worldWidth)
                cameraX = worldWidth - view.Width;

            if (cameraY < 0)
                cameraY = 0;
            else if (cameraY + view.Height > worldHeight)
                cameraY = worldHeight - view.Height;

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
