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

        public int health = 100;
        public static int cost = 250;

        //animation test
        private int frame = 0;
        private int counter = 0;


        public BallUnit() : base(-1,-1) //Sending an invalid number to the base class as a test, I think this constructor in only used to deserialize into after
        {
            destination = new Queue<Tile>();

            pos = new Vector2(ION.halfWidth - (scale / 2), -(scale / 4));
            targetPos = new Vector2(500, 500);

            movementSpeed = 2f;
        }

        public BallUnit(Vector2 newPos, Vector2 newTarget, int owner, int id) : base(owner,id)
        {
            pos = newPos;
            targetPos = newTarget;

            BaseTile playerBase = Grid.getPlayerBase(owner);
            inTileX = playerBase.getTileX();
            inTileY = playerBase.getTileY();

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

            //animation test code
            counter++;

            if (counter > 0)
            {
                frame = 0;
            }
            if (counter > 4)
            {
                frame = 1;
            }
            if (counter > 10)
            {
                frame = 2;
            }
            if (counter > 14)
            {
                counter = 0;
            }



            if (selected && frame < 2)
            {
                //do not remove
                //ION.spriteBatch.Draw(Images.getUnitImage(owner, (int)facing, selected), new Rectangle((int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x)), (int)(((pos.Y) * (scale / 15.0f)) + (y) + (baseHalfHeight * 2)), (int)(baseHalfWidth * 2), (int)(baseHalfHeight * 4)), Color.White);

                 ION.spriteBatch.Draw(Images.unit_selected_shooting[owner - 1, (int)facing, frame], new Rectangle((int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x)), (int)(((pos.Y) * (scale / 15.0f)) + (y) + (baseHalfHeight * 2)), (int)(baseHalfWidth * 2), (int)(baseHalfHeight * 4)), Color.White);
            }
            else if (!selected && frame < 2)
            {
                ION.spriteBatch.Draw(Images.unit_shooting[owner - 1, (int)facing, frame], new Rectangle((int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x)), (int)(((pos.Y) * (scale / 15.0f)) + (y) + (baseHalfHeight * 2)), (int)(baseHalfWidth * 2), (int)(baseHalfHeight * 4)), Color.White);
            }

            if (frame >= 2)
            {
                //do not remove
                ION.spriteBatch.Draw(Images.getUnitImage(owner, (int)facing, selected), new Rectangle((int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x)), (int)(((pos.Y) * (scale / 15.0f)) + (y) + (baseHalfHeight * 2)), (int)(baseHalfWidth * 2), (int)(baseHalfHeight * 4)), Color.White);

                //ION.spriteBatch.Draw(Images.unit_selected_shooting[owner - 1, (int)facing, frame], new Rectangle((int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x)), (int)(((pos.Y) * (scale / 15.0f)) + (y) + (baseHalfHeight * 2)), (int)(baseHalfWidth * 2), (int)(baseHalfHeight * 4)), Color.White);
            }
            //else
            //{
            //    ION.spriteBatch.Draw(Images.unit_shooting[owner - 1, (int)facing, frame], new Rectangle((int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x)), (int)(((pos.Y) * (scale / 15.0f)) + (y) + (baseHalfHeight * 2)), (int)(baseHalfWidth * 2), (int)(baseHalfHeight * 4)), Color.White);
            //}

        }

        internal void Deserialize(System.IO.MemoryStream memoryStream)
        {
            throw new NotImplementedException();
        }
    }
}
