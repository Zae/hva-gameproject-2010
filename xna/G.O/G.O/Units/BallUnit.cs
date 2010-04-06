using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ION.Units
{
    class BallUnit : Unit
    {

        public BallUnit(int owner)
        {
            this.owner = owner;
        }


        public override void draw(int x, int y, int width, int height)
        {
            ION.spriteBatch.Begin();

            ION.spriteBatch.Draw(getAppropriateImage(), new Rectangle(x,y,width,height), Color.White);



            ION.spriteBatch.End();
        }

        private Texture2D getAppropriateImage()
        {
            if (owner == Players.PLAYER1)
            {
                return Images.blueUnitImage;
            }
            else if (owner == Players.PLAYER2)
            {
                return Images.redUnitImage;
            }
            return null;
        }

    }
}
