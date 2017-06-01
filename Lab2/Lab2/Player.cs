﻿using Lab2;
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
    public class Player : Sprite
    {
        public Vector2 objectPreviousPosition;
        public Vector2 velocity;

        private bool hasJumped;
        private bool isGravity;
        private int offset = 15;

        private SpriteEffects flip = SpriteEffects.None;

        public Player(int animationX, int animationY, int totalFrames, float scale, Texture2D texture, Vector2 position, SpriteType spriteType = SpriteType.TEST) : base(animationX, animationY, totalFrames, scale, texture, position, spriteType)
        {
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
                velocity.Y = -12f;
                hasJumped = true;
                effect.Play(0.1f, 0f, 0f);
            }
            if (Game1.Instance.InputManager.up && !gravity && isGravity)
            {
                position.Y += 10f;
                velocity.Y = 12f;
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
                sourceRectangle = new Rectangle(0, 0, animationX, animationY);
            if (Game1.Instance.InputManager.right == Game1.Instance.InputManager.left)
                velocity.X = 0f;

            checkGravitation();

            if (gravity)
            {
                flip = SpriteEffects.None;
                UpdateBoundingBox();
            }
            else
            {
                flip = SpriteEffects.FlipVertically;
                UpdateBoundingBox();
            }
        }
        private void checkGravitation()
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
    public override void Update(GameTime pGameTime, SoundEffect effect)
        {
            objectPreviousPosition = position;
            position += velocity;
            base.Update(pGameTime, effect);
            checkPlayerLevelToolsCollision();
            checkGravitation();
            movement(effect, pGameTime);
            destinationRectangle = new Rectangle((int)position.X, (int)position.Y, animationX, animationY);   
    }
        private void checkPlayerLevelToolsCollision()
        {
            foreach (Sprite sprite in Game1.Instance.Manager.spriteList)
            {
                if (sprite.spriteType == SpriteType.GRAVITY_DOWN)
                {
                    if (boundingBox.Intersects(sprite.boundingBox) && Game1.Instance.InputManager.action)
                    {
                        gravity = false;
                    }
                }
                if (sprite.spriteType == SpriteType.GRAVITY_UP)
                {
                    if (boundingBox.Intersects(sprite.boundingBox) && Game1.Instance.InputManager.action)
                    {
                        gravity = true;
                    }
                }
                if (sprite.spriteType == SpriteType.BOX)
                {
                    sprite.gravity = gravity;
                    if (boundingBox.Intersects(sprite.boundingBox) && Game1.Instance.InputManager.action)
                    {
                        sprite.setPosition(position.X, position.Y);
                    }
                    if (boundingBox.Contains(sprite.boundingBox) && Game1.Instance.InputManager.action)
                    {
                        sprite.setPosition(position.X, position.Y);
                    }
                }

            }
        }
        public override void Draw(SpriteBatch sp)
        {
            sp.Draw(texture, destinationRectangle, sourceRectangle, Color.White, 0.0f, Vector2.Zero, flip, 0.0f);
        }
        public override void checkCollisions()
        {
            base.checkCollisions();
            for (int i = 0; i < Game1.Instance.Manager.TileMap.mapWidth; i++)
                for (int j = 0; j < Game1.Instance.Manager.TileMap.mapHeight; j++)
                {
                    if (Game1.Instance.Manager.TileMap.getTileAt(i, j).property == TileProperty.EARTH) //spadanie
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
            for (int i = 0; i < Game1.Instance.Manager.TileMap.mapWidth; i++)
                for (int j = 0; j < Game1.Instance.Manager.TileMap.mapHeight; j++)
                {
                    if (Game1.Instance.Manager.TileMap.getTileAt(i, j).property == TileProperty.FLOOR && boundingBox.Intersects(Game1.Instance.Manager.TileMap.getTileAt(i, j).getBoundingBox))  //utrzymywanie sie na powierzchni
                    {
                        hasJumped = false;
                    }
                    if (Game1.Instance.Manager.TileMap.getTileAt(i, j).property == TileProperty.FLOOR_LEFT && boundingBox.Intersects(Game1.Instance.Manager.TileMap.getTileAt(i, j).getBoundingBox))  //utrzymywanie sie na powierzchni
                    {
                        if (boundingBox.Bottom - offset > Game1.Instance.Manager.TileMap.getTileAt(i, j).getBoundingBox.Top)
                            position.X = objectPreviousPosition.X;
                        else if (boundingBox.Center.Y < Game1.Instance.Manager.TileMap.getTileAt(i, j).getBoundingBox.Center.Y)
                            hasJumped = false;
                    }
                    if (Game1.Instance.Manager.TileMap.getTileAt(i, j).property == TileProperty.FLOOR_RIGHT && boundingBox.Intersects(Game1.Instance.Manager.TileMap.getTileAt(i, j).getBoundingBox))  //utrzymywanie sie na powierzchni
                    {
                        if (boundingBox.Bottom - offset > Game1.Instance.Manager.TileMap.getTileAt(i, j).getBoundingBox.Top)
                            position.X = objectPreviousPosition.X;
                        else if (boundingBox.Center.Y < Game1.Instance.Manager.TileMap.getTileAt(i, j).getBoundingBox.Center.Y)
                            hasJumped = false;
                    }
                    if (Game1.Instance.Manager.TileMap.getTileAt(i, j).property == TileProperty.BOTTOM && boundingBox.Intersects(Game1.Instance.Manager.TileMap.getTileAt(i, j).getBoundingBox))  //utrzymywanie sie w zmianie grawitacji
                    {
                        isGravity = true;
                    }
                    if (Game1.Instance.Manager.TileMap.getTileAt(i, j).property == TileProperty.PLATFORM_CENTER && boundingBox.Intersects(Game1.Instance.Manager.TileMap.getTileAt(i, j).getBoundingBox))  //utrzymywanie sie na platformie
                    {
                        if (boundingBox.Center.Y < Game1.Instance.Manager.TileMap.getTileAt(i, j).getBoundingBox.Center.Y)
                            hasJumped = false;
                        else
                            velocity.Y = -velocity.Y;
                    }
                    if (Game1.Instance.Manager.TileMap.getTileAt(i, j).property == TileProperty.PLATFORM_LEFT && boundingBox.Intersects(Game1.Instance.Manager.TileMap.getTileAt(i, j).getBoundingBox))  //utrzymywanie sie na platformie
                    {
                        if (boundingBox.Bottom - offset > Game1.Instance.Manager.TileMap.getTileAt(i, j).getBoundingBox.Top)
                            position.X = objectPreviousPosition.X;
                        else if (boundingBox.Center.Y < Game1.Instance.Manager.TileMap.getTileAt(i, j).getBoundingBox.Center.Y)
                            hasJumped = false;
                        else
                            velocity.Y = -velocity.Y;
                    }
                    if (Game1.Instance.Manager.TileMap.getTileAt(i, j).property == TileProperty.PLATFORM_RIGHT && boundingBox.Intersects(Game1.Instance.Manager.TileMap.getTileAt(i, j).getBoundingBox))  //utrzymywanie sie na platformie
                    {
                        if (boundingBox.Bottom - offset > Game1.Instance.Manager.TileMap.getTileAt(i, j).getBoundingBox.Top)
                            position.X = objectPreviousPosition.X;
                        else if (boundingBox.Center.Y < Game1.Instance.Manager.TileMap.getTileAt(i, j).getBoundingBox.Center.Y)
                            hasJumped = false;
                        else
                            velocity.Y = -velocity.Y;
                    }
                    if (Game1.Instance.Manager.TileMap.getTileAt(i, j).property == TileProperty.BASE_LEFT && boundingBox.Intersects(Game1.Instance.Manager.TileMap.getTileAt(i, j).getBoundingBox))  //uderzenie o sciane
                    {
                        position.X = objectPreviousPosition.X;
                    }
                    if (Game1.Instance.Manager.TileMap.getTileAt(i, j).property == TileProperty.BASE_RIGHT && boundingBox.Intersects(Game1.Instance.Manager.TileMap.getTileAt(i, j).getBoundingBox))  //uderzenie o sciane
                    {
                        position.X = objectPreviousPosition.X;
                    }
                    if (Game1.Instance.Manager.TileMap.getTileAt(i, j).property == TileProperty.TRAP && boundingBox.Intersects(Game1.Instance.Manager.TileMap.getTileAt(i, j).getBoundingBox))  //wpada w polapke
                    {
                        position = new Vector2(150, 800);
                    }
                    if (boundingBox.Intersects(Game1.Instance.Manager.TileMap.getNextLevelBoundingRectangle))  //przejscie do kolejnego poziomu
                    {
                        Game1.Instance.isFinishing = true;
                        if (Game1.Instance.InputManager.enter)
                        {
                            Game1.Instance.Manager.loadLevel(++Game1.Instance.levelNumber);
                            Game1.Instance.isFinishing = false;
                        }
                    }
                    else
                    {
                        Game1.Instance.isFinishing = false;
                    }

                }
        }
    }
}