using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ION
{
    public class BallUnit : Unit
    {

        public BallUnit() : base(-1) //Sending an invalid number to the base class as a test, I think this constructor in only used to deserialize into after
        {
            destination = new Queue<Tile>();

            pos = new Vector2(ION.halfWidth - (scale / 2), -(scale / 4));
            targetPos = new Vector2(500, 500);

            movementSpeed = 2f;

        }

        public BallUnit(Vector2 newPos, Vector2 newTarget, int owner) : base(owner)
        {
            pos = newPos;
            targetPos = newTarget;

            movementSpeed = 2f;
        }


        public override void draw(float x, float y)
        {
            //virtualPos.X = ((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x);
            //virtualPos.Y = ((pos.Y) * (scale / 15.0f)) + (y);
            //ION.spriteBatch.Begin();
            //ION.spriteBatch.Draw(Images.unitImage, new Rectangle((int)virtualPos.X, (int)virtualPos.Y, (int)(baseHalfWidth * 2), (int)(baseHalfWidth * 2)), Color.White);
            //ION.spriteBatch.End();
            //virtualPos.X += baseHalfWidth;
            //virtualPos.Y += baseHalfWidth * 1.5f;

            //if (selected)
            //{
                //NORMAL TEXTURE PROPORTIONS
                ION.spriteBatch.Draw(Images.getUnitImage(owner,(int)facing,selected), new Rectangle((int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x)), (int)(((pos.Y) * (scale / 15.0f)) + (y) + (baseHalfHeight * 2)), (int)(baseHalfWidth * 2), (int)(baseHalfHeight * 4)), Color.White);

                //EGG TEXTURE PROPORTIONS
                //ION.spriteBatch.Draw(Images.blueUnitChargeImage, new Rectangle((int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x)), (int)(((pos.Y) * (scale / 15.0f)) + (y) + (scale * 0.5f)), (int)(baseHalfWidth * 2), (int)(baseHalfWidth * 2)), Color.White);
            //}
            //else
            //{
            //    //NORMAL TEXTURE PROPORTIONS
            //    ION.spriteBatch.Draw(Images.blueUnitImage, new Rectangle((int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x)), (int)(((pos.Y) * (scale / 15.0f)) + (y) + (baseHalfHeight * 2)), (int)(baseHalfWidth * 2), (int)(baseHalfHeight * 4)), Color.White);

            //    //EGG TEXTURE PROPORTIONS
            //    //ION.spriteBatch.Draw(Images.blueUnitImage, new Rectangle((int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x)), (int)(((pos.Y) * (scale / 15.0f)) + (y) + (scale * 0.5f)), (int)(baseHalfWidth * 2), (int)(baseHalfWidth * 2)), Color.White);
            //}

        }



        internal void Deserialize(System.IO.MemoryStream memoryStream)
        {
            throw new NotImplementedException();
        }
    }
}
