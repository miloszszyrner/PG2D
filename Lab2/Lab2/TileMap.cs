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
        Tile[,] background;
		Tile[,] decorations;
		Tile[,] foreground;
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
                {1, TileProperty.TRAP },
                //{2, TileProperty. },
                {3, TileProperty.BACKGROUND },
                {4, TileProperty.COLUMN_DOWN_LEFT },
                {5, TileProperty.COLUMN_DOWN_RIGHT },
                {6, TileProperty.BACKGROUND_BROKEN },
                {7, TileProperty.COLUMN_MIDDLE_DECO_LEFT },
                {8, TileProperty.COLUMN_MIDDLE_DECO_RIGHT }, 
                {9, TileProperty.COLUMN_MIDDLE_LEFT }, 
                {10, TileProperty.COLUMN_MIDDLE_RIGHT },
                {11, TileProperty.COLUMN_TOP_LEFT },
                {12, TileProperty.FLOOR },
                {13, TileProperty.COLUMN_TOP_RIGHT },
                {14, TileProperty.PLATFORM_CENTER },
				{15, TileProperty.PLATFORM_LEFT },
				{16, TileProperty.PLATFORM_RIGHT },
                {17, TileProperty.STAIRS_PART1 },
                {18, TileProperty.FLOOR_LEFT },
                {19, TileProperty.STAIRS_PART2 },
                {20, TileProperty.STAIRS_PART3 },
                {21, TileProperty.STAIRS_PART4 },
                {22, TileProperty.BASE },
				{23, TileProperty.BASE_LEFT }, 
                {24, TileProperty.FLOOR_RIGHT }, 
                {25, TileProperty.BASE_RIGHT },
				{26, TileProperty.BOTTOM },
				{27, TileProperty.BOTTOM },
				{28, TileProperty.BOTTOM },
				{29, TileProperty.BROKEN },
				{30, TileProperty.WINDOW_DOWN_LEFT }, 
                {31, TileProperty.WINDOW_DOWN_RIGHT }, 
                {32, TileProperty.WINDOW_MIDDLE_LEFT },
				{33, TileProperty.WINDOW_MIDDLE_RIGHT },
				{34, TileProperty.WINDOW_UP_LEFT },
				{35, TileProperty.WINDOW_UP_RIGHT },
				{36, TileProperty.EARTH }
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
