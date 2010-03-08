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
        private int tileSize = 140;

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
                    map = new Tile[width,height];

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

            }
            catch(Exception e) {
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            //TODO
            //In which order do we paint the tiles?
            for (int x=0; x < width; x++)
            {
                for(int y=0; y < height; y++) 
                {
                    map[x, y].draw((x * tileSize) + 50, (y * tileSize) + 50, spriteBatch);
                }
            }

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
