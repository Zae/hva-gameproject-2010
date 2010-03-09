using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace G.O
{
    public abstract class Tile
    {

        public abstract void draw(int x, int y, SpriteBatch spriteBatch);

        public abstract void update();

    }
}
