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
    public abstract class MovingSprite : Sprite
    {
        public Rectangle sourceRectangle;
        public Rectangle destinationRectangle;

        public Vector2 objectPreviousPosition;
        public Vector2 velocity;

        public bool hasJumped;
        public bool isGravity;

        public int animationX;
        public int animationY;
        public int currentFrame = 0;
        public int totalFrames;

        public float elapsed;
        public float delay = 100f;

        public MovingSprite(int animationX, int animationY, int totalFrames, float scale, Texture2D texture, Vector2 position, SpriteType spriteType = SpriteType.TEST) : base(scale, texture, position, spriteType)
        {
            this.animationX = animationX;
            this.animationY = animationY;
            this.totalFrames = totalFrames;

        }
        public override void UpdateBoundingBox()
        {
            boundingBox = new Rectangle((int)position.X, (int)position.Y, animationX, animationY);
        }
        public override void Update(GameTime pGameTime, SoundEffect effect)
        {
            base.Update(pGameTime, effect);
            checkCollisions();
            checkGravitation();
        }
        public virtual void checkCollisions()
        {
            int offset = 15;
            for (int i = 0; i < Game1.Instance.DisplayManager.Manager.TileMap.mapWidth; i++)
                for (int j = 0; j < Game1.Instance.DisplayManager.Manager.TileMap.mapHeight; j++)
                {
                    if (Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).property == TileProperty.EARTH) //spadanie
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
            for (int i = 0; i < Game1.Instance.DisplayManager.Manager.TileMap.mapWidth; i++)
                for (int j = 0; j < Game1.Instance.DisplayManager.Manager.TileMap.mapHeight; j++)
                {

                    if (Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).property == TileProperty.FLOOR && boundingBox.Intersects(Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).getBoundingBox))  //utrzymywanie sie na powierzchni
                    {
                        hasJumped = false;
                    }
                    if (Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).property == TileProperty.FLOOR_LEFT && boundingBox.Intersects(Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).getBoundingBox))  //utrzymywanie sie na powierzchni
                    {
                        if (boundingBox.Bottom - offset > Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).getBoundingBox.Top)
                            position.X = objectPreviousPosition.X;
                        else if (boundingBox.Center.Y < Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).getBoundingBox.Center.Y)
                            hasJumped = false;
                    }
                    if (Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).property == TileProperty.FLOOR_RIGHT && boundingBox.Intersects(Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).getBoundingBox))  //utrzymywanie sie na powierzchni
                    {
                        if (boundingBox.Bottom - offset > Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).getBoundingBox.Top)
                            position.X = objectPreviousPosition.X;
                        else if (boundingBox.Center.Y < Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).getBoundingBox.Center.Y)
                            hasJumped = false;
                    }
                    if (Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).property == TileProperty.BOTTOM && boundingBox.Intersects(Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).getBoundingBox))  //utrzymywanie sie w zmianie grawitacji
                    {
                        isGravity = true;
                    }
                    if (Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).property == TileProperty.PLATFORM_CENTER && boundingBox.Intersects(Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).getBoundingBox))  //utrzymywanie sie na platformie
                    {
                        if (boundingBox.Center.Y < Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).getBoundingBox.Center.Y)
                            hasJumped = false;
                        else
                            velocity.Y = -velocity.Y;
                    }
                    if (Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).property == TileProperty.PLATFORM_LEFT && boundingBox.Intersects(Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).getBoundingBox))  //utrzymywanie sie na platformie
                    {
                        if (boundingBox.Bottom - offset > Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).getBoundingBox.Top)
                            position.X = objectPreviousPosition.X;
                        else if (boundingBox.Center.Y < Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).getBoundingBox.Center.Y)
                            hasJumped = false;
                        else
                            velocity.Y = -velocity.Y;
                    }
                    if (Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).property == TileProperty.PLATFORM_RIGHT && boundingBox.Intersects(Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).getBoundingBox))  //utrzymywanie sie na platformie
                    {
                        if (boundingBox.Bottom - offset > Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).getBoundingBox.Top)
                            position.X = objectPreviousPosition.X;
                        else if (boundingBox.Center.Y < Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).getBoundingBox.Center.Y)
                            hasJumped = false;
                        else
                            velocity.Y = -velocity.Y;
                    }
                    if (Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).property == TileProperty.BASE_LEFT && boundingBox.Intersects(Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).getBoundingBox))  //uderzenie o sciane
                    {
                        position.X = objectPreviousPosition.X;
                    }
                    if (Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).property == TileProperty.BASE_RIGHT && boundingBox.Intersects(Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).getBoundingBox))  //uderzenie o sciane
                    {
                        position.X = objectPreviousPosition.X;
                    }
                }

        }
        public void Animate(GameTime pGameTime, int row)
        {
            elapsed += (float)pGameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsed >= delay)
            {
                if (currentFrame >= totalFrames)
                    currentFrame = 0;
                else
                    currentFrame++;
                elapsed = 0;
            }
            sourceRectangle = new Rectangle(animationX * currentFrame, row * animationY, animationX, animationY);

        }
        public void checkGravitation()
        {
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
        }
    }
}
