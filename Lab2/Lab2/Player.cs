using Lab2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.Threading;

namespace ToA
{
    public class Player : MovingSprite
    {
        private SpriteEffects flip = SpriteEffects.None;

        public Player(int animationX, int animationY, int totalFrames, float scale, Texture2D texture, Vector2 position, SpriteType spriteType = SpriteType.TEST) : base(animationX, animationY, totalFrames, scale, texture, position, spriteType)
        {
        }
        private void movement(GameTime pGameTime)
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
                position.Y -= 6.5f;
                velocity.Y = -9f;
                hasJumped = true;
                Game1.Instance.SoundManager.Sounds["Jump"].Play(Game1.Instance.spriteEffectVolume, 0f, 0f);
                Animate(pGameTime, 2);
            }
            if (Game1.Instance.InputManager.up && !gravity && isGravity)
            {
                position.Y += 6.5f;
                velocity.Y = 9f;
                isGravity = false;
                Game1.Instance.SoundManager.Sounds["Jump"].Play(Game1.Instance.spriteEffectVolume, 0f, 0f);
            }
            if (Game1.Instance.InputManager.right)
            {
                velocity.X = -3f;
                if (hasJumped)
                {
                    Animate(pGameTime, 3);
                }
                else
                {
                    Animate(pGameTime, 1);
                }
            }
            else if (Game1.Instance.InputManager.left)
            {
                velocity.X = 3f;
                if (hasJumped)
                {
                    Animate(pGameTime, 2);
                }
                else
                {
                    Animate(pGameTime, 0);
                }
            }
            else
                sourceRectangle = new Rectangle(0, 0, animationX, animationY);
           
            if (Game1.Instance.InputManager.right == Game1.Instance.InputManager.left)
                velocity.X = 0f;

            if(!Game1.Instance.InputManager.right && !Game1.Instance.InputManager.left && hasJumped)
            {
                Animate(pGameTime, 2);
            }

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
        
    public override void Update(GameTime pGameTime)
        {
            objectPreviousPosition = position;
            position += velocity;
            base.Update(pGameTime);
            checkPlayerLevelToolsCollision();
            movement(pGameTime);
            destinationRectangle = new Rectangle((int)position.X, (int)position.Y, animationX, animationY);   
    }
        private void checkPlayerLevelToolsCollision()
        {
            foreach (Sprite sprite in Game1.Instance.DisplayManager.Manager.spriteList)
            {
                if (sprite.spriteType == SpriteType.GRAVITY_DOWN)
                {
                    if (boundingBox.Intersects(sprite.boundingBox) && Game1.Instance.InputManager.action)
                    {
                        gravity = false;
                        Game1.Instance.SoundManager.Sounds["Gravity"].Play(Game1.Instance.spriteEffectVolume, 0f, 0f);
                    }
                }
                if (sprite.spriteType == SpriteType.GRAVITY_UP)
                {
                    if (boundingBox.Intersects(sprite.boundingBox) && Game1.Instance.InputManager.action)
                    {
                        gravity = true;
                        Game1.Instance.SoundManager.Sounds["Gravity"].Play(Game1.Instance.spriteEffectVolume, 0f, 0f);
                    }
                }
                if (sprite.spriteType == SpriteType.BOX)
                {
                    sprite.gravity = gravity;

                    if (boundingBox.Bottom > sprite.boundingBox.Top && boundingBox.Bottom < sprite.boundingBox.Bottom && boundingBox.Intersects(sprite.boundingBox))
                    {
                        position.Y = objectPreviousPosition.Y;
                        hasJumped = false;
                    }
                    else if (boundingBox.Right > sprite.boundingBox.Left && boundingBox.Right < sprite.boundingBox.Right && boundingBox.Bottom > sprite.boundingBox.Top)
                    {
                        sprite.setPosition(boundingBox.Right, sprite.position.Y);
                    }
                    else if (boundingBox.Left < sprite.boundingBox.Right && boundingBox.Right > sprite.boundingBox.Right && boundingBox.Bottom > sprite.boundingBox.Top)
                    {
                        sprite.setPosition(boundingBox.Left - 100, sprite.position.Y);
                    }
                    for (int i = 0; i < Game1.Instance.DisplayManager.Manager.TileMap.mapWidth; i++)
                        for (int j = 0; j < Game1.Instance.DisplayManager.Manager.TileMap.mapHeight; j++)
                        {
                            if (Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).property == TileProperty.BASE_LEFT && sprite.boundingBox.Intersects(Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).getBoundingBox) && boundingBox.Intersects(sprite.boundingBox))  //uderzenie o sciane
                            {
                                sprite.position.X = objectPreviousPosition.X + (boundingBox.Right - boundingBox.Left);
                                position.X = objectPreviousPosition.X;
                            }
                            if (Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).property == TileProperty.BASE_RIGHT && sprite.boundingBox.Intersects(Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).getBoundingBox) && boundingBox.Intersects(sprite.boundingBox))  //uderzenie o sciane
                            {
                                sprite.position.X = objectPreviousPosition.X - (sprite.boundingBox.Right - sprite.boundingBox.Left);
                                position.X = objectPreviousPosition.X;
                            }
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
           
            for (int i = 0; i < Game1.Instance.DisplayManager.Manager.TileMap.mapWidth; i++)
                for (int j = 0; j < Game1.Instance.DisplayManager.Manager.TileMap.mapHeight; j++)
                {
                    if (Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).property == TileProperty.TRAP && boundingBox.Intersects(Game1.Instance.DisplayManager.Manager.TileMap.getTileAt(i, j).getBoundingBox))  //wpada w polapke
                    {
                        Game1.Instance.SoundManager.Sounds["Death"].Play(Game1.Instance.spriteEffectVolume, 0f, 0f);
                        Game1.Instance.DisplayManager.Manager.loadLevel(Game1.Instance.levelNumber);

                    }
                    if (boundingBox.Intersects(Game1.Instance.DisplayManager.Manager.TileMap.getNextLevelBoundingRectangle))  //przejscie do kolejnego poziomu
                    {
                        Game1.Instance.isFinishing = true;
                        if (Game1.Instance.InputManager.enter)
                        {
                            Game1.Instance.SoundManager.Sounds["InteriorDoorUnlock"].Play(Game1.Instance.spriteEffectVolume, 0f, 0f);
                            if(++Game1.Instance.levelNumber <=3)
                                Game1.Instance.DisplayManager.Manager.loadLevel(Game1.Instance.levelNumber);
                            Game1.Instance.isFinishing = false;
                        }
                    }
                    else
                    {
                        Game1.Instance.isFinishing = false;
                    }

                }
        }
        private void AnimateJump(GameTime pGameTime, int row)
        {
            while (hasJumped)
            {
                if (currentFrame >= totalFrames)
                    currentFrame = 0;
                else
                    currentFrame++;

                sourceRectangle = new Rectangle(animationX * currentFrame, row * animationY, animationX, animationY);
            }
        }
    }
}
