using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace GO
{
    public class StateTitle : State
    {
        public enum SELECTION
        {
            NEWGAME = 1,
            QUIT
        }

        private const String title = "Land Conquerer"; //"G.O."
        private const String newGame = "New game";
        private const String quit = "Quit";

        //Initialy the first option is selected
        public SELECTION selection = SELECTION.NEWGAME;

        public bool upPressed = false;
        public bool downPressed = false;
    
        public StateTitle()
        {
          
           
        }

        public override void draw()
        {
            GO.get().GraphicsDevice.Clear(Color.Black);

            GO.spriteBatch.Begin();
            GO.spriteBatch.DrawString(Fonts.font, title, new Vector2((GO.width / 2) - 15, (GO.height / 2) - 150), Color.White);
            //spriteBatch.DrawString(Fonts.font, "press any key to start the test", new Vector2((GO.width/2)-100, (GO.height / 2)), Color.White);

            if (selection == SELECTION.NEWGAME)
            {
                //Draw highlighted
                GO.spriteBatch.DrawString(Fonts.font, newGame, new Vector2((GO.width / 2) - 100, (GO.height / 2)), Color.Red);
            }
            else
            {
                //Draw normally
                GO.spriteBatch.DrawString(Fonts.font, newGame, new Vector2((GO.width / 2) - 100, (GO.height / 2)), Color.White);
            }

            if (selection == SELECTION.QUIT)
            {
                //Draw highlighted
                GO.spriteBatch.DrawString(Fonts.font, quit, new Vector2((GO.width / 2) - 100, (GO.height / 2) - 50), Color.Red);
            }
            else
            {
                //Draw normally
                GO.spriteBatch.DrawString(Fonts.font, quit, new Vector2((GO.width / 2) - 100, (GO.height / 2) - 50), Color.White);
            }



            GO.spriteBatch.End();
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
                selectionDown();
                upPressed = true;
            }
            else if(keyState.IsKeyUp(Keys.Up) && upPressed) 
            {
                upPressed = false;
            }


            if (keyState.IsKeyDown(Keys.Down) && !downPressed)
            {
                selectionDown();
                downPressed = true;
            }
            else if(keyState.IsKeyUp(Keys.Down) && downPressed) 
            {
                downPressed = false;
            }

            if (keyState.IsKeyDown(Keys.Enter))
            {
                makeSelection();
            }

        }

        private void selectionUp()
        {
            selection++;
            if ((int)selection > Enum.GetNames(typeof(SELECTION)).Length)
            {
                selection = SELECTION.NEWGAME;
            }
        }

        private void selectionDown()
        {
            selection--;
            if (selection < SELECTION.NEWGAME)
            {
                selection = (SELECTION) Enum.GetNames(typeof(SELECTION)).Length;
            }
        }

        private void makeSelection()
        {
            if (selection == SELECTION.NEWGAME)
            {
                GO.get().setState(new StateTest());
            }
            else if (selection == SELECTION.QUIT)
            {
                GO.get().Exit();
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
