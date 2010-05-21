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
        protected int visualZ = -1;
        protected int visualX = -1;
        protected int visualY = -1;
        protected bool selected = false;

        public int indexX = -1;
        public int indexY = -1;

        private static float scale = 15;

        public static float baseHalfWidth = baseHalfWidthConstant * scale;
        public static float baseHalfHeight = baseHalfHeightConstant * scale;

        private const float baseHalfWidthConstant = 3;
        private const float baseHalfHeightConstant = 1;

        public abstract void draw(float translationX, float translationY);
        public abstract void drawDebug(float translationX, float translationY);

        public abstract void update();
        public abstract void releaseMomentum();

        public bool accessable;

        public Tile()
        {
            accessable = false;
        }

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

        public static bool zoomIn()
        {
            if (scale <= 25)
            {
                scale += 1;

                baseHalfWidth = baseHalfWidthConstant * scale;
                baseHalfHeight = baseHalfHeightConstant * scale;
                return true;
            }

            return false;
        }

        public static bool zoomOut()
        {
            if (scale >= 8)
            {
                scale -= 1;
                baseHalfWidth = baseHalfWidthConstant * scale;
                baseHalfHeight = baseHalfHeightConstant* scale;
                return true;
            }

            return false;
        }

        public Vector2 GetPos(float translationX, float translationY)
        {
            return new Vector2((ION.halfWidth + (visualX * baseHalfWidth) * (15.0f / scale) - (baseHalfWidth * (15.0f / scale))), ((visualY * baseHalfHeight) * (15.0f / scale) - (baseHalfWidth + baseHalfHeight) * (15.0f / scale)));
        }


        public bool FreeTile(Tile currentTile, List<Unit> allUnits)
        {
            bool answer = true;
            if (accessable)
            {
                for (int i = 0; i < allUnits.Count(); i++)
                {
                    if (allUnits[i].getTileX() == currentTile.indexX && allUnits[i].getTileY() == currentTile.indexY)
                    {
                        answer = false;
                    }
                }
            }
            else
            {
                answer = false;
            }
            return answer;

        }


    }
}
