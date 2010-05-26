﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ION
{
    public class Robot : Unit
    {

       //public int health = 1000;
        public static int cost = 250;

        //animation test
        private int FiringFrame = 0;
        private int FiringCounter = 0;
        private int UnderFireFrame = 0;
        private int UnderFireCounter = 0;

        public Robot() : base(-1,-1) //Sending an invalid number to the base class as a test, I think this constructor in only used to deserialize into after
        {
            destination = new Queue<Tile>();

            pos = new Vector2(ION.halfWidth - (scale / 2), -(scale / 4));
            targetPos = new Vector2(500, 500);

            movementSpeed = 2f;
        }

        public Robot(Vector2 newPos, Vector2 newTarget, int owner, int id) : base(owner,id)
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
            //drawFiringAnimation(float x, float y)

            ION.spriteBatch.Draw(Images.white1px, new Rectangle((int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x) + ((int)Tile.baseHalfWidth * 0.63)), (int)(((pos.Y) * (scale / 15.0f)) + (y) + (baseHalfHeight * 2)+(int)(baseHalfHeight*0.55)), (int)(baseHalfWidth * 0.75), (int)(baseHalfHeight * 3)), Color.Gray);

            if (selected)
            {
                ION.spriteBatch.Draw(Images.selectionBoxBack, new Rectangle((int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x)), (int)(((pos.Y) * (scale / 15.0f)) + (y) + (baseHalfHeight * 2)), (int)(baseHalfWidth * 2), (int)(baseHalfHeight * 4)), Color.White);
            }

            if (!drawFiringAnimation(x, y))
            {
                ION.spriteBatch.Draw(Images.getUnitImage(owner, (int)facing, true), new Rectangle((int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x)), (int)(((pos.Y) * (scale / 15.0f)) + (y) + (baseHalfHeight * 2)), (int)(baseHalfWidth * 2), (int)(baseHalfHeight * 4)), Color.White);
            }

     
            if (selected)
            {
                ION.spriteBatch.Draw(Images.selectionBoxFront, new Rectangle((int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x)), (int)(((pos.Y) * (scale / 15.0f)) + (y) + (baseHalfHeight * 2)), (int)(baseHalfWidth * 2), (int)(baseHalfHeight * 4)), Color.White);
            }

            
            

            drawUnderFireAnimation(x,y);
        }

        private void drawUnderFireAnimation(float x, float y)
        {
            if (underFire)
            {
                //animation test code
                UnderFireCounter++;

                if (UnderFireCounter > 0)
                {
                    UnderFireFrame = 0;
                }
                if (UnderFireCounter > 14)
                {
                    UnderFireFrame = 1;            
                }
                if (UnderFireCounter > 25)
                {
                    UnderFireCounter = 0;
                    underFire = false;
                    return;
                }

                ION.spriteBatch.Draw(Images.bulletImpact[UnderFireFrame], new Rectangle((int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x)), (int)(((pos.Y) * (scale / 15.0f)) + (y) + (baseHalfHeight * 0.5)), (int)(baseHalfWidth * 0.5), (int)(baseHalfHeight * 4)), Color.White);
                
            }
        }

        private bool drawFiringAnimation(float x, float y)
        {
            if (firing)
            {
                //animation test code
                FiringCounter++;

                if (FiringCounter > 0)
                {
                    FiringFrame = 0;
                }
                if (FiringCounter > 4)
                {
                    FiringFrame = 1;
                }
                if (FiringCounter > 10)
                {
                    FiringFrame = 2;
                }
                if (FiringCounter > 14)
                {
                    FiringCounter = 0;
                }

                if (FiringFrame < 2)
                {
                    //do not remove
                    //ION.spriteBatch.Draw(Images.getUnitImage(owner, (int)facing, selected), new Rectangle((int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x)), (int)(((pos.Y) * (scale / 15.0f)) + (y) + (baseHalfHeight * 2)), (int)(baseHalfWidth * 2), (int)(baseHalfHeight * 4)), Color.White);

                    ION.spriteBatch.Draw(Images.unit_selected_shooting[owner - 1, (int)facing, FiringFrame], new Rectangle((int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x)), (int)(((pos.Y) * (scale / 15.0f)) + (y) + (baseHalfHeight * 2)), (int)(baseHalfWidth * 2), (int)(baseHalfHeight * 4)), Color.White);
                }

                if (FiringFrame >= 2)
                {
                    //do not remove
                    ION.spriteBatch.Draw(Images.getUnitImage(owner, (int)facing, true), new Rectangle((int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x)), (int)(((pos.Y) * (scale / 15.0f)) + (y) + (baseHalfHeight * 2)), (int)(baseHalfWidth * 2), (int)(baseHalfHeight * 4)), Color.White);

                    //ION.spriteBatch.Draw(Images.unit_selected_shooting[owner - 1, (int)facing, FiringFrame], new Rectangle((int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x)), (int)(((pos.Y) * (scale / 15.0f)) + (y) + (baseHalfHeight * 2)), (int)(baseHalfWidth * 2), (int)(baseHalfHeight * 4)), Color.White);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        internal void Deserialize(System.IO.MemoryStream memoryStream)
        {
            throw new NotImplementedException();
        }
    }
}
