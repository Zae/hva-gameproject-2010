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

        public BallUnit()
        {
            pos = new Vector2(ION.halfWidth - (scale / 2), -(scale / 4));
            targetPos = new Vector2(500, 500);

            movementSpeed = 5f;

        }

        public BallUnit(Vector2 newPos, Vector2 newTarget)
        {
            pos = newPos;
            targetPos = newTarget;

            movementSpeed = 5f;


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

            if (selected)
            {
                ION.spriteBatch.Begin();

                //NORMAL TEXTURE PROPORTIONS
                ION.spriteBatch.Draw(Images.blueUnitChargeImage, new Rectangle((int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x)), (int)(((pos.Y) * (scale / 15.0f)) + (y)+(baseHalfHeight*2)), (int)(baseHalfWidth * 2), (int)(baseHalfHeight * 4 )), Color.White);
                
                ////EGG TEXTURE PROPORTIONS
                //ION.spriteBatch.Draw(Images.blueUnitChargeImage, new Rectangle((int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x)), (int)(((pos.Y) * (scale / 15.0f)) + (y) + (scale * 0.5f)), (int)(baseHalfWidth * 2), (int)(baseHalfWidth * 2)), Color.White);
               
                ION.spriteBatch.End();
            }
            else
            {
                ION.spriteBatch.Begin();

                //NORMAL TEXTURE PROPORTIONS
                ION.spriteBatch.Draw(Images.blueUnitImage, new Rectangle((int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x)), (int)(((pos.Y) * (scale / 15.0f)) + (y) + (baseHalfHeight * 2)), (int)(baseHalfWidth * 2), (int)(baseHalfHeight * 4)), Color.White);

                ////EGG TEXTURE PROPORTIONS
                //ION.spriteBatch.Draw(Images.blueUnitImage, new Rectangle((int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x)), (int)(((pos.Y) * (scale / 15.0f)) + (y) + (scale * 0.5f)), (int)(baseHalfWidth * 2), (int)(baseHalfWidth * 2)), Color.White);
               
                ION.spriteBatch.End();
            }
        }



        internal void Deserialize(System.IO.MemoryStream memoryStream)
        {
            throw new NotImplementedException();
        }
    }
}
