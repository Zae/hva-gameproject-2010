using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace G.O
{
    class MountainTile : Tile
    {
        public override void draw(int x, int y, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(Fonts.font, "Mountain Tile", new Vector2(x, y), Color.Black);
            spriteBatch.End();
        }

        public override void update()
        {
        }

    }
}
