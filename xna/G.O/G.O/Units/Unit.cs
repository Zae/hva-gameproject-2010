using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace ION
{
    public abstract class Unit : IDepthEnabled
    {
        private Colors hitmapColor = new Colors();

        public int owner;

        public enum direction { south = 0, southEast = 1, east = 2, northEast = 3, north = 4, northWest = 5, west = 6, southWest = 7 };
        public direction facing = direction.north;

        public int id;
        public int health;
        public static int cost;

        protected Vector2 pos, targetPos, virtualPos;//replaced two int values with a 2d vector

        //TODO magic numbers?
        public int inTileX = 0;
        public int inTileY = 0;

        public abstract void draw(float x, float y);

        public bool firing = false;

        // new
        protected float movementSpeed;
        protected float captureSpeed;

        protected static float scale = 15;

        public static float baseHalfWidth = baseHalfWidthConstant * scale;
        public static float baseHalfHeight = baseHalfHeightConstant * scale;

        private const float baseHalfWidthConstant = 3;
        private const float baseHalfHeightConstant = 1;

        protected float visualZ;
        protected float visualX;
        protected float visualY;

        public int scan = 0;

        public bool selected = false;

        public Queue<Tile> destination;

        public Unit(int owner,int id)
        {
            this.owner = owner;
            this.id = id;
            destination = new Queue<Tile>();
        }

        public void Update(float translationX, float translationY)
        {
            if (health < 0)
            {
                //kill this unit
            }
            
            if (pos != targetPos)
            {
                move();
                //map.selectOnMap(mouseState.X, mouseState.Y, translationX, translationY);
            }
            else
            {
                // Code for waypoints
                if (destination.Count() != 0)//(destination.Last<Tile>() != null)//here
                {
                    
                    //targetPos = new Vector2(destination.Last<Tile>().indexX, destination.Last<Tile>().indexY);

                    Tile temp = destination.Dequeue();

                    targetPos = temp.GetPos(translationX, translationY);
                }
                else if(scan > 50)
                {
                    ////We have nowhere to go, might as well shoot some enemies
                    //List<Unit> enemies = Grid.get().getPlayerEnemies(Grid.playerNumber);
                    //int distance = 3;
                    //foreach (Unit u in enemies)
                    //{
                    //    if (u.inTileX - inTileX > -distance && u.inTileX - inTileX < distance && u.inTileY - inTileY > -3 && u.inTileY - inTileY < 3)
                    //    {
                    //        firing = true;
                    //        //fire on this unit.
                    //        u.hit();
                    //        break;
                    //    }
                    //    firing = false;
                    //}
                    //scan = 0;
                }
                scan++;
                // Code for waypoints
            }

            //will move this to the above if statement when the unit know its tile
            //virtualPos.X = ((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (translationX) + (baseHalfWidth * 1.1f);
            //virtualPos.Y = ((pos.Y) * (scale / 15.0f)) + (translationY) + (baseHalfWidth * 1.6f);


        }

        public void hit()
        {
            health -= 1;
        }


        public bool UpdateTile(Vector2 newInTile)
        {
            //if the player is occuping a new tile it will update & return true
            if (inTileX != (int)newInTile.X && inTileY != (int)newInTile.Y)
            {
                inTileX = (int)newInTile.X;
                inTileY = (int)newInTile.Y;

                //Tell the Grid this Unit's tile position has changed
                Grid.updateDepthEnabledItem(this);

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


        ////PathFinding
        //public void FindPath(Tile[,] map, List<Unit> allUnits)
        //{
        //    //1) Add the starting square (or node) to the open list.
        //    //2) Repeat the following:
        //    //a) Look for the lowest F cost square on the open list. We refer to this as the current square.
        //    //b) Switch it to the closed list.
        //    //c) For each of the 8 squares adjacent to this current square …
        //    //    *
        //    //      If it is not walkable or if it is on the closed list, ignore it. Otherwise do the following.           
        //    //    *
        //    //      If it isn’t on the open list, add it to the open list. Make the current square the parent of this square. Record the F, G, and H costs of the square. 
        //    //    *
        //    //      If it is on the open list already, check to see if this path to that square is better, using G cost as the measure. A lower G cost means that this is a better path. If so, change the parent of the square to the current square, and recalculate the G and F scores of the square. If you are keeping your open list sorted by F score, you may need to resort the list to account for the change.
        //    //d) Stop when you:
        //    //    * Add the target square to the closed list, in which case the path has been found (see note below), or
        //    //    * Fail to find the target square, and the open list is empty. In this case, there is no path.   
        //    //3) Save the path. Working backwards from the target square, go from each square to its parent square until you reach the starting square. That is your path.

        //    float f = 0.0f;
        //    //create the open list of nodes, initially containing only our starting node
        //    List<Tile> openNodes = new List<Tile>();
        //    List<float> openNodesF = new List<float>();
        //    openNodes.Add((Tile)map[inTileX, inTileY]);
        //    openNodesF.Add(0f);

        //    //    create the closed list of nodes, initially empty
        //    List<Tile> closedNodes = new List<Tile>();
        //    List<float> closedNodesG = new List<float>();
        //    float g = 0.0f;

        //    //    while (we have not reached our goal) {
        //    while ((closedNodes.Count() != 0 && closedNodes[closedNodes.Count() - 1] != destination.First<Tile>()) || closedNodes.Count() == 0)//while closedNodes does not contain the target tile
        //    {
        //        //        consider the best node in the open list (the node with the lowest f value)
        //        // NOTE: positive elements in the array have a distance of 10, negitave elements have a distance of 14
        //        Tile currentTile = openNodes[openNodes.Count() - 1];// the current tile
        //        Tile[] checkTiles = new Tile[8];// the 8 tiles touching the current
        //        if (map[inTileX - 1, inTileY] != null)
        //        {
        //            checkTiles[0] = map[inTileX - 1, inTileY];
        //        }
        //        if (map[inTileX - 1, inTileY - 1] != null)
        //        {
        //            checkTiles[1] = map[inTileX - 1, inTileY - 1];
        //        }
        //        if (map[inTileX, inTileY - 1] != null)
        //        {
        //            checkTiles[2] = map[inTileX, inTileY - 1];
        //        }
        //        if (map[inTileX + 1, inTileY - 1] != null)
        //        {
        //            checkTiles[3] = map[inTileX + 1, inTileY - 1];
        //        }
        //        if (map[inTileX + 1, inTileY] != null)
        //        {
        //            checkTiles[4] = map[inTileX + 1, inTileY];
        //        }
        //        if (map[inTileX + 1, inTileY + 1] != null)
        //        {
        //            checkTiles[5] = map[inTileX + 1, inTileY + 1];
        //        }
        //        if (map[inTileX, inTileY + 1] != null)
        //        {
        //            checkTiles[6] = map[inTileX, inTileY + 1];
        //        }
        //        if (map[inTileX - 1, inTileY + 1] != null)
        //        {
        //            checkTiles[7] = map[inTileX - 1, inTileY + 1];
        //        }

        //        int closestTile = 0;// specifys the closest tile to touching the current tile to the destination tile
        //        float closest = 1000000000000000f;// initalized as huge so that any distance will be smaller
        //        for (int i = 0; i < 8; i++)// checks which tile is closest to the target
        //        {
        //            if (checkTiles[i] != null && map[checkTiles[i].indexX, checkTiles[i].indexY].FreeTile(checkTiles[i], allUnits))
        //            {
        //                Vector2 temp = checkTiles[i].GetPos(1, 1) - currentTile.GetPos(1, 1);
        //                if ((float)temp.Length() < closest)
        //                {
        //                    closest = (float)temp.Length();
        //                    closestTile = i;
        //                }
        //            }
        //        }



        //        //        if (this node is the goal) {
        //        if (false/*temp*/ && checkTiles[closestTile] == destination.First<Tile>())//here
        //        {
        //            //            then we're done
        //        }
        //        else
        //        {
        //            //            move the current node to the closed list and consider all of its neighbors
        //            closedNodes.Add(checkTiles[closestTile]);
        //            if (closestTile % 2 == 0)
        //            {
        //                closedNodesG.Add(10f); g += 10f;
        //            }
        //            else
        //            {
        //                closedNodesG.Add(14f); g += 14f;
        //            }
        //            //            for (each neighbor) {
        //            for (int i = 0; i < 8; i++)
        //            {
        //                bool ifCheck = false;
        //                for (int j = 0; j < closedNodes.Count(); j++)
        //                {

        //                    //                if (this neighbor is in the closed list and our current g value is lower) {
        //                    if (g < g + 10)
        //                    {
        //                        ifCheck = true;
        //                        //                    update the neighbor with the new, lower, g value 
        //                        // checkTiles[i].gValue or checkTiles[i,1] where 1 is the gValue and 0 is the tile
        //                        //                    change the neighbor's parent to our current node
        //                    }
        //                }
        //                for (int j = 0; j < openNodes.Count(); j++)
        //                {
        //                    //                else if (this neighbor is in the open list and our current g value is lower) {
        //                    if (!ifCheck && checkTiles[i] == openNodes[j])
        //                    {
        //                        ifCheck = true;
        //                        //                    update the neighbor with the new, lower, g value 
        //                        //                    change the neighbor's parent to our current node
        //                    }

        //                }
        //                //                else this neighbor is not in either the open or closed list {
        //                if (!ifCheck)
        //                {
        //                    //                    add the neighbor to the open list and set its g value

        //                }
        //            }
        //        }
        //    }


        //}


        //move units towards their target
        void move()
        {
            //Rules for moving units
            //1.	Units only move in 8 directions
            //2.	Units will form in a circle around the selected way point
            //3.	If there is an obstacle in the way, plot a course around it
            //4.	If there is a unit in the way, don’t perform move this turn
            //5.	If the unit has no waypoints, treat as obstacle
            //6.	If there is are obstacle stopping you from getting your waypoint or on your waypoint, go to next waypoint  if there is one,  otherwise stop
            //7.	If there is a unit in the way, and it is trying to move into your tile, treat it as an obstacle

            //if not at target
            if (pos != targetPos)
            {


                //// working //
                ////TODO @emmet
                ////1.	Units only move in 8 directions
                //// this code gets the angle of the vector in radians (note: this angle is either left or right of "X = 0")
                //Vector2 tempAngle = targetPos - pos;
                //double length = Math.Sqrt((tempAngle.X * tempAngle.X) + (tempAngle.Y * tempAngle.Y));
                //float angle = (float)(Math.Acos(tempAngle.Y / length));

                //// this accounts for the angle being to the left of "X = 0"
                //if (tempAngle.X < 0.0f)
                //    angle = (MathHelper.Pi * 2.0f) - angle;

                //// converts angle to degrees
                //angle = MathHelper.ToDegrees(angle);


                //if (angle < 0)
                //    angle += 360;

                //if (tempAngle.Length() > movementSpeed)//move toward target at speed
                //{
                //    Vector2 directionVector;
                //    if (angle < 0)
                //        angle += 360;
                //    if (angle < 0)
                //        angle += 360;
                //    if (angle < 35.75 || angle > 324.25)
                //    {
                //        angle = 0f;//direction = new Vector2(0, 1);
                //        facing = direction.south;
                //    }
                //    else if (angle < 80.75)
                //    {
                //        angle = 71.5f;//direction = new Vector2(1, 1);
                //        facing = direction.southEast;
                //    }
                //    else if (angle < 99.25)
                //    {
                //        angle = 90f;//direction = new Vector2(1, 0);
                //        facing = direction.east;
                //    }
                //    else if (angle < 144.25)
                //    {
                //        angle = 108.5f;//direction = new Vector2(1, -1);
                //        facing = direction.northEast;
                //    }
                //    else if (angle < 160.75)
                //    {
                //        angle = 180f;//direction = new Vector2(0, -1);
                //        facing = direction.north;
                //    }
                //    else if (angle < 215.75)
                //    {
                //        angle = 151.5f;//direction = new Vector2(-1, -1);
                //        facing = direction.northWest;
                //    }
                //    else if (angle < 279.25)
                //    {
                //        angle = 270f;//direction = new Vector2(-1, 0);
                //        facing = direction.west;
                //    }
                //    else
                //    {
                //        angle = 288.5f;//direction = new Vector2(-1, -1);
                //        facing = direction.southWest;
                //    }


                //    directionVector = new Vector2((float)Math.Sin((double)angle), (float)Math.Cos((double)angle));
                //    //normalize the length to the of the direction the unit is moving
                //    directionVector.Normalize();
                //    //multiply by the units speed
                //    directionVector = directionVector * movementSpeed;
                //    //move the unit by that amount
                //    pos += directionVector;
                //}
                //else
                //{
                //    pos = targetPos;//pop to target
                //}

                ////// working //





                // old code

                Vector2 temp = targetPos - pos;
                if (temp.Length() > movementSpeed)//move toward target at speed
                {
                    //normalize the length to the of the direction the unit is moving
                    temp.Normalize();
                    //multiply by the units speed
                    temp = temp * movementSpeed;
                    //move the unit by that amount
                    pos += temp;

                    //Update the direction it faces
                    //TODO @michiel
                    if (temp.X > 0)
                    {
                        if (temp.Y > 0)
                        {
                            facing = direction.southEast;
                        }
                        else if (temp.Y < 0)
                        {
                            facing = direction.northEast;
                        }
                        else
                        {
                            facing = direction.east;
                        }
                    }
                    else if (temp.X < 0)
                    {
                        if (temp.Y > 0)
                        {
                            facing = direction.southWest;
                        }
                        else if (temp.Y < 0)
                        {
                            facing = direction.northWest;
                        }
                        else
                        {
                            facing = direction.west;
                        }
                    }
                    else
                    {
                        if (temp.Y > 0)
                        {
                            facing = direction.south;
                        }
                        else if (temp.Y < 0)
                        {
                            facing = direction.north;
                        }
                    }

                }
                else
                {
                    pos = targetPos;//pop to target
                }

                // old code
            }
        }

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

        // Queue Stuff
        // this is for waypoints (shift-click)
        public void AddDestination(Tile newDest)
        {
            destination.Enqueue(newDest);
        }

        public void EmptyWayPoints()
        {
            destination = new Queue<Tile>();
        }
        // Queue Stuff

        public void DrawWayPoints(float translationX, float translationY)
        {
            //When selected and owned by the player
            if (selected && owner == Grid.playerNumber)
            {
                for (int i = 0; i < destination.Count; i++)
                {
                    ION.spriteBatch.Draw(Images.unitWayPoint, new Rectangle((int)(((destination.ToArray()[i].GetPos(translationX, translationY).X + 30 - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (translationX)), (int)(((destination.ToArray()[i].GetPos(translationX, translationY).Y + 35) * (scale / 15.0f)) + (translationY) + (scale * 0.5f)), (int)(30 * (scale / 15.0f)), (int)(30 * (scale / 15.0f))), Microsoft.Xna.Framework.Graphics.Color.White);
                }
                if (pos != targetPos)// - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (translationX)
                {// * (scale / 15.0f)) + (translationY) + (scale * 0.5f))
                    ION.spriteBatch.Draw(Images.unitWayPoint, new Rectangle((int)(((targetPos.X + 30 - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (translationX)), (int)(((targetPos.Y + 35) * (scale / 15.0f)) + (translationY) + (scale * 0.5f)), (int)(30 * (scale / 15.0f)), (int)(30 * (scale / 15.0f))), Microsoft.Xna.Framework.Graphics.Color.White);
                }
            }
        }
    }
}
