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
        public static int cost = 250;

        public static int maxHealth = 100;

        public static int firingRange = 5;

        public static float fireRate = 1.0f;

        public Rectangle healtRectangle = new Rectangle();

        public int ticksIntoMovement = 0;
        public static int tileToTileTicks = Grid.TPS / 3;

        public static int damageType = 1;


        public static int minDamage = 2;
        public static int maxDamage = 4;

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

        public Robot(Tile newPos, int owner, int id) : base(owner,id)
        {
            health = maxHealth;
            
            position = newPos;
            targetPosition = null;

            BaseTile playerBase = Grid.getPlayerBase(owner);
            inTileX = playerBase.getTileX();
            inTileY = playerBase.getTileY();
        }

        //move units towards their target
        public override void move()
        {
            
            //check if the target position is accesable
            if (!moving && !targetPosition.accessable)
            {
                //todo

                if (destination.Count == 0)
                {
                    targetPosition = null;
                }
                //else
                //{
                //    EmptyWayPoints();
                //}


            }
            
            else if (!moving)
            {
                
                //find out if the next move 


                position.accessable = true;

                moving = true;

                targetPosition.accessable = false;

                inTileX = targetPosition.indexX;
                inTileY = targetPosition.indexY;


                movement.X = targetPosition.drawingRectangle.X - position.drawingRectangle.X;
                movement.Y = targetPosition.drawingRectangle.Y - position.drawingRectangle.Y;
  
                movement.X /= tileToTileTicks;
                movement.Y /= tileToTileTicks;
            }
    
                            
        }

        public override void Update(float translationX, float translationY)
        {
            //Debug.WriteLine("ROBOT UPDATE!");
        
            
            if (moving)
            {
                if (ticksIntoMovement == tileToTileTicks)
                {
                    position = targetPosition;
         
                    Grid.updateDepthEnabledItem(this);
                    ticksIntoMovement = 0;
                    targetPosition = null;
                    moving = false;
                }

                fixHeading();

            }


            showDetails = false;

            if (health < 0)
            {

                //start dying
                Die();

                //last time this method returns
                //return returnValue;
            }

            if (targetPosition != null)
            {
                move();
            }
            else
            {
                // Code for waypoints
                if (!moving && destination.Count() != 0)
                {

                    targetPosition = destination.Dequeue();

                    //targetPos = temp.GetPos(translationX, translationY);            
                }

            }
            if (scan > Grid.TPS * fireRate)
            {
 
                List<Unit> enemies = Grid.get().getPlayerEnemies(owner);
                if (enemies.Count == 0) firing = false;
                
                foreach (Unit u in enemies)
                {
                    if ((u.inTileX - inTileX > -firingRange && u.inTileX - inTileX < firingRange) && (u.inTileY - inTileY > -firingRange && u.inTileY - inTileY < firingRange))
                    {
                        firing = true;
                        SoundManager.fireSound();
                        //fire on this unit.
                        u.hit(Damage.getDamage(minDamage, maxDamage), damageType);
                        face(u.focalPoint);
                        break;
                    }
                    //firing = false;
                }
                scan = 0;

                //HACK
                //attack base if no enemy units around
                int otherPlayer = 1;
              
                if (owner == 1)
                {
                    otherPlayer = 2;
                }
                BaseTile enemyBase = Grid.getPlayerBase(otherPlayer);

                if (enemyBase != null && !enemyBase.dying)
                {
                    if ((enemyBase.getTileX() - inTileX > -firingRange && enemyBase.getTileX() - inTileX < firingRange) && (enemyBase.getTileY() - inTileY > -firingRange && enemyBase.getTileY() - inTileY < firingRange))
                    {
                        firing = true;
                        SoundManager.fireSound();
                        //fire on this unit.
                        enemyBase.hit(Damage.getDamage(minDamage, maxDamage), damageType);
                        face(enemyBase.focalPoint);
                      
                    }
                }


            }
            scan++;

            if (moving)
            {
                ticksIntoMovement++;
            }

            
           
        }

        public override void draw(float x, float y)
        {


 
            drawingRectangle.X = position.drawingRectangle.X;
            drawingRectangle.Y = position.drawingRectangle.Y - position.drawingRectangle.Height;
            drawingRectangle.Width = position.drawingRectangle.Width;
            drawingRectangle.Height = position.drawingRectangle.Height*2;



            if (moving)
            {
                //Debug.WriteLine("ticksIntoMovement: "+ticksIntoMovement);
                //Debug.WriteLine("movement.X: " + movement.X);
                //Debug.WriteLine("mult: " + (ticksIntoMovement*movement.X));


                //selectionRectangle.X += (int)(ticksIntoMovement * movement.X) + (int)(Grid.get().intermediate * movement.X);
                //selectionRectangle.Y += (int)(ticksIntoMovement * movement.Y) + (int)(Grid.get().intermediate * movement.Y);

                drawingRectangle.X += (int)(((float)ticksIntoMovement * movement.X) + (Grid.get().intermediate * movement.X));
                drawingRectangle.Y += (int)(((float)ticksIntoMovement * movement.Y) + (Grid.get().intermediate * movement.Y));
                //drawingRectangle.X += (int)(ticksIntoMovement * movement.X);
                //drawingRectangle.Y += (int)(ticksIntoMovement * movement.Y);
            }

            selectionRectangle.X = drawingRectangle.X + (int)(Tile.baseHalfWidth * 0.63);
            selectionRectangle.Y = drawingRectangle.Y + (int)(baseHalfHeight * 0.55);
            selectionRectangle.Width = (int)(baseHalfWidth * 0.75);
            selectionRectangle.Height = (int)(baseHalfHeight * 3);


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
                healtRectangle.X = (int)(drawingRectangle.X 
                    + (Tile.baseHalfWidth * 0.56));
                healtRectangle.Y = (int)(drawingRectangle.Y
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
                if (deathCounter > 8)
                {
                    deathFrame = 1;
                }
                if (deathCounter > 16)
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
                if (FiringCounter > 9)
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
