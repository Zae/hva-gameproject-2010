using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ION.UI
{
    class Label : GUIComponent
    {
        Vector2 position;
        string message;

        public Label(int screenX, int screenY, string message) : base(screenX,screenY,Images.white1px)
        {
            position = new Vector2(screenX, screenY);
            this.message = message;
        }

        public override void draw()
        {
            ION.spriteBatch.DrawString(Fonts.font, message, position,Color.Gray);
        }

        public override void offset(int screenX, int screenY)
        {
            position.X += screenX;
            position.Y += screenY;
        }
    }
}
