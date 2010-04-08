using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

namespace ION.Tools
{
    static class HitmapColorPool
    {

        public static ArrayList colors = new ArrayList();
        
        //Colors that are taken for the Tile hitmap
        //red 255,0,0
        //green 0,255,0
        //blue 0,0,255
        //cyan 0,128,128 or 0,127,127 can't seem to remember

        public static int red = 1;
        public static int green = 1;
        public static int blue = 1;

        public static Color reserveColor()
        {
            if (colors.Count > 0)
            {
                Color returnColor = (Color) colors[colors.Count - 1];
                colors.Remove(colors.Count - 1);
                return returnColor;
            }
            else
            {
                return getUniqueColor();
            }
        }

        public static void donateColor(Color c)
        {
            colors.Add(c);
        }

        private static Color getUniqueColor()
        {
            Color c = new Color(red, green, blue);

            red++;
            if (red > 254)
            {
                green++;
                red = 1;
            }
            if (green > 254)
            {
                blue++;
                green = 1;
            }
            //if (blue > 254)
            //{
            //    //You are screwed
            //}

            return c;
            
        }




    }
}
