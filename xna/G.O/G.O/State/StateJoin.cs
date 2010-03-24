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
    class StateJoin : State
    {
        public enum SELECTION
        {
            BACK = 1,
            JOIN,
            REFRESH

        }
        public SELECTION selection = SELECTION.BACK;



        private Rectangle backButton;
        private Rectangle hostsTable;
        private Rectangle joinButton;
        private Rectangle refreshButton;

        private bool mousePressed = false;
        public bool upPressed = false;
        public bool downPressed = false;
        public bool enterPressed = false;


        public StateJoin()
        {
            backButton = new Rectangle((ION.width / 2) - 500, (ION.height / 2) +300, Images.buttonBack.Width, Images.buttonBack.Height);
            joinButton = new Rectangle((ION.width / 2) - 125, (ION.height / 2) + 300, Images.buttonJoin.Width, Images.buttonJoin.Height);
            refreshButton = new Rectangle((ION.width / 2) + 225, (ION.height / 2) + 300, Images.buttonRefresh.Width, Images.buttonRefresh.Height);
            hostsTable = new Rectangle((ION.width / 2) - 300, (ION.height / 2) - 70, Images.tableHosts.Width, Images.tableHosts.Height);
        }

        public override void draw()
        {
            ION.get().GraphicsDevice.Clear(Color.Black);
            ION.spriteBatch.Begin();
            ION.spriteBatch.Draw(Images.ION_LOGO, new Rectangle((ION.width / 2) - 200, (ION.height / 2) - 170, Images.ION_LOGO.Width, Images.ION_LOGO.Height), Color.White);
            ION.spriteBatch.Draw(Images.tableHosts, hostsTable, Color.White);
            //ION.spriteBatch.DrawString(Fonts.font, title, new Vector2((ION.width / 2) - 15, (ION.height / 2) - 150), Color.White);



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


            if (selection == SELECTION.REFRESH)
            {
                //Draw highlighted
                ION.spriteBatch.Draw(Images.buttonRefresh, refreshButton, Color.White);
            }
            else
            {
                //Draw normally
                ION.spriteBatch.Draw(Images.buttonRefresh, refreshButton, Color.White);
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

            if (mouseIn(mouseState.X, mouseState.Y, refreshButton))
            {
                selection = SELECTION.REFRESH;
                if (mouseState.LeftButton == ButtonState.Released && mousePressed == true)
                {
                    makeSelection();
                    mousePressed = false;
                }

            }
            if (mouseIn(mouseState.X, mouseState.Y, joinButton))
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

            //Keyboard handling
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

            if (keyState.IsKeyUp(Keys.Enter) && enterPressed == true)
            {
                enterPressed = false;
                makeSelection();
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
            if (selection == SELECTION.REFRESH)
            {
                //TODO
            }
            else if (selection == SELECTION.BACK)
            {
                StateMP st = new StateMP();

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

        private void selectionUp()
        {

            selection--;
            if (selection < SELECTION.BACK)
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
                selection = SELECTION.BACK;
            }
            Console.WriteLine("sel: " + (int)selection);


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
