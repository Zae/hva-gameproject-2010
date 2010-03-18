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

        private const String title = "LAND CONQUERER"; //"G.O."
        private const String newGame = "New game";
        private const String quit = "Quit";
        private const String multiplayer = "Multiplayer Game";

        //Initialy the first option is selected
        public SELECTION selection = SELECTION.NEWGAME;

        public bool upPressed = false;
        public bool downPressed = false;
        public bool enterPressed = false;

        public StateTitle()
        {
            
        }

        public override void draw()
        {
            ION.get().GraphicsDevice.Clear(Color.Black);

            ION.spriteBatch.Begin();




            ION.spriteBatch.Draw(Images.ION_LOGO, new Rectangle((ION.width / 2) - 200, (ION.height / 2) - 170, Images.ION_LOGO.Width, Images.ION_LOGO.Height), Color.White);
            
            //spriteBatch.DrawString(Fonts.font, "press any key to start the test", new Vector2((GO.width/2)-100, (GO.height / 2)), Color.White);

            if (selection == SELECTION.NEWGAME)
            {
                //Draw highlighted
                ION.spriteBatch.Draw(Images.buttonNewGameF, new Rectangle((ION.width / 2) - 125, (ION.height / 2) , Images.buttonNewGame.Width, Images.buttonNewGame.Height), Color.White);
            }
            else
            {
                //Draw normally
                ION.spriteBatch.Draw(Images.buttonNewGame, new Rectangle((ION.width / 2) - 125, (ION.height / 2) , Images.buttonNewGame.Width, Images.buttonNewGame.Height), Color.White);
            }
            if (selection == SELECTION.MULTIPLAYER)
            {
                //Draw highlighted
                ION.spriteBatch.Draw(Images.buttonMPF, new Rectangle((ION.width / 2) - 125, (ION.height / 2) +70, Images.buttonNewGame.Width, Images.buttonNewGame.Height), Color.White);
            }
            else
            {
                //Draw normally
                ION.spriteBatch.Draw(Images.buttonMP, new Rectangle((ION.width / 2) - 125, (ION.height / 2) +70, Images.buttonNewGame.Width, Images.buttonNewGame.Height), Color.White);
            }


            if (selection == SELECTION.QUIT)
            {
                //Draw highlighted
                ION.spriteBatch.Draw(Images.buttonQuitF, new Rectangle((ION.width / 2) - 125, (ION.height / 2) +140, Images.buttonNewGame.Width, Images.buttonNewGame.Height), Color.White);
            }
            else
            {
                //Draw normally
                ION.spriteBatch.Draw(Images.buttonQuit, new Rectangle((ION.width / 2) - 125, (ION.height / 2) +140, Images.buttonNewGame.Width, Images.buttonNewGame.Height), Color.White);
            }



            ION.spriteBatch.End();
        }

        public override void update(int ellapsed)
        {

            KeyboardState keyState = Keyboard.GetState();


            //if (keyState.GetPressedKeys().Length > 0)
            //{
            //    GO.get().state = new StateTest();
            //}
            //if (keyState.IsKeyDown(Keys.Left))
            //    playerX -= 1;

            //if (keyState.IsKeyDown(Keys.Right))
            //    playerX += 1;


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

            if (keyState.IsKeyDown(Keys.Enter) && !enterPressed)
            {
                makeSelection();
                enterPressed = true;
            }

            if (keyState.IsKeyDown(Keys.Enter) && enterPressed)
            {

                enterPressed = false;
            }

        }

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