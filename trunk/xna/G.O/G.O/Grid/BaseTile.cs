using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ION
{
    class BaseTile : ResourceTile
    {

        private Color baseColor = new Color();

        public BaseTile(int indexX, int indexY, int owner)
        {
            this.indexX = indexX;
            this.indexY = indexY;
            this.owner = owner;
            this.nextOwner = owner;
        }

        public override void draw(int translationX, int translationY)
        {
            //GO.primitiveBatch.Begin(PrimitiveType.LineList);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth) + 1, (visualY * baseHalfHeight) + translationY + (baseHalfHeight)), Color.Brown);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (visualX * baseHalfWidth) + translationX, (visualY * baseHalfHeight) + translationY + (baseHalfHeight*2) - 1), Color.Brown);
            ////GO.primitiveBatch.AddVertex(new Vector2(GO.baseHalfWidth + (visualX * baseHalfWidth) + translationX + (baseHalfWidth), (visualY * baseHalfHeight) + translationY + (baseHalfHeight)),Color.Brown);
            ////GO.primitiveBatch.AddVertex(new Vector2(GO.baseHalfWidth + (visualX * baseHalfWidth) + translationX, (visualY * baseHalfHeight) + translationY), Color.Brown);
            //GO.primitiveBatch.End();

            //GO.primitiveBatch.Begin(PrimitiveType.LineList);
            ////GO.primitiveBatch.AddVertex(new Vector2(GO.baseHalfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth), (visualY * baseHalfHeight) + translationY + (baseHalfHeight)), Color.Brown);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (visualX * baseHalfWidth) + translationX, (visualY * baseHalfHeight) + translationY + (baseHalfHeight*2) -1), Color.Brown);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (visualX * baseHalfWidth) + translationX + (baseHalfWidth) - 1, (visualY * baseHalfHeight) + translationY + (baseHalfHeight)), Color.Brown);
            ////GO.primitiveBatch.AddVertex(new Vector2(GO.baseHalfWidth + (visualX * baseHalfWidth) + translationX, (visualY * baseHalfHeight) + translationY), Color.Brown);
            //GO.primitiveBatch.End();

            //GO.primitiveBatch.Begin(PrimitiveType.LineList);
            ////GO.primitiveBatch.AddVertex(new Vector2(GO.baseHalfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth), (visualY * baseHalfHeight) + translationY + (baseHalfHeight)), Color.Brown);
            ////GO.primitiveBatch.AddVertex(new Vector2(GO.baseHalfWidth + (visualX * baseHalfWidth) + translationX, (visualY * baseHalfHeight) + translationY + baseHalfHeight), Color.Brown);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (visualX * baseHalfWidth) + translationX + (baseHalfWidth) - 1, (visualY * baseHalfHeight) + translationY + (baseHalfHeight)), Color.Brown);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (visualX * baseHalfWidth) + translationX, (visualY * baseHalfHeight) + translationY + 1), Color.Brown);
            //GO.primitiveBatch.End();

            //GO.primitiveBatch.Begin(PrimitiveType.LineList);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth) + 1, (visualY * baseHalfHeight) + translationY + (baseHalfHeight)), Color.Brown);
            ////GO.primitiveBatch.AddVertex(new Vector2(GO.baseHalfWidth + (visualX * baseHalfWidth) + translationX, (visualY * baseHalfHeight) + translationY + baseHalfHeight), Color.Brown);
            ////GO.primitiveBatch.AddVertex(new Vector2(GO.baseHalfWidth + (visualX * baseHalfWidth) + translationX + (baseHalfWidth), (visualY * baseHalfHeight) + translationY + (baseHalfHeight)),Color.Brown);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (visualX * baseHalfWidth) + translationX, (visualY * baseHalfHeight) + translationY + 1), Color.Brown);
            //GO.primitiveBatch.End();

            ION.spriteBatch.Begin();
            ION.spriteBatch.Draw(Images.baseImage, new Rectangle(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth), (visualY * baseHalfHeight) + translationY - (baseHalfWidth), baseHalfWidth * 2, baseHalfWidth * 2), getBaseColor(owner));
            ION.spriteBatch.End();
        }

        private Color getBaseColor(int owner)
        {
            if (owner == Players.NEUTRAL)
            {
                baseColor.R = 255;
                baseColor.G = 255;
                baseColor.B = 255;

                baseColor.A = 255;
            }

            else if (owner == Players.PLAYER1)
            {
                baseColor.R = 20;
                baseColor.G = 20;
                baseColor.B = 255;

                //tileColor.A = (byte)(charge * 255);
                baseColor.A = 255;
            }
            else if (owner == Players.PLAYER2)
            {
                baseColor.R = 255;
                baseColor.G = 20;
                baseColor.B = 20;

                //tileColor.A = (byte)(charge * 255);
                baseColor.A = 255;
            }

            return baseColor;
        }


    }
}
