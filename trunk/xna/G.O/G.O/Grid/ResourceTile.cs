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

        public int owner = Players.NEUTRAL;

        public int nextOwner = Players.NEUTRAL;

        public float charge = 0;
        
        public float nextCharge = 0;

        public const float MAX_CHARGE = 1.0f;

        public Color tileColor = new Color();

        public bool isSpiking = false;
        public int spikeCount = 0;
        public static float spikeCharge = 0.9f;
        public static int spikeDuration = 5;

        //Tells you if FloodFill.getPath() has passed this tile. Only use from FloodFill.cs
        public bool floofFillFlag = false;

        public ResourceTile()
        {
            accessable = true;
        }

        public ResourceTile(int indexX, int indexY)
        {
            accessable = true;

            this.indexX = indexX;
            this.indexY = indexY;

            drawingRectangle = new Rectangle();
        }
        public override void drawDebug(float translationX, float translationY)
        {           
            Vector2 location = new Vector2(ION.halfWidth + (visualX * baseHalfWidth) + translationX - 40, (visualY * baseHalfHeight) + translationY + baseHalfHeight);
            //ION.spriteBatch.DrawString(Fonts.font, "(z=" + visualZ + ":x=" + visualX + ":y=" + visualY + ")", location, Color.Black);
            ION.spriteBatch.DrawString(Fonts.font, "charge: "+charge, location, Color.Black);           
        }

        public override void draw(float translationX, float translationY)
        {
            drawingRectangle.X = (int)(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth));
            drawingRectangle.Y = (int)((visualY * baseHalfHeight) + translationY);
            drawingRectangle.Width = (int)(baseHalfWidth * 2);
            drawingRectangle.Height = (int)(baseHalfHeight * 2);

            tileColor = getAppropriateColor(owner, charge);

            //if (owner == Players.NEUTRAL)
            //{
    
            //}

            ION.spriteBatch.Draw(Images.borderImage, drawingRectangle, Color.White);
            ION.spriteBatch.Draw(Images.resourceImage, drawingRectangle, tileColor);
            


            //if (owner != Players.NEUTRAL)
            //{
            //    Texture2D chargeImage = Images.getChargeCountImage(charge);   
            //    ION.spriteBatch.Draw(chargeImage, new Rectangle((int)(ION.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth)), (int)((visualY * baseHalfHeight) + translationY), (int)(baseHalfWidth * 2), (int)(baseHalfHeight * 2)), Color.White);
            //}
        }

        public override void update()
        {
            owner = nextOwner;
            charge = nextCharge;
        }

        public virtual void donate(float charge)
        {
            if (charge < 0.0f)
            {
                Debug.WriteLine("NEGATIVE CHARGE ON DONATE!");
            }
            
            if (nextCharge - charge < 0.0f)
            {
                nextCharge = 0.0f;
                //Debug.WriteLine("DONATION RESULTS IN  < 0");
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
                //Debug.WriteLine("RECEPTION RESULTS IN > 1");
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

        public virtual Color getAppropriateColor(int owner, float charge)
        {
            if (owner == Players.NEUTRAL)
            {
                tileColor.R = 255;
                tileColor.G = 255;
                tileColor.B = 255;

                tileColor.A = 50;
            }

            //if (isSpiking)
            //{
            //    tileColor.R = 255;
            //    tileColor.G = 255;
            //    tileColor.B = 0;

            //    tileColor.A = 255;


            //}

            else if (owner == Players.PLAYER1)
            {
                if (charge > 1.0f)
                {
                    //Debug.WriteLine("charge?" + charge);
                    float temp = charge - 1.0f;
                    tileColor.R = 0;
                    tileColor.G = (byte)(temp * 255);
                   // tileColor.G = 255;
                    tileColor.B = 255;

                    //tileColor.A = (byte)(charge * 255);
                    tileColor.A = 255;
                    return tileColor;
                }

                tileColor.R = 0;
                tileColor.G = (byte)(255 - (charge * 255));
                tileColor.B = 255;

                //tileColor.A = (byte)(charge * 255);
                //tileColor.A = (byte)(255 - (charge * 255));
                tileColor.A = 170;
            }
            else if (owner == Players.PLAYER2)
            {
                if (charge > 1.0f)
                {
                    float temp = charge - 1.0f;
                    tileColor.R = 255;
                    tileColor.G = (byte)(temp * 255);
                    tileColor.B = 0;

                    //tileColor.A = (byte)(charge * 255);
                    tileColor.A = 255;
                    return tileColor;
                }

                tileColor.R = 255;
                tileColor.G = (byte)(255 - (charge * 255));
                tileColor.B = 0;

                //tileColor.A = (byte)(charge * 255);
                //tileColor.A = (byte)(255 - (charge * 255));
                tileColor.A = 170;
            }

            return tileColor;
        }

    }
}
