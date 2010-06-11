using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ION.UI
{
    class CashFlowDisplay : GUIComponent
    {
        Vector2 position;
        Color opacity;
        float amount;

        public CashFlowDisplay(int screenX, int screenY, float amount) : base(screenX,screenY,Images.white1px)
        {
            position = new Vector2(screenX, screenY);
            opacity = Color.Red;
            this.amount = amount;
        }
        public override void draw()
        {
            ION.spriteBatch.DrawString(Fonts.font, amount.ToString(), position, opacity);
 
            if (opacity.A > 5)
            {
                opacity.A -= 10;
                position.Y += 10;
            }
            else
            {
                GUIManager.statusBar.remove(this);
            }
        }
        public override void offset(int screenX, int screenY)
        {
            position.X += screenX;
            position.Y += screenY;
        }
    }
}