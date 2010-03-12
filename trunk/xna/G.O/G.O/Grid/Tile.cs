using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace GO
{
    public abstract class Tile
    {
        protected int visualZ;
        protected int visualX;
        protected int visualY;
        protected bool selected = false;

        protected int indexX;
        protected int indexY;

        //protected const int baseWidth = 140;
        //protected const int baseHalfHeight = 46;

        public static int baseWidth = (int)(140*0.8);
        public static int baseHalfHeight = (int)(46*0.8);

        //public abstract void draw(int x, int y, SpriteBatch spriteBatch);

        public abstract void draw(int translationX, int translationY);
        public abstract void drawDebug(int translationX, int translationY);

        public abstract void update();

        public void setIndexZ(int newIndex)
        {
            visualZ = newIndex;
        }

        public int getIndexZ()
        {
            return visualZ;
        }

        public void setVisualX(int newVisualIndex)
        {
            visualX = newVisualIndex;
        }

        public int getVisualX()
        {
            return visualX;
        }

        public void setVisualY(int newIndex)
        {
            visualY = newIndex;
        }

        public int getVisualY()
        {
            return visualY;
        }

        public bool isSelected()
        {
            return selected;
        }

        public void setSelected(bool newSelected)
        {
            selected = newSelected;
        }
    }
}
