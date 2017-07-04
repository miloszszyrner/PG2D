using Microsoft.Xna.Framework;
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
                {1, TileProperty.BACKGROUND  }, //tlo				
                {2, TileProperty.EARTH },
                {3, TileProperty.COLUMN_DOWN_LEFT },
                {4, TileProperty.COLUMN_DOWN_RIGHT },
                {5, TileProperty.COLUMN_MIDDLE_DECO_LEFT },
                {6, TileProperty.COLUMN_MIDDLE_DECO_RIGHT },
                {7, TileProperty.WINDOW_MIDDLE_RIGHT },
                {8, TileProperty.COLUMN_MIDDLE_LEFT },
                {9, TileProperty.COLUMN_MIDDLE_RIGHT },
                {10, TileProperty.COLUMN_TOP_LEFT },
                {11, TileProperty.COLUMN_TOP_RIGHT },
                {12, TileProperty.PLATFORM_CENTER },
                {13, TileProperty.PLATFORM_CENTER }, //L	
				{14, TileProperty.WINDOW_UP_LEFT },
                {15, TileProperty.PLATFORM_CENTER }, //R
				{16, TileProperty.EARTH },
                {17, TileProperty.TRAP },
                {18, TileProperty.TRAP },
                {19, TileProperty.TRAP },
                {20, TileProperty.TRAP },
                {21, TileProperty.WINDOW_UP_RIGHT },
                {22, TileProperty.STAIRS_PART1 },
                {23, TileProperty.STAIRS_PART2 },
                {24, TileProperty.STAIRS_PART3 },
                {25, TileProperty.STAIRS_PART4 },
                {26, TileProperty.BASE },
                {27, TileProperty.BASE_LEFT },
                {28, TileProperty.EARTH },
                {29, TileProperty.BASE_RIGHT },
                {30, TileProperty.BOTTOM },
                {31, TileProperty.BOTTOM },
                {32, TileProperty.BOTTOM },
                {33, TileProperty.BROKEN },
                {34, TileProperty.BACKGROUND_BROKEN},
                {35, TileProperty.EARTH }, //nieużywane, placeholder
                {36, TileProperty.FLOOR },
                {37, TileProperty.FLOOR }, //L
                {38, TileProperty.FLOOR }, //R
                {39, TileProperty.WINDOW_DOWN_LEFT },
                {40, TileProperty.WINDOW_DOWN_RIGHT },
                {41, TileProperty.WINDOW_MIDDLE_LEFT },
                {42, TileProperty.EARTH } //nieużywane, placeholder

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
