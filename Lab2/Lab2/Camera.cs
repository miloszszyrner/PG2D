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
            float cameraWidth = view.Width;

            float worldWidth = tileMap.mapWidth;

            if (cameraX < 0)
                cameraX = 0;
            else if (cameraX + cameraWidth > worldWidth)
                cameraX = worldWidth - cameraWidth;

            transform = Matrix.CreateTranslation(new Vector3(-centre.X, -centre.Y, 0)) * Matrix.CreateScale(new Vector3(Zoom, Zoom, 1));

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
