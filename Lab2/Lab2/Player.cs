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
    public class Player : MovingSprite
    {
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
        
    public override void Update(GameTime pGameTime, SoundEffect effect)
        {
            objectPreviousPosition = position;
            position += velocity;
            base.Update(pGameTime, effect);
            checkPlayerLevelToolsCollision();
            movement(effect, pGameTime);
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
                        Game1.Instance.DisplayManager.Manager.loadLevel(Game1.Instance.levelNumber);

                    }
                    if (boundingBox.Intersects(Game1.Instance.DisplayManager.Manager.TileMap.getNextLevelBoundingRectangle))  //przejscie do kolejnego poziomu
                    {
                        Game1.Instance.isFinishing = true;
                        if (Game1.Instance.InputManager.enter)
                        {
                            Game1.Instance.DisplayManager.Manager.loadLevel(++Game1.Instance.levelNumber);
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
