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
            QUIT

        }
        //Initialy the first option is selected
        public SELECTION selection = SELECTION.NEWGAME;

     
        public Rectangle newGameButton;
        public Rectangle mpButton;
        public Rectangle quitButton;

        public bool mousePressed = false; 

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
        public Boolean mouseIn(int mx, int my, Rectangle rect)
        {
            if ((mx > rect.X && mx < (rect.X + rect.Width)) && (my > rect.Y && my < (rect.Y + rect.Height)))
            {
                return true;
            }

            return false;
        
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
