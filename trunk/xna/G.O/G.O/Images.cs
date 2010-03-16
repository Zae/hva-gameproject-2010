using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ION
{
    class Images
    {

        public static Texture2D mountainImage;
        public static Texture2D borderImage;
        public static Texture2D resourceImage;
        public static Texture2D white1px;
        public static Texture2D tileHitmapImage;
        public static Texture2D unitImage;
        public static Texture2D unitChargeImage;
        public static Texture2D unitHitmapImage;

        public static Texture2D[] chargeCountImages = new Texture2D[10];

        public static Texture2D getChargeCountImage(float charge) 
        {
            if (charge < 1.0f)
            {
                return chargeCountImages[(int)(charge*10)];
            }
            else
            {
                return chargeCountImages[chargeCountImages.Length-1];
            }
            
        }
    }
}
