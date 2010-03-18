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

        private const String title = "MULTIPLAYER MODE"; //"G.O."
        private const String host = "Host a game";
        private const String join = "Join a game";
        private const String back = "Back to title menu";

        public bool upPressed = false;
        public bool downPressed = false;
        public bool enterPressed = false;
        
        public override void draw()
        {
            ION.get().GraphicsDevice.Clear(Color.Black);
            ION.spriteBatch.Begin();
            ION.spriteBatch.Draw(Images.ION_LOGO, new Rectangle((ION.width / 2) - 200, (ION.height / 2) - 170, Images.ION_LOGO.Width, Images.ION_LOGO.Height), Color.White);
            //ION.spriteBatch.DrawString(Fonts.font, title, new Vector2((ION.width / 2) - 15, (ION.height / 2) - 150), Color.White);

           

            if (selection == SELECTION.HOST)
            {
                //Draw highlighted
                ION.spriteBatch.Draw(Images.buttonHostF, new Rectangle((ION.width / 2) - 125, (ION.height / 2), Images.buttonNewGame.Width, Images.buttonNewGame.Height), Color.White);
            }
            else
            {
                //Draw normally
                ION.spriteBatch.Draw(Images.buttonHost, new Rectangle((ION.width / 2) - 125, (ION.height / 2), Images.buttonNewGame.Width, Images.buttonNewGame.Height), Color.White);
            }
            if (selection == SELECTION.JOIN)
            {
                //Draw highlighted
                ION.spriteBatch.Draw(Images.buttonJoinF, new Rectangle((ION.width / 2) - 125, (ION.height / 2)+70 , Images.buttonNewGame.Width, Images.buttonNewGame.Height), Color.White);
            }
            else
            {
                //Draw normally
                ION.spriteBatch.Draw(Images.buttonJoin, new Rectangle((ION.width / 2) - 125, (ION.height / 2)+70, Images.buttonNewGame.Width, Images.buttonNewGame.Height), Color.White);
            }


            if (selection == SELECTION.BACK)
            {
                //Draw highlighted
                ION.spriteBatch.Draw(Images.buttonBackF, new Rectangle((ION.width / 2) - 125, (ION.height / 2)+140, Images.buttonNewGame.Width, Images.buttonNewGame.Height), Color.White);
            }
            else
            {
                //Draw normally
                ION.spriteBatch.Draw(Images.buttonBack, new Rectangle((ION.width / 2) - 125, (ION.height / 2)+140, Images.buttonNewGame.Width, Images.buttonNewGame.Height), Color.White);
            }

            ION.spriteBatch.End();
        }


        public override void update(int ellapsed)
        {

            KeyboardState keyState = Keyboard.GetState();





            if (keyState.IsKeyDown(Keys.Up) && !upPressed)
            {
                selectionUp();
                upPressed = true;
            }
            else if (keyState.IsKeyUp(Keys.Up) && upPressed)
            {
                upPressed = false;
            }


            if (keyState.IsKeyDown(Keys.Down) && !downPressed)
            {
                selectionDown();
                downPressed = true;
            }
            else if (keyState.IsKeyUp(Keys.Down) && downPressed)
            {
                downPressed = false;
            }

            if (keyState.IsKeyDown(Keys.Enter))
            {
                enterPressed = true;
            }

            if (keyState.IsKeyUp(Keys.Enter) && enterPressed==true)
            {
                enterPressed = false;
                makeSelection();
            }
        }

        private void selectionUp()
        {

            selection--;
            if (selection < SELECTION.HOST)
            {
                Console.WriteLine("if sel: " + (int)selection);
                selection = (SELECTION)Enum.GetNames(typeof(SELECTION)).Length;
            }
            Console.WriteLine("sel: " + (int)selection);
        }

        private void selectionDown()
        {
            selection++;
            if ((int)selection > Enum.GetNames(typeof(SELECTION)).Length)
            {
                Console.WriteLine("if sel: " + (int)selection);
                selection = SELECTION.HOST;
            }
            Console.WriteLine("sel: " + (int)selection);


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
                //st.enterPressed = true;
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

            MediaPlayer.Play(Music.titleSong);
            MediaPlayer.IsRepeating = true;
        }


        public override void focusLost()
        {

            MediaPlayer.Stop();
        }

    }
}
