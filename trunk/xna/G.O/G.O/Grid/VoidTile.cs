using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ION
{
    class VoidTile : Tile
    {

        public VoidTile(int indexX, int indexY)
        {
            accessable = false;

            this.indexX = indexX;
            this.indexY = indexY;
        }

        public override void drawDebug(float translationX, float translationY)
        {
        }

        public override void draw(float translationX, float translationY)
        {          
        }

        public override void update()
        {
        }
    }
}
