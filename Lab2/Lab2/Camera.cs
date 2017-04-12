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
        public void Update(GameTime gameTime, Sprite hero)
        {
            centre = new Vector2(hero.position.X + (hero.Size.Width / 2) - 900,0);
            transform = Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) * Matrix.CreateTranslation(new Vector3(-centre.X, -centre.Y, 0));

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
