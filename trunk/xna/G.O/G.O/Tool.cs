using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GO
{
    class Tool
    {
        //public static int toClosestInt(float value)
        //{
        //    if (value > 0)
        //    {
        //        if ((value - (int)value) > 0.5f)
        //        {
        //            return (int)value + 1;
        //        }
        //        else
        //        {
        //            return (int)value;
        //        }
        //    }
        //    else if (value < 0)
        //    {
        //        if ((value - (int)value) < -0.5f)
        //        {
        //            return (int)value - 1;
        //        }
        //        else
        //        {
        //            return (int)value;
        //        }
        //    }
        //    return 0;
        //}

        public static int closestEvenInt(float value)
        {
            //If the int value is uneven
            if (((int)value) % 2 != 0)
            {
                if ((int)value < 0)
                {
                    return ((int)value - 1);
                }
                else
                {
                    return ((int)value + 1);
                }
            }
            else
            {
                return (int)value;
            }

        }
    }
}
