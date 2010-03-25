using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace ION
{
    public class ResourceTile : Tile
    {

        //remembering past interations
        public bool canDonateNW = true;
        public bool canDonateN = true;
        public bool canDonateNE = true;
        public bool canDonateE = true;
        public bool canDonateSE = true;
        public bool canDonateS = true;
        public bool canDonateSW = true;
        public bool canDonateW = true;

        public int owner = Players.NEUTRAL;

        public int nextOwner = Players.NEUTRAL;

        public float charge = 0;
        
        public float nextCharge = 0;

        protected Unit unit = null;

        //This is buggy but for testing
        //private const float minimumFlux = 0.03f;

        public const float MAX_CHARGE = 1.0f;

        private Color tileColor = new Color();

        public ResourceTile()
        {
        }

        public ResourceTile(int indexX, int indexY)
        {
            sID = sID+1;
            id = sID;
            
            this.indexX = indexX;
            this.indexY = indexY;
        }
        public override void drawDebug(int translationX, int translationY)
        {
            ION.spriteBatch.Begin();
            Vector2 location = new Vector2(ION.halfWidth + (visualX * baseHalfWidth) + translationX - 40, (visualY * baseHalfHeight) + translationY + baseHalfHeight);
            //ION.spriteBatch.DrawString(Fonts.font, "(z=" + visualZ + ":x=" + visualX + ":y=" + visualY + ")", location, Color.Black);
            ION.spriteBatch.DrawString(Fonts.font, "charge: "+charge, location, Color.Black);
            ION.spriteBatch.End();
        }

        public bool hasUnit()
        {
            if (unit != null)
            {
                return true;
            }
            return false;
        }

        public void setUnit(Unit unit)
        {
            this.unit = unit;
        }


        



        public override void draw(int translationX, int translationY)
        {


            //GO.primitiveBatch.Begin(PrimitiveType.LineList);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth) + 1, (visualY * baseHalfHeight) + translationY + (baseHalfHeight)), Color.Yellow);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (visualX * baseHalfWidth) + translationX, (visualY * baseHalfHeight) + translationY + (baseHalfHeight * 2) - 1), Color.Yellow);
            ////GO.primitiveBatch.AddVertex(new Vector2(GO.baseHalfWidth + (visualX * baseHalfWidth) + translationX + (baseHalfWidth), (visualY * baseHalfHeight) + translationY + (baseHalfHeight)),Color.Yellow);
            ////GO.primitiveBatch.AddVertex(new Vector2(GO.baseHalfWidth + (visualX * baseHalfWidth) + translationX, (visualY * baseHalfHeight) + translationY), Color.Yellow);
            //GO.primitiveBatch.End();

            //GO.primitiveBatch.Begin(PrimitiveType.LineList);
            ////GO.primitiveBatch.AddVertex(new Vector2(GO.baseHalfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth), (visualY * baseHalfHeight) + translationY + (baseHalfHeight)), Color.Yellow);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (visualX * baseHalfWidth) + translationX, (visualY * baseHalfHeight) + translationY + (baseHalfHeight * 2) - 1), Color.Yellow);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (visualX * baseHalfWidth) + translationX + (baseHalfWidth) - 1, (visualY * baseHalfHeight) + translationY + (baseHalfHeight)), Color.Yellow);
            ////GO.primitiveBatch.AddVertex(new Vector2(GO.baseHalfWidth + (visualX * baseHalfWidth) + translationX, (visualY * baseHalfHeight) + translationY), Color.Yellow);
            //GO.primitiveBatch.End();

            //GO.primitiveBatch.Begin(PrimitiveType.LineList);
            ////GO.primitiveBatch.AddVertex(new Vector2(GO.baseHalfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth), (visualY * baseHalfHeight) + translationY + (baseHalfHeight)), Color.Yellow);
            ////GO.primitiveBatch.AddVertex(new Vector2(GO.baseHalfWidth + (visualX * baseHalfWidth) + translationX, (visualY * baseHalfHeight) + translationY + baseHalfHeight), Color.Yellow);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (visualX * baseHalfWidth) + translationX + (baseHalfWidth) - 1, (visualY * baseHalfHeight) + translationY + (baseHalfHeight)), Color.Yellow);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (visualX * baseHalfWidth) + translationX, (visualY * baseHalfHeight) + translationY + 1), Color.Yellow);
            //GO.primitiveBatch.End();

            //GO.primitiveBatch.Begin(PrimitiveType.LineList);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth) + 1, (visualY * baseHalfHeight) + translationY + (baseHalfHeight)), Color.Yellow);
            ////GO.primitiveBatch.AddVertex(new Vector2(GO.baseHalfWidth + (visualX * baseHalfWidth) + translationX, (visualY * baseHalfHeight) + translationY + baseHalfHeight), Color.Yellow);
            ////GO.primitiveBatch.AddVertex(new Vector2(GO.baseHalfWidth + (visualX * baseHalfWidth) + translationX + (baseHalfWidth), (visualY * baseHalfHeight) + translationY + (baseHalfHeight)),Color.Yellow);
            //GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (visualX * baseHalfWidth) + translationX, (visualY * baseHalfHeight) + translationY + 1), Color.Yellow);
            //GO.primitiveBatch.End();

            tileColor = getAppropriateColor(owner, charge);

            ION.spriteBatch.Begin();

            //if (owner != Players.NEUTRAL)
            //{
            //    ION.spriteBatch.Draw(Images.borderImage, new Rectangle(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth), (visualY * baseHalfHeight) + translationY, baseHalfWidth * 2, baseHalfHeight * 2), tileColor);
            //}


            ION.spriteBatch.Draw(Images.resourceImage, new Rectangle(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth), (visualY * baseHalfHeight) + translationY, baseHalfWidth*2, baseHalfHeight * 2), tileColor);
            
            
            
            ION.spriteBatch.End();

            if (selected)
            {
                ION.primitiveBatch.Begin(PrimitiveType.LineList);
                ION.primitiveBatch.AddVertex(new Vector2(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth) + 1, (visualY * baseHalfHeight) + translationY + (baseHalfHeight)), Color.Red);
                //GO.primitiveBatch.AddVertex(new Vector2(GO.baseHalfWidth + (visualX * baseHalfWidth) + translationX, (visualY * baseHalfHeight) + translationY + (baseHalfHeight * 2) - 1), Color.Yellow);
                ION.primitiveBatch.AddVertex(new Vector2(ION.halfWidth + (visualX * baseHalfWidth) + translationX + (baseHalfWidth), (visualY * baseHalfHeight) + translationY + (baseHalfHeight)), Color.Red);
                //GO.primitiveBatch.AddVertex(new Vector2(GO.baseHalfWidth + (visualX * baseHalfWidth) + translationX, (visualY * baseHalfHeight) + translationY), Color.Yellow);
                ION.primitiveBatch.End();

                ION.primitiveBatch.Begin(PrimitiveType.LineList);
                //GO.primitiveBatch.AddVertex(new Vector2(GO.baseHalfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth) + 1, (visualY * baseHalfHeight) + translationY + (baseHalfHeight)), Color.Yellow);
                ION.primitiveBatch.AddVertex(new Vector2(ION.halfWidth + (visualX * baseHalfWidth) + translationX, (visualY * baseHalfHeight) + translationY + (baseHalfHeight * 2) - 1), Color.Red);
                //GO.primitiveBatch.AddVertex(new Vector2(GO.baseHalfWidth + (visualX * baseHalfWidth) + translationX + (baseHalfWidth), (visualY * baseHalfHeight) + translationY + (baseHalfHeight)),Color.Yellow);
                ION.primitiveBatch.AddVertex(new Vector2(ION.halfWidth + (visualX * baseHalfWidth) + translationX, (visualY * baseHalfHeight) + translationY), Color.Red);
                ION.primitiveBatch.End();
            }

            //if (selected)
            //{
            //    addCharge(0.04f,Players.PLAYER1);
            //}

            //if (owner != Players.NEUTRAL)
            //{
            //    Texture2D chargeImage = Images.getChargeCountImage(charge);

            //    ION.spriteBatch.Begin();
            //    ION.spriteBatch.Draw(chargeImage, new Rectangle(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth), (visualY * baseHalfHeight) + translationY, baseHalfWidth * 2, baseHalfHeight * 2), Color.White);
            //    ION.spriteBatch.End();
            //}

            if(unit != null) {
                unit.draw(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth), (visualY * baseHalfHeight) + translationY, baseHalfWidth * 2, baseHalfHeight * 2);
            }

   
        }

        public override void update()
        {
            //Debug.WriteLine("UPDATING TILE!");
            owner = nextOwner;
            charge = nextCharge;
        }

        //public float getCharge()
        //{
        //    return charge;
        //}

        //public void setCharge(float newCharge)
        //{
        //    if (newCharge <= 1.0f && newCharge >= 0.0f)
        //    {
        //        charge = newCharge;
        //    }
        //}

        //public virtual void addCharge(float addition, int player)
        //{
        //    if (player != owner)
        //    {
        //        if (nextCharge - addition < 0.0f)
        //        {
        //            owner = player;
        //            nextCharge = 0.0f;
        //        }
        //        else
        //        {
        //            nextCharge -= addition;
        //        }
        //    }
        //    else
        //    {

        //        if (nextCharge + addition > 1.0f)
        //        {
        //            nextCharge = 1.0f;
        //        }
        //        else
        //        {
        //            nextCharge += addition;
        //        }
        //    }
        //}

        //public virtual void removeCharge(float addition, int owner)
        //{
        //    if (nextCharge - addition < 0.0f)
        //    {
        //        nextCharge = 0.0f;
        //        nextOwner = Players.NEUTRAL;
        //    }
        //    else
        //    {
        //        nextCharge -= addition;
        //    }

        //}

        public virtual void donate(float charge)
        {
            if (nextCharge - charge < 0.0f)
            {
                nextCharge = 0.0f;
                Debug.WriteLine("DONATION RESULTS IN  < 0");
            }
            else
            {
                nextCharge -= charge;
            }
        }

        public virtual void receive(float charge)
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

        public virtual void sustain(float charge, int player)
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

        private Color getAppropriateColor(int owner, float charge)
        {
            if (owner == Players.NEUTRAL)
            {
                tileColor.R = 255;
                tileColor.G = 255;
                tileColor.B = 255;

                tileColor.A = 255;
            }
            
            else if (owner == Players.PLAYER1)
            {
                tileColor.R = (byte)(255 - (charge * 255));
                tileColor.G = (byte)(255 - (charge * 255));
                tileColor.B = 255;

                //tileColor.A = (byte)(charge * 255);
                tileColor.A = 255;
            }
            else if (owner == Players.PLAYER2)
            {
                tileColor.R = 255;
                tileColor.G = (byte)(255 - (charge * 255));
                tileColor.B = (byte)(255 - (charge * 255));

                //tileColor.A = (byte)(charge * 255);
                tileColor.A = 255;
            }

            return tileColor;
        }

   




    }
}
