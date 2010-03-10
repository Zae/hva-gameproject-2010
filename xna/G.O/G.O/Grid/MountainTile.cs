using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GO
{
    class MountainTile : Tile
    {
        //public override void draw(int x, int y, SpriteBatch spriteBatch)
        //{
        //    spriteBatch.Begin();
        //    spriteBatch.DrawString(Fonts.font, "(M :z=" + indexZ + ":x=" + indexX + ":y=" + indexY + ")", new Vector2(x, y), Color.Black);
        //    spriteBatch.End();
        //}
        public override void draw(int translationX, int translationY)
        {
            GO.spriteBatch.Begin();
            Vector2 location = new Vector2(GO.halfWidth + (indexX * tileWidth) + translationX - 40, (indexY * tileHeight) + translationY + tileHeight);
            GO.spriteBatch.DrawString(Fonts.font, "(M :z=" + indexZ + ":x=" + indexX + ":y=" + indexY + ")", location, Color.Black);
            GO.spriteBatch.End();

            GO.primitiveBatch.Begin(PrimitiveType.LineList);
            GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (indexX * tileWidth) + translationX - (tileWidth) + 1, (indexY * tileHeight) + translationY + (tileHeight)), Color.Brown);
            GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (indexX * tileWidth) + translationX, (indexY * tileHeight) + translationY + (tileHeight*2) - 1), Color.Brown);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (indexX * tileWidth) + translationX + (tileWidth), (indexY * tileHeight) + translationY + (tileHeight)),Color.Brown);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (indexX * tileWidth) + translationX, (indexY * tileHeight) + translationY), Color.Brown);
            GO.primitiveBatch.End();

            GO.primitiveBatch.Begin(PrimitiveType.LineList);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (indexX * tileWidth) + translationX - (tileWidth), (indexY * tileHeight) + translationY + (tileHeight)), Color.Brown);
            GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (indexX * tileWidth) + translationX, (indexY * tileHeight) + translationY + (tileHeight*2) -1), Color.Brown);
            GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (indexX * tileWidth) + translationX + (tileWidth) - 1, (indexY * tileHeight) + translationY + (tileHeight)), Color.Brown);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (indexX * tileWidth) + translationX, (indexY * tileHeight) + translationY), Color.Brown);
            GO.primitiveBatch.End();

            GO.primitiveBatch.Begin(PrimitiveType.LineList);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (indexX * tileWidth) + translationX - (tileWidth), (indexY * tileHeight) + translationY + (tileHeight)), Color.Brown);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (indexX * tileWidth) + translationX, (indexY * tileHeight) + translationY + tileHeight), Color.Brown);
            GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (indexX * tileWidth) + translationX + (tileWidth) - 1, (indexY * tileHeight) + translationY + (tileHeight)), Color.Brown);
            GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (indexX * tileWidth) + translationX, (indexY * tileHeight) + translationY + 1), Color.Brown);
            GO.primitiveBatch.End();

            GO.primitiveBatch.Begin(PrimitiveType.LineList);
            GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (indexX * tileWidth) + translationX - (tileWidth) + 1, (indexY * tileHeight) + translationY + (tileHeight)), Color.Brown);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (indexX * tileWidth) + translationX, (indexY * tileHeight) + translationY + tileHeight), Color.Brown);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (indexX * tileWidth) + translationX + (tileWidth), (indexY * tileHeight) + translationY + (tileHeight)),Color.Brown);
            GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (indexX * tileWidth) + translationX, (indexY * tileHeight) + translationY + 1), Color.Brown);
            GO.primitiveBatch.End();
            
        }


        public override void update()
        {
        }

    }
}
