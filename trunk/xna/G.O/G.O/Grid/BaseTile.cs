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

        private Rectangle drawingRectangle = new Rectangle();

        public BaseTile(int indexX, int indexY, int owner)
        {
            accessable = true;

            baseImage = Players.getBaseImage(owner);
            
            this.indexX = indexX;
            this.indexY = indexY;
            this.owner = owner;
            this.nextOwner = owner;
            this.charge = 1.0f;
            this.nextCharge = 1.0f;
        }

        public override void draw(float translationX, float translationY)
        {

            tileColor = getAppropriateColor(owner, charge);


            selectionRectangle0.X = (int)(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth*1.5));
            selectionRectangle0.Y = (int)((visualY * baseHalfHeight) + translationY - (baseHalfHeight * 4));
            selectionRectangle0.Width = (int)(baseHalfWidth * 3);
            selectionRectangle0.Height = (int)(baseHalfWidth * 2);


            selectionRectangle1.X = (int)(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth * 1));
            selectionRectangle1.Y = (int)((visualY * baseHalfHeight) + translationY - (baseHalfHeight * 9));
            selectionRectangle1.Width = (int)(baseHalfWidth * 2);
            selectionRectangle1.Height = (int)(baseHalfWidth * 2);

            drawingRectangle.X = (int)(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth * 2));
            drawingRectangle.Y = (int)((visualY * baseHalfHeight) + translationY - (baseHalfHeight * 9));
            drawingRectangle.Width = (int)(baseHalfWidth * 4);
            drawingRectangle.Height = (int)(baseHalfWidth * 4);

            //DEBUG shows you the selection boxes
            //ION.spriteBatch.Draw(Images.white1px, selectionRectangle0, Color.Green);
            //ION.spriteBatch.Draw(Images.white1px, selectionRectangle1, Color.Purple);

            ION.spriteBatch.Draw(Images.resourceImage, new Rectangle((int)(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth)), (int)((visualY * baseHalfHeight) + translationY), (int)(baseHalfWidth * 2), (int)(baseHalfHeight * 2)), tileColor);
                    
            ION.spriteBatch.Draw(baseImage, drawingRectangle, Color.White);
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
    }
}
