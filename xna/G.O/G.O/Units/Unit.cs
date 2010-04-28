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

        protected int health = 100;
        //protected int tileX;
        //protected int tileY;
        protected Vector2 pos, targetPos, virtualPos;//replaced two int values with a 2d vector


        public int inTileX = 100;
        public int inTileY = 100;

        public abstract void draw(float x, float y);



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

        public bool selected = false;

        public Queue<Tile> destination;

        public Unit()
        {
            destination = new Queue<Tile>();
        }

        public void Update(float translationX, float translationY)
        {
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
                // Code for waypoints
            }

            //will move this to the above if statement when the unit know its tile
            virtualPos.X = ((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (translationX) + (baseHalfWidth * 1.1f);
            virtualPos.Y = ((pos.Y) * (scale / 15.0f)) + (translationY) + (baseHalfWidth * 1.6f);


        }

        public void UpdateTile(Vector2 newInTile)
        {
            inTileX = (int)newInTile.X;
            inTileY = (int)newInTile.Y;

            //Tell the Grid this Unit's tile position has changed
            Grid.updateDepthEnabledItem(this);

            //Debug.WriteLine("new tilexy: " + inTileX + "," + inTileY);
            //Vector2 tilePos = map.GetTile(pos.X, pos.Y, translationX, translationX);
            //inTileX = tilePos.X;
            //inTileY = tilePos.Y;
        }

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
                Vector2 temp = targetPos - pos;
                if (temp.Length() > movementSpeed)//move toward target at speed
                {
                    //normalize the length to the of the direction the unit is moving
                    temp.Normalize();
                    //multiply by the units speed
                    temp = temp * movementSpeed;
                    //move the unit by that amount
                    pos += temp;
                }
                else
                {
                    pos = targetPos;//pop to target
                }
            }
        }

        public static void zoomIn()
        {
            if (scale <= 80)
            {
                scale += 1;

                baseHalfWidth = baseHalfWidthConstant * scale;
                baseHalfHeight = baseHalfHeightConstant * scale;
            }


        }

        public static void zoomOut()
        {
            if (scale >= 5)
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
            if (selected)
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
