using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace G.O
{
    public class ResourceTile : Tile
    {

        public int owner = Players.NEUTRAL;

        public int charge = 0;

        public const int MAX_CHARGE = 100;


        public override void draw(int x, int y, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(Fonts.font, "(N :z="+indexZ+":x="+indexX+":y="+indexY+")", new Vector2(x, y), Color.Black);
            spriteBatch.End();
        }

        public override void draw(int translation, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            Vector2 location = new Vector2(GO.halfWidth + (indexX * tileWidth), (indexY * tileHeight));
            spriteBatch.DrawString(Fonts.font, "(N :z=" + indexZ + ":x=" + indexX + ":y=" + indexY + ")", location, Color.Black);
            spriteBatch.End();
        }

        public override void update()
        {
            
        }




    }
}
