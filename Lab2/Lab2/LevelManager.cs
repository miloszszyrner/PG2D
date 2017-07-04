using Lab2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace ToA
{
    public class LevelManager 
    {
        private GraphicsDevice graphicsDevice;
        private ContentManager content;
        public List<Sprite> spriteList;
        private TileMap tileMap;
        private XElement xDoc;
        private Song backgroundMusic;
        private SoundEffect jumpEffect;
        private int spriteCountPerLevel;
        private Camera camera;
        private SpriteFont font;
        private SpriteBatch sp;
        String tileSetFileName;
        String tileMapFileName;

        public Camera Camera
        {
            get
            {
                return camera;
            }
        }

        public TileMap TileMap
        {
            get
            {
                return tileMap;
            }
        }

        public LevelManager(String filename, GraphicsDevice graphicsDevice, ContentManager content)
        {
            xDoc = XElement.Load(filename);
            this.graphicsDevice = graphicsDevice;
            this.content = content;
            IEnumerable<XElement> game = xDoc.Elements();
            foreach (var level in game)
            {
                Game1.Instance.NumberOfLevels++;
            }
            loadLevel(Game1.Instance.levelNumber);
        }

        public void loadLevel(int levelId)
        {
            Game1.Instance.InputManager.enter = false;
            //if (spriteList != null)
            //    unloadTextures();
            IEnumerable<XElement> game = xDoc.Elements();
            foreach (var level in game)
            {
                if (Convert.ToInt32(level.Element("LevelId").Value) == levelId)
                {
                    spriteCountPerLevel = Convert.ToInt32(level.Element("SpriteCount").Value);
                    spriteList = new List<Sprite>(spriteCountPerLevel);
                    foreach (var sprite in level.Descendants("Sprite"))
                    {
                        Texture2D textureToLoad = content.Load<Texture2D>(sprite.Element("Name").Value);
                        int positionX = Convert.ToInt32((from position in sprite.Descendants("Position") select position.Element("X")).First().Value);
                        int positionY = Convert.ToInt32((from position in sprite.Descendants("Position") select position.Element("Y")).First().Value);
                        SpriteType type = (SpriteType)Enum.Parse(typeof(SpriteType), sprite.Element("SpriteType").Value);
                        if (sprite.Descendants("Animation").Any())
                        {
                            int animationX = Convert.ToInt32((from animation in sprite.Descendants("Animation") select animation.Element("SizeX")).First().Value);
                            int animationY = Convert.ToInt32((from animation in sprite.Descendants("Animation") select animation.Element("SizeY")).First().Value);
                            int frames = Convert.ToInt32((from animation in sprite.Descendants("Animation") select animation.Element("Frames")).First().Value);
                            switch (type)
                            {
                                case SpriteType.PLAYER:
                                    spriteList.Add(new Player(animationX, animationY, frames, 1f, textureToLoad, new Vector2(positionX, positionY), type));
                                    break;
                                case SpriteType.BOX:
                                    spriteList.Add(new Box(animationX, animationY, frames, 1f, textureToLoad, new Vector2(positionX, positionY), type));
                                    break;
                                case SpriteType.ENEMY:
                                    spriteList.Add(new Enemy(animationX, animationY, frames, 1f, textureToLoad, new Vector2(positionX, positionY), type));
                                    break;
                            }
                        }
                        else
                        {
                            switch (type)
                            {
                                case SpriteType.GRAVITY_UP:
                                    spriteList.Add(new GravitySprite(1f, textureToLoad, new Vector2(positionX, positionY), type));
                                    break;
                                case SpriteType.GRAVITY_DOWN:
                                    spriteList.Add(new GravitySprite(1f, textureToLoad, new Vector2(positionX, positionY), type));
                                    break;
                                case SpriteType.BUTTON:
                                    spriteList.Add(new Button(1f, textureToLoad, new Vector2(positionX, positionY), type));
                                    break;

                            }
                        }
                    }

                    tileMapFileName = (from tile in level.Descendants("TileMap") select tile.Element("FileName")).First().Value;
                    tileSetFileName = (from tile in level.Descendants("TileMap") select tile.Element("FileSet")).First().Value;
                    tileMap = new TileMap(tileMapFileName, tileSetFileName, content);
                    if (level.Descendants("Sounds").Any() && Game1.Instance.SoundManager.Sounds.Count == 0)
                    {
                        Game1.Instance.SoundManager.Sounds.Add("Jump", content.Load<SoundEffect>((from animation in level.Descendants("Sounds") select animation.Element("Jump")).First().Value));
                        Game1.Instance.SoundManager.Sounds.Add("Death", content.Load<SoundEffect>((from animation in level.Descendants("Sounds") select animation.Element("Death")).First().Value));
                        Game1.Instance.SoundManager.Sounds.Add("InteriorDoorUnlock", content.Load<SoundEffect>((from animation in level.Descendants("Sounds") select animation.Element("InteriorDoorUnlock")).First().Value));
                        Game1.Instance.SoundManager.Sounds.Add("Gravity", content.Load<SoundEffect>((from animation in level.Descendants("Sounds") select animation.Element("Gravity")).First().Value));
                    }
                    if (level.Descendants("Music").Any() && Game1.Instance.SoundManager.Songs.Count == 0)
                    {
                        Game1.Instance.SoundManager.Songs.Add("BackgroundMusic", content.Load<Song>((from animation in level.Descendants("Music") select animation.Element("BackgroundMusic")).First().Value));
                        Game1.Instance.SoundManager.Songs.Add("GameMenuMusic", content.Load<Song>((from animation in level.Descendants("Music") select animation.Element("GameMenuMusic")).First().Value));
                    }
                    font = content.Load<SpriteFont>("Content/Tekst");
                }
            }
            camera = new Camera(graphicsDevice.Viewport);
        }
        private void unloadTextures()
        {
            foreach(Sprite sprite in spriteList)
                sprite.texture.Dispose();
            foreach (Tile tile in TileMap.background)
                tile.texture.Dispose();
            foreach (Tile tile in TileMap.foreground)
                tile.texture.Dispose();
            foreach (Tile tile in TileMap.decorations)
                tile.texture.Dispose();
        }
        public void Update(GameTime gameTime)
        {
            foreach (Sprite sprite in spriteList)
            {  
                sprite.Update(gameTime);
                if (sprite.spriteType == SpriteType.PLAYER)
                    camera.Update(gameTime, sprite, tileMap);
            }
        }
        public void Draw(SpriteBatch sp)
        {
            this.sp = sp;
            Texture2D backgroundTexture = content.Load<Texture2D>("Content/background_1");
            Sprite background = new Button(1f, backgroundTexture, Vector2.Zero, SpriteType.BUTTON);
            background.Draw(sp);
            tileMap.Draw(sp);
            foreach (Sprite sprite in spriteList)
            {   
                sprite.Draw(sp);
            }
            sp.DrawString(font, (Game1.Instance.isFinishing == true) ? "Press Enter to reach next level" : "", tileMap.getNextLevelBoundingRectangle.Center.ToVector2(), Color.White);
        }

    }
}
