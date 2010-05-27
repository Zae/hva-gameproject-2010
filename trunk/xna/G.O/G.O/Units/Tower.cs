using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ION
{
    public class Tower : Unit
    {
        public static int cost = 500;

        //animation test
        private int FiringFrame = 0;
        private int FiringCounter = 0;

        private int UnderFireFrame = 0;
        private int UnderFireCounter = 0;
        public int UnderFireOffsetX = 0;
        public int UnderFireOffsetY = 0;

        
        public Tower(Vector2 newPos, int owner, int id) : base(owner,id)
        {
            damage = 2;

            pos = newPos;

            BaseTile playerBase = Grid.getPlayerBase(owner);
            inTileX = playerBase.getTileX();
            inTileY = playerBase.getTileY();

            movementSpeed = 2f;
        }

        public override void move()
        {
            EmptyWayPoints();
        }

        public override void draw(float x, float y)
        {
            selectionRectangle.X = (int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x) + (Tile.baseHalfWidth * 0.63));
            selectionRectangle.Y = (int)(((pos.Y) * (scale / 15.0f)) + (y) + (baseHalfHeight * 2) + (baseHalfHeight * 0.55));
            selectionRectangle.Width = (int)(baseHalfWidth * 0.75);
            selectionRectangle.Height = (int)(baseHalfHeight * 3);

            drawingRectangle.X = (int)(((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (x));
            drawingRectangle.Y = (int)(((pos.Y) * (scale / 15.0f)) + (y) + (baseHalfHeight * 2));
            drawingRectangle.Width = (int)(baseHalfWidth * 2);
            drawingRectangle.Height = (int)(baseHalfHeight * 4);

            //ION.spriteBatch.Draw(Images.white1px, selectionRectangle, Color.Gray);

            if (selected)
            {
                ION.spriteBatch.Draw(Images.selectionBoxBack, drawingRectangle, Color.White);
            }

            if (!drawFiringAnimation(x, y))
            {
                ION.spriteBatch.Draw(Images.getUnitImage(owner, (int)facing, true), drawingRectangle, Color.White);
            }

            if (selected)
            {
                ION.spriteBatch.Draw(Images.selectionBoxFront, drawingRectangle, Color.White);
            }

            drawUnderFireAnimation(x, y);
        }

        private void drawUnderFireAnimation(float x, float y)
        {
            if (underFire)
            {
                if (UnderFireCounter == 0)
                {
                    UnderFireOffsetX = (int)(Tool.unsafeRandom.NextDouble() * selectionRectangle.Width);
                    UnderFireOffsetY = (int)(Tool.unsafeRandom.NextDouble() * selectionRectangle.Height);
                }

                //animation test code
                UnderFireCounter++;

                if (UnderFireCounter > 0)
                {
                    UnderFireFrame = 0;
                }
                if (UnderFireCounter > 5)
                {
                    UnderFireFrame = 1;
                }
                if (UnderFireCounter > 10)
                {
                    UnderFireCounter = 0;
                    underFire = false;
                    return;
                }

                ION.spriteBatch.Draw(Images.bulletImpact[UnderFireFrame], new Rectangle(selectionRectangle.X + UnderFireOffsetX, selectionRectangle.Y + UnderFireOffsetY, (int)(baseHalfWidth * 0.5), (int)(baseHalfHeight)), Color.White);
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

    }
}
