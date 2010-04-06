using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ION
{
    class BallUnit : Unit
    {

        public BallUnit()
        {
            pos = new Vector2(ION.halfWidth - (scale / 2), -(scale / 4));
            targetPos = new Vector2(500, 500);

            movementSpeed = 2;
        }

        public override void draw(float x, float y, float width, float height)
        {
            ION.spriteBatch.Begin();
            ION.spriteBatch.Draw(Images.blueUnitImage, new Rectangle((int)((pos.X - ION.halfWidth) * (scale / 15.0f)) + ION.halfWidth + (int)x, (int)((pos.Y) * (scale / 15.0f)) + (int)y, (int)(baseHalfWidth * 2), (int)(baseHalfWidth * 2)), Color.White);
            ION.spriteBatch.End(); 
        }
    }
}
