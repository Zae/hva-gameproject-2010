using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GO
{
    class Grid
    {

        private Tile[,] map;
        private int width;
        private int height;
        private int tileCount;

        public int mouseWorldX = 0;
        public int mouseWorldY = 0;

        private Tile selectedTile = null;

        private Tile guessTile = null;
      

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

                            map[colom,row] = createTile(rawLevel[i],colom,row);
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

        public void mousePressed(int x, int y, int translationX, int translationY)
        {

            //translate the screen input to world coordinates

            //...

            
            mouseWorldX = x - translationX - GO.halfWidth;
            mouseWorldY = y - translationY;

            int tilesVertical = mouseWorldY / (Tile.baseHalfHeight);
            int tilesHorizontal = mouseWorldX / (Tile.baseWidth);
            //Debug.WriteLine("tileVertical:" + tilesVertical + " tilesHorizontal:"+tilesHorizontal);

            Tile tile = getTile(tilesVertical,tilesHorizontal);
           
            if (tile != null)
            {
                if (selectedTile != null)
                {
                    selectedTile.setSelected(false);
                }

                guessTile = tile;

                Tile realTile = getRealTile(x, y, translationX, translationY, tile);

                if (realTile != null)
                {
                    realTile.setSelected(true);
                    selectedTile = tile;
                }
               

                //Debug.WriteLine("gotTile:" + tile.ToString());



            }
            else
            {
                if (selectedTile != null)
                {
                    selectedTile.setSelected(false);
                    selectedTile = null;
                }
            }
            //update selected tile (or null when none is selected)

        }

        private Tile getRealTile(int x, int y, int translationX, int translationY, Tile guess) 
        {
            string sColorval = "";
            uint[] myUint = new uint[1];

            int tileX = GO.halfWidth + (guess.getVisualX() * Tile.baseWidth) + translationX - (Tile.baseWidth);
            int tileY = (guessTile.getVisualY() * Tile.baseHalfHeight) + translationY;

            int virtualX = x - tileX;
            int virtualY = y - tileY;

            if (virtualX >= 0 && virtualX < Images.tileHitmapImage.Width && virtualY >= 0 && virtualY < Images.tileHitmapImage.Height)
            {
                Images.tileHitmapImage.GetData<uint>(0, new Rectangle(virtualX, virtualY, 1, 1), myUint, 0, 1);
                sColorval = myUint[0].ToString();
            }

            Debug.WriteLine("Color under mouse is: "+sColorval);

            //if (guessTile != null)
            //{
            //    GO.spriteBatch.Begin();
            //    GO.spriteBatch.Draw(Images.tileHitmapImage, new Rectangle(GO.halfWidth + (guessTile.getVisualX() * Tile.baseWidth) + translationX - (Tile.baseWidth), (guessTile.getVisualY() * Tile.baseHalfHeight) + translationY, Tile.baseWidth * 2, Tile.baseHalfHeight * 2), Color.White);
            //    GO.spriteBatch.End();
            //}

        

            return null;
        }

        private Tile getTile(int tilesVertical, int tilesHorizontal)
        {
            //if (viewDirection == SOUTH_WEST)
            //{
            //    int x = width - 1;
            //    int y = 0;

            //    //for (int i = 0; i < tilesVertical;i++ )
            //    //{
            //    //    x--;
            //    //    y++;
            //    //}
            //    y += tilesVertical;
            //    x += tilesHorizontal + (-tilesVertical);

            //    if (x >= 0 && x <= width - 1 && y >= 0 && y <= height - 1)
            //    {
            //        return map[x, y];
            //    }
               
            //}
            for (int i = 0; i < perspectiveMap.Length; i++)
            {
                if (perspectiveMap[i].getVisualX() == tilesHorizontal && perspectiveMap[i].getVisualY() == tilesVertical)
                {
                    return perspectiveMap[i];
                }
            }

            return null;
           
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
                        map[x, y].setVisualX(relativeX);
                        map[x, y].setVisualY(relativeY);
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

        public void draw(int translationX, int translationY)
        {
            for (int i = 0; i < tileCount; i++)
            {
                perspectiveMap[i].draw(translationX, translationY);
                //perspectiveMap[i].drawDebug(translationX, translationY);
            }
            for (int i = 0; i < tileCount; i++)
            {
                //perspectiveMap[i].draw(translationX, translationY);
                perspectiveMap[i].drawDebug(translationX, translationY);
            }


            if (guessTile != null)
            {
                GO.spriteBatch.Begin();
                GO.spriteBatch.Draw(Images.tileHitmapImage, new Rectangle(GO.halfWidth + (guessTile.getVisualX() * Tile.baseWidth) + translationX - (Tile.baseWidth), (guessTile.getVisualY() * Tile.baseHalfHeight) + translationY, Tile.baseWidth * 2, Tile.baseHalfHeight * 2), Color.White);
                GO.spriteBatch.End();
            }


            GO.spriteBatch.Begin();
            Vector2 location = new Vector2(mouseWorldX + translationX + GO.halfWidth, mouseWorldY + translationY);
            GO.spriteBatch.DrawString(Fonts.font, "MOSUE POS", location, Color.Blue);
            GO.spriteBatch.End();
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

        private Tile createTile(char c, int x, int y)
        {
            if (c == 'N')
            {
                return new ResourceTile(x,y);
            }
            else if (c == 'M')
            {
                return new MountainTile(x,y);
            }

            return null;
        }

    }
}
