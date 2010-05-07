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

        public const int tilesPerTexture = 16;

        private Rectangle temp = new Rectangle();

        public float width;
        public float height;

        public int countX;
        public int countY;

        public ThemeManager(string theme, int tileCountX, int tileCountY)
        {
            countX = (int)tileCountX / (int) tilesPerTexture;
            countY = (int)tileCountY / (int) tilesPerTexture;

            width = tilesPerTexture * ((float)Tile.baseHalfWidth * 2);
            height = tilesPerTexture * ((float)Tile.baseHalfHeight * 2);
            
            if (theme == MOON_THEME)
            {
               //groundTexture =
                MountainTile.themeColor = Color.Gray;
                ION.get().loadGroundTexture("ground_moon");           
            }
            else if (theme == DIRT_THEME)
            {
                //groundTexture =
                MountainTile.themeColor = Color.Yellow;
                ION.get().loadGroundTexture("ground_dirt");
            }

            else {
                Debug.WriteLine("FATAL ERROR: Don't recognize the supplied theme > "+theme);
                ION.get().Exit();
            }

        }

        public void resizeGroundTexture()
        {
            width = tilesPerTexture * (Tile.baseHalfWidth * 2);
            height = tilesPerTexture * (Tile.baseHalfHeight * 2);
            
        }

        public void drawGroundTexture()
        {
            temp.X = ION.halfWidth + (int)StateTest.translationX-(((int)width/2));
            temp.Y = (int)StateTest.translationY- (((int)height/2)*2);
            temp.Width = (int)width;
            temp.Height = (int)height;
            ION.spriteBatch.Draw(Images.groundTexture, temp, Color.White);
            
            for(int i=0;i<countX+1;i++) {
                temp.X -= ((int)width/2);
                temp.Y += ((int)height/2);
                 ION.spriteBatch.Draw(Images.groundTexture, temp, Color.White);
               // temp.Width = (int)width;
              //  temp.Height = (int)height;
            }
         

           
        }
    }
}
