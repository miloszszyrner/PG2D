﻿using Lab2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace ToA
{
    [Serializable()]
    public class LevelManager : ISerializable
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
                            spriteList.Add(new Sprite(animationX, animationY, frames, 1f, textureToLoad, new Vector2(positionX, positionY), type));
                        }
                        else
                        {
                            spriteList.Add(new Sprite(1f, textureToLoad, new Vector2(positionX, positionY), type));
                        }
                    }

                    tileMapFileName = (from tile in level.Descendants("TileMap") select tile.Element("FileName")).First().Value;
                    tileSetFileName = (from tile in level.Descendants("TileMap") select tile.Element("FileSet")).First().Value;
                    tileMap = new TileMap(tileMapFileName, tileSetFileName, content);
                    jumpEffect = content.Load<SoundEffect>(level.Element("SoundEffect").Value);
                    backgroundMusic = content.Load<Song>(level.Element("Song").Value);
                    font = content.Load<SpriteFont>("Content/Tekst");
                    MediaPlayer.Play(backgroundMusic);
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Volume = 0.1f;
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
                sprite.Update(gameTime, jumpEffect);
                if (sprite.spriteType == SpriteType.PLAYER)
                    camera.Update(gameTime, sprite, tileMap);
            }
        }
        public void Draw(SpriteBatch sp)
        {
            this.sp = sp;
            tileMap.Draw(sp);
            foreach (Sprite sprite in spriteList)
            {   
                sprite.Draw(sp);
            }
            sp.DrawString(font, (Game1.Instance.isFinishing == true) ? "Press Enter to reach next level" : "", tileMap.getNextLevelBoundingRectangle.Center.ToVector2(), Color.White);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}