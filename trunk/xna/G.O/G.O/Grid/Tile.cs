using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ION
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

        private static int scale = 40;

        public static int baseHalfWidth = baseHalfWidthConstant * scale;
        public static int baseHalfHeight = baseHalfHeightConstant * scale;

        //private const int baseHalfWidthConstant = 140;
        //private const int baseHalfHeightConstant = 46;

        private const int baseHalfWidthConstant = 3; 
        private const int baseHalfHeightConstant = 1;


        //public abstract void draw(int x, int y, SpriteBatch spriteBatch);

        public abstract void draw(int translationX, int translationY);
        public abstract void drawDebug(int translationX, int translationY);

        public abstract void tileVersusTile(Tile other);

        public abstract void tileAidTile(Tile other);

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
            if (scale <= 80)
            {
                scale += 1;

                baseHalfWidth = baseHalfWidthConstant * scale;
                baseHalfHeight = baseHalfHeightConstant * scale;
            }
          
           
        }

        public static void zoomOut()
        {
            if (scale >= 5)
            {
                scale -= 1;
                baseHalfWidth = baseHalfWidthConstant * scale;
                baseHalfHeight = baseHalfHeightConstant* scale;
            }
               
        }

    }
}
