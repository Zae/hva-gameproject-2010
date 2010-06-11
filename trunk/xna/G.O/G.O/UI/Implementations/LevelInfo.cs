using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ION.UI
{
    class LevelInfo : GUIComponent
    {

        Vector2 position;
        String emptyString = "";

        public LevelInfo(int screenX, int screenY)
            : base(screenX, screenY, Images.white1px)
        {
            position = new Vector2(screenX, screenY);

        }

        public override void draw()
        {
            //ION.spriteBatch.DrawString(Fonts.small, "Collect "+Grid.toCollect+" ION, "+(int)Grid.get().totalCollected+" so far", position, Color.Gray);
            ////ION.spriteBatch.DrawString(Fonts.small, "Level: " + StateTest.get().levels[StateTest.get().level], position, Color.Gray);

            if (StateTest.victoryCondition == StateTest.ANNIHILATION)
            {
                ION.spriteBatch.DrawString(Fonts.font, "Destroy all enemy bases!", position, Color.Gray);
            }
            else if (StateTest.victoryCondition == StateTest.RESOURCE_RACE)
            {
                ION.spriteBatch.DrawString(Fonts.font, "Reach "+(int)Grid.toCollect+" ION!", position, Color.Gray);
            }
        }

        public override void offset(int screenX, int screenY)
        {
            position.X += screenX;
            position.Y += screenY;
        }

    }
}
