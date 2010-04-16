using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
 
 
namespace ION
{
    public abstract class Tile
    {
        protected int visualZ;
        protected int visualX;
        protected int visualY;
        protected bool selected = false;

        public int indexX;
        public int indexY;

        public int id = -1;

        public static int sID = -1;

        //protected const int baseHalfWidth = 140;
        //protected const int baseHalfHeight = 46;

        private static float scale = 15;

        public static float baseHalfWidth = baseHalfWidthConstant * scale;
        public static float baseHalfHeight = baseHalfHeightConstant * scale;

        //private const int baseHalfWidthConstant = 140;
        //private const int baseHalfHeightConstant = 46;

        private const float baseHalfWidthConstant = 3;
        private const float baseHalfHeightConstant = 1;


        //public abstract void drawDebug(int x, int y, SpriteBatch spriteBatch);

        public abstract void draw(float translationX, float translationY);
        public abstract void drawDebug(float translationX, float translationY);

        //public abstract void tileVersusTile(Tile b);

        //public abstract void tileAidTile(Tile b);

        public abstract void update();
        public abstract void releaseMomentum();

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

        public Vector2 GetPos(float translationX, float translationY)
        {
            return new Vector2((ION.halfWidth + (visualX * baseHalfWidth) * (15.0f / scale) - (baseHalfWidth * (15.0f / scale))), ((visualY * baseHalfHeight) * (15.0f / scale) - (baseHalfWidth + baseHalfHeight) * (15.0f / scale)));
        }

    }
}
