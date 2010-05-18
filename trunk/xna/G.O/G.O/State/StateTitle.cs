using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace ION
{
    public class StateTitle : State
    {
        public enum SELECTION
        {
            NEWGAME = 1,
            MULTIPLAYER,
            OPTIONS,
            QUIT

        }
        //Initialy the first option is selected
        public SELECTION selection = SELECTION.NEWGAME;

     
        public Rectangle newGameButton;
        public Rectangle mpButton;
        public Rectangle quitButton;
        public Rectangle optionsButton;
        public Rectangle background_overlay;
        public Rectangle background_starfield;

        private bool mousePressed = false;
        public bool upPressed = false;
        public bool downPressed = false;
        public bool enterPressed = false;

        public StateTitle()
        {
            newGameButton = new Rectangle(125, 125 , Images.buttonNewGame.Width, Images.buttonNewGame.Height);
            mpButton = new Rectangle(125, 200, Images.buttonNewGame.Width, Images.buttonNewGame.Height);
            optionsButton = new Rectangle(125, 275, Images.buttonOptions.Width, Images.buttonOptions.Height);
            quitButton = new Rectangle(125, 350, Images.buttonNewGame.Width, Images.buttonNewGame.Height);
            background_overlay = new Rectangle(ION.width-Images.background_overlay.Width, 0, Images.background_overlay.Width, Images.background_overlay.Height);
            background_starfield = new Rectangle(0, 0, Images.background_starfield.Width, Images.background_starfield.Height);
        }

        public override void draw()
        {
            ION.get().GraphicsDevice.Clear(Color.Black);

            ION.spriteBatch.Begin();

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

            if (selection == SELECTION.NEWGAME)
            {
                //Draw highlighted
                ION.spriteBatch.Draw(Images.buttonNewGameF, newGameButton, Color.White);
            }
            else
            {
                //Draw normally
                ION.spriteBatch.Draw(Images.buttonNewGame, newGameButton, Color.White);
            }
            if (selection == SELECTION.MULTIPLAYER)
            {
                //Draw highlighted
                ION.spriteBatch.Draw(Images.buttonMPF, mpButton, Color.White);
            }
            else
            {
                //Draw normally
                ION.spriteBatch.Draw(Images.buttonMP, mpButton, Color.White);
            }
            if (selection == SELECTION.OPTIONS)
            {
                //Draw highlighted
                ION.spriteBatch.Draw(Images.buttonOptionsF, optionsButton, Color.White);
            }
            else
            {
                //Draw normally
                ION.spriteBatch.Draw(Images.buttonOptions, optionsButton, Color.White);
            }

            if (selection == SELECTION.QUIT)
            {
                //Draw highlighted
                ION.spriteBatch.Draw(Images.buttonQuitF, quitButton, Color.White);
            }
            else
            {
                //Draw normally
                ION.spriteBatch.Draw(Images.buttonQuit, quitButton, Color.White);
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

            if (mouseIn(mouseState.X, mouseState.Y, newGameButton))
            {
                selection= SELECTION.NEWGAME;
                if (mouseState.LeftButton == ButtonState.Released && mousePressed == true)
                {
                    makeSelection();
                    mousePressed = false;
                }

            }
            if (mouseIn(mouseState.X, mouseState.Y, mpButton))
            {
                selection = SELECTION.MULTIPLAYER;
                if (mouseState.LeftButton == ButtonState.Released && mousePressed == true)
                {
                    makeSelection();
                    mousePressed = false;
                }

            }
            if (mouseIn(mouseState.X, mouseState.Y, optionsButton))
            {
                selection = SELECTION.OPTIONS;
                if (mouseState.LeftButton == ButtonState.Released && mousePressed == true)
                {
                    makeSelection();
                    mousePressed = false;
                }

            }
            if (mouseIn(mouseState.X, mouseState.Y, quitButton))
            {
                selection = SELECTION.QUIT;
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

            if(keyState.IsKeyDown(Keys.N)){
                ION.get().setState(new StateNetworkTest());
            }
            if (keyState.IsKeyDown(Keys.T))
            {
                ION.get().setState(new StateTicTacToe());
            }
        }

        

        private void makeSelection()
        {
            if (selection == SELECTION.NEWGAME)
            {
                ION.get().setState(new StateTest());
            }
            else if (selection == SELECTION.QUIT)
            {
                ION.get().Exit();
            }
            else if (selection == SELECTION.OPTIONS)
            {
                ION.get().Exit();
            }
            else if (selection == SELECTION.MULTIPLAYER)
            {
                Console.WriteLine("sel: " + (int)selection);
                ION.get().setState(new StateMP());
            }
        }


        //checks if the mouse coordinates are in the rectangle
       
     
        private void selectionUp()
        {

            selection--;
            if (selection < SELECTION.NEWGAME)
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
                selection = SELECTION.NEWGAME;
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
