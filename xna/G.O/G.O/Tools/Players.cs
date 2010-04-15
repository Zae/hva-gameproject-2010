using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ION
{
    public class Players
    {

        public static int NEUTRAL = 0;
        public static int PLAYER1 = 1;
        public static int PLAYER2 = 2;

        public static Color PLAYER1_COLOR = Color.Blue;
        public static Color PLAYER2_COLOR = Color.Red;

        public static Texture2D getBaseImage(int owner)
        {
            if (owner == 1)
            {
                return Images.blueBaseImage;

            }
            else if (owner == 2)
            {
                return Images.redBaseImage;

            }
            else
            {
                return Images.baseImage;
            }
        }
    }
}
