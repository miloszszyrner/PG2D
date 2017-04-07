﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
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
        private SpriteEffects flip = SpriteEffects.None;

        private bool isPlayerControlled;
        private bool hasJumped;
		private bool isGravity;
		private bool gravity;

        private Rectangle sourceRectangle;
        private Rectangle destinationRectangle;

        private int currentFrame = 0;
        private int totalFrames = 13;

        private float elapsed;
        private float delay = 100f;
    
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
            this.gravity = true;
        }
        private void UpdateBoundingBox()
        {
            boundingBox = new BoundingBox(new Vector3(position, 0), new Vector3(position.X + (95 * ScaleFactor), position.Y + (157 * ScaleFactor), 0));
         //   Console.WriteLine(texture.Height * ScaleFactor);
         
        }
        private void UpdateBoundingSphere()
        {
            //boundingSphere = new BoundingSphere(new Vector3(position.X + (texture.Width / 2), position.Y + (texture.Height / 2), 0), (float)Math.Sqrt(Math.Pow((position.X + texture.Width) - (position.X + (texture.Width / 2)), 2) + Math.Pow((position.Y + texture.Height) - (position.Y + (texture.Height / 2)), 2)));
            boundingSphere = new BoundingSphere(new Vector3(position.X + ((texture.Width * ScaleFactor) / 2), position.Y + ((texture.Height * ScaleFactor) / 2), 0), Math.Max((texture.Height * ScaleFactor) / 2, (texture.Width * ScaleFactor) / 2));
        }

		public void Update(GameTime pGameTime, SoundEffect effect)
		{
			position += velocity;
			UpdateBoundingBox();
			UpdateBoundingSphere();

			if (isPlayerControlled)
			{

				checkCollisions();
				movement(effect, pGameTime);
                destinationRectangle = new Rectangle((int)position.X, (int)position.Y, 95, 157);
			}
		}
        public void Draw(SpriteBatch sp)
        {
            sp.Draw(texture, position, null, Color.White, 0.0f, Vector2.Zero, scale, flip, 0);
		    }
		private void checkCollisions()
		{
			for (int i = 0; i < Game1.Instance.TileMap.mapWidth; i++)
				for (int j = 0; j < Game1.Instance.TileMap.mapHeight; j++)
				{
					if (Game1.Instance.TileMap.getTileAt(i, j).property == TileProperty.EARTH) //spadanie
					{
						if (gravity)
						{
							hasJumped = true;
						}
						else
						{
							isGravity = false;
						}
					}
				}
			for (int i = 0; i < Game1.Instance.TileMap.mapWidth; i++)
				for (int j = 0; j < Game1.Instance.TileMap.mapHeight; j++)
				{
					if (Game1.Instance.TileMap.getTileAt(i, j).property == TileProperty.FLOOR && boundingBox.Intersects(Game1.Instance.TileMap.getTileAt(i, j).getBoundingBox))  //utrzymywanie sie na powierzchni
					{
						hasJumped = false;
					}
					if (Game1.Instance.TileMap.getTileAt(i, j).property == TileProperty.FLOOR_LEFT && boundingBox.Intersects(Game1.Instance.TileMap.getTileAt(i, j).getBoundingBox))  //utrzymywanie sie na powierzchni
					{
						hasJumped = false;
					}
					if (Game1.Instance.TileMap.getTileAt(i, j).property == TileProperty.FLOOR_RIGHT && boundingBox.Intersects(Game1.Instance.TileMap.getTileAt(i, j).getBoundingBox))  //utrzymywanie sie na powierzchni
					{
						hasJumped = false;
					}
					if (Game1.Instance.TileMap.getTileAt(i, j).property == TileProperty.BOTTOM && boundingBox.Intersects(Game1.Instance.TileMap.getTileAt(i, j).getBoundingBox))  //utrzymywanie sie w zmianie grawitacji
					{
						isGravity = true;
					}
					if (Game1.Instance.TileMap.getTileAt(i, j).property == TileProperty.PLATFORM_CENTER && boundingBox.Intersects(Game1.Instance.TileMap.getTileAt(i, j).getBoundingBox))  //utrzymywanie sie na platformie
					{
						hasJumped = false;
					}
					if (Game1.Instance.TileMap.getTileAt(i, j).property == TileProperty.PLATFORM_LEFT && boundingBox.Intersects(Game1.Instance.TileMap.getTileAt(i, j).getBoundingBox))  //utrzymywanie sie na platformie
					{
						hasJumped = false;
					}
					if (Game1.Instance.TileMap.getTileAt(i, j).property == TileProperty.PLATFORM_RIGHT && boundingBox.Intersects(Game1.Instance.TileMap.getTileAt(i, j).getBoundingBox))  //utrzymywanie sie na platformie
					{
						hasJumped = false;
					}
					if (Game1.Instance.TileMap.getTileAt(i, j).property == TileProperty.BASE_LEFT && boundingBox.Intersects(Game1.Instance.TileMap.getTileAt(i, j).getBoundingBox))  //uderzenie o sciane
					{
						velocity.X = 0f;
						position.X--;
					}
					if (Game1.Instance.TileMap.getTileAt(i, j).property == TileProperty.BASE_RIGHT && boundingBox.Intersects(Game1.Instance.TileMap.getTileAt(i, j).getBoundingBox))  //uderzenie o sciane
					{
						velocity.X = 0f;
						position.X++;
					}
					if (Game1.Instance.TileMap.getTileAt(i, j).property == TileProperty.TRAP && boundingBox.Intersects(Game1.Instance.TileMap.getTileAt(i, j).getBoundingBox))  //wpada w polapke
					{
						Console.WriteLine("Dead");
					}

				}
		}

		private void movement(SoundEffect effect, GameTime pGameTime)
		{
			if (Game1.Instance.InputManager.changeGravity)
			{
				if (!gravity)
					gravity = true;
				else
					gravity = false;
			}
			if (Game1.Instance.InputManager.up && gravity && !hasJumped)
			{
				position.Y -= 10f;
				velocity.Y = -5f;
				hasJumped = true;
				effect.Play(0.1f,0f,0f);
			}
			if (Game1.Instance.InputManager.up && !gravity && isGravity)
			{
				position.Y += 10f;
				velocity.Y = 5f;
				isGravity = false;
				effect.Play();
			}
            if (Game1.Instance.InputManager.right)
            {
                velocity.X = -3f;
                Animate(pGameTime, 1);
            }
			else if (Game1.Instance.InputManager.left)
            {
                velocity.X = 3f;
                Animate(pGameTime, 0);
            }
            else
                sourceRectangle = new Rectangle(0,0, 95, 157);
            if (Game1.Instance.InputManager.right == Game1.Instance.InputManager.left)
				velocity.X = 0f;

			if (hasJumped && gravity)
			{
				float i = 1;
				velocity.Y += 0.15f * i; //grawitacja
			}
			if (!hasJumped && gravity)
				velocity.Y = 0f;

			if (!isGravity && !gravity)
			{
				float i = 1;
				velocity.Y -= 0.15f * i; //grawitacja
			}
			if (isGravity && !gravity)
				velocity.Y = 0f;

            if (gravity)
                flip = SpriteEffects.None;
            else
                flip = SpriteEffects.FlipVertically;
		}
        private void Animate(GameTime pGameTime, int row)
        {
            elapsed += (float)pGameTime.ElapsedGameTime.TotalMilliseconds;
            if(elapsed >= delay)
            {
                if (currentFrame >= totalFrames)
                    currentFrame = 0;
                else
                    currentFrame++;
                elapsed = 0;
            }
            sourceRectangle = new Rectangle(95 * currentFrame, row * 157, 95, 157);

        }
    }
}
