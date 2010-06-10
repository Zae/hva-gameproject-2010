using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace ION
{
    public class BaseTile : ResourceTile, IDepthEnabled
    {
        private Texture2D baseImage;

        private Rectangle selectionRectangle0 = new Rectangle();
        private Rectangle selectionRectangle1 = new Rectangle();

        private Rectangle drawingRectangleBig = new Rectangle();

        //TODO this is not how it should be, but with this we can reuse the selection box for units.
        private Rectangle selectionBoxRectangle = new Rectangle();

        public bool selected = false;
        public bool showDetails = false;

        public int health;
        public static int maxHealth = 500;
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


  
            //DEBUG shows you the selection boxes
            //ION.spriteBatch.Draw(Images.white1px, selectionRectangle0, Color.Green);
            //ION.spriteBatch.Draw(Images.white1px, selectionRectangle1, Color.Purple);
            
            
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
        }

        public override void update()
        {
            showDetails = false;
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
    }
}
