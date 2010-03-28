using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Net;
using System.IO;
using FluorineFx.Net;
using FluorineFx.Messaging.Api.Service;

namespace ION
{


    class StateHost : State
    {

        public enum SELECTION
        {
            BACK = 1,
            START

        }
        public SELECTION selection = SELECTION.BACK;



        private Rectangle backButton;
        private Rectangle startButton;

        private bool mousePressed = false;
        public bool upPressed = false;
        public bool downPressed = false;
        public bool enterPressed = false;



        




        public StateHost()
        {
            backButton = new Rectangle((ION.width / 2) - 500, (ION.height / 2) + 300, Images.buttonBack.Width, Images.buttonBack.Height);
            startButton = new Rectangle((ION.width / 2) - 125, (ION.height / 2) + 300, Images.buttonJoin.Width, Images.buttonJoin.Height);



        }



        public override void draw()
        {
            ION.get().GraphicsDevice.Clear(Color.Black);
            ION.spriteBatch.Begin();
            ION.spriteBatch.Draw(Images.ION_LOGO, new Rectangle((ION.width / 2) - 200, (ION.height / 2) - 170, Images.ION_LOGO.Width, Images.ION_LOGO.Height), Color.White);

            if (selection == SELECTION.BACK)
            {
                //Draw highlighted
                ION.spriteBatch.Draw(Images.buttonBackF, backButton, Color.White);
            }
            else
            {
                //Draw normally
                ION.spriteBatch.Draw(Images.buttonBack, backButton, Color.White);
            }
            if (selection == SELECTION.START)
            {
                //Draw highlighted
                ION.spriteBatch.Draw(Images.buttonNewGameF, startButton, Color.White);
            }
            else
            {
                //Draw normally
                ION.spriteBatch.Draw(Images.buttonNewGame, startButton, Color.White);
            }

            ION.spriteBatch.End();

        }

        public override void update(int ellapsed)
        {

            //mouse handling
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                mousePressed = true;
            }



            if (mouseIn(mouseState.X, mouseState.Y, startButton))
            {
                selection = SELECTION.START;
                if (mouseState.LeftButton == ButtonState.Released && mousePressed == true)
                {
                    makeSelection();
                    mousePressed = false;
                }

            }
            if (mouseIn(mouseState.X, mouseState.Y, backButton))
            {
                selection = SELECTION.BACK;
                if (mouseState.LeftButton == ButtonState.Released && mousePressed == true)
                {
                    makeSelection();
                    mousePressed = false;
                }

            }

        }

        public Boolean mouseIn(int mx, int my, Rectangle rect)
        {
            if ((mx > rect.X && mx < (rect.X + rect.Width)) && (my > rect.Y && my < (rect.Y + rect.Height)))
            {
                return true;
            }

            return false;

        }

        private void makeSelection()
        {
            if (selection == SELECTION.START)
            {
                //TODO
            }
            else if (selection == SELECTION.BACK)
            {
                StateMP st = new StateMP();

                ION.get().setState(st);
            }

          
        }





        public override void focusGained()
        {
            ION.get().IsMouseVisible = true;
            //MediaPlayer.Play(Music.titleSong);
            //MediaPlayer.IsRepeating = true;
        }

        public override void focusLost()
        {
            ION.get().IsMouseVisible = false;
            //MediaPlayer.Stop();
        }



    }


   




}
