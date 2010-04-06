using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace ION
{
    class BaseTile : ResourceTile
    {

        private Color baseColor = new Color();

        public BaseTile(int indexX, int indexY, int owner)
        {
            sID++;
            id = sID;
            
            this.indexX = indexX;
            this.indexY = indexY;
            this.owner = owner;
            this.nextOwner = owner;
            this.charge = 999.0f;
            this.nextCharge = 999.0f;
        }

        public override void draw(float translationX, float translationY)
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
            ION.spriteBatch.Draw(Images.baseImage, new Rectangle((int)(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth)), (int)((visualY * baseHalfHeight) + translationY - (baseHalfWidth)), (int)(baseHalfWidth * 2), (int)(baseHalfWidth * 2)), getBaseColor(owner));
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

        public override void donate(float charge)
        {
            //if (nextCharge - charge < 0.0f)
            //{
            //    nextCharge = 0.0f;
            //    Debug.WriteLine("DONATION RESULTS IN  < 0");
            //}
            //else
            //{
            //    nextCharge -= charge;
            //}
        }

        public override void receive(float charge)
        {
            if (nextCharge + charge > 1.0f)
            {
                nextCharge = 1.0f;
                Debug.WriteLine("RECEPTION RESULTS IN > 1");
            }
            else
            {
                nextCharge += charge;
            }
        }

        public override void sustain(float charge, int player)
        {
            if (nextCharge - charge < 0.0f)
            {
                nextCharge = 0.0f;
                nextOwner = player;
            }
            else
            {
                nextCharge -= charge;
            }
        }

        //public override void addCharge(float addition, int player)
        //{
        //    //if (player != owner)
        //    //{
        //    //    if (charge - addition < 0.0f)
        //    //    {
        //    //        nextOwner = player;
        //    //        nextCharge = 0.0000000001f;
        //    //    }
        //    //    else
        //    //    {
        //    //        nextCharge = charge - addition;
        //    //    }
        //    //}
        //    //else
        //    //{

        //    //    if (charge + addition > 1.0f)
        //    //    {
        //    //        nextCharge = 1.0f;
        //    //    }
        //    //    else
        //    //    {
        //    //        nextCharge = charge + addition;
        //    //    }
        //    //}
        //}

        //public override void removeCharge(float addition, int owner)
        //{
        //    //if (charge - addition < 0.0f)
        //    //{
        //    //    nextCharge = 0.0f;
        //    //    nextOwner = Players.NEUTRAL;
        //    //}
        //    //else
        //    //{
        //    //    nextCharge = charge - addition;
        //    //}

        //}


    }
}
