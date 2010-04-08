using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ION
{
    public abstract class State
    {

        public State()
        {

        }

        public abstract void draw();
       
        public abstract void update(int ellapsed);

        public abstract void focusLost();

        public abstract void focusGained();

        public Boolean mouseIn(int mx, int my, Rectangle rect)
        {
            if ((mx > rect.X && mx < (rect.X + rect.Width)) && (my > rect.Y && my < (rect.Y + rect.Height)))
            {
                return true;
            }

            return false;

        }
        

    }
}
