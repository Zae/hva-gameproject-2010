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
using ION.MultiPlayer;
using ION.Tools;

namespace ION
{
    public class Grid
    {
        private bool online=false;

        private static Grid instance = null;
        
        public static Tile[,] map;
        public static Sector[,] sectors;

        public static int width;
        public static int height;
        public static int tileCount;

        public float mouseWorldX = 0;
        public float mouseWorldY = 0;

        public static float resourceRate = 1.0f;

        //public int gameTick = 0;
        //public int lastTick = 0;

        public Tile selectedTile = null;

        public static Tile[] perspectiveMap;
        public static List<ResourceTile> resourceTiles;
        public static List<MountainTile> mountainTiles;
        public static List<IDepthEnabled> depthItems;
        public static BaseTile[] playerBases;

        private GridStrategy updateStrategy;

        private StupidAI stupidAI;

        public List<Unit> unitRemovals = new List<Unit>();
        public List<IDepthEnabled> depthItemsRemovals = new List<IDepthEnabled>();


        private int viewDirection = 1;
        private const int SOUTH_WEST = 1;
        private const int NORTH_WEST = 2;
        private const int NORTH_EAST = 3;
        private const int SOUTH_EAST = 4;

        //private bool drawHitTest = false;
        private float virtualX = 0;
        private float virtualY = 0;

        private List<ResourceTile> tempNeighbours = new List<ResourceTile>();

        private ThemeManager theme;

        private RemoteSharedObject GridRSO;

        // a list to hold all units
        public List<Unit> allUnits = new List<Unit>();


        public static float resources = Robot.cost * 4;
        public static int playerNumber = -1;
        private static int playerUnitId = -1;

        public float totalCollected = 0;
        public static float toCollect = 2500;

        public static int[] playerInfluences;

        //Timing controls
        public DateTime startTime = DateTime.Now;
        public DateTime currentTime;

        public TimeSpan passedTime;

        public static int TPS = 30; // Ticks per Second
        public static int TPT = 1000 / TPS; //Timesclice Per Tick (in milliseconds)
        public int TCP = 0; //Ticks Currently Processed

        public float TTP = 0; //Ticks To Process
        public float intermediate = 0.0f; //Our progress between ticks, for drawing purposes only

        public static void setTPS(int newTPS) 
        {
            TPS = newTPS;
            TPT = 1000 / TPS;
        }

        public void update(int ellapsed, List<Unit> units, float translationX, float translationY)
        {
            currentTime = DateTime.Now;
           // ION.get().IsMouseVisible = false;
            passedTime = currentTime - startTime;
           // Debug.WriteLine("passed time:" + passedTime.Milliseconds);

            TTP = ((float)passedTime.TotalMilliseconds / TPT) - TCP;

            //Debug.WriteLine("ttp:" + TTP);
            if (TTP > 1)
            {
                //do the next tick
                intermediate = TTP - 1;
            }
            else
            {
                intermediate = TTP;
                //dont do the rest of the update
                return;
            }

        

            //Temp AI
            if (!StateTest.get().online)
            {
                stupidAI.act();          
            }

            SoundManager.update();

            ////Checksum test
            //if (gameTick % 100 == 0)
            //{
            //    CheckSumProduct scp = CheckSumProduct.getCheckSum();
            //    Debug.WriteLine("product at tick "+gameTick+" = " + scp.sum);
            //}

            bool working = true;
            while (working)
            {
                working = CommandDispatcher.executeCommand(TCP);
            }

            updateStrategy.update(ellapsed);

            for (int i = 0; i < units.Count; i++)
            {

                if (map[units[i].inTileX, units[i].inTileY] is ResourceTile)
                {
                    ResourceTile rt = (ResourceTile)map[units[i].inTileX, units[i].inTileY];
                    if (rt.owner == units[i].owner)
                    {
                        if (rt.charge < 0.9)
                        {
                            rt.receive(0.02f);
                        }
                    }
                    else
                    {
                        rt.sustain(0.050f, units[i].owner);
                    }
                }


                foreach (ResourceTile r in getNeighbourhood(units[i].inTileX, units[i].inTileY))
                {
                    if (r.owner == units[i].owner)
                    {
                        if (r.charge < 0.8)
                        {
                            r.receive(0.02f);
                        }
                    }
                    else
                    {
                        r.sustain(0.03f, units[i].owner);
                    }
                }

                //updates the unit
                units[i].Update(translationX, translationY);
            }

            //Reset the influence variables
            for (int i = 0; i < playerInfluences.Length; i++)
            {
                playerInfluences[i] = 0;
            }

            //Update the resources of the player
            foreach (ResourceTile rt in resourceTiles)
            {
                playerInfluences[rt.owner] += 1;

                if (rt.owner == playerNumber)
                {
                    //TODO find a good place for this
                    float f = (rt.charge * resourceRate);

                    resources += f;
                    totalCollected += f;
                }
            }

            foreach (Unit u in unitRemovals)
            {
                allUnits.Remove(u);
      
            }

            foreach (IDepthEnabled ide in depthItemsRemovals)
            {
               
                depthItems.Remove(ide);
            }

            TCP++;
        }

        public static BaseTile getPlayerBase(int owner)
        {
            return playerBases[owner - 1];
        }

        public List<Unit> getPlayerEnemies(int player)
        {
            List<Unit> enemies = new List<Unit>();
            foreach (Unit u in allUnits)
            {
                if (u.owner != player)
                {
                    enemies.Add(u);
                }
            }

            return enemies;
        }

        public static Grid get()
        {
            return instance;
        }

        public static int getNewId()
        {
            playerUnitId++;
            return playerUnitId;
        }

        public void selectTile(float x, float y, float translationX, float translationY)
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
            selectedTile = getTile(tilesVertical, tilesHorizontal, color);
        }

        public Tile getTile(float x, float y, float translationX, float translationY)
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
            return getTile(tilesVertical, tilesHorizontal, color);
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

            //Debug.WriteLine(FiringFrame++ + " Color under mouse is: " + Colors.getColor(myUint[0]) + " uint:" + sColorval);

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
            //init the render
            ION.spriteBatch.Begin();
            //ION.spriteBatch.Begin(SpriteBlendMode.AlphaBlend,SpriteSortMode.Immediate,SaveStateMode.None);
            //ION.graphics.GraphicsDevice.SamplerStates[0].MinFilter = TextureFilter.GaussianQuad;
            //ION.graphics.GraphicsDevice.SamplerStates[0].MagFilter = TextureFilter.GaussianQuad;

            //draw the ground texture
            theme.drawGroundTexture();

            /*
            for (int i = 0; i < resourceTiles.Count(); i++)
            {
                if (resourceTiles[i] is BaseTile)
                    resourceTiles[i].draw(translationX, translationY);
            }

            for (int i = 0; i < resourceTiles.Count(); i++)
            {
                if (resourceTiles[i] is BaseTile) { }
                else
                    resourceTiles[i].draw(translationX, translationY);
            }
            */
            
            foreach (ResourceTile rt in resourceTiles)
            {
                    rt.draw(translationX, translationY);
                    //rt.drawDebug(translationX, translationY);
            }
            
            foreach (BaseTile bt in playerBases)
            {
                bt.drawResourceTile(translationX, translationY);
            }

            for (int i = 0; i < allUnits.Count; i++)
            {
                allUnits[i].DrawWayPoints(translationX, translationY);
            }

            foreach (IDepthEnabled de in depthItems)
            {
                de.drawDepthEnabled(translationX, translationY);
            }

            //if (drawHitTest)
            //{
            //    ION.spriteBatch.Draw(Images.tileHitmapImage, new Rectangle(0, 0, Images.tileHitmapImage.Width, Images.tileHitmapImage.Height), Color.White);
            //    ION.spriteBatch.Draw(Images.white1px, new Rectangle((int)virtualX, (int)virtualY, 5, 5), Color.Black);
            //}

            //GridStrategy might want to do some debug rendering
            //updateStrategy.drawDebug();

   

            //finish up the render
            ION.spriteBatch.End();
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
                MountainTile newTile = new MountainTile(x, y, 20);
                addDepthEnabledItem(newTile);
                mountainTiles.Add(newTile);
                return newTile;
            }
            else if (c == 'C')
            {
                MountainTile newTile = new MountainTile(x, y, 21);
                addDepthEnabledItem(newTile);
                mountainTiles.Add(newTile);
                return newTile;
            }
            else if (c == '5')
            {
                MountainTile newTile = new MountainTile(x, y, 0);
                addDepthEnabledItem(newTile);
                mountainTiles.Add(newTile);
                return newTile;
            }
            else if (c == '8')
            {
                MountainTile newTile = new MountainTile(x, y, 1);
                addDepthEnabledItem(newTile);
                mountainTiles.Add(newTile);
                return newTile;
            }
            else if (c == '9')
            {
                MountainTile newTile = new MountainTile(x, y, 2);
                addDepthEnabledItem(newTile);
                mountainTiles.Add(newTile);
                return newTile;
            }
            else if (c == '+')
            {
                MountainTile newTile = new MountainTile(x, y, 3);
                addDepthEnabledItem(newTile);
                mountainTiles.Add(newTile);
                return newTile;
            }
            else if (c == '7')
            {
                MountainTile newTile = new MountainTile(x, y, 4);
                addDepthEnabledItem(newTile);
                mountainTiles.Add(newTile);
                return newTile;
            }
            else if (c == '/')
            {
                MountainTile newTile = new MountainTile(x, y, 5);
                addDepthEnabledItem(newTile);
                mountainTiles.Add(newTile);
                return newTile;
            }
            else if (c == '6')
            {
                MountainTile newTile = new MountainTile(x, y, 6);
                addDepthEnabledItem(newTile);
                mountainTiles.Add(newTile);
                return newTile;
            }
            else if (c == '4')
            {
                MountainTile newTile = new MountainTile(x, y, 7);
                addDepthEnabledItem(newTile);
                mountainTiles.Add(newTile);
                return newTile;
            }
            else if (c == '2')
            {
                MountainTile newTile = new MountainTile(x, y, 8);
                addDepthEnabledItem(newTile);
                mountainTiles.Add(newTile);
                return newTile;
            }
            else if (c == '3')
            {
                MountainTile newTile = new MountainTile(x, y, 9);
                addDepthEnabledItem(newTile);
                mountainTiles.Add(newTile);
                return newTile;
            }
            else if (c == '-')
            {
                MountainTile newTile = new MountainTile(x, y, 10);
                addDepthEnabledItem(newTile);
                mountainTiles.Add(newTile);
                return newTile;
            }
            else if (c == '1')
            {
                MountainTile newTile = new MountainTile(x, y, 11);
                addDepthEnabledItem(newTile);
                mountainTiles.Add(newTile);
                return newTile;
            }
            else if (c == '*')
            {
                MountainTile newTile = new MountainTile(x, y, 12);
                addDepthEnabledItem(newTile);
                mountainTiles.Add(newTile);
                return newTile;
            }
            else if (c == 'V')
            {
                VoidTile newTile = new VoidTile(x, y);
                //addDepthEnabledItem(newTile);
                return newTile;
            }
            else if (c == 'A')//places a tower for player 1
            {
                ResourceTile newTile = new ResourceTile(x, y);
                resourceTiles.Add(newTile);

                // creates a new tower for player 1 at the map cords
                Tower t = new Tower(newTile, 1, 1, getNewId());
                allUnits.Add(t);
                addDepthEnabledItem(t);

                return newTile;
            }
            else if (c == 'B')//places a tower for player 2 / AI
            {
                ResourceTile newTile = new ResourceTile(x, y);
                resourceTiles.Add(newTile);

                // creates a new tower for player 2 at the map cords
                Tower t = new Tower(newTile, 1, 2, getNewId());
                allUnits.Add(t);
                addDepthEnabledItem(t);

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
            
            foreach (Unit u in allUnits)
            {
                if (u.selected && u.owner == playerNumber)
                {
                    selection.Add(u);
                }
            }

            return selection;
        }

        
        public Vector2 GetTile(float x, float y, float translationX, float translationY)
        {
            selectTile(x, y, translationX, translationY);

            if (selectedTile == null)
            {
                Debug.WriteLine("HEEELP TILE IS NULL! getTile()");
                //Can't return null so we send something that is invalid
                return new Vector2(-1, -1);
            }

            return new Vector2(selectedTile.indexX, selectedTile.indexY);
        }

        public void createUnit(int owner, int id)
        {
            BaseTile playerBase = getPlayerBase(owner);
            Robot newUnit = new Robot(playerBase,
                owner, id);
            allUnits.Add(newUnit);
            addDepthEnabledItem(newUnit);
        }

        public void createTowerUnit(int owner, int towerId, int robotId)
        {
            //find the Robot that turns into a Tower
            Unit u = Grid.get().getUnit(owner, robotId);

            //if the Robot is found
            if (u != null && u is Robot)
            {
                u.Die();
                Tower newUnit = new Tower(Grid.map[u.inTileX,u.inTileY], 0, owner, towerId);
      
                allUnits.Add(newUnit);
                addDepthEnabledItem(newUnit);
            }      
        }

        public static void addDepthEnabledItem(IDepthEnabled newItem)
        {
            //add the item to the list
            int index = -1;
            bool inserted = false;

            //Debug.WriteLine("INSERTING DEPTH ENABLED ITEM!");

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

        public void removeDepthEnabledItem(IDepthEnabled ide)
        {
            depthItemsRemovals.Add(ide);
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
            if (!online)
            {
                int aiPlayer = 1; if (playerNumber == 1) aiPlayer = playerNumber + 1;
                stupidAI = new StupidAI(aiPlayer);
        
            }
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

                    //See if this level can fit in the number of available Sectors
                    if ((width / Sector.TILES_PER_SECTOR_AXIS) > Sector.MAX_SECTOR_ON_AXIS || (width / Sector.TILES_PER_SECTOR_AXIS) > Sector.MAX_SECTOR_ON_AXIS)
                    {
                        Debug.WriteLine("FATAL ERROR: Cannot house this amount of Sectors");
                        ION.get().Exit();
                    }

                    if ((width % Sector.TILES_PER_SECTOR_AXIS > 0 || height % Sector.TILES_PER_SECTOR_AXIS > 0)) 
                    {
                        Debug.WriteLine("FATAL ERROR: This Level is not formatted for the current Tile per Sector");
                        ION.get().Exit();
                    }

                    sectors = new Sector[width/Sector.TILES_PER_SECTOR_AXIS,height/Sector.TILES_PER_SECTOR_AXIS];
                    //sectors.

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

                    //Load the theme
                    string themeName = (XmlRdr.GetAttribute(3+(playerCount*2)));
                    string groundTexture = (XmlRdr.GetAttribute(4 + (playerCount * 2)));
                    theme = new ThemeManager(themeName,groundTexture,width,height);

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
                        if (rawLevel[i] == 'N' || rawLevel[i] == 'M' || rawLevel[i] == 'C' || rawLevel[i] == 'V' || rawLevel[i] == '1' || rawLevel[i] == '2' || rawLevel[i] == '3' || rawLevel[i] == '4' || rawLevel[i] == '5' || rawLevel[i] == '6' || rawLevel[i] == '7' || rawLevel[i] == '8' || rawLevel[i] == '9' || rawLevel[i] == '/' || rawLevel[i] == '*' || rawLevel[i] == '+' || rawLevel[i] == '-' || rawLevel[i] == 'A' || rawLevel[i] == 'B')
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
                    foreach (ResourceTile rt in toRemove)
                    {
                        resourceTiles.Remove(rt);
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

        public void onResize()
        {
            theme.resizeGroundTexture();
        }

        public List<Unit> getPlayerUnits(int number)
        {
            List<Unit> playerUnits = new List<Unit>();
            foreach(Unit u in allUnits) 
            {
                if (u.owner == number)
                {
                    playerUnits.Add(u);
                }
            }
            return playerUnits;
        }

        public Unit getUnit(int unitOwner, int unitId)
        {
            foreach (Unit u in allUnits)
            {
                if (u.owner == unitOwner && u.id == unitId)
                {
                    return u;
                }
            }
            return null;
        }

        public bool isValid(int x, int y)
        {
            if (x >= 0 && x < Grid.width && y >= 0 && y < Grid.height)
            {
                return true;
            }
            return false;
        }

        public List<ResourceTile> getNeighbourhood(int x, int y)
        {
            tempNeighbours.Clear();
            
            //Horizontal and Vertical
            if (isValid(x, y - 1) && Grid.map[x, y - 1] is ResourceTile)
            {
                tempNeighbours.Add((ResourceTile)Grid.map[x, y - 1]);
            }

            if (isValid(x + 1, y) && Grid.map[x + 1, y] is ResourceTile)
            {

                tempNeighbours.Add((ResourceTile)Grid.map[x + 1, y]);

            }

            if (isValid(x, y + 1) && Grid.map[x, y + 1] is ResourceTile)
            {

                tempNeighbours.Add((ResourceTile)Grid.map[x, y + 1]);
  
            }

            if (isValid(x - 1, y) && Grid.map[x - 1, y] is ResourceTile)
            {

                tempNeighbours.Add((ResourceTile)Grid.map[x - 1, y]);

            }

            //if (doDiagonal)
            //{
                if (isValid(x - 1, y - 1) && Grid.map[x - 1, y - 1] is ResourceTile)
                {

                    tempNeighbours.Add((ResourceTile)Grid.map[x - 1, y - 1]);
 

                }
                if (isValid(x + 1, y - 1) && Grid.map[x + 1, y - 1] is ResourceTile)
                {

                    tempNeighbours.Add((ResourceTile)Grid.map[x + 1, y - 1]);

                }
                if (isValid(x + 1, y + 1) && Grid.map[x + 1, y + 1] is ResourceTile)
                {

                    tempNeighbours.Add((ResourceTile)Grid.map[x + 1, y + 1]);

                }
                if (isValid(x - 1, y + 1) && Grid.map[x - 1, y + 1] is ResourceTile)
                {

                    tempNeighbours.Add((ResourceTile)Grid.map[x - 1, y + 1]);

                }
            //}

                return tempNeighbours;
        }

        public void removeUnit(Unit u)
        {
            unitRemovals.Add(u);
        }


    }
}
