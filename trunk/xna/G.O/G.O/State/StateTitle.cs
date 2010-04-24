﻿using System;
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
            QUIT

        }
        //Initialy the first option is selected
        public SELECTION selection = SELECTION.NEWGAME;

     
        public Rectangle newGameButton;
        public Rectangle mpButton;
        public Rectangle quitButton;

        private bool mousePressed = false;
        public bool upPressed = false;
        public bool downPressed = false;
        public bool enterPressed = false;

        public StateTitle()
        {
            newGameButton = new Rectangle((ION.width / 2) - 125, (ION.height / 2) , Images.buttonNewGame.Width, Images.buttonNewGame.Height);
            mpButton = new Rectangle((ION.width / 2) - 125, (ION.height / 2) + 70, Images.buttonNewGame.Width, Images.buttonNewGame.Height);
            quitButton = new Rectangle((ION.width / 2) - 125, (ION.height / 2) + 140, Images.buttonNewGame.Width, Images.buttonNewGame.Height);

        }

        public override void draw()
        {
            ION.get().GraphicsDevice.Clear(Color.Black);

            ION.spriteBatch.Begin();

            //logo
            ION.spriteBatch.Draw(Images.ION_LOGO, new Rectangle((ION.width / 2) - 200, (ION.height / 2) - 170, Images.ION_LOGO.Width, Images.ION_LOGO.Height), Color.White);
            


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
