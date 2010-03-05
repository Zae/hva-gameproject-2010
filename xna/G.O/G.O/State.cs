using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace G.O
{
    public class State
    {

        public State()
        {

        }

        public virtual void draw(SpriteBatch spritebatch)
        {
            Console.WriteLine("update State");
        }

        public virtual void update()
        {
            Console.WriteLine("update State");
        }

    }
}
