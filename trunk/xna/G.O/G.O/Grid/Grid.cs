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
    public class Grid
    {
        private static Grid instance = null;
        
        public static Tile[,] map;
        public static Sector[,] sectors;

        public static int width;
        public static int height;
        public static int tileCount;

        public float mouseWorldX = 0;
        public float mouseWorldY = 0;

        public Tile selectedTile = null;

        public static Tile[] perspectiveMap;
        public static List<ResourceTile> resourceTiles;
        public static List<MountainTile> mountainTiles;
        public static List<IDepthEnabled> depthItems;
        public static BaseTile[] playerBases;

        private GridStrategy updateStrategy;

        private int viewDirection = 1;
        private const int SOUTH_WEST = 1;
        private const int NORTH_WEST = 2;
        private const int NORTH_EAST = 3;
        private const int SOUTH_EAST = 4;

        private bool drawHitTest = false;
        private float virtualX = 0;
        private float virtualY = 0;

        private RemoteSharedObject GridRSO;

        // a list to hold the blue army
        public List<Unit> blueArmy = new List<Unit>();

        public static int playerNumber = -1;
        public static int[] playerInfluences;
        public static float resources = 0;

        public static BaseTile getPlayerBase(int owner)
        {
            return playerBases[owner - 1];
        }

        public static Grid get()
        {
            return instance;
        }

        public void selectTile(float x, float y, float translationX, float translationY)
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
            selectedTile = getTile(tilesVertical, tilesHorizontal, color);
        }

        public GridStrategy getUpdateStrategy()
        {
            return updateStrategy;
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
                if (color == Colors.color_red)
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


            //Need to translate the coordinate for this to work, but it would be faster
            //if (lookForX >= 0 && lookForX < width && lookForY >= 0 && lookForY < height)
            //{
            //    return map[lookForX, lookForY];
            //}

            return null;

        }

        public void draw(float translationX, float translationY)
        {
            ION.spriteBatch.Begin();

            foreach (ResourceTile rt in resourceTiles)
            {
                    rt.draw(translationX, translationY);
                    //rt.drawDebug(translationX, translationY);
            }
            ION.spriteBatch.End();

            ION.spriteBatch.Begin();
            foreach (IDepthEnabled de in depthItems)
            {
                de.drawDepthEnabled(translationX, translationY);
            }
            ION.spriteBatch.End();

            if (drawHitTest)
            {
                ION.spriteBatch.Begin();
                ION.spriteBatch.Draw(Images.tileHitmapImage, new Rectangle(0, 0, Images.tileHitmapImage.Width, Images.tileHitmapImage.Height), Color.White);
                ION.spriteBatch.Draw(Images.white1px, new Rectangle((int)virtualX, (int)virtualY, 5, 5), Color.Black);
                ION.spriteBatch.End();
            }

            //GridStrategy might want to do some debug rendering
            //updateStrategy.drawDebug();

            ION.spriteBatch.Begin();
            for (int i = 0; i < blueArmy.Count; i++)
            {
                blueArmy[i].DrawWayPoints(translationX, translationY);
            }
            ION.spriteBatch.End();
        }

        public void update(int ellapsed, List<Unit> blueArmy, float translationX, float translationY)
        {
            updateStrategy.update(ellapsed);

            for (int i = 0; i < blueArmy.Count(); i++)
            {
                //TODO units should make a more difuse effect on the grid
                if (blueArmy[i] != null)
                {
                    selectTile(blueArmy[i].GetVirtualPos().X, blueArmy[i].GetVirtualPos().Y, translationX, translationY);
                    if (selectedTile is ResourceTile)
                    {
                        ResourceTile rt = (ResourceTile)selectedTile;
                        if (rt.owner == playerNumber)
                        {
                            rt.receive(0.02f);
                        }
                        else
                        {
                            rt.sustain(0.02f, playerNumber);
                        }
                    }
                }

                //updates the unit
                blueArmy[i].Update(translationX, translationY);

                //tells the unit what tile it is currently on
                Vector2 temp = GetTile(blueArmy[i].GetVirtualPos().X, blueArmy[i].GetVirtualPos().Y, translationX, translationY);
                if (temp != null)
                {
                    blueArmy[i].UpdateTile(temp);
                }
            }

            //Reset the influence variables
            for (int i = 0; i < playerInfluences.Length; i++)
            {
                playerInfluences[i] = 0;
            }

            //Update the resources of the player
            foreach(ResourceTile rt in resourceTiles) 
            {
                playerInfluences[rt.owner] += 1;

                if (rt.owner == playerNumber)
                {
                    //TODO find a good place for this
                    resources += (rt.charge /1000);
                }
            }

            /** Disabled for performance, works perfectly tho! **/
            if (GridRSO != null && GridRSO.Connected)
            {
                byte[] rs = Serializer.Serialize(map, Grid.width, Grid.height);
                GridRSO.SetAttribute("Grid", rs);
            }
        }

        private Tile createTile(char c, int x, int y)
        {
            if (c == 'N')
            {
                ResourceTile newTile = new ResourceTile(x, y);
                resourceTiles.Add(newTile);
                return newTile;
            }
            else if (c == 'M')
            {
                MountainTile newTile = new MountainTile(x, y);
                addDepthEnabledItem(newTile);
                return newTile;
            }

            return null;
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
        }

        void GridRSO_OnConnect(object sender, EventArgs e)
        {
        }

        void GridRSO_NetStatus(object sender, NetStatusEventArgs e)
        {
        }

        public List<Unit> getSelection() 
        {
            List<Unit> selection = new List<Unit>();
            
            foreach (Unit u in blueArmy)
            {
                if (u.selected && u.owner == playerNumber)
                {
                    selection.Add(u);
                }
            }

            return selection;
        }

        // new
        public Vector2 GetTile(float x, float y, float translationX, float translationY)
        {
            selectTile(x, y, translationX, translationY);

            if (selectedTile == null)
            {
                Debug.WriteLine("HEEELP TILE IS NULL! getTile()");
                return new Vector2(0,0);

            }

            return new Vector2(selectedTile.indexX, selectedTile.indexY);
        }

        public Vector2 GetTileScreenPos(Vector2 tileCords, float translationX, float translationY)
        {
            return new Vector2(map[(int)tileCords.X, (int)tileCords.Y].GetPos(translationX, translationY).X, map[(int)tileCords.X, (int)tileCords.Y].GetPos(translationX, translationY).Y);
        }

        public void CreateBlueUnit(float translationX, float translationY)
        {
            BallUnit newUnit = new BallUnit(GetTileScreenPos(new Vector2(12, 12), translationX, translationY), GetTileScreenPos(new Vector2(11, 13), translationX, translationY));
            blueArmy.Add(newUnit);
            addDepthEnabledItem(newUnit);
        }

        public static void addDepthEnabledItem(IDepthEnabled newItem)
        {
            //add the item to the list
            int index = -1;
            bool inserted = false;
            //TODO hardcoding for a south-west perspective
            foreach (IDepthEnabled other in depthItems)
            {
                index++;

                if (other.getTileX() > newItem.getTileX())
                {
                    continue;
                }
                else if (other.getTileX() == newItem.getTileX())
                {
                    if (other.getTileY() >= newItem.getTileY())
                    {
                        depthItems.Insert(index, newItem);
                        inserted = true;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    depthItems.Insert(index, newItem);
                    inserted = true;
                    break;
                }
            }
            if (!inserted)
            {
                depthItems.Add(newItem);
            }
        }

        public static void removeDepthEnabledItem(IDepthEnabled newItem)
        {
            depthItems.Remove(newItem);
        }

        public static void updateDepthEnabledItem(IDepthEnabled newItem)
        {
            //TODO 
            //Now I just remove and re-insert the item
            //A faster algorithm could be made

            depthItems.Remove(newItem);
            addDepthEnabledItem(newItem);

            //The best implementation would be along these lines:
            //Get the current index
            //Look for the next item if it is not the last item in the list
            //If the next item is more in the foreground on the basis of the perspective
            //Change direction and look for the previous item more to the beginning of the list
            //If that item is more in the foreground, continue the search in that direction untill you find its right place
            //This way items that are most in the foreground are not penelized to search through everything in the background
            //Because this is what happens when calling addDepthEnabledItem()
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

        public Grid(String levelname, GridStrategy strategy, int playerNumber)
        {
            instance = this;

            Grid.playerNumber = playerNumber;

            updateStrategy = strategy;
            updateStrategy.reset();

            resourceTiles = new List<ResourceTile>();
            mountainTiles = new List<MountainTile>();
            depthItems = new List<IDepthEnabled>();

            if (ION.get().serverConnection != null && ION.get().serverConnection.GameConnection.Connected)
            {
                GridRSO = RemoteSharedObject.GetRemote("Grid", ION.get().serverConnection.GameConnection.Uri.ToString(), false);
                GridRSO.NetStatus += new NetStatusHandler(GridRSO_NetStatus);
                GridRSO.OnConnect += new ConnectHandler(GridRSO_OnConnect);
                GridRSO.OnDisconnect += new DisconnectHandler(GridRSO_OnDisconnect);
                GridRSO.Sync += new SyncHandler(GridRSO_Sync);
                GridRSO.Connect(ION.get().serverConnection.GameConnection);
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
                    playerInfluences = new int[playerCount + 1];

                    int[] positionsX = new int[playerCount];
                    int[] positionsY = new int[playerCount];
                    for (int i = 0; i < playerCount; i++)
                    {
                        positionsX[i] = int.Parse(XmlRdr.GetAttribute(3 + (i * 2)));
                        positionsY[i] = int.Parse(XmlRdr.GetAttribute(3 + (i * 2) + 1));

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


                    Debug.WriteLine("resourceTileCount:" + resourceTiles.Count);
                    //finally take the player position and put them into the grid
                    List<ResourceTile> toRemove = new List<ResourceTile>();
                    playerBases = new BaseTile[playerCount];
                    for (int i = 0; i < playerCount; i++)
                    {
                        BaseTile newBase = new BaseTile(positionsX[i], positionsY[i], i + 1);
                        playerBases[i] = newBase;
                        map[positionsX[i], positionsY[i]] = newBase;
                        addDepthEnabledItem(newBase);

                        //remove the ResrouceTiles from the map


                        foreach (ResourceTile rt in resourceTiles)
                        {
                            if (rt.indexX == positionsX[i] && rt.indexY == positionsY[i])
                            {
                                toRemove.Add(rt);
                            }
                        }
                    }
                    Debug.WriteLine("toremove size:" + toRemove.Count);
                    foreach (ResourceTile rt in toRemove)
                    {
                        resourceTiles.Remove(rt);
                    }

                    Debug.WriteLine("resourceTileCount:" + resourceTiles.Count);
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
    }
}
