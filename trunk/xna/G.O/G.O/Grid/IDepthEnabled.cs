using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ION
{
    interface IDepthEnabled
    {

        int getTileX();

        int getTileY();

        void drawDepthEnabled(float translationX, float translationY);
       

    }
}
