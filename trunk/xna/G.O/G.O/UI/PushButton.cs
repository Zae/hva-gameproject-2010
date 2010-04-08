using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ION.UI
{
    class PushButton 
    {
        public Texture2D normal;
        public Texture2D mouseOver;
        public Texture2D onPress;

        public Rectangle dimensions;
        
        public PushButton(int x, int y, int width, int height, Texture2D normal, Texture2D mouseOver, Texture2D onPress)
        {
           
            dimensions = new Rectangle(x, y, width, height);
            this.normal = normal;
            this.mouseOver = mouseOver;
            this.onPress = onPress;
        }

        public virtual void onMouseDown()
        {

        }
        public virtual void onMouseOver()
        {

        }
        public virtual void onMouseRelease()
        {

        }

        public void draw()
        {

        }

        public void update()
        {

        }



    }
}
