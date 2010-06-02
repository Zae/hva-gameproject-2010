using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using ION.Tools;

namespace ION
{
    public abstract class Unit : IDepthEnabled
    {
        private Colors hitmapColor = new Colors();

        public int owner;

        public Rectangle drawingRectangle = new Rectangle();
        public Rectangle selectionRectangle = new Rectangle();
        public Point focalPoint = new Point();

        public enum direction { south = 0, southEast = 1, east = 2, northEast = 3, north = 4, northWest = 5, west = 6, southWest = 7 };
        public direction facing = direction.north;

        //don't initialize these, subclasses of unit must specify these in the constructor (or base())
        public int id;
        public int health;
        public int damage;
        public int damageType; 

        protected Vector2 pos, targetPos, virtualPos;

        //the index of the tile the unit is on
        public int inTileX = 0;
        public int inTileY = 0;

        public abstract void draw(float x, float y);

        public bool firing = false;
        public bool underFire = false;

        protected float movementSpeed;

        protected static float scale = 15; //TODO this should be externalized

        public static float baseHalfWidth = baseHalfWidthConstant * scale;
        public static float baseHalfHeight = baseHalfHeightConstant * scale;

        private const float baseHalfWidthConstant = 3;
        private const float baseHalfHeightConstant = 1;

        protected float visualZ;
        protected float visualX;
        protected float visualY;

        public int scan = 0;

        public bool selected = false;
        public bool showDetails = false; //used to show only details such as health, but not mark this unit selected.

        public Queue<Tile> destination;

        private List<Tile> failed;

        public Unit(int owner,int id)
        {
            this.owner = owner;
            this.id = id;
            destination = new Queue<Tile>();
            failed = new List<Tile>();
        }

        public bool Update(Vector2 newInTile, List<Unit> allUnits, Tile[,] grid, float translationX, float translationY)
        {
            bool returnValue = false;
            returnValue = UpdateTile(newInTile, allUnits, grid, translationX, translationY);

            if (health < 0)
            {
                //put this unit in some sort of death animation list somewhere


                grid[inTileX, inTileY].accessable = true;

                Die();

                //the end
                return returnValue;
            }
            
            if (pos != targetPos)
            {
                move();
            }
            else
            {
                // Code for waypoints
                if (destination.Count() != 0)//(destination.Last<Tile>() != null)//here
                {
                    
                    //targetPos = new Vector2(destination.Last<Tile>().indexX, destination.Last<Tile>().indexY);

                    Tile temp = destination.Dequeue();

                    targetPos = temp.GetPos(translationX, translationY);

                    failed = new List<Tile>();
                }
                
            }
            if (scan > 4)
            {
                
                List<Unit> enemies = Grid.get().getPlayerEnemies(owner);
                if (enemies.Count == 0) firing = false;
                int distance = 4;
                foreach (Unit u in enemies)
                {
                    if ((u.inTileX - inTileX > -distance && u.inTileX - inTileX < distance) && (u.inTileY - inTileY > -distance && u.inTileY - inTileY < distance))
                    {
                        firing = true;
                        SoundManager.fireSound(Grid.TPS * 2);
                        //fire on this unit.
                        u.hit(Damage.getDamage(damage,damage+10),u.damageType);
                        faceUnit(u);
                        break;
                    }
                    firing = false;
                }
                scan = 0;
            }
            scan++;
            // Code for waypoints

            //will move this to the above if statement when the unit know its tile
            virtualPos.X = ((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (translationX) + (baseHalfWidth * 1.1f);
            virtualPos.Y = ((pos.Y) * (scale / 15.0f)) + (translationY) + (baseHalfWidth * 1.6f);

            return returnValue;
        }

        private void faceUnit(Unit u)
        {
            //get the center of the selectionbox of the unit.
            //compare it with the center of this unit's selection box
            //work out the number of degrees
            //select the right facing 
        }

        public void hit(int damageTaken, int damageType)
        {
            
            health -= damageTaken;

            //this will trigger or keep alive the animation
            underFire = true;
        }


        public bool UpdateTile(Vector2 newInTile, List<Unit> allUnits, Tile[,] grid, float translationX, float translationY)
        {
            
            if (newInTile.X < 0)
            {
               
                    return false;
            }
            
            //if the player is occuping a new tile it will update & return true
            if (inTileX != (int)newInTile.X || inTileY != (int)newInTile.Y)
            {
                
                
                bool test = false;
                if (!grid[(int)newInTile.X, (int)newInTile.Y].accessable)
                {
                    test = true;
                }
                else
                {
                    for (int i = 0; i < failed.Count(); i++)
                    {
                        if (grid[(int)newInTile.X, (int)newInTile.Y] == failed[i])
                        {
                            test = true;
                        }
                    }

                    //for (int i = 0; i < allUnits.Count(); i++)
                    //{
                    //    if (allUnits[i].inTileX == (int)newInTile.X && allUnits[i].inTileY == (int)newInTile.Y)
                    //    {
                    //        test = true;
                    //    }
                    //}
                }


                if (test)
                {//here
                    ////look at the current touching tiles, which are accessable and not in the failed list
                    //Tile[] checkTiles = new Tile[8];// the 8 tiles touching the current
                    //if (inTileX > 0)
                    //{
                    //    checkTiles[0] = grid[inTileX - 1, inTileY];
                    //}
                    //if (inTileX > 0 && inTileY > 0)
                    //{
                    //    checkTiles[1] = grid[inTileX - 1, inTileY - 1];
                    //}
                    //if (inTileY > 0)
                    //{
                    //    checkTiles[2] = grid[inTileX, inTileY - 1];
                    //}
                    //if (inTileX < grid.GetLength(0) - 1 && inTileY > 0)
                    //{
                    //    checkTiles[3] = grid[inTileX + 1, inTileY - 1];
                    //}
                    //if (inTileX < grid.GetLength(0) - 1)
                    //{
                    //    checkTiles[4] = grid[inTileX + 1, inTileY];
                    //}
                    //if (inTileX < grid.GetLength(0) - 1 && inTileY < grid.GetLength(1) - 1)
                    //{
                    //    checkTiles[5] = grid[inTileX + 1, inTileY + 1];
                    //}
                    //if (inTileY < grid.GetLength(1) - 1)
                    //{
                    //    checkTiles[6] = grid[inTileX, inTileY + 1];
                    //}
                    //if (inTileX > 0 && inTileY < grid.GetLength(1) - 1)
                    //{
                    //    checkTiles[7] = grid[inTileX - 1, inTileY + 1];
                    //}
                    //int closestDistance = -1;
                    //int closestTile = -1;



                    //for (int i = 0; i < 8; i++)
                    //{
                    //    if (checkTiles[i] != null && grid[checkTiles[i].indexX, checkTiles[i].indexY].FreeTile(checkTiles[i], allUnits))
                    //    {
                    //        bool inFailedList = false;
                    //        for (int j = 0; j < failed.Count(); j++)
                    //        {
                    //            if (failed[j] == checkTiles[i])
                    //            {
                    //                inFailedList = true;
                    //            }
                    //        }
                    //        if (!inFailedList)
                    //        {
                    //            Vector2 temp = checkTiles[i].GetPos(1, 1) - targetPos;
                    //            if (temp.Length() < (int)closestDistance || closestDistance < 0)
                    //            {
                    //                closestDistance = (int)temp.Length();
                    //                closestTile = i;
                    //            }
                    //            //if (temp.Length() == (int)closestDistance && closestTile != i)
                    //            //{
                    //            //    Random r = new Random((int)temp.Length());
                    //            //    if ((r.Next() % 2) > 1)
                    //            //    {
                    //            //        closestDistance = (int)temp.Length();
                    //            //        closestTile = i;
                    //            //    }
                    //            //}
                    //        }
                    //    }

                    //}

                    ////if all touching tiles are not acccessable or in failed list, go to next waypoint and empty failed list
                    //if (closestTile < 0)
                    //{
                    //    if (destination.Count() > 0)
                    //    {
                    //        Tile temp = destination.Dequeue();
                    //        targetPos = temp.GetPos(translationX, translationY);
                    //    }
                    //    failed = new List<Tile>();
                    //}
                    ////otherwise go to the tile closest to the destination (if two are the same distance, randomly choose one)
                    //else
                    //{
                    //    failed.Add(checkTiles[closestTile]);
                    //    targetPos = checkTiles[closestTile].GetPos(translationX, translationY);
                    //}

                    ////once waypoint is reached empty failed list

                    
                    


                    ////set waypoint to current tile
                    //EmptyWayPoints();

                    ////destination.Enqueue(grid[inTileX, inTileY]);

                    ////pos = targetPos;
                    //targetPos = grid[inTileX, inTileY].GetPos(translationX, translationY);

                    ////Tile temp = destination.Dequeue();
                    ////targetPos = temp.GetPos(translationX, translationY);

                    return false;
                }
                else
                {
                    grid[inTileX, inTileY].accessable = true;
                    inTileX = (int)newInTile.X;
                    inTileY = (int)newInTile.Y;
                    grid[inTileX, inTileY].accessable = false;

                    //Tell the Grid this Unit's tile position has changed
                    Grid.updateDepthEnabledItem(this);
                }

                return true;
            }
            else
            {
                return false;
            }

            //Debug.WriteLine("new tilexy: " + inTileX + "," + inTileY);
            //Vector2 tilePos = map.GetTile(pos.X, pos.Y, translationX, translationX);
            //inTileX = tilePos.X;
            //inTileY = tilePos.Y;
        }


        //PathFinding
        public void FindPath(Tile[,] map, List<Unit> allUnits)
        {
            //1) Add the starting square (or node) to the open list.
            //2) Repeat the following:
            //a) Look for the lowest F cost square on the open list. We refer to this as the current square.
            //b) Switch it to the closed list.
            //c) For each of the 8 squares adjacent to this current square …
            //    *
            //      If it is not walkable or if it is on the closed list, ignore it. Otherwise do the following.           
            //    *
            //      If it isn’t on the open list, add it to the open list. Make the current square the parent of this square. Record the F, G, and H costs of the square. 
            //    *
            //      If it is on the open list already, check to see if this path to that square is better, using G cost as the measure. A lower G cost means that this is a better path. If so, change the parent of the square to the current square, and recalculate the G and F scores of the square. If you are keeping your open list sorted by F score, you may need to resort the list to account for the change.
            //d) Stop when you:
            //    * Add the target square to the closed list, in which case the path has been found (see note below), or
            //    * Fail to find the target square, and the open list is empty. In this case, there is no path.   
            //3) Save the path. Working backwards from the target square, go from each square to its parent square until you reach the starting square. That is your path.

            float f = 0.0f;
            //create the open list of nodes, initially containing only our starting node
            List<Tile> openNodes = new List<Tile>();
            List<float> openNodesF = new List<float>();
            openNodes.Add((Tile)map[inTileX, inTileY]);
            openNodesF.Add(0f);

            //    create the closed list of nodes, initially empty
            List<Tile> closedNodes = new List<Tile>();
            List<float> closedNodesG = new List<float>();
            float g = 0.0f;

            //    while (we have not reached our goal) {
            while ((closedNodes.Count() != 0 && closedNodes[closedNodes.Count() - 1] != destination.First<Tile>()) || closedNodes.Count() == 0)//while closedNodes does not contain the target tile
            {
                //        consider the best node in the open list (the node with the lowest f value)
                // NOTE: positive elements in the array have a distance of 10, negitave elements have a distance of 14
                Tile currentTile = openNodes[openNodes.Count() - 1];// the current tile
                Tile[] checkTiles = new Tile[8];// the 8 tiles touching the current
                if (map[inTileX - 1, inTileY] != null)
                {
                    checkTiles[0] = map[inTileX - 1, inTileY];
                }
                if (map[inTileX - 1, inTileY - 1] != null)
                {
                    checkTiles[1] = map[inTileX - 1, inTileY - 1];
                }
                if (map[inTileX, inTileY - 1] != null)
                {
                    checkTiles[2] = map[inTileX, inTileY - 1];
                }
                if (map[inTileX + 1, inTileY - 1] != null)
                {
                    checkTiles[3] = map[inTileX + 1, inTileY - 1];
                }
                if (map[inTileX + 1, inTileY] != null)
                {
                    checkTiles[4] = map[inTileX + 1, inTileY];
                }
                if (map[inTileX + 1, inTileY + 1] != null)
                {
                    checkTiles[5] = map[inTileX + 1, inTileY + 1];
                }
                if (map[inTileX, inTileY + 1] != null)
                {
                    checkTiles[6] = map[inTileX, inTileY + 1];
                }
                if (map[inTileX - 1, inTileY + 1] != null)
                {
                    checkTiles[7] = map[inTileX - 1, inTileY + 1];
                }

                int closestTile = 0;// specifys the closest tile to touching the current tile to the destination tile
                float closest = 1000000000000000f;// initalized as huge so that any distance will be smaller
                for (int i = 0; i < 8; i++)// checks which tile is closest to the target
                {
                    if (checkTiles[i] != null && map[checkTiles[i].indexX, checkTiles[i].indexY].FreeTile(checkTiles[i], allUnits))
                    {
                        Vector2 temp = checkTiles[i].GetPos(1, 1) - currentTile.GetPos(1, 1);
                        if ((float)temp.Length() < closest)
                        {
                            closest = (float)temp.Length();
                            closestTile = i;
                        }
                    }
                }



                //        if (this node is the goal) {
                if (false/*temp*/ && checkTiles[closestTile] == destination.First<Tile>())//here
                {
                    //            then we're done
                }
                else
                {
                    //            move the current node to the closed list and consider all of its neighbors
                    closedNodes.Add(checkTiles[closestTile]);
                    if (closestTile % 2 == 0)
                    {
                        closedNodesG.Add(10f); g += 10f;
                    }
                    else
                    {
                        closedNodesG.Add(14f); g += 14f;
                    }
                    //            for (each neighbor) {
                    for (int i = 0; i < 8; i++)
                    {
                        bool ifCheck = false;
                        for (int j = 0; j < closedNodes.Count(); j++)
                        {

                            //                if (this neighbor is in the closed list and our current g value is lower) {
                            if (g < g + 10)
                            {
                                ifCheck = true;
                                //                    update the neighbor with the new, lower, g value 
                                // checkTiles[i].gValue or checkTiles[i,1] where 1 is the gValue and 0 is the tile
                                //                    change the neighbor's parent to our current node
                            }
                        }
                        for (int j = 0; j < openNodes.Count(); j++)
                        {
                            //                else if (this neighbor is in the open list and our current g value is lower) {
                            if (!ifCheck && checkTiles[i] == openNodes[j])
                            {
                                ifCheck = true;
                                //                    update the neighbor with the new, lower, g value 
                                //                    change the neighbor's parent to our current node
                            }

                        }
                        //                else this neighbor is not in either the open or closed list {
                        if (!ifCheck)
                        {
                            //                    add the neighbor to the open list and set its g value

                        }
                    }
                }
            }


        }

        public abstract void move();

        ////move units towards their target
        //void move()
        //{
        //    //Rules for moving units
        //    //1.	Units only move in 8 directions
        //    //2.	Units will form in a circle around the selected way point
        //    //3.	If there is an obstacle in the way, plot a course around it
        //    //4.	If there is a unit in the way, don’t perform move this turn
        //    //5.	If the unit has no waypoints, treat as obstacle
        //    //6.	If there is are obstacle stopping you from getting your waypoint or on your waypoint, go to next waypoint  if there is one,  otherwise stop
        //    //7.	If there is a unit in the way, and it is trying to move into your tile, treat it as an obstacle

        //    //if not at target
        //    if (pos != targetPos)
        //    {


        //        //// working //
        //        ////TODO @emmet
        //        ////1.	Units only move in 8 directions
        //        //// this code gets the angle of the vector in radians (note: this angle is either left or right of "X = 0")
        //        //Vector2 tempAngle = targetPos - pos;
        //        //double length = Math.Sqrt((tempAngle.X * tempAngle.X) + (tempAngle.Y * tempAngle.Y));
        //        //float angle = (float)(Math.Acos(tempAngle.Y / length));

        //        //// this accounts for the angle being to the left of "X = 0"
        //        //if (tempAngle.X < 0.0f)
        //        //    angle = (MathHelper.Pi * 2.0f) - angle;

        //        //// converts angle to degrees
        //        //angle = MathHelper.ToDegrees(angle);


        //        //if (angle < 0)
        //        //    angle += 360;

        //        //if (tempAngle.Length() > movementSpeed)//move toward target at speed
        //        //{
        //        //    Vector2 directionVector;
        //        //    if (angle < 0)
        //        //        angle += 360;
        //        //    if (angle < 0)
        //        //        angle += 360;
        //        //    if (angle < 35.75 || angle > 324.25)
        //        //    {
        //        //        angle = 0f;//direction = new Vector2(0, 1);
        //        //        facing = direction.south;
        //        //    }
        //        //    else if (angle < 80.75)
        //        //    {
        //        //        angle = 71.5f;//direction = new Vector2(1, 1);
        //        //        facing = direction.southEast;
        //        //    }
        //        //    else if (angle < 99.25)
        //        //    {
        //        //        angle = 90f;//direction = new Vector2(1, 0);
        //        //        facing = direction.east;
        //        //    }
        //        //    else if (angle < 144.25)
        //        //    {
        //        //        angle = 108.5f;//direction = new Vector2(1, -1);
        //        //        facing = direction.northEast;
        //        //    }
        //        //    else if (angle < 160.75)
        //        //    {
        //        //        angle = 180f;//direction = new Vector2(0, -1);
        //        //        facing = direction.north;
        //        //    }
        //        //    else if (angle < 215.75)
        //        //    {
        //        //        angle = 151.5f;//direction = new Vector2(-1, -1);
        //        //        facing = direction.northWest;
        //        //    }
        //        //    else if (angle < 279.25)
        //        //    {
        //        //        angle = 270f;//direction = new Vector2(-1, 0);
        //        //        facing = direction.west;
        //        //    }
        //        //    else
        //        //    {
        //        //        angle = 288.5f;//direction = new Vector2(-1, -1);
        //        //        facing = direction.southWest;
        //        //    }


        //        //    directionVector = new Vector2((float)Math.Sin((double)angle), (float)Math.Cos((double)angle));
        //        //    //normalize the length to the of the direction the unit is moving
        //        //    directionVector.Normalize();
        //        //    //multiply by the units speed
        //        //    directionVector = directionVector * movementSpeed;
        //        //    //move the unit by that amount
        //        //    pos += directionVector;
        //        //}
        //        //else
        //        //{
        //        //    pos = targetPos;//pop to target
        //        //}

        //        ////// working //





        //        // old code

        //        Vector2 temp = targetPos - pos;
        //        if (temp.Length() > movementSpeed)//move toward target at speed
        //        {
        //            //normalize the length to the of the direction the unit is moving
        //            temp.Normalize();
        //            //multiply by the units speed
        //            temp = temp * movementSpeed;
        //            //move the unit by that amount
        //            pos += temp;

        //            //Update the direction it faces
        //            //TODO @michiel
        //            if (temp.X > 0)
        //            {
        //                if (temp.Y > 0)
        //                {
        //                    facing = direction.southEast;
        //                }
        //                else if (temp.Y < 0)
        //                {
        //                    facing = direction.northEast;
        //                }
        //                else
        //                {
        //                    facing = direction.east;
        //                }
        //            }
        //            else if (temp.X < 0)
        //            {
        //                if (temp.Y > 0)
        //                {
        //                    facing = direction.southWest;
        //                }
        //                else if (temp.Y < 0)
        //                {
        //                    facing = direction.northWest;
        //                }
        //                else
        //                {
        //                    facing = direction.west;
        //                }
        //            }
        //            else
        //            {
        //                if (temp.Y > 0)
        //                {
        //                    facing = direction.south;
        //                }
        //                else if (temp.Y < 0)
        //                {
        //                    facing = direction.north;
        //                }
        //            }

        //        }
        //        else
        //        {
        //            pos = targetPos;//pop to target
        //        }

        //        // old code
        //    }
        //}

        public static void zoomIn()
        {
            if (scale <= 25)
            {
                scale += 1;

                baseHalfWidth = baseHalfWidthConstant * scale;
                baseHalfHeight = baseHalfHeightConstant * scale;
            }
        }

        public static void zoomOut()
        {
            if (scale >= 8)
            {
                scale -= 1;
                baseHalfWidth = baseHalfWidthConstant * scale;
                baseHalfHeight = baseHalfHeightConstant * scale;
            }
        }

        public float GetScale()
        {
            return baseHalfWidthConstant;
        }

        public void SetTarget(Vector2 newTarget)
        {
            targetPos = newTarget;
        }

        public Vector2 GetVirtualPos()
        {
            return virtualPos;
        }
        // new

        public Vector2 GetTile()
        {
            return new Vector2(inTileX, inTileY);
        }

        //Inherited from IDepthEnabled
        public int getTileX()
        {
            return inTileX;
        }

        public int getTileY()
        {
            return inTileY;
        }

        public void drawDepthEnabled(float translationX, float translationY)
        {
            draw(translationX, translationY);
        }

        public bool hitTest(int x, int y)
        {
            if(selectionRectangle.Contains(x,y))
            {
                return true;
            }
            return false;
        }

        public bool hitTest(Rectangle r)
        {
            if (selectionRectangle.Intersects(r))
            {
                return true;
            }
            return false;
        }

        public int getOwner()
        {
            return owner;
        }

        // Queue Stuff
        // this is for waypoints (shift-click)
        public void AddDestination(Tile newDest)
        {
            destination.Enqueue(newDest);
        }

        public void EmptyWayPoints()
        {
            destination = new Queue<Tile>();
            failed = new List<Tile>();
        }
        // Queue Stuff

        public void DrawWayPoints(float translationX, float translationY)
        {
            //When selected and owned by the player
            if (selected && owner == Grid.playerNumber)
            {
                int size = destination.Count;
                Tile[] asArray = destination.ToArray();
                
                for (int i = 0; i < size; i++)
                {
                    ION.spriteBatch.Draw(Images.unitWayPoint, asArray[i].drawingRectangle, Microsoft.Xna.Framework.Graphics.Color.White);
                }

                //TODO 
                if (pos != targetPos)// - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (translationX)
                {// * (scale / 15.0f)) + (translationY) + (scale * 0.5f))
                    //ION.spriteBatch.Draw(Images.unitWayPoint, asArray[i].drawingRectangle, Microsoft.Xna.Framework.Graphics.Color.White);
                }
            }
        }

        public void Die()
        {
            Grid.map[inTileX, inTileY].accessable = true;
            //remove it from the grid
            Grid.get().removeUnit(this);
        }
    }
}
