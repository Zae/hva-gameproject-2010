﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ION.UI
{
    class Button : GUIComponent
    {

        private bool mouseOver = false;
        private ButtonHandler handler;

        //TODO not really a nice fix
        private DateTime lastTime = DateTime.Now;
        private DateTime now;
        private TimeSpan cooldown = new TimeSpan(0, 0, 0, 0,250); //set the number of milliseconds

        public Button(int x, int y, Texture2D imageNormal, ButtonHandler handler) : base(x, y, imageNormal)
        {
            this.handler = handler;
        }

        public override void draw()
        {
            base.draw();

            if (!mouseOver)
            {
                ION.spriteBatch.Draw(Images.buttonOverlay, screenRectangle, Color.White);
            }
            if (imageNormal == Images.newUnitButtonNormal)
            {
                ION.spriteBatch.DrawString(Fonts.font, Robot.cost.ToString(), new Vector2(screenRectangle.X+14, screenRectangle.Y + 16), Color.Red);
            }
            else if (imageNormal == Images.towerButtonNormal)
            {
                ION.spriteBatch.DrawString(Fonts.font, Tower.cost.ToString(), new Vector2(screenRectangle.X+14, screenRectangle.Y + 16), Color.Red);
            }
        }

        public override bool handleMouse(Point evalPoint, bool leftPressed)
        {
            if(screenRectangle.Contains(evalPoint)) 
            {
                mouseOver = true;

                if (leftPressed)
                {
                    now = DateTime.Now;

                    if (now - lastTime > cooldown)
                    {
                        lastTime = now;
                        handler.run();
                    }
                   
                }

                return true;
            }
            else 
            {
                mouseOver = false;
                return false;
            }

        }

    }
}
