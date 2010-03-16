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

        private int step = 0;

        private int viewDirection = 1;
        private const int SOUTH_WEST = 1;
        private const int NORTH_WEST = 2;
        private const int NORTH_EAST = 3;
        private const int SOUTH_EAST = 4;

        private bool drawHitTest = false;
        private float virtualX = 0;
        private float virtualY = 0;


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

                   // Debug.WriteLine("The raw level data reads: " + rawLevel);

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
                Console.WriteLine("exception in grid: "+ e.ToString());
            }
        }

        public void mouseRightPressed(int x, int y, int translationX, int translationY)
        {
            drawHitTest = true;


            //translate the screen input to world coordinates
            mouseWorldX = x - translationX - GO.halfWidth;
            mouseWorldY = y - translationY;

            //get the true value from the origin in tile units
            float tilesVerticalQ = (float)(((float)mouseWorldY / (float)Tile.baseHalfHeight)) - 1;
            float tilesHorizontalQ = (float)((float)mouseWorldX / (float)Tile.baseHalfWidth);

            //get the closest even value to that position
            int tilesVertical = Tool.closestEvenInt(tilesVerticalQ);
            int tilesHorizontal = Tool.closestEvenInt(tilesHorizontalQ);
            //int tilesVertical = (int)tilesVerticalQ;
            //int tilesHorizontal = (int)tilesHorizontalQ;
            Debug.WriteLine("********");
            Debug.WriteLine("tileHQ:" + tilesHorizontalQ + " tilesVQ:" + tilesVerticalQ);
            Debug.WriteLine("tileH:" + tilesHorizontal + " tilesV:" + tilesVertical);
            //Debug.WriteLine("INTtileHQ:" + (int)tilesHorizontalQ + "INTtilesVQ:" + (int)tilesVerticalQ);

            //get the color at that position on the hitmap
            uint color = doHitmapTest(x, y, translationX, translationY, tilesHorizontal, tilesVertical);

            //pass the position and the color and see if you get back anything




            Tile tile = getTile(tilesVertical, tilesHorizontal, color);

            if (tile != null && tile is ResourceTile)
            {
                ((ResourceTile)tile).addCharge(0.1f, Players.PLAYER2);
                
                //if (selectedTile != null)
                //{
                //    selectedTile.setSelected(false);
                //}

                //tile.setSelected(true);
                //selectedTile = tile;



                ////Debug.WriteLine("gotTile:" + tile.ToString());



            }
            else
            {
                //if (selectedTile != null)
                //{
                //    selectedTile.setSelected(false);
                //    selectedTile = null;
                //}
            }
            //update selected tile (or null when none is selected)

        }

        public void mouseRightReleased(int x, int y, int translationX, int translationY)
        {
            drawHitTest = false;

        }

        public void mouseLeftReleased(int x, int y, int translationX, int translationY)
        {
            drawHitTest = false;
             
        }

        public void mouseLeftPressed(int x, int y, int translationX, int translationY)
        {
            drawHitTest = true;
            
            
            //translate the screen input to world coordinates
            mouseWorldX = x - translationX - GO.halfWidth;
            mouseWorldY = y - translationY;

            //get the true value from the origin in tile units
            float tilesVerticalQ = (float)(((float)mouseWorldY / (float)Tile.baseHalfHeight))-1;
            float tilesHorizontalQ = (float)((float)mouseWorldX / (float)Tile.baseHalfWidth);

            //get the closest even value to that position
            int tilesVertical = Tool.closestEvenInt(tilesVerticalQ);
            int tilesHorizontal = Tool.closestEvenInt(tilesHorizontalQ);
            //int tilesVertical = (int)tilesVerticalQ;
            //int tilesHorizontal = (int)tilesHorizontalQ;
            //Debug.WriteLine("********");
            //Debug.WriteLine("tileHQ:" + tilesHorizontalQ + " tilesVQ:" + tilesVerticalQ);
            //Debug.WriteLine("tileH:" + tilesHorizontal + " tilesV:"+tilesVertical);
            //Debug.WriteLine("INTtileHQ:" + (int)tilesHorizontalQ + "INTtilesVQ:" + (int)tilesVerticalQ);

            //get the color at that position on the hitmap
            uint color = doHitmapTest(x, y, translationX, translationY, tilesHorizontal, tilesVertical);

            //pass the position and the color and see if you get back anything




            Tile tile = getTile(tilesVertical,tilesHorizontal, color);

            if (tile != null)
            {
                if (selectedTile != null)
                {
                    selectedTile.setSelected(false);
                }

                    tile.setSelected(true);
                    selectedTile = tile;
                


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

        private uint doHitmapTest(int x, int y, int translationX, int translationY, int visualX, int visualY) 
        {

            string sColorval = "NONE";
            uint[] myUint = new uint[1];

            int tileX = GO.halfWidth + (visualX * Tile.baseHalfWidth) + translationX - (Tile.baseHalfWidth);
            int tileY = (visualY * Tile.baseHalfHeight) + translationY;

            virtualX = x - tileX;
            virtualY = y - tileY;

            virtualX = Images.tileHitmapImage.Width * (virtualX / (Tile.baseHalfWidth * 2));
            virtualY = Images.tileHitmapImage.Height * (virtualY / (Tile.baseHalfHeight * 2));

            if (virtualX >= 0 && virtualX < Images.tileHitmapImage.Width && virtualY >= 0 && virtualY < Images.tileHitmapImage.Height)
            {
                Images.tileHitmapImage.GetData<uint>(0, new Rectangle((int)virtualX, (int)virtualY, 1, 1), myUint, 0, 1);
                sColorval = myUint[0].ToString();
            }

            //Debug.WriteLine(frame++ + " Color under mouse is: " + Colors.getColor(myUint[0]) + " uint:" + sColorval);

            return myUint[0];
        }

        private Tile getTile(int tilesVertical, int tilesHorizontal, uint color)
        {

            int lookForX = tilesHorizontal;
            int lookForY = tilesVertical;

            if (viewDirection == SOUTH_WEST)
            {
                if(color == Colors.color_red)
                {
                    lookForX--;
                    lookForY--;
                }
                else if (color == Colors.color_green)
                {
                    lookForX--;
                    lookForY++;
                }
                else if (color == Colors.color_cyan)
                {
                    lookForX++;
                    lookForY++;
                }
                else if (color == Colors.color_blue)
                {
                    lookForX++;
                    lookForY--;
                }
            }

            for (int i = 0; i < perspectiveMap.Length; i++)
            {
                if (perspectiveMap[i].getVisualX() == lookForX && perspectiveMap[i].getVisualY() == lookForY)
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
                    //Debug.WriteLine("i = " + i);
                    //Debug.WriteLine("tileCount = " + tileCount);
                    //Debug.WriteLine("x = " + x);
                    //Debug.WriteLine("y = " + y);
                    
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
            //for (int i = 0; i < tileCount; i++)
            //{
            //    //perspectiveMap[i].draw(translationX, translationY);
            //    perspectiveMap[i].drawDebug(translationX, translationY);
            //}


            if (guessTile != null)
            {
                GO.spriteBatch.Begin();
                GO.spriteBatch.Draw(Images.tileHitmapImage, new Rectangle(GO.halfWidth + (guessTile.getVisualX() * Tile.baseHalfWidth) + translationX - (Tile.baseHalfWidth), (guessTile.getVisualY() * Tile.baseHalfHeight) + translationY, Tile.baseHalfWidth * 2, Tile.baseHalfHeight * 2), Color.White);
                GO.spriteBatch.End();
            }


            //GO.spriteBatch.Begin();
            //Vector2 location = new Vector2(mouseWorldX + translationX + GO.halfWidth, mouseWorldY + translationY);
            //GO.spriteBatch.DrawString(Fonts.font, "MOSUE POS", location, Color.Blue);
            //GO.spriteBatch.End();
            //Old way
            //for (int x=0; x < width; x++)
            //{
            //    for(int y=0; y < height; y++) 
            //    {
            //        map[x, y].draw((x * tileSize) + 50, (y * tileSize) + 50, spriteBatch);
            //    }
            //}

            if (drawHitTest)
            {
                GO.spriteBatch.Begin();
                GO.spriteBatch.Draw(Images.tileHitmapImage, new Rectangle(0, 0, Images.tileHitmapImage.Width, Images.tileHitmapImage.Height), Color.White);
                GO.spriteBatch.Draw(Images.white1px, new Rectangle((int)virtualX,(int)virtualY,5,5),Color.Black);
                GO.spriteBatch.End();
            }
        }

        public void update(int ellapsed)
        {
            //do unit stuff

            if (step == 0)
            {
                tileVersusTile();


                step++;
            }
            else if (step == 15)
            {
                tileAidTile();


                step = 0;
            }
            else
            {
                step++;
            }

            //now tell all Tiles to update, we use the perspective map for that
            //because it might be faster?
            for (int i = 0; i < tileCount; i++)
            {
                perspectiveMap[i].update();
            }
        }

        private void tileVersusTile()
        {
            Tile todo;

            //This method looks at the grid from a overhead perspective where x increased in the
            //right direction and y increases in the downward direction.

            for (int i = 0; i < width; i++)
            {
                for (int ii = 0; ii < height; ii++)
                {
                    todo = map[i, ii];

                    //The tile to the right of this tile
                    if(isValid(i+1,ii)) 
                    {
                        todo.tileVersusTile(map[i + 1, ii]);
                    }

                    //The tile to the bottom-right of this tile
                    if(isValid(i+1,ii+1))
                    {
                        todo.tileVersusTile(map[i + 1, ii+1]);
                    }

                    //The tile to the bottom of this tile
                    if (isValid(i, ii+1))
                    {
                        todo.tileVersusTile(map[i, ii+1]);
                    }

                    //The tile to the bottom left of this tile
                    if (isValid(i - 1, ii+1))
                    {
                        todo.tileVersusTile(map[i - 1, ii+1]);
                    }

                }
            }
        }

        private bool isValid(int x, int y)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                return true;
            }
            return false;
        }

        private void tileAidTile()
        {
            Tile todo;

            //This method looks at the grid from a overhead perspective where x increased in the
            //right direction and y increases in the downward direction.

            for (int i = 0; i < width; i++)
            {
                for (int ii = 0; ii < height; ii++)
                {
                    todo = map[i, ii];

                    //The tile to the right of this tile
                    if (isValid(i + 1, ii))
                    {
                        todo.tileAidTile(map[i + 1, ii]);
                    }

                    //The tile to the bottom-right of this tile
                    if (isValid(i + 1, ii + 1))
                    {
                        todo.tileAidTile(map[i + 1, ii + 1]);
                    }

                    //The tile to the bottom of this tile
                    if (isValid(i, ii + 1))
                    {
                        todo.tileAidTile(map[i, ii + 1]);
                    }

                    //The tile to the bottom left of this tile
                    if (isValid(i - 1, ii + 1))
                    {
                        todo.tileAidTile(map[i - 1, ii + 1]);
                    }

                }
            }

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
