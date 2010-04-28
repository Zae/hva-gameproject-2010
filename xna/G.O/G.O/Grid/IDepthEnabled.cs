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

        /// <summary>
        /// blabla
        /// </summary>
        /// <param name="translationX">bla</param>
        /// <param name="translationY">bla</param>
        void drawDepthEnabled(float translationX, float translationY);
       

    }
}
