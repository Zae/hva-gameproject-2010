using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace G.O
{
    public abstract class Tile
    {

        public abstract void draw(int x, int y, float scale);

        public abstract void update();

    }
}
