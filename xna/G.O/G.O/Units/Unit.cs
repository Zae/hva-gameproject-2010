using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;

namespace ION
{
    public abstract class Unit : Serializable
    {
        private Colors hitmapColor = new Colors();

        protected int owner;

        protected int health = 100;
        //protected int tileX;
        //protected int tileY;
        protected Vector2 pos, targetPos, virtualPos;//replaced two int values with a 2d vector


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

        public bool selected = false;



        public void Update(float translationX, float translationY)
        {
            if (pos != targetPos)
            {
                move();
                //map.mouseLeftPressed(mouseState.X, mouseState.Y, translationX, translationY);
            }

            //will move this to the above if statement when the unit know its tile
            virtualPos.X = ((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (translationX) + (baseHalfWidth * 1.1f);
            virtualPos.Y = ((pos.Y) * (scale / 15.0f)) + (translationY) + (baseHalfWidth * 1.6f);


        }

        public void UpdateTile(Vector2 newInTile)
        {
            inTileX = (int)newInTile.X;
            inTileY = (int)newInTile.Y;



            //Vector2 tilePos = map.GetTile(pos.X, pos.Y, translationX, translationX);
            //inTileX = tilePos.X;
            //inTileY = tilePos.Y;
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

        public Byte[] SetTarget(Vector2 newTarget)
        {
            targetPos = newTarget;
            
            return Serializer.Serialize(this);
        }
        public Vector2 GetTarget()
        {
            return targetPos;
        }

        public Vector2 GetVirtualPos()
        {
            return virtualPos;
        }
        // new

        public Vector2 GetTile()
        {
            return new Vector2(inTileX, inTileY);
        }

        #region Serializable Members

        public MemoryStream Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            //instead of calling GetTile and writing the X and Y values seperate
            //I cut the middle man and directly write inTileX and inTileY.
            bw.Write(inTileX);
            bw.Write(inTileY);
            bw.Write(this.targetPos.X);
            bw.Write(this.targetPos.Y);

            return ms;
        }

        public void Deserialize(MemoryStream inData)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
