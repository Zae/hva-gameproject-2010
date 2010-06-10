using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ION
{
    public class MountainTile : Tile, IDepthEnabled
    {

        public static Color themeColor = Color.White;
        public Color currentColor = Color.White;
        public int imageNumber;

        public MountainTile(int indexX, int indexY, int selectImage)
        {

            imageNumber = selectImage;
            accessable = false;

            this.indexX = indexX;
            this.indexY = indexY;

            currentColor = themeColor;

            //For everything that is not a Mountain but Crystal for instance
            if (imageNumber > 20)
            {
                //Set it semi-transparent
                currentColor.A = 130;
            }
        }

        public override void drawDebug(float translationX, float translationY)
        {
            //Vector2 location = new Vector2(ION.halfWidth + (visualX * baseHalfWidth) + translationX - 40, (visualY * baseHalfHeight) + translationY + baseHalfHeight);
            //ION.spriteBatch.DrawString(Fonts.font, "(z=" + visualZ + ":x=" + visualX + ":y=" + visualY + ")", location, Color.White);            
        }

        public override void draw(float translationX, float translationY)
        {
            if (imageNumber < 13)
            {
                //ION.spriteBatch.Draw(Images.mountainFloorImage, new Rectangle((int)(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth)), (int)((visualY * baseHalfHeight) + translationY - (baseHalfWidth + baseHalfHeight)), (int)(baseHalfWidth * 2), (int)(baseHalfWidth * 2)), themeColor);
                ION.spriteBatch.Draw(Images.canyonFloorImage[imageNumber], new Rectangle((int)(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth)), (int)((visualY * baseHalfHeight) + translationY), (int)(baseHalfWidth * 2), (int)((baseHalfWidth * 2) / 3)), currentColor);
            }
            else if (imageNumber == 20)
            {
                //ION.spriteBatch.Draw(Images.mountainFloorImage, new Rectangle((int)(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth)), (int)((visualY * baseHalfHeight) + translationY - (baseHalfWidth + baseHalfHeight)), (int)(baseHalfWidth * 2), (int)(baseHalfWidth * 2)), themeColor);
                ION.spriteBatch.Draw(Images.mountainImage, new Rectangle((int)(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth)), (int)((visualY * baseHalfHeight) + translationY - (baseHalfWidth + baseHalfHeight)), (int)(baseHalfWidth * 2), (int)(baseHalfWidth * 2)), currentColor);
            }
            else if (imageNumber == 21)
            {
                //ION.spriteBatch.Draw(Images.iceFloorImage, new Rectangle((int)(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth)), (int)((visualY * baseHalfHeight) + translationY - (baseHalfWidth + baseHalfHeight)), (int)(baseHalfWidth * 2), (int)(baseHalfWidth * 2)), themeColor);
                ION.spriteBatch.Draw(Images.iceImage, new Rectangle((int)(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth)), (int)((visualY * baseHalfHeight) + translationY - (baseHalfWidth + baseHalfHeight)), (int)(baseHalfWidth * 2), (int)(baseHalfWidth * 2)), currentColor);
            }
            else if (imageNumber == 22)
            {
                //ION.spriteBatch.Draw(Images.glassFloorImage, new Rectangle((int)(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth)), (int)((visualY * baseHalfHeight) + translationY - (baseHalfWidth + baseHalfHeight)), (int)(baseHalfWidth * 2), (int)(baseHalfWidth * 2)), themeColor);
                ION.spriteBatch.Draw(Images.glassImage, new Rectangle((int)(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth)), (int)((visualY * baseHalfHeight) + translationY - (baseHalfWidth + baseHalfHeight)), (int)(baseHalfWidth * 2), (int)(baseHalfWidth * 2)), currentColor);
            }
            else
            {
                //ION.spriteBatch.Draw(Images.crystalFloorImage, new Rectangle((int)(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth)), (int)((visualY * baseHalfHeight) + translationY - (baseHalfWidth + baseHalfHeight)), (int)(baseHalfWidth * 2), (int)(baseHalfWidth * 2)), themeColor);
                ION.spriteBatch.Draw(Images.crystalImage, new Rectangle((int)(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth)), (int)((visualY * baseHalfHeight) + translationY - (baseHalfWidth + baseHalfHeight)), (int)(baseHalfWidth * 2), (int)(baseHalfWidth * 2)), currentColor);
            }
        }


        public override void update()
        {
            imageNumber++;
            imageNumber %= 40;
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
            return false;
        }

        public bool hitTest(Rectangle r)
        {
            return false;
        }

        public int getOwner()
        {
            return Players.NEUTRAL;
        }

        public void displayDetails()
        {
            //showDetails = true;
        }

    }
}
