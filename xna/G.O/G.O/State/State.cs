using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace G.O
{
    public abstract class State
    {

        public State()
        {

        }

        public abstract void draw(SpriteBatch spriteBatch);
       
        public abstract void update(int ellapsed);

        public abstract void focusLost();

        public abstract void focusGained();
        

    }
}
