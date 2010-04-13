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
using ION.GridStrategies;
using FluorineFx;
using FluorineFx.Net;

namespace ION
{
    class Grid
    {

        public static Tile[,] map;
        public static int width;
        public static int height;
        public static int tileCount;

        public float mouseWorldX = 0;
        public float mouseWorldY = 0;

        private Tile selectedTile = null;

        public static Tile[] perspectiveMap;



        private GridStrategy updateStrategy;

        private int viewDirection = 1;
        private const int SOUTH_WEST = 1;
        private const int NORTH_WEST = 2;
        private const int NORTH_EAST = 3;
        private const int SOUTH_EAST = 4;

        private bool drawHitTest = false;
        private float virtualX = 0;
        private float virtualY = 0;

        public RemoteSharedObject GridRSO;
        public RemoteSharedObject LocalUnits;
        public RemoteSharedObject RemoteUnits;



        public void mouseRightPressed(float x, float y, float translationX, float translationY, List<Unit> blueArmy)
        {
            mouseRightPressed(x, y, translationX, translationY);
            if (selectedTile != null)
            {
                for (int i = 0; i < blueArmy.Count(); i++)
                {
                    if (blueArmy[i] != null && blueArmy[i].selected)
                    {
                        Byte[] buffer = blueArmy[i].SetTarget(selectedTile.GetPos(translationX, translationY));
                        this.LocalUnits.SetAttribute("SetTarget", buffer);
                    }
                }
            }

        }



        public void mouseRightPressed(float x, float y, float translationX, float translationY)
        {
            //drawHitTest = true;

            //translate the screen input to world coordinates
            mouseWorldX = x - translationX - ION.halfWidth;
            mouseWorldY = y - translationY;

            //get the true value from the origin in tile units
            float tilesVerticalQ = (float)(((float)mouseWorldY / (float)Tile.baseHalfHeight)) - 1;
            float tilesHorizontalQ = (float)((float)mouseWorldX / (float)Tile.baseHalfWidth);

            //get the closest even value to that position
            int tilesVertical = Tool.closestEvenInt(tilesVerticalQ);
            int tilesHorizontal = Tool.closestEvenInt(tilesHorizontalQ);

            //get the color at that position on the hitmap
            uint color = doHitmapTest(x, y, translationX, translationY, tilesHorizontal, tilesVertical);

            //pass the position and the color and see if you get back anything
            Tile tile = getTile(tilesVertical, tilesHorizontal, color);

            if (tile != null && tile is ResourceTile)
            {
                ResourceTile rt = (ResourceTile)tile;

                if (rt.owner != Players.PLAYER2)
                {
                    rt.sustain(0.06f, Players.PLAYER2);
                }
                else
                {
                    rt.receive(0.06f);
                }

                if (selectedTile != null)
                {
                    selectedTile.setSelected(false);
                }

                tile.setSelected(true);
                selectedTile = tile;
            }
            else
            {
                if (selectedTile != null)
                {
                    selectedTile.setSelected(false);
                    selectedTile = null;
                }
            }

        }

        public GridStrategy getUpdateStrategy()
        {
            return updateStrategy;
        }

        public void mouseRightReleased(float x, float y, float translationX, float translationY)
        {
            drawHitTest = false;
        }

        public void mouseLeftReleased(float x, float y, float translationX, float translationY)
        {
            drawHitTest = false;
        }

        public void mouseLeftPressed(float x, float y, float translationX, float translationY)
        {
            //drawHitTest = true;

            //translate the screen input to world coordinates
            mouseWorldX = x - translationX - ION.halfWidth;
            mouseWorldY = y - translationY;

            //get the true value from the origin in tile units
            float tilesVerticalQ = (float)(((float)mouseWorldY / (float)Tile.baseHalfHeight)) - 1;
            float tilesHorizontalQ = (float)((float)mouseWorldX / (float)Tile.baseHalfWidth);

            //get the closest even value to that position
            int tilesVertical = Tool.closestEvenInt(tilesVerticalQ);
            int tilesHorizontal = Tool.closestEvenInt(tilesHorizontalQ);

            //get the color at that position on the hitmap
            uint color = doHitmapTest(x, y, translationX, translationY, tilesHorizontal, tilesVertical);

            //pass the position and the color and see if you get back anything
            Tile tile = getTile(tilesVertical, tilesHorizontal, color);

            if (tile != null && tile is ResourceTile)
            {
                ResourceTile rt = (ResourceTile)tile;

                if (rt.owner != Players.PLAYER1)
                {
                    rt.sustain(0.06f, Players.PLAYER1);
                }
                else
                {
                    rt.receive(0.06f);
                }

                if (selectedTile != null)
                {
                    selectedTile.setSelected(false);
                }

                tile.setSelected(true);
                selectedTile = tile;
            }
            else
            {
                if (selectedTile != null)
                {
                    selectedTile.setSelected(false);
                    selectedTile = null;
                }
            }
        }


        public void mouseLeftPressed(float x, float y, float translationX, float translationY, List<Unit> blueArmy)
        {
            mouseLeftPressed(x, y, translationX, translationY);
            if (selectedTile != null)
            {
                for (int i = 0; i < blueArmy.Count(); i++)
                {
                    //unselect this unit by default
                    blueArmy[i].selected = false;
                    //check if the current tile matches the units tile, if so changed the units selected to true
                    if (GetTile(x, y, translationX, translationY) == blueArmy[i].GetTile())
                    {
                        blueArmy[i].selected = true;//select unit
                    }


                }
            }

        }

        public void createUnit(float x, float y, float translationX, float translationY, int owner)
        {
            //translate the screen input to world coordinates
            mouseWorldX = x - translationX - ION.halfWidth;
            mouseWorldY = y - translationY;

            //get the true value from the origin in tile units
            float tilesVerticalQ = (float)(((float)mouseWorldY / (float)Tile.baseHalfHeight)) - 1;
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




            Tile tile = getTile(tilesVertical, tilesHorizontal, color);

            if (tile != null)
            {
                if (tile is ResourceTile)
                {
                    ResourceTile resourceTile = (ResourceTile)tile;
                    if (!resourceTile.hasUnit())
                    {
                        //BallUnit b = new BallUnit(owner);
                       // resourceTile.setUnit(b);
                    }
                }
            }
            else
            {
                
            }
           
        }


        private uint doHitmapTest(float x, float y, float translationX, float translationY, int visualX, int visualY) 
        {
            string sColorval = "NONE";
            uint[] myUint = new uint[1];

            int tileX = (int)(ION.halfWidth + (visualX * Tile.baseHalfWidth) + translationX - (Tile.baseHalfWidth));
            int tileY = (int)((visualY * Tile.baseHalfHeight) + translationY);

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

        public void draw(float translationX, float translationY)
        {
            for (int i = 0; i < tileCount; i++)
            {
                perspectiveMap[i].draw(translationX, translationY);
                //perspectiveMap[i].drawDebug(translationX, translationY);
            }

            if (drawHitTest)
            {
                ION.spriteBatch.Begin();
                ION.spriteBatch.Draw(Images.tileHitmapImage, new Rectangle(0, 0, Images.tileHitmapImage.Width, Images.tileHitmapImage.Height), Color.White);
                ION.spriteBatch.Draw(Images.white1px, new Rectangle((int)virtualX, (int)virtualY, 5, 5), Color.Black);
                ION.spriteBatch.End();
            }

            updateStrategy.draw();
        }
        public void update(int ellapsed, List<Unit> blueArmy, float translationX, float translationY)
        {


            updateStrategy.update(ellapsed);
            for (int i = 0; i < blueArmy.Count(); i++)
            {
                if (blueArmy[i] != null)
                {
                    mouseLeftPressed(blueArmy[i].GetVirtualPos().X, blueArmy[i].GetVirtualPos().Y, translationX, translationY);
                }
            }

            /** Disabled for performance, works perfectly tho! **/
            if (GridRSO != null && GridRSO.Connected)
            {
                //byte[] rs = Serializer.Serialize(map, Grid.width, Grid.height);
                //GridRSO.SetAttribute("Grid", rs);
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


        public Grid(String levelname, GridStrategy strategy)
        {
            updateStrategy = strategy;
            updateStrategy.reset();
            //updateStrategy = new FlowStrategy();

            if (ION.get().serverConnection != null && ION.get().serverConnection.GameConnection != null && ION.get().serverConnection.GameConnection.Connected)
            {
                GridRSO = RemoteSharedObject.GetRemote("Grid", ION.get().serverConnection.GameConnection.Uri.ToString(), false);
                GridRSO.NetStatus += new NetStatusHandler(GridRSO_NetStatus);
                GridRSO.OnConnect += new ConnectHandler(GridRSO_OnConnect);
                GridRSO.OnDisconnect += new DisconnectHandler(GridRSO_OnDisconnect);
                GridRSO.Sync += new SyncHandler(GridRSO_Sync);
                GridRSO.Connect(ION.get().serverConnection.GameConnection);
            }

            if (ION.get().serverConnection != null && ION.get().serverConnection.GameConnection != null && ION.get().serverConnection.GameConnection.Connected)
            {
                if (ION.get().serverConnection.isHost)
                {
                    LocalUnits = RemoteSharedObject.GetRemote("ServerUnits", ION.get().serverConnection.GameConnection.Uri.ToString(), false);
                    RemoteUnits = RemoteSharedObject.GetRemote("ClientUnits", ION.get().serverConnection.GameConnection.Uri.ToString(), false);
                }
                else
                {
                    LocalUnits = RemoteSharedObject.GetRemote("ClientUnits", ION.get().serverConnection.GameConnection.Uri.ToString(), false);
                    RemoteUnits = RemoteSharedObject.GetRemote("ServerUnits", ION.get().serverConnection.GameConnection.Uri.ToString(), false);
                }
                LocalUnits.NetStatus += new NetStatusHandler(LocalUnits_NetStatus);
                LocalUnits.OnConnect += new ConnectHandler(LocalUnits_OnConnect);
                LocalUnits.OnDisconnect += new DisconnectHandler(LocalUnits_OnDisconnect);
                LocalUnits.Sync += new SyncHandler(LocalUnits_Sync);
                LocalUnits.Connect(ION.get().serverConnection.GameConnection);
            }

            //load the xml file into the XmlTextReader object. 
            try
            {
                string execPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
                //Debug.WriteLine("Execution path is: "+execPath);

                //Read the level file using the execution path we got earlier
                XmlTextReader XmlRdr = new System.Xml.XmlTextReader(execPath + "\\Content\\levelItems\\" + levelname);

                //Read the first Node
                XmlRdr.Read();

                //If it is a Level Node we can continue
                if (XmlRdr.NodeType == XmlNodeType.Element && XmlRdr.Name == "Level")
                {
                    width = int.Parse(XmlRdr.GetAttribute(0));
                    height = int.Parse(XmlRdr.GetAttribute(1));
                    tileCount = width * height;
                    ////Now read the player count and positions of these players
                    int playerCount = int.Parse(XmlRdr.GetAttribute(2));

                    int[] positionsX = new int[playerCount];
                    int[] positionsY = new int[playerCount];
                    for (int i = 0; i < playerCount; i++)
                    {
                        positionsX[i] = int.Parse(XmlRdr.GetAttribute(3 + (i * 2)));
                        positionsY[i] = int.Parse(XmlRdr.GetAttribute(3 + (i * 2) + 1));

                        //map[xPos, yPos] = new BaseTile(xPos, yPos, i + 1);
                    }


                    map = new Tile[width, height];
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

                            map[colom, row] = createTile(rawLevel[i], colom, row);
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
                    
                    //finally take the player position and put them into the map
                    for (int i = 0; i < playerCount; i++)
                    {
                        map[positionsX[i], positionsY[i]] = new BaseTile(positionsX[i], positionsY[i], i + 1);
                    }

                 
               

                }
                else
                {
                    Debug.WriteLine("FATAL ERROR: Level file did not contain a valid Level Node");
                    ION.get().Exit();
                }

                settleIndexZ(SOUTH_WEST);

            }
            catch (Exception e)
            {
                Console.WriteLine("exception in grid: " + e.ToString());
            }
        }

        void LocalUnits_Sync(object sender, SyncEventArgs e)
        {
            throw new NotImplementedException(); 
        }

        void LocalUnits_OnDisconnect(object sender, EventArgs e)
{
 	throw new NotImplementedException();
}

        void LocalUnits_OnConnect(object sender, EventArgs e)
{
 	throw new NotImplementedException();
}

        void LocalUnits_NetStatus(object sender, NetStatusEventArgs e)
{
 	throw new NotImplementedException();
}
        public void mouseLeftPressed(float x, float y, float translationX, float translationY, Unit soldier)
        {
            mouseLeftPressed(x, y, translationX, translationY);
            if (selectedTile != null)
            {
                if (soldier != null)
                {
                    soldier.SetTarget(selectedTile.GetPos(translationX, translationY));
                }
            }
        }


        void GridRSO_Sync(object sender, SyncEventArgs e)
        {
            byte[] grid = (byte[])GridRSO.GetAttribute("Grid");
            if (grid != null)
            {
                float[,] f = Serializer.DeserializeFloat(grid);
            }
        }

        void GridRSO_OnDisconnect(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        void GridRSO_OnConnect(object sender, EventArgs e)
        {
            
            //throw new NotImplementedException();
        }

        void GridRSO_NetStatus(object sender, NetStatusEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public Tile[,] getMap()
        {
            return map;
        }

        public Tile[] getPerspectiveMap()
        {
            return perspectiveMap;
        }

        //public Vector2 GetBlueBlueBase()
        //{
        //    Vector2 basePosition = new Vector2();
        //    return basePosition;
        //}

        // new
        public Vector2 GetTile(float x, float y, float translationX, float translationY)
        {
            //translate the screen input to world coordinates
            mouseWorldX = x - translationX - ION.halfWidth;
            mouseWorldY = y - translationY;

            //get the true value from the origin in tile units
            float tilesVerticalQ = (float)(((float)mouseWorldY / (float)Tile.baseHalfHeight)) - 1;
            float tilesHorizontalQ = (float)((float)mouseWorldX / (float)Tile.baseHalfWidth);

            //get the closest even value to that position
            int tilesVertical = Tool.closestEvenInt(tilesVerticalQ);
            int tilesHorizontal = Tool.closestEvenInt(tilesHorizontalQ);

            //get the color at that position on the hitmap
            uint color = doHitmapTest(x, y, translationX, translationY, tilesHorizontal, tilesVertical);

            //pass the position and the color and see if you get back anything
            Tile tile = getTile(tilesVertical, tilesHorizontal, color);

            if (tile != null && tile is ResourceTile)
            {
                ResourceTile rt = (ResourceTile)tile;

                //if (rt.owner != Players.PLAYER1)
                //{
                //    rt.sustain(0.06f, Players.PLAYER1);
                //}
                //else
                //{
                //    rt.receive(0.06f);
                //}

                //if (selectedTile != null)
                //{
                //    selectedTile.setSelected(false);
                //}

                tile.setSelected(true);
                selectedTile = tile;
            }
            else
            {
                if (selectedTile != null)
                {
                    selectedTile.setSelected(false);
                    selectedTile = null;
                }
            }

            return new Vector2(tile.indexX, tile.indexY);
        }

        public Vector2 GetTileScreenPos(Vector2 tileCords, float translationX, float translationY)
        {
            return new Vector2(map[(int)tileCords.X, (int)tileCords.Y].GetPos(translationX, translationY).X, map[(int)tileCords.X, (int)tileCords.Y].GetPos(translationX, translationY).Y);
        }
        // new
    }
}
