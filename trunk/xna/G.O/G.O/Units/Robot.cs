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

        //fire animation helper variables
        private int FiringFrame = 0;
        private int FiringCounter = 0;

        //under-fire animation helper variables
        private int UnderFireFrame = 0;
        private int UnderFireCounter = 0;
        public int UnderFireOffsetX = 0;
        public int UnderFireOffsetY = 0;

        public int deathFrame = 0;
        public int deathCounter = 0;

        public Vector2 movement;

        public Robot(Vector2 newPos, int owner, int id) : base(owner,id)
        {
            health = maxHealth;
            
            pos = newPos;
            targetPos = new Vector2(pos.X,pos.Y);

            damage = 4;

            BaseTile playerBase = Grid.getPlayerBase(owner);
            inTileX = playerBase.getTileX();
            inTileY = playerBase.getTileY();
        }

        //move units towards their target
        public override void move()
        {
            if (!moving)
            {
                moving = true;

                movement = targetPos - pos;
                //movement.Normalize();
                movement.X /= tileToTileTicks;
                movement.Y /= tileToTileTicks;
                //Debug.WriteLine("START MOVE with speed: " + movement);


            }
    
                            
        }

        public override void Update(float translationX, float translationY)
        {
            //Debug.WriteLine("ROBOT UPDATE!");
            base.Update(translationX, translationY);

            
            if (moving)
            {
                if (ticksIntoMovement > tileToTileTicks)
                {
                    pos = targetPos;
                    ticksIntoMovement = 0;
                    moving = false;
                }

                fixHeading();

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

            if (moving)
            {
                Debug.WriteLine("ticksIntoMovement: "+ticksIntoMovement);
                Debug.WriteLine("movement.X: " + movement.X);
                Debug.WriteLine("mult: " + (ticksIntoMovement*movement.X));
                
                
                selectionRectangle.X += (int)(ticksIntoMovement * movement.X) + (int)(Grid.get().intermediate * movement.X);
                selectionRectangle.Y += (int)(ticksIntoMovement * movement.Y) + (int)(Grid.get().intermediate * movement.Y);

                drawingRectangle.X += (int)(ticksIntoMovement * movement.X) + (int)(Grid.get().intermediate * movement.X);
                drawingRectangle.Y += (int)(ticksIntoMovement * movement.Y) + (int)(Grid.get().intermediate * movement.Y);
            }

            focalPoint.X = selectionRectangle.Center.X;
            focalPoint.Y = selectionRectangle.Center.Y;

            //ION.spriteBatch.Draw(Images.white1px, selectionRectangle, Color.Gray);
            if (dying)
            {     
                drawDeathAnimation();
                return;
            }


            if (selected)
            {
                ION.spriteBatch.Draw(Images.selectionBoxBack, drawingRectangle, Color.White);
            }

            ION.spriteBatch.Draw(Images.getUnitImage(owner, (int)facing), drawingRectangle, Color.White);
            drawFiringAnimation();
            drawUnderFireAnimation();
            

            if (selected || showDetails)
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

            if (selected)
            {
                ION.spriteBatch.Draw(Images.selectionBoxFront, drawingRectangle, Color.White);
            }
   
        }

        private void drawUnderFireAnimation()
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

                ION.spriteBatch.Draw(Images.bulletImpact[UnderFireFrame], new Rectangle(selectionRectangle.X + UnderFireOffsetX - (int)(baseHalfWidth * 0.5), selectionRectangle.Y + UnderFireOffsetY - (int)(baseHalfHeight * 1.0), (int)(baseHalfWidth * 0.75), (int)(baseHalfHeight * 1.5)), Color.White);
            }
        }

        private void drawDeathAnimation()
        {           
                //animation test code
                deathCounter++;

                if (deathCounter > 0)
                {
                    deathFrame = 0;
                }
                if (deathCounter > 10)
                {
                    deathFrame = 1;
                }
                if (deathCounter > 15)
                {
                    deathFrame = 2;
                }
                if (deathCounter > 20)
                {
                    Grid.get().removeDepthEnabledItem(this);
                    return;
                }
            
                int a = 255 - (int)(255*((float)deathCounter/(float)20));
               
                Color c = new Color();
                c.R = (byte)a;
                c.G = (byte)a;
                c.B = (byte)a;
                c.A = (byte)a;

                ION.spriteBatch.Draw(Images.getUnitImage(owner, (int)facing), drawingRectangle, c);

                drawingRectangle.Y -= drawingRectangle.Height;
                drawingRectangle.Height *= 2;

                ION.spriteBatch.Draw(Images.explosion_overlay[deathFrame], drawingRectangle, Color.White);
            
        }

        public override void setFiring()
        {
            base.setFiring();
            FiringCounter = 0;
        }

        private bool drawFiringAnimation()
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
                if (FiringCounter > (Grid.TPS))
                {
                    FiringCounter = 0;
                    firing = false;
                }

                if (FiringFrame < 2)
                {
                    ION.spriteBatch.Draw(Images.unit_shooting_overlay[(int)facing, FiringFrame], drawingRectangle, Color.White);
                }
                else if (FiringFrame >= 2)
                {
                    //show nothing
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void fixHeading()
        {
            if (!firing)
            {

                if (movement.X > 0)
                {
                    if (movement.Y > 0)
                    {
                        facing = direction.southEast;
                    }
                    else if (movement.Y < 0)
                    {
                        facing = direction.northEast;
                    }
                    else
                    {
                        facing = direction.east;
                    }
                }
                else if (movement.X < 0)
                {
                    if (movement.Y > 0)
                    {
                        facing = direction.southWest;
                    }
                    else if (movement.Y < 0)
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
                    if (movement.Y > 0)
                    {
                        facing = direction.south;
                    }
                    else if (movement.Y < 0)
                    {
                        facing = direction.north;
                    }
                }
            }
        }

        internal void Deserialize(System.IO.MemoryStream memoryStream)
        {
            throw new NotImplementedException();
        }
    }

}
