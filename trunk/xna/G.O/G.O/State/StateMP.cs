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


        public Rectangle newGameButton;
        public Rectangle mpButton;
        public Rectangle quitButton;
        public Rectangle optionsButton;
        public Rectangle background_overlay;
        public Rectangle background_starfield;
        //
        private Rectangle hostButton;
        private Rectangle joinButton;
        private Rectangle backButton;

        private bool mousePressed = false;
        public bool upPressed = false;
        public bool downPressed = false;
        public bool enterPressed = false;


        public StateMP()
        {
            newGameButton = new Rectangle(125, 125, Images.buttonNewGame.Width, Images.buttonNewGame.Height);
            mpButton = new Rectangle(125, 200, Images.buttonNewGame.Width, Images.buttonNewGame.Height);
            optionsButton = new Rectangle(125, 275, Images.buttonOptions.Width, Images.buttonOptions.Height);
            quitButton = new Rectangle(125, 350, Images.buttonNewGame.Width, Images.buttonNewGame.Height);
            background_overlay = new Rectangle(ION.width - Images.background_overlay.Width, 0, Images.background_overlay.Width, Images.background_overlay.Height);
            background_starfield = new Rectangle(0, 0, Images.background_starfield.Width, Images.background_starfield.Height);
            //
            hostButton = new Rectangle(375, 200, Images.buttonNewGame.Width, Images.buttonNewGame.Height);
            joinButton = new Rectangle(375, 275, Images.buttonNewGame.Width, Images.buttonNewGame.Height);
            backButton = new Rectangle(375, 350, Images.buttonNewGame.Width, Images.buttonNewGame.Height);

            if (ION.instance.serverConnection == null)
            {
                ION.instance.serverConnection = new ServerConnection();
            }

         
        }

        public override void draw()
        {
            
            ION.get().GraphicsDevice.Clear(Color.Black);
            ION.spriteBatch.Begin();
            //
            //logo
            Double w = Math.Ceiling((Double)ION.width / (Double)Images.background_overlay.Width);
            Double h = Math.Ceiling((Double)ION.height / (Double)Images.background_overlay.Height);
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    ION.spriteBatch.Draw(Images.background_starfield, new Rectangle(background_starfield.X + (i * background_starfield.Width), background_starfield.Y + (j * background_starfield.Height), background_starfield.Width, background_starfield.Height), Color.White);
                }
            }
            ION.spriteBatch.Draw(Images.background_overlay, background_overlay, Color.White);
            //
            ION.spriteBatch.Draw(Images.buttonNewGame, newGameButton, Color.Gray);
            ION.spriteBatch.Draw(Images.buttonMP, mpButton, Color.Gray);
            ION.spriteBatch.Draw(Images.buttonOptions, optionsButton, Color.Gray);
            ION.spriteBatch.Draw(Images.buttonQuit, quitButton, Color.Gray);
            //
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
            //mouse handling
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

        private void makeSelection()
        {
            if (selection == SELECTION.HOST)
            {
                StateHost st = new StateHost();

                ION.get().setState(st);
            }
            else if (selection == SELECTION.BACK)
            {
                StateTitle st = new StateTitle();
         
                ION.get().setState(st);
            }
           
            else if (selection == SELECTION.JOIN)
            {
                //Console.WriteLine("join");
                //"http://landconquerer.appspot.com/landconquerer
               // WebRequest req = WebRequest.Create("http://google.com");
                //WebResponse response = req.GetResponse();
                //StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.ASCII);
                StateJoin st = new StateJoin();

                ION.get().setState(st);
                //Console.WriteLine(sr.ReadToEnd());
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
