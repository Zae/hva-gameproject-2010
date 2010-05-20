using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace ION.UI
{
    class InfluenceDisplayer : GUIComponent
    {

        //THIS IS FIXED TO DISPLAY INFLUENCE FOR ONLY TWO PLAYERS

        private float pixelsPerInfluencePoint;

        private Rectangle player1Influence;
        private Rectangle player2Influence;

        private int intoMiddleX = 9;
       

        public InfluenceDisplayer(int x, int y, Texture2D imageNormal) : base(x,y,imageNormal)
        {
            pixelsPerInfluencePoint = ((float)imageNormal.Width -(2*intoMiddleX))/ (float)Grid.resourceTiles.Count;

            //Debug.WriteLine("rtc=" + Grid.resourceTiles.Count);
            //Debug.WriteLine("imgwdth=" + imageNormal.Width);
            //Debug.WriteLine("ppi=" + pixelsPerInfluencePoint);

            player1Influence = new Rectangle(0,0,0,0);
            player2Influence = new Rectangle(0,0,0,0);
        }

        public override void draw()
        {
            //Do for two players

            player1Influence.Width = (int)(Grid.playerInfluences[Players.PLAYER1] * pixelsPerInfluencePoint);
            player2Influence.Width = (int)(Grid.playerInfluences[Players.PLAYER2] * pixelsPerInfluencePoint);
            player1Influence.X = screenRectangle.Right - player1Influence.Width - intoMiddleX;
            player2Influence.X = screenRectangle.Left + intoMiddleX;

            player1Influence.Y = screenRectangle.Top;
            player2Influence.Y = screenRectangle.Top;

            ION.spriteBatch.Draw(Images.white1px, player1Influence, Color.DarkBlue);
            ION.spriteBatch.Draw(Images.white1px, player2Influence, Color.Red);

            //Draws the overlay
            base.draw();
        }

        public override void offset(int screenX, int screenY)
        {
            base.offset(screenX, screenY);

            //Now update the Rectangles
            //player2Influence.X = screenRectangle.X;
            //player2Influence.Y = screenRectangle.Y;
            player2Influence.Height = screenRectangle.Height - 1;

            
            //player1Influence.Y = screenRectangle.Y;
            player1Influence.Height = screenRectangle.Height - 1;

        }
    }
}
