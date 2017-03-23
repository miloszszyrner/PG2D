﻿using Microsoft.Xna.Framework;
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
        public Texture2D texture { get; }

        public Vector2 position;
        public Vector2 velocity;

        public BoundingBox boundingBox { get; set; }
        public BoundingSphere boundingSphere { get; set; }

        public float ScaleFactor;
        public Rectangle Size;
        
        public bool isPlayerControlled;
        public bool hasJumped;
    
        public float scale
        {
            get { return ScaleFactor; }
            set
            {
                ScaleFactor = value;
                Size = new Rectangle(0, 0, (int)(texture.Width * scale), (int)(texture.Height * scale));
            }
        }
        public void setPosition(float x, float y)
        {
            position = new Vector2(x, y);
        }
        public Sprite(float scale,Texture2D texture, Vector2 position, bool isPlayerControlled = false)
        {
            this.ScaleFactor = scale;
            this.texture = texture;
            this.position = position;
            this.isPlayerControlled = isPlayerControlled;
            this.hasJumped = true;
        }
        private void UpdateBoundingBox()
        {
            boundingBox = new BoundingBox(new Vector3(position, 0), new Vector3(position.X + (texture.Height * ScaleFactor), position.Y + (texture.Width * ScaleFactor), 0));
         //   Console.WriteLine(texture.Height * ScaleFactor);
         
        }
        
        private void UpdateBoundingSphere()
        {
            //boundingSphere = new BoundingSphere(new Vector3(position.X + (texture.Width / 2), position.Y + (texture.Height / 2), 0), (float)Math.Sqrt(Math.Pow((position.X + texture.Width) - (position.X + (texture.Width / 2)), 2) + Math.Pow((position.Y + texture.Height) - (position.Y + (texture.Height / 2)), 2)));
            boundingSphere = new BoundingSphere(new Vector3(position.X + ((texture.Width * ScaleFactor) / 2), position.Y + ((texture.Height * ScaleFactor) / 2), 0), Math.Max((texture.Height * ScaleFactor) / 2, (texture.Width * ScaleFactor) / 2));
        }

        public void Update(GameTime pGameTime)
        {
            UpdateBoundingBox();
            UpdateBoundingSphere();

            position += velocity;

            if(isPlayerControlled)
            {
                //if (Game1.Instance.InputManager.Pressed(Input.Down))
                //{
                //    position.Y += 100 * pGameTime.ElapsedGameTime.Milliseconds / 1000F;
                //}
                if (Game1.Instance.InputManager.Pressed(Input.Left))
                {
                    velocity.X = -3f;
                }
                else if (Game1.Instance.InputManager.Pressed(Input.Right))
                {
                    velocity.X = 3f;
                }
                else
                {
                    velocity.X = 0f;
                }
                if (Game1.Instance.InputManager.Pressed(Input.Up) && hasJumped == false)
                {
                    position.Y -= 10f;
                    velocity.Y = -5f; //wysokość skoku
                    hasJumped = true;
                }
                if (hasJumped == true)
                {
                    float i = 1;
                    velocity.Y += 0.15f * i; //grawitacja
                }
                if (position.Y + texture.Height >= 600) //podłoga
                {
                    hasJumped = false;
                }
                if (hasJumped == false)
                {
                    velocity.Y = 0f;
                }
            }
            
        }
        public void Draw(SpriteBatch sp)
        {
            sp.Draw(texture, position, null, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
        }
    }
}
