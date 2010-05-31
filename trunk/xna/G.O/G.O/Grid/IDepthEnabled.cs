using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ION
{
    public interface IDepthEnabled
    {

        int getTileX();

        int getTileY();

        bool hitTest(int x,int y);
        bool hitTest(Rectangle r);

        /// <summary>
        /// blabla
        /// </summary>
        /// <param name="translationX">bla</param>
        /// <param name="translationY">bla</param>
        void drawDepthEnabled(float translationX, float translationY);
       

    }
}
