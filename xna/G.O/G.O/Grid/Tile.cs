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

        //protected const int baseHalfWidth = 140;
        //protected const int baseHalfHeight = 46;

        private static float scale = 0.8f;  

        public static int baseHalfWidth = (int)(140*scale);
        public static int baseHalfHeight = (int)(46*scale);

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
        public static void zoomIn()
        {
            if (scale <= 1.55f)
            scale += 0.05f;
            baseHalfWidth = (int)(140 * scale);
            baseHalfHeight = (int)(46 * scale);
        }
        public static void zoomOut()
        {
            if (scale >= 0.45f)
                scale -= 0.05f;
                baseHalfWidth = (int)(140 * scale);
                baseHalfHeight = (int)(46 * scale);
        }

    }
}
