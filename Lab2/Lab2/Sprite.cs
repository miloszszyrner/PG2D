using Microsoft.Xna.Framework;
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
        
        private bool isPlayerControlled;
        private bool hasJumped;
		private bool isGravity;
		private bool gravity;
    
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
            boundingBox = new BoundingBox(new Vector3(position, 0), new Vector3(position.X + (texture.Width * ScaleFactor), position.Y + (texture.Height * ScaleFactor), 0));
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
				movement(effect);
			}
		}
        public void Draw(SpriteBatch sp)
        {
            sp.Draw(texture, position, null, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
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
					if (Game1.Instance.TileMap.getTileAt(i, j).property == TileProperty.GROUND && boundingBox.Intersects(Game1.Instance.TileMap.getTileAt(i, j).getBoundingBox))  //utrzymywanie sie w zmianie grawitacji
					{
						isGravity = true;
					}
					if (Game1.Instance.TileMap.getTileAt(i, j).property == TileProperty.PLATFORM && boundingBox.Intersects(Game1.Instance.TileMap.getTileAt(i, j).getBoundingBox))  //utrzymywanie sie na platformie
					{
						hasJumped = false;
					}
					if (Game1.Instance.TileMap.getTileAt(i, j).property == TileProperty.WALL_LEFT && boundingBox.Intersects(Game1.Instance.TileMap.getTileAt(i, j).getBoundingBox))  //uderzenie o sciane
					{
						velocity.X = 0f;
						position.X--;
					}
					if (Game1.Instance.TileMap.getTileAt(i, j).property == TileProperty.WALL_RIGHT && boundingBox.Intersects(Game1.Instance.TileMap.getTileAt(i, j).getBoundingBox))  //uderzenie o sciane
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
		private void movement(SoundEffect effect)
		{

			if (Game1.Instance.InputManager.up && !hasJumped)
			{
				position.Y -= 10f;
				velocity.Y = -5f;
				hasJumped = true;
				effect.Play();
			}
			if (Game1.Instance.InputManager.right)
				velocity.X = -3f;
			if (Game1.Instance.InputManager.left)
				velocity.X = 3f;
			if (Game1.Instance.InputManager.right == Game1.Instance.InputManager.left)
				velocity.X = 0f;

			if (hasJumped && gravity)
			{
				float i = 1;
				velocity.Y += 0.15f * i; //grawitacja
			}
			if (!hasJumped && gravity)
				velocity.Y = 0f;

			if (Game1.Instance.InputManager.changeGravity)
			{
				if (!gravity)
					gravity = true;
				else
					gravity = false;
			}
			if (!isGravity && !gravity)
			{
				float i = 1;
				velocity.Y -= 0.15f * i; //grawitacja
			}
			if (isGravity && !gravity)
				velocity.Y = 0f;
		}
    }
}
