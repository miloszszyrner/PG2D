using Lab2;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ToA
{
    public class Manager
    {
        private GraphicsDevice graphicsDevice;
        private ContentManager content;
        private List<Sprite> spriteList;
        private TileMap tileMap;
        private XElement xDoc;
        private Song song;
        private SoundEffect soundEffect;
        private int spriteCountPerLevel;
        private int levelId;
        public Manager(String filename, GraphicsDevice graphicsDevice, ContentManager content)
        {
            xDoc = XElement.Load(filename);
            this.graphicsDevice = graphicsDevice;
            this.content = content;
            parseXML(1);
        }

        private void parseXML(int levelId)
        {
            IEnumerable<XElement> game = xDoc.Elements();
            foreach (var level in game)
            {
                if(Convert.ToInt32(level.Element("LevelId").Value) == levelId)
                {
                    spriteCountPerLevel = Convert.ToInt32(level.Element("SpriteCount").Value);
                    spriteList = new List<Sprite>(spriteCountPerLevel);
                    foreach(var sprite in (IEnumerable <XElement>)level)
                    {
                        Texture2D textureToLoad = Texture2D.FromStream(graphicsDevice, File.OpenRead(sprite.Element("Name").Value));
                        var positions = from position in xDoc.Elements("Sprite") select position;
                        int positionX = Convert.ToInt32(sprite.Element("Name").Value);
                        int positionY = Convert.ToInt32(sprite.Element("Name").Value);
                        SpriteType type =(SpriteType) Enum.Parse(typeof(SpriteType), sprite.Element("SpriteType").Value);
                        spriteList.Add(new Sprite(1f, textureToLoad, new Vector2(150, 800), SpriteType.BOX);
                    }

                }
                
            }
        }
    }
}
