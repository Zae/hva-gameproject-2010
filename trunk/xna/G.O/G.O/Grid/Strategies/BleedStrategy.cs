using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ION.GridStrategies
{
    public class BleedStrategy : GridStrategy
    {
        public override void update(int ellapsed)
        {
            ////do unit stuff
            //step++;

            //if (step == 5)
            //{
            //    tileVersusTile();   
            //}
            //else if (step == 10)
            //{
            //    tileAidTile();

            //    step = 0;
            //}

            ////now tell all Tiles to update, we use the perspective map for that
            ////because it might be faster?
            //for (int i = 0; i < tileCount; i++)
            //{
            //    perspectiveMap[i].update();
            //}
        }
    }
}