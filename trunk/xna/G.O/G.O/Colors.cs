using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GO
{
    class Colors
    {
        const uint color_red = 4294901760;
        const uint color_green = 4278255360;
        const uint color_pink = 0; // This one struck me as strange, but that's what I got...
        const uint color_cyan = 4278255615;
        const uint color_blue = 4278190335;
        const uint color_yellow = 4294967040;
        const uint color_black = 4278190080;

        public static string getColor(uint color)
        {
            if (color == color_red)
            {
                return "Red";
            }
            else if (color == color_green)
            {
                return "Green";
            }
            else if (color == color_pink)
            {
                return "Pink?";
            }
            else if (color == color_cyan)
            {
                return "Cyan";
            }
            else if (color == color_blue)
            {
                return "Blue";
            }
            return "Unidentified";
        }
    }
}
