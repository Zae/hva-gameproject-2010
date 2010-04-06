using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ION
{
    public abstract class Unit
    {
        private Colors hitmapColor = new Colors();

        protected int owner;

        protected int health = 100;
        //protected int tileX;
        //protected int tileY;
        protected Vector2 pos, targetPos;//replaced two int values with a 2d vector


        protected int inTileX;
        protected int inTileY;

        public abstract void draw(float x, float y, float width, float height);



        // new
        protected float movementSpeed;
        protected float captureSpeed;

        protected static float scale = 15;

        public static float baseHalfWidth = baseHalfWidthConstant * scale;
        public static float baseHalfHeight = baseHalfHeightConstant * scale;
        
        private const float baseHalfWidthConstant = 3;
        private const float baseHalfHeightConstant = 1;

        protected float visualZ;
        protected float visualX;
        protected float visualY;
        
        public void Update()
        {
            move();
        }

        //move units towards their target
        void move()
        {
            //if not at target
            if (pos != targetPos)
            {
                Vector2 temp = targetPos - pos;
                if (temp.Length() > movementSpeed)//move toward target at speed
                {
                    //normalize the length to the of the direction the unit is moving
                    temp.Normalize();
                    //multiply by the units speed
                    temp = temp * movementSpeed;
                    //move the unit by that amount
                    pos += temp;
                }
                else
                {
                    pos = targetPos;//pop to target
                }
            }
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
                baseHalfHeight = baseHalfHeightConstant * scale;
            }

        }

        public float GetScale()
        {
            return baseHalfWidthConstant;
        }
        // new

    }
}
