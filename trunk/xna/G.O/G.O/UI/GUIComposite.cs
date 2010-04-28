using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ION.UI
{
    class GUIComposite : GUIComponent
    {

        private List<GUIComponent> children;

        public GUIComposite(int screenX, int screenY, Texture2D imageNormal): base(screenX, screenY, imageNormal)
        {
            children = new List<GUIComponent>();
        }

        public void add(GUIComponent newChild)
        {
            children.Add(newChild);
            newChild.offset(screenRectangle.X, screenRectangle.Y);
        }

        public void remove(GUIComponent toRemove)
        {
            children.Remove(toRemove);
        }

        public override void draw() 
        {
            base.draw();
            foreach(GUIComponent guic in children) 
            {
                guic.draw();
            }
        }

        public void clear()
        {
            children.Clear();
        }

        public override bool handleMouse(int x, int y)
        {
            return base.handleMouse(x, y);
        } 

    }
}
