using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ION.UI
{
    public class GUIComponent
    {

        public Rectangle screenRectangle;

        private Texture2D imageNormal;

        public GUIComponent(int screenX, int screenY, Texture2D imageNormal)
        {
            screenRectangle = new Rectangle();
            screenRectangle.X = screenX;
            screenRectangle.Y = screenY;

            this.imageNormal = imageNormal;
            screenRectangle.Width = this.imageNormal.Width;
            screenRectangle.Height = this.imageNormal.Height;
        }

        public virtual void draw()
        {
            ION.spriteBatch.Draw(imageNormal, screenRectangle, Color.White);
        }

        public virtual void offset(int screenX, int screenY)
        {
            screenRectangle.Offset(screenX, screenY);
        }

        public virtual bool handleMouse(Point evalPoint)
        {
            if (screenRectangle.Contains(evalPoint))
            {
                return true;
            }
            return false;
        }

    }
}
