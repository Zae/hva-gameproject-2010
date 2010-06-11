using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ION.Tools;

namespace ION
{
    public class Tower : Unit
    {
        public static int cost = 500;

        public static int maxHealth = 250;

        public static int firingRange = 5;
        public static float fireRate = 1.0f;



        public static int minDamage = 2;
        public static int maxDamage = 4;

        public static int damageType = 1;


        public Rectangle healtRectangle = new Rectangle();

        public Rectangle firingRectangle = new Rectangle();

        //fire animation helper variables
        private int FiringFrame = 0;
        private int FiringCounter = 0;

        public int deathFrame = 0;
        public int deathCounter = 0;

        //under-fire animation helper variables
        private int UnderFireFrame = 0;
        private int UnderFireCounter = 0;
        public int UnderFireOffsetX = 0;
        public int UnderFireOffsetY = 0;

        public int tmp = 0;

        public Tower(Tile newPos, int owner, int id) : base(owner,id)
        {

            health = maxHealth;

            position = newPos;
            targetPosition = null;

            inTileX = position.indexX;
            inTileY = position.indexY;

            position.accessable = false;
      
            init();
        }

        private void init()
        {
            FiringFrame = 0;
            FiringCounter = 0;
        }

        public override void move()
        {
            EmptyWayPoints();

            tmp++;
            if (tmp > 15)
            {
                facing++;
                if ((int)facing > 7)
                    facing = 0;

                tmp = 0;
            }
        }

        public override void draw(float x, float y)
        {
            //TODO is selectionRectangle different from Robot's? Does it matter?


            drawingRectangle.X = position.drawingRectangle.X;
            drawingRectangle.Y = position.drawingRectangle.Y - position.drawingRectangle.Height;
            drawingRectangle.Width = position.drawingRectangle.Width;
            drawingRectangle.Height = position.drawingRectangle.Height * 2;

            selectionRectangle.X = (int)(drawingRectangle.X + (Tile.baseHalfWidth * 0.63));
            selectionRectangle.Y = (int)(drawingRectangle.Y + (baseHalfHeight * 0.55));
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

            ION.spriteBatch.Draw(Images.getTurretImage(owner, (int)facing), drawingRectangle, Color.White);
            drawFiringAnimation(x, y);
            drawUnderFireAnimation(x, y);

            if (selected)
            {
                ION.spriteBatch.Draw(Images.selectionBoxFront, drawingRectangle, Color.White);
            }

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
                else if (health > (maxHealth / 3))
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

            int a = 255 - (int)(255 * ((float)deathCounter / (float)20));

            Color c = new Color();
            c.R = (byte)a;
            c.G = (byte)a;
            c.B = (byte)a;
            c.A = (byte)a;

            ION.spriteBatch.Draw(Images.getTurretImage(owner, (int)facing), drawingRectangle, c);

            drawingRectangle.Y -= drawingRectangle.Height;
            drawingRectangle.Height *= 2;

            ION.spriteBatch.Draw(Images.explosion_overlay[deathFrame], drawingRectangle, Color.White);

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

        public override void Update(float translationX, float translationY)
        {
            showDetails = false;

            if (health < 0)
            {

                //start dying
                Die();

                //last time this method returns
                //return returnValue;
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
                        SoundManager.turretSound();
                        u.hit(Damage.getDamage(minDamage,maxDamage), damageType);
                        face(u.focalPoint);
                        break;
                    }
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
        }

        private bool drawFiringAnimation(float x, float y)
        {
            if (firing)
            {

                firingRectangle.X = drawingRectangle.X - (int)(baseHalfWidth);
                firingRectangle.Y = drawingRectangle.Y - (int)(baseHalfHeight * 4);
                firingRectangle.Width = drawingRectangle.Width*2;
                firingRectangle.Height = drawingRectangle.Height * 2;

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
                    ION.spriteBatch.Draw(Images.tower_shooting_overlay[(int)facing, FiringFrame], firingRectangle, Color.White);
                }
                else if (FiringFrame >= 2)
                {
                    //Do not shot fire animation 
                }
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
