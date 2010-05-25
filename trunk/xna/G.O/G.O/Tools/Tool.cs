using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ION
{
    class Tool
    {
        //public static int toClosestInt(float value)
        //{
        //    //float temp = value % 1;
        //    if (temp > 0.5)
        //    {
        //        return (int)value + 1;
        //    }
        //    else
        //    {
        //        return (int)value;
        //    }
        //}

        public static int closestEvenInt(float value)
        {
            //If the int value is uneven
            if (((int)value) % 2 != 0)
            {
                if ((int)value < 1)
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
