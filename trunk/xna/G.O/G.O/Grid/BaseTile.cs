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

            //ION.spriteBatch.Draw
            //(Images.white1px, new Rectangle((int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x) + ((int)Tile.baseHalfWidth * 0.63)), (int)(((pos.Y) * (scale / 15.0f)) + (y) + (baseHalfHeight * 2) + (int)(baseHalfHeight * 0.55)), 
            //(int)(baseHalfWidth * 0.75), (int)(baseHalfHeight * 3)), Color.Gray);


            //ION.spriteBatch.Draw(Images.white1px, new Rectangle((int)(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth * 2)), (int)((visualY * baseHalfHeight) + translationY - (baseHalfHeight * 10)), (int)(baseHalfWidth * 4), (int)(baseHalfWidth * 4)), Color.Gray);


            ION.spriteBatch.Draw(Images.resourceImage, new Rectangle((int)(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth)), (int)((visualY * baseHalfHeight) + translationY), (int)(baseHalfWidth * 2), (int)(baseHalfHeight * 2)), tileColor);
         
            
            ION.spriteBatch.Draw(baseImage, new Rectangle((int)(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth*2)), (int)((visualY * baseHalfHeight) + translationY - (baseHalfHeight*10)), (int)(baseHalfWidth * 4), (int)(baseHalfWidth * 4)), Color.White);
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
    }
}
