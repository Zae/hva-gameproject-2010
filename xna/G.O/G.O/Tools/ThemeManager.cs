using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace ION
{
    class ThemeManager
    {
        public const string MOON_THEME = "Moon";
        public const string DIRT_THEME = "Dirt";

        //public const int tilesPerTexture = 16;

        private Rectangle temp = new Rectangle();

        public float width;
        public float height;

        public int countX;
        public int countY;

        public int xDiff;

        public ThemeManager(string theme,string ground, int tileCountX, int tileCountY)
        {
            countX = tileCountX;
            countY = tileCountY;

            xDiff = tileCountX - tileCountY;

           
            width = (tileCountX+tileCountY-1) * ((float)Tile.baseHalfWidth);
            height = (tileCountX + tileCountY - 1) * ((float)Tile.baseHalfHeight);
            
            if (theme == MOON_THEME)
            {
                MountainTile.themeColor = Color.Gray;
                ION.get().loadThemedResources("/leveltextures/"+ground);           
            }
            else if (theme == DIRT_THEME)
            {
                MountainTile.themeColor = Color.Yellow;
                ION.get().loadThemedResources("/leveltextures/"+ground);
            }

            else {
                Debug.WriteLine("FATAL ERROR: Don't recognize the supplied theme > "+theme);
                ION.get().Exit();
            }

            if (Images.groundTexture == null)
            {
                Debug.WriteLine("FATAL ERROR: Didn't load groundTexture! > " + theme);
                ION.get().Exit();
            }

        }

        public void resizeGroundTexture()
        {
            width = (countX + countY - 1) * (Tile.baseHalfWidth);
            height = (countX + countY - 1) * (Tile.baseHalfHeight);
        }

        public void drawGroundTexture()
        {
            //for (int j = -2; j < countY + 2; j++)
            //{

            //    temp.X = ION.halfWidth + (int)StateTest.translationX - (((int)width / 2) - (j * (int)width / 2));
            //    temp.Y = (int)StateTest.translationY - (((int)height / 2) * 2 - (j * (((int)height / 2))));
            //    temp.Width = (int)width;
            //    temp.Height = (int)height;
            //    ION.spriteBatch.Draw(Images.groundTexture, temp, Color.White);

            //    for (int i = 0; i < countX; i++)
            //    {
            //        temp.X -= ((int)width / 2);
            //        temp.Y += ((int)height / 2);
            //        ION.spriteBatch.Draw(Images.groundTexture, temp, Color.White);
            //        // temp.Width = (int)width;
            //        //  temp.Height = (int)height;
            //    }

            //}
            //temp.X -= ((int)width / 2);
            //temp.Y += ((int)height / 2);
            temp.X = ION.halfWidth - ((int)width / 2) + (int)StateTest.get().translationX - (int)(xDiff * (Tile.baseHalfWidth / 2));
            temp.Y = (int)StateTest.get().translationY;// -((int)height / 2);
            temp.Width = (int)width;
            temp.Height = (int)height;
            //temp.X = ION.halfWidth + (int)StateTest.translationX - (((int)width / 2));
            //temp.Y = (int)StateTest.translationY - (((int)height / 2) * 2 );
            ION.spriteBatch.Draw(Images.groundTexture, temp, Color.White);

           
        }
    }
}
