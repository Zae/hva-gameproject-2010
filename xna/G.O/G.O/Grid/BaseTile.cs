using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using ION.Tools;

namespace ION
{
    public class BaseTile : ResourceTile, IDepthEnabled
    {
        private Texture2D baseImage;

        private Rectangle selectionRectangle0 = new Rectangle();
        private Rectangle selectionRectangle1 = new Rectangle();

        private Rectangle drawingRectangleBig = new Rectangle();

        public Vector2 focalPoint = new Vector2();


        //TODO this is not how it should be, but with this we can reuse the selection box for units.
        private Rectangle selectionBoxRectangle = new Rectangle();

        public bool selected = false;
        public bool showDetails = false;

        private bool underFire = false;

        public int health;
        public static int maxHealth = 500;
        public bool dying = false;
        public bool dead = false;

        //under-fire animation helper variables
        private int UnderFireFrame = 0;
        private int UnderFireCounter = 0;
        public int UnderFireOffsetX = 0;
        public int UnderFireOffsetY = 0;

        public int deathFrame = 0;
        public int deathCounter = 0;


        public Rectangle healtRectangle = new Rectangle();


        public BaseTile(int indexX, int indexY, int owner)
        {
            accessable = true;

            baseImage = Players.getBaseImage(owner);

            health = maxHealth;
            
            this.indexX = indexX;
            this.indexY = indexY;
            this.owner = owner;
            this.nextOwner = owner;
            this.charge = 5.0f;
            this.nextCharge = 5.0f;
        }

        public override void draw(float translationX, float translationY)
        {
            if (!dying && health < 0)
            {

                //start dying
                Die();

                //last time this method returns
                //return returnValue;
            }
            
            
            if (selected)
            {
                selectionBoxRectangle.X = (int)(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth * 3));
                selectionBoxRectangle.Y = (int)((visualY * baseHalfHeight) + translationY - (baseHalfHeight * 11));
                selectionBoxRectangle.Width = (int)(baseHalfWidth * 6);
                selectionBoxRectangle.Height = (int)(baseHalfHeight * 12);


                ION.spriteBatch.Draw(Images.selectionBoxBack, selectionBoxRectangle, Color.White);
            }

            tileColor = getAppropriateColor(owner, charge);


            selectionRectangle0.X = (int)(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth*1.5));
            selectionRectangle0.Y = (int)((visualY * baseHalfHeight) + translationY - (baseHalfHeight * 4));
            selectionRectangle0.Width = (int)(baseHalfWidth * 3);
            selectionRectangle0.Height = (int)(baseHalfWidth * 2);


            selectionRectangle1.X = (int)(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth * 1));
            selectionRectangle1.Y = (int)((visualY * baseHalfHeight) + translationY - (baseHalfHeight * 9));
            selectionRectangle1.Width = (int)(baseHalfWidth * 2);
            selectionRectangle1.Height = (int)(baseHalfWidth * 2);

            drawingRectangleBig.X = (int)(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth * 2));
            drawingRectangleBig.Y = (int)((visualY * baseHalfHeight) + translationY - (baseHalfHeight * 9));
            drawingRectangleBig.Width = (int)(baseHalfWidth * 4);
            drawingRectangleBig.Height = (int)(baseHalfWidth * 4);

            drawingRectangle.X = (int)(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth));
            drawingRectangle.Y = (int)((visualY * baseHalfHeight) + translationY);
            drawingRectangle.Width = (int)(baseHalfWidth * 2);
            drawingRectangle.Height = (int)(baseHalfHeight * 2);


            focalPoint.X = drawingRectangle.Center.X;
            focalPoint.Y = drawingRectangle.Center.Y;
  
            //DEBUG shows you the selection boxes
            //ION.spriteBatch.Draw(Images.white1px, selectionRectangle0, Color.Green);
            //ION.spriteBatch.Draw(Images.white1px, selectionRectangle1, Color.Purple);


            if (dying)
            {
                drawDeathAnimation();
                return;
            }

            
            ION.spriteBatch.Draw(baseImage, drawingRectangleBig, Color.White);

            if (selected)
            {
                ION.spriteBatch.Draw(Images.selectionBoxFront, selectionBoxRectangle, Color.White);
            }

            if (selected || showDetails)
            {
                //Draw health and energy stuff
                healtRectangle.X = (int)(drawingRectangleBig.X
                    + (Tile.baseHalfWidth * 1.12));
                healtRectangle.Y = (int)(drawingRectangleBig.Y
                    + (baseHalfHeight * 0.2));
                healtRectangle.Width = (int)(baseHalfWidth * 1.8);
                healtRectangle.Height = (int)(baseHalfHeight * 1.0);

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
                    ION.spriteBatch.Draw(Images.unitHealth[2], drawingRectangleBig, healthBarColor);
                }
                else
                {
                    ION.spriteBatch.Draw(Images.unitHealth[1], drawingRectangleBig, healthBarColor);
                }

                ION.spriteBatch.Draw(Images.white1px, healtRectangle, healthBarColor);

                ION.spriteBatch.Draw(Images.unitHealth[0], drawingRectangleBig, Color.White);

                //Reset the flag
                showDetails = false;
            }

            if (underFire)
            {
                drawUnderFireAnimation();
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
            if (deathCounter > 25)
            {
                deathFrame = 1;
            }
            if (deathCounter > 45)
            {
                deathFrame = 2;
            }
            if (deathCounter > 75)
            {
                Grid.get().removeDepthEnabledItem(this);
                dead = true;
                return;
            }

            int a = 255 - (int)(255 * ((float)deathCounter / (float)75));

            Color c = new Color();
            c.R = (byte)a;
            c.G = (byte)a;
            c.B = (byte)a;
            c.A = (byte)a;

            ION.spriteBatch.Draw(baseImage, drawingRectangleBig, c);

            drawingRectangle.Y -= drawingRectangle.Height;
            drawingRectangle.Height *= 2;

            drawingRectangleBig.X -= drawingRectangleBig.Width/2;
            drawingRectangleBig.Y -= drawingRectangleBig.Height;
            drawingRectangleBig.Width *= 2;
            drawingRectangleBig.Height *= 2;

            ION.spriteBatch.Draw(Images.explosion_overlay[deathFrame], drawingRectangleBig, Color.White);

        }

        public void drawResourceTile(float translationX, float translationY)
        {
            ION.spriteBatch.Draw(Images.resourceImage, new Rectangle((int)(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth)), (int)((visualY * baseHalfHeight) + translationY), (int)(baseHalfWidth * 2), (int)(baseHalfHeight * 2)), tileColor);
            ION.spriteBatch.Draw(Images.borderImage, new Rectangle((int)(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth)), (int)((visualY * baseHalfHeight) + translationY), (int)(baseHalfWidth * 2), (int)(baseHalfHeight * 2)), Color.White);
        }
        
        //Inherited from IDepthEnabled
        public int getTileX()
        {
            return indexX;
        }

        public int getTileY()
        {
            return indexY;
        }

        public void drawDepthEnabled(float translationX, float translationY)
        {
            draw(translationX, translationY);
        }

        public bool hitTest(int x, int y)
        {
            if(selectionRectangle0.Contains(x,y) || selectionRectangle1.Contains(x,y) )
            {
                return true;
            }
            return false;
        }


        public bool hitTest(Rectangle r)
        {
            if (selectionRectangle0.Intersects(r) || selectionRectangle1.Intersects(r))
            {
                return true;
            }
            return false;
        }

        public int getOwner()
        {
            return owner;
        }

        public void displayDetails()
        {
            showDetails = true;
        }

        public void Die()
        {
            //open the unit's tile for traffic
            //Grid.map[inTileX, inTileY].accessable = true;

            //Remove it from gameplay
            //Grid.get().removeUnit(this);

            SoundManager.baseExplosion();


            //Start the dying sequence
            dying = true;
            
        }

        public Vector2 getFocalPoint()
        {
            return focalPoint;
        }


        public void hit(int damageTaken, int damageType)
        {

            health -= damageTaken;

            //this will trigger or keep alive the animation
            underFire = true;
        }

        private void drawUnderFireAnimation()
        {
            if (underFire)
            {
                if (UnderFireCounter == 0)
                {
                    UnderFireOffsetX = (int)(Tool.unsafeRandom.NextDouble() * selectionRectangle1.Width);
                    UnderFireOffsetY = (int)(Tool.unsafeRandom.NextDouble() * selectionRectangle1.Height);
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

                ION.spriteBatch.Draw(Images.bulletImpact[UnderFireFrame], new Rectangle(selectionRectangle1.X + UnderFireOffsetX - (int)(baseHalfWidth * 0.5), selectionRectangle1.Y + UnderFireOffsetY - (int)(baseHalfHeight * 1.0), (int)(baseHalfWidth * 0.75), (int)(baseHalfHeight * 1.5)), Color.White);
            }
        }
    }
}
