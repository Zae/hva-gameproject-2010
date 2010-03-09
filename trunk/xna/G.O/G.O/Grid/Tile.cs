using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace G.O
{
    public abstract class Tile
    {
        protected int indexZ;
        protected int indexX;
        protected int indexY;

        //protected const int tileSize = 140;
        protected const int tileWidth = 140;
        protected const int tileHeight = 70;

        public abstract void draw(int x, int y, SpriteBatch spriteBatch);

        public abstract void draw(int translation, SpriteBatch spriteBatch);


        public abstract void update();

        public void setIndexZ(int newIndex)
        {
            indexZ = newIndex;
        }

        public int getIndexZ()
        {
            return indexZ;
        }

        public void setIndexX(int newIndex)
        {
            indexX = newIndex;
        }

        public int getIndexX()
        {
            return indexX;
        }

        public void setIndexY(int newIndex)
        {
            indexY = newIndex;
        }

        public int getIndexY()
        {
            return indexY;
        }
    }
}
