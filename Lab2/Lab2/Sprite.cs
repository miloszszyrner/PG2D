using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToA;

namespace Lab2
{
    class Sprite
    {
        public BoundingBox boundingBox { get; set; }
        public Vector2 position;
        public Texture2D texture { get; }
        public BoundingSphere boundingSphere { get; set; }
        public bool isPlayerControlled;
        public void setPosition(float x, float y)
        {
            position = new Vector2(x, y);
        }
        public Sprite(Texture2D texture, Vector2 position, bool isPlayerControlled = false)
        {
            this.texture = texture;
            this.position = position;
            this.isPlayerControlled = isPlayerControlled;
        }
        public Sprite(Texture2D texture)
        {
            this.texture = texture;
            this.position = position;
        }

        private void UpdateBoundingBox()
        {
            boundingBox = new BoundingBox(new Vector3(position, 0), new Vector3(position.X + texture.Width, position.Y + texture.Height, 0));
        }
        private void UpdateBoundingSphere()
        {
            //boundingSphere = new BoundingSphere(new Vector3(position.X + (texture.Width / 2), position.Y + (texture.Height / 2), 0), (float)Math.Sqrt(Math.Pow((position.X + texture.Width) - (position.X + (texture.Width / 2)), 2) + Math.Pow((position.Y + texture.Height) - (position.Y + (texture.Height / 2)), 2)));
            boundingSphere = new BoundingSphere(new Vector3(position.X + (texture.Width / 2), position.Y + (texture.Height / 2), 0), Math.Max(texture.Height / 2, texture.Width / 2));
        }
        public void Update(GameTime pGameTime)
        {
            UpdateBoundingBox();
            UpdateBoundingSphere();
           
            if(isPlayerControlled)
            {
                if (Game1.Instance.InputManager.Pressed(Input.Up))
                {
                    position.Y -= 100 * pGameTime.ElapsedGameTime.Milliseconds / 1000F;
                }
                if (Game1.Instance.InputManager.Pressed(Input.Down))
                {
                    position.Y += 100 * pGameTime.ElapsedGameTime.Milliseconds / 1000F;
                }
                if (Game1.Instance.InputManager.Pressed(Input.Left))
                {
                    position.X -= 100 * pGameTime.ElapsedGameTime.Milliseconds / 1000F;
                }
                if (Game1.Instance.InputManager.Pressed(Input.Right))
                {
                    position.X += 100 * pGameTime.ElapsedGameTime.Milliseconds / 1000F;
                }
            }
            
        }
        public void Draw(SpriteBatch sp)
        {
            sp.Draw(texture, position, null, Color.White);
        }
    }
}
