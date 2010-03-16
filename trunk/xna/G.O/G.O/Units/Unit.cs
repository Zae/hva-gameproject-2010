using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ION
{
    public abstract class Unit
    {
        private Colors hitmapColor = new Colors();

        protected int owner;

        protected int health = 100;
        protected int tileX;
        protected int tileY;

        protected int inTileX;
        protected int inTileY;

        public abstract void draw(int x, int y, int width, int height);


    }
}
