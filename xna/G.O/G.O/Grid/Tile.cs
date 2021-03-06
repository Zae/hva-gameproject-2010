﻿using System;
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

        public Rectangle drawingRectangle;

        private static float scale = 15;

        public static float baseHalfWidth = baseHalfWidthConstant * scale;
        public static float baseHalfHeight = baseHalfHeightConstant * scale;

        private const float baseHalfWidthConstant = 3;
        private const float baseHalfHeightConstant = 1;

        public abstract void draw(float translationX, float translationY);
        public abstract void drawDebug(float translationX, float translationY);

        public abstract void update();

        public bool accessable;

        public int type = 0;

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

        public void calcSize()
        {
            drawingRectangle.X = (int)(ION.halfWidth + (visualX * baseHalfWidth) + StateTest.get().translationX - (baseHalfWidth));
            drawingRectangle.Y = (int)((visualY * baseHalfHeight) + StateTest.get().translationY);
            drawingRectangle.Width = (int)(baseHalfWidth * 2);
            drawingRectangle.Height = (int)(baseHalfHeight * 2);
        }

        //public Vector2 GetPos(float translationX, float translationY)
        //{
        //    Vector2 v = new Vector2();
        //    v.X = drawingRectangle.X;
        //    v.Y = drawingRectangle.Y;
        //    return v;
        //}


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
