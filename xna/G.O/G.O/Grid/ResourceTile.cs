using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using GO.Units;

namespace GO
{
    public class ResourceTile : Tile
    {

        public int owner = Players.NEUTRAL;

        private int nextOwner = Players.NEUTRAL;

        public float charge = 0;

        private float nextCharge = 0;

        private Unit unit = null;

        //This is buggy but for testing
        private const float minimumFlux = 0.05f;

        public const float MAX_CHARGE = 1.0f;

        private Color tileColor = new Color();

        public ResourceTile(int indexX, int indexY)
        {
            this.indexX = indexX;
            this.indexY = indexY;
        }
        public override void drawDebug(int translationX, int translationY)
        {
            GO.spriteBatch.Begin();
            Vector2 location = new Vector2(GO.halfWidth + (visualX * baseHalfWidth) + translationX - 40, (visualY * baseHalfHeight) + translationY + baseHalfHeight);
            GO.spriteBatch.DrawString(Fonts.font, "(z=" + visualZ + ":x=" + visualX + ":y=" + visualY + ")", location, Color.Black);
            GO.spriteBatch.End();
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


        public override void tileVersusTile(Tile other)
        {
            if (other is ResourceTile)
            {
                ResourceTile otherResourceTile = (ResourceTile)other;
                if (otherResourceTile.owner != owner)
                {
                    if (otherResourceTile.charge > charge)
                    {
                        if (otherResourceTile.charge - charge > minimumFlux)
                        {
                            addCharge(minimumFlux, owner);
                            otherResourceTile.removeCharge(minimumFlux);
                        }

                    }
                    else if (charge > otherResourceTile.charge)
                    {
                        if (charge - otherResourceTile.charge > minimumFlux)
                        {
                            otherResourceTile.addCharge(minimumFlux, owner);
                            removeCharge(minimumFlux);
                        }

                    }

                }
            }
        }

        public override void tileAidTile(Tile other)
        {
            if (other is ResourceTile)
            {
                ResourceTile otherResourceTile = (ResourceTile)other;
                if (otherResourceTile.owner == owner)
                {
                    if (otherResourceTile.charge > charge)
                    {
                        if (otherResourceTile.charge - charge > minimumFlux)
                        {
                            addCharge(minimumFlux, owner);
                            otherResourceTile.removeCharge(minimumFlux);
                        }

                    }
                    else if (charge > otherResourceTile.charge)
                    {
                        if (charge - otherResourceTile.charge > minimumFlux)
                        {
                            otherResourceTile.addCharge(minimumFlux, owner);
                            removeCharge(minimumFlux);
                        }

                    }

                }
            }

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

            GO.spriteBatch.Begin();
            //GO.spriteBatch.Draw(Images.borderImage, new Rectangle(GO.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth), (visualY * baseHalfHeight) + translationY, baseHalfWidth * 2, baseHalfHeight * 2), Color.White);
            GO.spriteBatch.Draw(Images.resourceImage, new Rectangle(GO.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth), (visualY * baseHalfHeight) + translationY, baseHalfWidth*2, baseHalfHeight * 2), tileColor);

            GO.spriteBatch.End();

            if (selected)
            {
                GO.primitiveBatch.Begin(PrimitiveType.LineList);
                GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth) + 1, (visualY * baseHalfHeight) + translationY + (baseHalfHeight)), Color.Red);
                //GO.primitiveBatch.AddVertex(new Vector2(GO.baseHalfWidth + (visualX * baseHalfWidth) + translationX, (visualY * baseHalfHeight) + translationY + (baseHalfHeight * 2) - 1), Color.Yellow);
                GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (visualX * baseHalfWidth) + translationX + (baseHalfWidth), (visualY * baseHalfHeight) + translationY + (baseHalfHeight)), Color.Red);
                //GO.primitiveBatch.AddVertex(new Vector2(GO.baseHalfWidth + (visualX * baseHalfWidth) + translationX, (visualY * baseHalfHeight) + translationY), Color.Yellow);
                GO.primitiveBatch.End();

                GO.primitiveBatch.Begin(PrimitiveType.LineList);
                //GO.primitiveBatch.AddVertex(new Vector2(GO.baseHalfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth) + 1, (visualY * baseHalfHeight) + translationY + (baseHalfHeight)), Color.Yellow);
                GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (visualX * baseHalfWidth) + translationX, (visualY * baseHalfHeight) + translationY + (baseHalfHeight * 2) - 1), Color.Red);
                //GO.primitiveBatch.AddVertex(new Vector2(GO.baseHalfWidth + (visualX * baseHalfWidth) + translationX + (baseHalfWidth), (visualY * baseHalfHeight) + translationY + (baseHalfHeight)),Color.Yellow);
                GO.primitiveBatch.AddVertex(new Vector2(GO.halfWidth + (visualX * baseHalfWidth) + translationX, (visualY * baseHalfHeight) + translationY), Color.Red);
                GO.primitiveBatch.End();
            }

            if (selected)
            {
                addCharge(0.04f,Players.PLAYER1);
            }

            if (owner != Players.NEUTRAL)
            {
                Texture2D chargeImage = Images.getChargeCountImage(charge);

                GO.spriteBatch.Begin();
                GO.spriteBatch.Draw(chargeImage, new Rectangle(GO.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth), (visualY * baseHalfHeight) + translationY, baseHalfWidth*2, baseHalfHeight*2), Color.White);
                GO.spriteBatch.End();
            }

            if(unit != null) {
                unit.draw(GO.halfWidth + (visualX * baseHalfWidth) + translationX - (baseHalfWidth), (visualY * baseHalfHeight) + translationY, baseHalfWidth * 2, baseHalfHeight * 2);
            }

   
        }

        public override void update()
        {
            //Debug.WriteLine("UPDATING TILE!");
            owner = nextOwner;
            charge = nextCharge;
        }

        public float getCharge()
        {
            return charge;
        }

        //public void setCharge(float newCharge)
        //{
        //    if (newCharge <= 1.0f && newCharge >= 0.0f)
        //    {
        //        charge = newCharge;
        //    }
        //}

        public void addCharge(float addition, int player)
        {
            if (player != owner)
            {
                if (charge - addition < 0.0f)
                {
                    nextOwner = player;
                    nextCharge = 0.0f;
                }
            }
            else
            {

                if (charge + addition > 1.0f)
                {
                    nextCharge = 1.0f;
                }
                else
                {
                    nextCharge = charge + addition;
                }
            }
        }

        public void removeCharge(float addition)
        {
            if (charge - addition < 0.0f)
            {
                nextCharge = 0.0f;
                owner = Players.NEUTRAL;
            }
            else
            {
                nextCharge = charge - addition;
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
