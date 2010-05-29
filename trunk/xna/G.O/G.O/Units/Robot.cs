using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using ION.Tools;

namespace ION
{
    public class Robot : Unit
    {
        public const int cost = 250;

        public const int maxHealth = 100; 

        public Rectangle healtRectangle = new Rectangle();

        private static Random damageRandom;

        //fire animation helper variables
        private int FiringFrame = 0;
        private int FiringCounter = 0;

        //under-fire animation helper variables
        private int UnderFireFrame = 0;
        private int UnderFireCounter = 0;
        public int UnderFireOffsetX = 0;
        public int UnderFireOffsetY = 0;
 
        public Robot() : base(-1,-1) //Sending an invalid number to the base class as a test, I think this constructor in only used to deserialize into after
        {
            health = maxHealth;

            damage = 10;
            damageType = Damage.TYPE_SMALL_GUN;
            
            destination = new Queue<Tile>();

            pos = new Vector2(ION.halfWidth - (scale / 2), -(scale / 4));
            targetPos = new Vector2(500, 500);

            movementSpeed = 2f;
        }

        public Robot(Vector2 newPos, Vector2 newTarget, int owner, int id) : base(owner,id)
        {
            health = maxHealth;
            
            pos = newPos;
            targetPos = newTarget;

            BaseTile playerBase = Grid.getPlayerBase(owner);
            inTileX = playerBase.getTileX();
            inTileY = playerBase.getTileY();

            movementSpeed = 2f;
        }

        //move units towards their target
        public override void move()
        {
            //if not at target
            if (pos != targetPos)
            {

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

        public override void draw(float x, float y)
        {       
            selectionRectangle.X = (int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x) + (Tile.baseHalfWidth * 0.63));
            selectionRectangle.Y =   (int)(((pos.Y) * (scale/ 15.0f)) + (y) + (baseHalfHeight * 2)+(baseHalfHeight*0.55));
            selectionRectangle.Width = (int)(baseHalfWidth * 0.75);
            selectionRectangle.Height = (int)(baseHalfHeight * 3);

            drawingRectangle.X = (int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x));
            drawingRectangle.Y = (int)(((pos.Y) * (scale / 15.0f)) + (y) + (baseHalfHeight * 2));
            drawingRectangle.Width = (int)(baseHalfWidth * 2);
            drawingRectangle.Height = (int)(baseHalfHeight * 4);

            //ION.spriteBatch.Draw(Images.white1px, selectionRectangle, Color.Gray);

            if (selected)
            {
                ION.spriteBatch.Draw(Images.selectionBoxBack, drawingRectangle, Color.White);
            }

            if (!drawFiringAnimation(x, y))
            {
                ION.spriteBatch.Draw(Images.getUnitImage(owner, (int)facing, true), drawingRectangle, Color.White);
            }

            if (selected)
            {
                //Draw health and energy stuff
                healtRectangle.X = (int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x) 
                    + (Tile.baseHalfWidth * 0.56));
                healtRectangle.Y = (int)(((pos.Y) * (scale / 15.0f)) + (y) + (baseHalfHeight * 2) 
                    + (baseHalfHeight * 0.1));
                healtRectangle.Width = (int)(baseHalfWidth * 0.9);
                healtRectangle.Height = (int)(baseHalfHeight * 0.38);

                healtRectangle.Width = (int)(healtRectangle.Width * ((float)health / (float)maxHealth));

                //Get a good color for the healthbar
                Color healthBarColor;
                if (health > (maxHealth / 1.5))
                {
                    healthBarColor = Color.Green;
                }
                else if(health > (maxHealth / 3)) 
                {
                    healthBarColor = Color.Orange;
                }
                else 
                {
                    healthBarColor = Color.Red;
                }
                
                ION.spriteBatch.Draw(Images.selectionBoxFront, drawingRectangle, Color.White);
                if (health < maxHealth)
                {
                    ION.spriteBatch.Draw(Images.unitHealth[2], drawingRectangle, healthBarColor);
                }
                else
                {
                    ION.spriteBatch.Draw(Images.unitHealth[1], drawingRectangle, healthBarColor);
                }

                ION.spriteBatch.Draw(Images.white1px, healtRectangle, healthBarColor);

                ION.spriteBatch.Draw(Images.unitHealth[0], drawingRectangle, Color.White);
            }
   
            drawUnderFireAnimation(x,y);
        }

        private void drawUnderFireAnimation(float x, float y)
        {
            if (underFire)
            {
                if (UnderFireCounter == 0)
                {
                    UnderFireOffsetX = (int)(Tool.unsafeRandom.NextDouble() * selectionRectangle.Width);
                    UnderFireOffsetY = (int)(Tool.unsafeRandom.NextDouble() * selectionRectangle.Height);               
                }
                
                //animation test code
                UnderFireCounter++;

                if (UnderFireCounter > 0)
                {
                    UnderFireFrame = 0;
                }
                if (UnderFireCounter > 5)
                {
                    UnderFireFrame = 1;            
                }
                if (UnderFireCounter > 10)
                {
                    UnderFireCounter = 0;
                    underFire = false;
                    return;
                }

                ION.spriteBatch.Draw(Images.bulletImpact[UnderFireFrame], new Rectangle(selectionRectangle.X + UnderFireOffsetX, selectionRectangle.Y + UnderFireOffsetY, (int)(baseHalfWidth * 0.5), (int)(baseHalfHeight)), Color.White);
            }
        }

        private bool drawFiringAnimation(float x, float y)
        {
            if (firing)
            {
                //animation test code
                FiringCounter++;

                if (FiringCounter > 0)
                {
                    FiringFrame = 0;
                }
                if (FiringCounter > 3)
                {
                    FiringFrame = 1;
                }
                if (FiringCounter > 6)
                {
                    FiringFrame = 2;
                }
                if (FiringCounter > 9)
                {
                    FiringCounter = 0;
                }

                if (FiringFrame < 2)
                {
                    //do not remove
                    //ION.spriteBatch.Draw(Images.getUnitImage(owner, (int)facing, selected), new Rectangle((int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x)), (int)(((pos.Y) * (scale / 15.0f)) + (y) + (baseHalfHeight * 2)), (int)(baseHalfWidth * 2), (int)(baseHalfHeight * 4)), Color.White);

                    ION.spriteBatch.Draw(Images.unit_selected_shooting[owner - 1, (int)facing, FiringFrame], new Rectangle((int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x)), (int)(((pos.Y) * (scale / 15.0f)) + (y) + (baseHalfHeight * 2)), (int)(baseHalfWidth * 2), (int)(baseHalfHeight * 4)), Color.White);
                }
                else if (FiringFrame >= 2)
                {
                    //do not remove
                    ION.spriteBatch.Draw(Images.getUnitImage(owner, (int)facing, true), new Rectangle((int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x)), (int)(((pos.Y) * (scale / 15.0f)) + (y) + (baseHalfHeight * 2)), (int)(baseHalfWidth * 2), (int)(baseHalfHeight * 4)), Color.White);

                    //ION.spriteBatch.Draw(Images.unit_selected_shooting[owner - 1, (int)facing, FiringFrame], new Rectangle((int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x)), (int)(((pos.Y) * (scale / 15.0f)) + (y) + (baseHalfHeight * 2)), (int)(baseHalfWidth * 2), (int)(baseHalfHeight * 4)), Color.White);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        internal void Deserialize(System.IO.MemoryStream memoryStream)
        {
            throw new NotImplementedException();
        }
    }
}
