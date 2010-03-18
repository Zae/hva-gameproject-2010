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

namespace ION
{
    class StateMP : State
    {
        public enum SELECTION
        {
            HOST = 1,
            JOIN,
            BACK

        }
        public SELECTION selection = SELECTION.HOST;

 

        private Rectangle hostButton;
        private Rectangle joinButton;
        private Rectangle backButton;
        private bool mousePressed = false; 


        public StateMP()
        {
            hostButton = new Rectangle((ION.width / 2) - 125, (ION.height / 2), Images.buttonNewGame.Width, Images.buttonNewGame.Height);
            joinButton = new Rectangle((ION.width / 2) - 125, (ION.height / 2) + 70, Images.buttonNewGame.Width, Images.buttonNewGame.Height);
            backButton = new Rectangle((ION.width / 2) - 125, (ION.height / 2)+140, Images.buttonNewGame.Width, Images.buttonNewGame.Height);
        }

        public override void draw()
        {
            ION.get().GraphicsDevice.Clear(Color.Black);
            ION.spriteBatch.Begin();
            ION.spriteBatch.Draw(Images.ION_LOGO, new Rectangle((ION.width / 2) - 200, (ION.height / 2) - 170, Images.ION_LOGO.Width, Images.ION_LOGO.Height), Color.White);
            //ION.spriteBatch.DrawString(Fonts.font, title, new Vector2((ION.width / 2) - 15, (ION.height / 2) - 150), Color.White);

           

            if (selection == SELECTION.HOST)
            {
                //Draw highlighted
                ION.spriteBatch.Draw(Images.buttonHostF, hostButton, Color.White);
            }
            else
            {
                //Draw normally
                ION.spriteBatch.Draw(Images.buttonHost, hostButton, Color.White);
            }
            if (selection == SELECTION.JOIN)
            {
                //Draw highlighted
                ION.spriteBatch.Draw(Images.buttonJoinF, joinButton, Color.White);
            }
            else
            {
                //Draw normally
                ION.spriteBatch.Draw(Images.buttonJoin, joinButton, Color.White);
            }


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

            ION.spriteBatch.End();
        }


        public override void update(int ellapsed)
        {

            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                mousePressed = true;
            }

            if (mouseIn(mouseState.X, mouseState.Y, hostButton))
            {
                selection= SELECTION.HOST;
                if (mouseState.LeftButton == ButtonState.Released && mousePressed == true)
                {
                    makeSelection();
                    mousePressed = false;
                }

            }
            if (mouseIn(mouseState.X, mouseState.Y,joinButton))
            {
                selection = SELECTION.JOIN;
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
            if (selection == SELECTION.HOST)
            {
                //TODO
            }
            else if (selection == SELECTION.BACK)
            {
                StateTitle st = new StateTitle();
         
                ION.get().setState(st);
            }
           
            else if (selection == SELECTION.JOIN)
            {
                Console.WriteLine("join");
                //"http://landconquerer.appspot.com/landconquerer
                WebRequest req = WebRequest.Create("http://google.com");
                WebResponse response = req.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.ASCII);
                
                Console.WriteLine(sr.ReadToEnd());
            }
        }

        public override void focusGained()
        {
            ION.get().IsMouseVisible = true;
            MediaPlayer.Play(Music.titleSong);
            MediaPlayer.IsRepeating = true;
        }


        public override void focusLost()
        {
            ION.get().IsMouseVisible = false;
            MediaPlayer.Stop();
        }

    }
}
