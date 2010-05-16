using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ION.UI
{
    class Button : GUIComponent
    {

        private bool mouseOver = false;
        private ButtonHandler handler;

        public Button(int x, int y, Texture2D imageNormal, ButtonHandler handler) : base(x, y, imageNormal)
        {
            this.handler = handler;
        }

        public override void draw()
        {
            base.draw();

            if (!mouseOver)
            {
                ION.spriteBatch.Draw(Images.buttonOverlay, screenRectangle, Color.White);
            }
        }

        public override bool handleMouse(Point evalPoint, bool leftPressed)
        {
            if(screenRectangle.Contains(evalPoint)) 
            {
                mouseOver = true;

                if (leftPressed)
                {
                    handler.run();
                }

                return true;
            }
            else 
            {
                mouseOver = false;
                return false;
            }

        }

    }
}
