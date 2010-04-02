using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ION.GridStrategies
{
    public abstract class GridStrategy
    {
        public string name;
        public int speed = 0;

        public GridStrategy()
        {
            name = "GridStrategy";
        }
                

        public abstract void update(int ellapsed);

        public abstract void increaseSpeed();
        public abstract void decreaseSpeed();
    }
}
