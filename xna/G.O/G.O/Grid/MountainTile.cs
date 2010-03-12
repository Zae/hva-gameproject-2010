﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GO
{
    class MountainTile : Tile
    {

        public MountainTile(int indexX, int indexY)
        {
            this.indexX = indexX;
            this.indexY = indexY;
        }

        public override void drawDebug(int translationX, int translationY)
        {
            GO.spriteBatch.Begin();
            Vector2 location = new Vector2(GO.halfWidth + (visualX * baseWidth) + translationX - 40, (visualY * baseHalfHeight) + translationY + baseHalfHeight);
            GO.spriteBatch.DrawString(Fonts.font, "(z=" + visualZ + ":x=" + visualX + ":y=" + visualY + ")", location, Color.White);
            GO.spriteBatch.End();
        }

        public override void draw(int translationX, int translationY)
        {
            //GO.primitiveBatch.Begin(PrimitiveType.LineList);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (visualX * baseWidth) + translationX - (baseWidth) + 1, (visualY * baseHalfHeight) + translationY + (baseHalfHeight)), Color.Brown);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (visualX * baseWidth) + translationX, (visualY * baseHalfHeight) + translationY + (baseHalfHeight*2) - 1), Color.Brown);
            ////GO.primitiveBatch.AddVertex(new Vector2(GO.baseWidth + (visualX * baseWidth) + translationX + (baseWidth), (visualY * baseHalfHeight) + translationY + (baseHalfHeight)),Color.Brown);
            ////GO.primitiveBatch.AddVertex(new Vector2(GO.baseWidth + (visualX * baseWidth) + translationX, (visualY * baseHalfHeight) + translationY), Color.Brown);
            //GO.primitiveBatch.End();

            //GO.primitiveBatch.Begin(PrimitiveType.LineList);
            ////GO.primitiveBatch.AddVertex(new Vector2(GO.baseWidth + (visualX * baseWidth) + translationX - (baseWidth), (visualY * baseHalfHeight) + translationY + (baseHalfHeight)), Color.Brown);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (visualX * baseWidth) + translationX, (visualY * baseHalfHeight) + translationY + (baseHalfHeight*2) -1), Color.Brown);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (visualX * baseWidth) + translationX + (baseWidth) - 1, (visualY * baseHalfHeight) + translationY + (baseHalfHeight)), Color.Brown);
            ////GO.primitiveBatch.AddVertex(new Vector2(GO.baseWidth + (visualX * baseWidth) + translationX, (visualY * baseHalfHeight) + translationY), Color.Brown);
            //GO.primitiveBatch.End();

            //GO.primitiveBatch.Begin(PrimitiveType.LineList);
            ////GO.primitiveBatch.AddVertex(new Vector2(GO.baseWidth + (visualX * baseWidth) + translationX - (baseWidth), (visualY * baseHalfHeight) + translationY + (baseHalfHeight)), Color.Brown);
            ////GO.primitiveBatch.AddVertex(new Vector2(GO.baseWidth + (visualX * baseWidth) + translationX, (visualY * baseHalfHeight) + translationY + baseHalfHeight), Color.Brown);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (visualX * baseWidth) + translationX + (baseWidth) - 1, (visualY * baseHalfHeight) + translationY + (baseHalfHeight)), Color.Brown);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (visualX * baseWidth) + translationX, (visualY * baseHalfHeight) + translationY + 1), Color.Brown);
            //GO.primitiveBatch.End();

            //GO.primitiveBatch.Begin(PrimitiveType.LineList);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (visualX * baseWidth) + translationX - (baseWidth) + 1, (visualY * baseHalfHeight) + translationY + (baseHalfHeight)), Color.Brown);
            ////GO.primitiveBatch.AddVertex(new Vector2(GO.baseWidth + (visualX * baseWidth) + translationX, (visualY * baseHalfHeight) + translationY + baseHalfHeight), Color.Brown);
            ////GO.primitiveBatch.AddVertex(new Vector2(GO.baseWidth + (visualX * baseWidth) + translationX + (baseWidth), (visualY * baseHalfHeight) + translationY + (baseHalfHeight)),Color.Brown);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (visualX * baseWidth) + translationX, (visualY * baseHalfHeight) + translationY + 1), Color.Brown);
            //GO.primitiveBatch.End();

            GO.spriteBatch.Begin();
            GO.spriteBatch.Draw(Images.mountainImage, new Rectangle(GO.halfWidth + (visualX * baseWidth) + translationX - (baseWidth), (visualY * baseHalfHeight) + translationY - (baseWidth+baseHalfHeight), baseWidth * 2, baseWidth * 2), Color.White);
            GO.spriteBatch.End();          
        }


        public override void update()
        {
        }

    }
}