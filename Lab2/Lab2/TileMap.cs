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
        Tile[,] tileSet;
        XDocument xDoc;
        public Dictionary<int, TileProperty> tilePropertyType;
        public int mapWidth { get; }
        public int mapHeight { get; }
        int tilecount;
        int columns;
        int tilewidth;
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
            tileSet = new Tile[mapWidth, mapHeight];
            this.tileSetFileName = tileSetFileName;
            this.content = content;
            tilePropertyType = new Dictionary<int, TileProperty>
            {
                {4, TileProperty.EARTH },
                {2, TileProperty.FLOOR },//center
                {37, TileProperty.FLOOR },//left
                {38, TileProperty.FLOOR },//right
                {24, TileProperty.PLATFORM },//left
                {23, TileProperty.PLATFORM },//center
                {25, TileProperty.PLATFORM },//right
                {15, TileProperty.TRAP },
                {1, TileProperty.GROUND }, //left
                {29, TileProperty.WALL_RIGHT }, //right
                {16, TileProperty.DOOR },
                {17, TileProperty.DOOR },
                {20, TileProperty.DOOR },
                {19, TileProperty.DOOR },
                {3, TileProperty.CEILING }
            };
            getTileSet();
        }

        private void getTileSet()
        {
            string IDArray = xDoc.Root.Element("layer").Element("data").Value;
            string[] splitArray = IDArray.Split(',');

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
                        new Rectangle((int)sourcePos[intIDs[x, y] - 1].X, (int)sourcePos[intIDs[x, y] - 1].Y, tilewidth, tileheight),
                        tilePropertyType[intIDs[x, y]]
                        );
                }
            }
        }
        public Tile getTileAt(int x, int y)
        {
            return tileSet[x, y];
        }
        public void Draw(SpriteBatch sp)
        {
            foreach (Tile t in tileSet)
            {
                t.Draw(sp);
            }
        }
    }
}
