using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ION
{
    class MountainTile : Tile, IDepthEnabled
    {

        public MountainTile(int indexX, int indexY)
        {
            this.indexX = indexX;
            this.indexY = indexY;
        } 

        public override void drawDebug(float translationX, float translationY)
        {
            
            Vector2 location = new Vector2(ION.halfWidth + (visualX * baseHalfWidth) + translationX - 40, (visualY * baseHalfHeight) + translationY + baseHalfHeight);
            ION.spriteBatch.DrawString(Fonts.font, "(z=" + visualZ + ":x=" + visualX + ":y=" + visualY + ")", location, Color.White);
            
        }

        public override void releaseMomentum()
        {
            
        }

        public override void draw(float translationX, float translationY)
        {     
            ION.spriteBatch.Draw(Images.mountainImage, new Rectangle((int)(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth)), (int)((visualY * baseHalfHeight) + translationY - (baseHalfWidth+baseHalfHeight)), (int)(baseHalfWidth * 2), (int)(baseHalfWidth * 2)), Color.White);                 
        }


        public override void update()
        {
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
