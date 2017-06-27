﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ToA;

namespace ToA
{
    public class TileMap
    {
        public Tile[,] background { get; }
		public Tile[,] decorations { get; }
        public Tile[,] foreground { get; }
        XDocument xDoc;
        public Dictionary<int, TileProperty> tilePropertyType;
        public int mapWidth { get; }
        public int mapHeight { get; }
        int tilecount;
        int columns;
        public int tilewidth { get; }
        int tileheight;
        String tileSetFileName;
        ContentManager content;
        private Rectangle nextLevelBoundingRectangle;
        public Rectangle getNextLevelBoundingRectangle
        {
            get
            {
                return nextLevelBoundingRectangle;
            }
        }

        public TileMap(String tileMapFileName, String tileSetFileName, ContentManager content)
        {
            xDoc = XDocument.Load(tileMapFileName);
            mapWidth = int.Parse(xDoc.Root.Attribute("width").Value);
            mapHeight = int.Parse(xDoc.Root.Attribute("height").Value);
            tilewidth = int.Parse(xDoc.Root.Attribute("tilewidth").Value);
            tileheight = int.Parse(xDoc.Root.Attribute("tileheight").Value);
            tilecount = int.Parse(xDoc.Root.Element("tileset").Attribute("tilecount").Value);
            columns = int.Parse(xDoc.Root.Element("tileset").Attribute("columns").Value);
			background = new Tile[mapWidth, mapHeight];
			decorations = new Tile[mapWidth, mapHeight];
			foreground = new Tile[mapWidth, mapHeight];
			this.tileSetFileName = tileSetFileName;
            this.content = content;
            tilePropertyType = new Dictionary<int, TileProperty>
            {
                {1, TileProperty.BACKGROUND},
                {2, TileProperty.TRAP }, //kolce
                {3, TileProperty.BACKGROUND  }, //tlo
                {4, TileProperty.COLUMN_DOWN_LEFT },
                {5, TileProperty.COLUMN_DOWN_RIGHT },
                {6, TileProperty.COLUMN_MIDDLE_DECO_LEFT },
                {7, TileProperty.WINDOW_DOWN_LEFT },
                {8, TileProperty.COLUMN_MIDDLE_DECO_RIGHT },
                {9, TileProperty.COLUMN_MIDDLE_LEFT },
                {10, TileProperty.COLUMN_MIDDLE_RIGHT },
                {11, TileProperty.COLUMN_TOP_LEFT },
                {12, TileProperty.COLUMN_TOP_RIGHT },
               // {13, TileProperty.BACKGROUND },//13 - nieużyta
				{14, TileProperty.WINDOW_DOWN_RIGHT },
                //{15, TileProperty.BACKGROUND },//15 - nieużyta
				{16, TileProperty.SIGN },
                {17, TileProperty.SIGN },
                {18, TileProperty.PLATFORM_CENTER },
                {19, TileProperty.PLATFORM_LEFT },
                {20, TileProperty.PLATFORM_RIGHT },
                {21, TileProperty.WINDOW_MIDDLE_LEFT },
                {22, TileProperty.TRAP },
                {23, TileProperty.TRAP },
                {24, TileProperty.TRAP },
                {25, TileProperty.STAIRS_PART1 },
                {26, TileProperty.STAIRS_PART2 },
                {27, TileProperty.STAIRS_PART3 },
                {28, TileProperty.WINDOW_MIDDLE_RIGHT },
                {29, TileProperty.STAIRS_PART4 },
                {30, TileProperty.BASE },
                {31, TileProperty.BASE_LEFT },
                {32, TileProperty.BASE_RIGHT },
                {33, TileProperty.BOTTOM },
                {34, TileProperty.BOTTOM },
                {35, TileProperty.WINDOW_UP_LEFT },
                {36, TileProperty.BOTTOM },
                {37, TileProperty.BROKEN },
                {38, TileProperty.BACKGROUND_BROKEN},
                {39, TileProperty.FLOOR },
                {40, TileProperty.FLOOR_RIGHT },
                {41, TileProperty.FLOOR_LEFT },
                {42, TileProperty.WINDOW_UP_RIGHT },
            };
			foreach (XElement layer in xDoc.Root.Descendants("layer"))
			{
				switch (layer.Attribute("name").Value)
				{
					case "Background":
						getTileSet(layer.Element("data").Value.Split(','), background);
						break;
					case "Decorations":
						getTileSet(layer.Element("data").Value.Split(','),decorations);
						break;
					case "Foreground":
						getTileSet(layer.Element("data").Value.Split(','), foreground);
                        break;
				}
			}
            createNextLevelBoundingBox();
        }

        private void getTileSet(string[] splitArray, Tile[,] tileSet)
        {

            int[,] intIDs = new int[mapWidth, mapHeight];

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    intIDs[x, y] = int.Parse(splitArray[x + y * mapWidth]);
				}
            }
            int key = 0;
            Vector2[] sourcePos = new Vector2[tilecount];
            for (int x = 0; x < tilecount / columns; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    sourcePos[key] = new Vector2(y * tilewidth, x * tileheight);
                    key++;
                }
            }

			Texture2D sourceTex = content.Load<Texture2D>(tileSetFileName);

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
					tileSet[x, y] = new Tile
					(
                    new Vector2(x * tilewidth, y * tileheight),
					sourceTex,
					new Rectangle((int)sourcePos[intIDs[x, y]-1].X, (int)sourcePos[intIDs[x, y]-1].Y, tilewidth, tileheight),
					tilePropertyType[intIDs[x, y]]
					);
				}
            }
        }
        private void createNextLevelBoundingBox()
        {
            int[] positionX = new int[4];
            int[] positionY = new int[4];
            int i = 0;
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if(foreground[x,y].property == TileProperty.STAIRS_PART1 || foreground[x, y].property == TileProperty.STAIRS_PART2 || foreground[x, y].property == TileProperty.STAIRS_PART3 || foreground[x, y].property == TileProperty.STAIRS_PART4)
                    {
                        positionX[i] = foreground[x, y].boundingRectangle.X;
                        positionY[i] = foreground[x, y].boundingRectangle.Y;
                        i++;
                    }
                }
            }
            nextLevelBoundingRectangle =  new Rectangle(positionX.Min(), positionY.Min(), 2 * tilewidth, 2 * tileheight);
        }
        public Tile getTileAt(int x, int y)
        {
            return foreground[x, y];
        }
        public void Draw(SpriteBatch sp)
        {
            foreach (Tile t in background)
            {
                t.Draw(sp);
            }
			foreach (Tile t in decorations)
			{
				t.Draw(sp);
			}
			foreach (Tile t in foreground)
			{
				t.Draw(sp);
			}
		}
    }
}
