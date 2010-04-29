using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ION.UI
{
    class StrategyInfo : GUIComponent
    {

        Vector2 position;
        String emptyString = "";

        public StrategyInfo(int screenX, int screenY)
            : base(screenX, screenY, Images.white1px)
        {
            position = new Vector2(screenX, screenY);

        }

        public override void draw()
        {
            ION.spriteBatch.DrawString(Fonts.small, "Dynamics: "+StateTest.get().strategies[StateTest.get().strategy].name, position, Color.Gray);
        }

        public override void offset(int screenX, int screenY)
        {
            position.X += screenX;
            position.Y += screenY;
        }

    }
}
