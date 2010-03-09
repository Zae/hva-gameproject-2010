using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;

namespace G.O
{
    class Grid
    {

        private Tile[,] map;
        private int width;
        private int height;
        private int tileCount;
      

        private Tile[] perspectiveMap;


        private int viewDirection = 1;
        private const int SOUTH_WEST = 1;
        private const int NORTH_WEST = 2;
        private const int NORTH_EAST = 3;
        private const int SOUTH_EAST = 4;


        public Grid(String levelname)
        {
            
            
            //load the xml file into the XmlTextReader object. 
            try
            {
                string execPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
                //Debug.WriteLine("Execution path is: "+execPath);

                //Read the level file using the execution path we got earlier
                XmlTextReader XmlRdr = new System.Xml.XmlTextReader(execPath+"\\Content\\"+levelname);
               
                //Read the first Node
                XmlRdr.Read();

                //If it is a Level Node we can continue
                if (XmlRdr.NodeType == XmlNodeType.Element && XmlRdr.Name == "Level")
                {
                    width = int.Parse(XmlRdr.GetAttribute(0));
                    height = int.Parse(XmlRdr.GetAttribute(1));
                    tileCount = width * height;
                    map = new Tile[width,height];
                    perspectiveMap = new Tile[tileCount];

                    String rawLevel = XmlRdr.ReadElementContentAsString();

                    Debug.WriteLine("The raw level data reads: " + rawLevel);

                    int length = rawLevel.Length;

                    //for (int i = 0; i < length; i++)
                    //{
                    //    Debug.WriteLine("char " + i + " reads: " + rawLevel[i]);
                    //}

                    int row = -1;
                    int colom = 0;
                    bool newRowStarted = false;
                    for (int i = 0; i < length; i++)
                    {
                        if (rawLevel[i] == 'N' || rawLevel[i] == 'M')
                        {
                            if (!newRowStarted)
                            {
                                newRowStarted = true;
                                colom = 0;
                                row++;
                            }

                            map[colom,row] = createTile(rawLevel[i]);
                            colom++;
                        }
                        else
                        {
                            if (newRowStarted)
                            {
                                newRowStarted = false;
                            }
                        }
                    }
                }
                else
                {
                    Debug.WriteLine("FATAL ERROR: Level file did not contain a valid Level Node");
                    GO.get().Exit();
                }

                settleIndexZ(SOUTH_WEST);

            }
            catch(Exception e) {
            }
        }

        private void settleIndexZ(int newViewDirection)
        {
            //TODO
            if (newViewDirection == SOUTH_WEST)
            {
                int i = 0;

                int xMax = width - 1;
                int yMax = height - 1;

                int xStart = xMax;
                int yStart = 0;

                int x = xStart;
                int y = yStart;

                int initialRelativeX = 0;
                int relativeX = 0;
                int relativeY = 0;

                while (i < tileCount)
                {
                    Debug.WriteLine("i = " + i);
                    Debug.WriteLine("tileCount = " + tileCount);
                    Debug.WriteLine("x = " + x);
                    Debug.WriteLine("y = " + y);
                    
                    if (x <= xMax && y <= yMax && x >= 0 && y >= 0)
                    {
                        map[x, y].setIndexZ(i);
                        map[x, y].setIndexX(relativeX);
                        map[x, y].setIndexY(relativeY);
                        perspectiveMap[i] = map[x, y];
                        x++;
                        y++;
                        i++;

                        relativeX += 2;
                    }
                    else
                    {
                        if (xStart > 0)
                        {
                            xStart--;
                            x = xStart;
                            y = yStart;

                            initialRelativeX -= 1;
                            relativeX = initialRelativeX;

                            relativeY += 1;
                        }
                        else
                        {
                            yStart++;
                            x = xStart;
                            y = yStart;

                            initialRelativeX += 1;
                            relativeX = initialRelativeX;

                            relativeY += 1;
                        }

                        
                    }
                }

            }

            this.viewDirection = newViewDirection;
        }

        public void draw(int translation, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < tileCount; i++)
            {
                perspectiveMap[i].draw(translation,spriteBatch);
            }


            //Old way
            //for (int x=0; x < width; x++)
            //{
            //    for(int y=0; y < height; y++) 
            //    {
            //        map[x, y].draw((x * tileSize) + 50, (y * tileSize) + 50, spriteBatch);
            //    }
            //}

        }

        public void update()
        {
            //TODO
            //Which kind of operation do we have to do on each tile and in which order?
        }

        private Tile createTile(char c)
        {
            if (c == 'N')
            {
                return new ResourceTile();
            }
            else if (c == 'M')
            {
                return new MountainTile();
            }

            return null;
        }

    }
}
