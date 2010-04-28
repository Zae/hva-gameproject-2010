using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ION.UI
{
    class ResourceCounter : GUIComponent
    {

        Vector2 position;
        String emptyString = "";

        public ResourceCounter(int screenX, int screenY) : base(screenX,screenY,Images.white1px)
        {
            position = new Vector2(screenX, screenY);
            
        }

        public override void draw()
        {
            ION.spriteBatch.DrawString(Fonts.font, emptyString+(int)Grid.resources, position,Color.Gray);
        }

        public override void offset(int screenX, int screenY)
        {
            position.X += screenX;
            position.Y += screenY;
        }

    }
}
