using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using ION.Tools;
using ION.MultiPlayer;

namespace ION
{
    public abstract class Unit : IDepthEnabled
    {

        private Colors hitmapColor = new Colors();

        //The player that owns this unit
        public int owner;
        
        //Rectangle used to draw the unit's graphic in
        public Rectangle drawingRectangle = new Rectangle();

        public IDepthEnabled firingTarget;
        public IDepthEnabled attackTarget;
        public Rectangle selectionRectangle = new Rectangle();
        public Vector2 focalPoint = new Vector2();



        public enum direction { south = 0, southEast = 1, east = 2, northEast = 3, north = 4, northWest = 5, west = 6, southWest = 7 };
        public direction facing = direction.north;

        //don't initialize these, subclasses of unit must specify these in the constructor (or base())
        public int id;
        public int health;
        //public int damage;
  
        public bool moving = false;

        //protected Vector2 pos, targetPos;//, virtualPos;

        public Tile position;
        public Tile targetPosition;

 
        //the index of the tile the unit is on
        public int inTileX = 0;
        public int inTileY = 0;

        public abstract void draw(float x, float y);

        public bool firing = false;
        public bool underFire = false;
        public bool dying = false;

        protected static float scale = 15; //TODO this should be externalized

        public static float baseHalfWidth = baseHalfWidthConstant * scale;
        public static float baseHalfHeight = baseHalfHeightConstant * scale;

        private const float baseHalfWidthConstant = 3;
        private const float baseHalfHeightConstant = 1;

        public int scan = 0;

        public bool selected = false;
        public bool showDetails = false; //used to show only details such as health, but not mark this unit selected.

        public Queue<Tile> destination;

        public Unit(int owner,int id)
        {
            this.owner = owner;
            this.id = id;
            destination = new Queue<Tile>();
        }

        public abstract void Update(float translationX, float translationY);

        protected void face(Vector2 facePos)
        {
            //get the center of the selectionbox of the unit.
            //compare it with the center of this unit's selection box
            //work out the number of degrees
            //select the right facing 

            //        //// working //
            //        ////TODO @emmet
            //        ////1.	Units only move in 8 directions
            //        //// this code gets the angle of the vector in radians (note: this angle is either left or right of "X = 0")
            
            Vector2 tempAngle = facePos - focalPoint;
            double length = Math.Sqrt((tempAngle.X * tempAngle.X) + (tempAngle.Y * tempAngle.Y));
            float angle = (float)(Math.Acos(tempAngle.Y / length));

            // this accounts for the angle being to the left of "X = 0"
            if (tempAngle.X < 0.0f)
                angle = (MathHelper.Pi * 2.0f) - angle;

            // converts angle to degrees
            angle = MathHelper.ToDegrees(angle);

            if (angle < 0)
            {
                angle += 360;
            }     

    
                if (angle < 0)
                    angle += 360;
                //if (angle > 360)
                //{
                //    Debug.WriteLine("Unit.face() : to big an angle");
                //}
                if (angle < 35.75 || angle > 324.25)
                {
                    facing = direction.south;
                }
                else if (angle < 80.75)
                {
                    facing = direction.southEast;
                }
                else if (angle < 99.25)
                {
                    facing = direction.east;
                }
                else if (angle < 144.25)
                {
                    facing = direction.northEast;
                }
                else if (angle < 160.75)
                {
                    facing = direction.north;
                }
                else if (angle < 215.75)
                {
                    facing = direction.northWest;
                }
                else if (angle < 279.25)
                {
                    facing = direction.west;
                }
                else
                {
                    facing = direction.southWest;
                }
     
        }

        public void hit(int damageTaken, int damageType)
        {
            
            health -= damageTaken;

            //this will trigger or keep alive the animation
            underFire = true;
        }

        public abstract void move();

        public virtual void setFiring()
        {
            firing = true;
        }

        public void setAttackTarget(IDepthEnabled attackTarget)
        {
            this.attackTarget = attackTarget;
        }

        public static void zoomIn()
        {
            if (scale <= 25)
            {
                scale += 1;

                baseHalfWidth = baseHalfWidthConstant * scale;
                baseHalfHeight = baseHalfHeightConstant * scale;

                calcMovement();
            }
        }

        public static void zoomOut()
        {
            if (scale >= 8)
            {
                scale -= 1;
                baseHalfWidth = baseHalfWidthConstant * scale;
                baseHalfHeight = baseHalfHeightConstant * scale;

                calcMovement();
            }
        }

        private static void calcMovement() 
        {

            foreach (Unit u in Grid.get().allUnits)
            {
                if (u is Robot && u.moving)
                {
                    Robot r = (Robot)u;
                    
                    r.movement.X = r.targetPosition.drawingRectangle.X - r.position.drawingRectangle.X;
                    r.movement.Y = r.targetPosition.drawingRectangle.Y - r.position.drawingRectangle.Y;

                    r.movement.X /= Robot.tileToTileTicks;
                    r.movement.Y /= Robot.tileToTileTicks;
                }
            }
        }

        public float GetScale()
        {
            return baseHalfWidthConstant;
        }

        //public void SetTarget(Vector2 newTarget)
        //{
        //    targetPos = newTarget;
        //}

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
        // this is for waypoints
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
                int size = destination.Count;
                Tile[] asArray = destination.ToArray();
                
                for (int i = 0; i < size; i++)
                {
                    ION.spriteBatch.Draw(Images.unitWayPoint, asArray[i].drawingRectangle, Microsoft.Xna.Framework.Graphics.Color.White);
                }


                if (moving && position != targetPosition)
                {
                    ION.spriteBatch.Draw(Images.unitWayPoint, targetPosition.drawingRectangle, Microsoft.Xna.Framework.Graphics.Color.White);
                }

            }
        }

        public void Die()
        {
            //open the unit's tile for traffic
            Grid.map[inTileX, inTileY].accessable = true;

            //Remove it from gameplay
            Grid.get().removeUnit(this);

            SoundManager.explosionSound();

            //Start the dying sequence
            dying = true;
        }

        public virtual void stop()
        {
            //do stopping things like dumping waypoints etc. 
            EmptyWayPoints();
        }

        public void displayDetails()
        {
            showDetails = true;
        }
    }
}
