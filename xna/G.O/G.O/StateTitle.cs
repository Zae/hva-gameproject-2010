using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace G.O
{
    public class StateTitle : State
    {
        private const int SELECTION_NEWGAME = 1;
        private const int SELECTION_QUIT = 2;

        //The number of selectable options
        private const int SELECTION_COUNT = 2;

        private const String title = "Land Conquerer"; //"G.O."
        private const String newGame = "New game";
        private const String quit = "Quit";

        //Initialy the first option is selected
        public int selection = 1;

        public bool upPressed = false;
        public bool downPressed = false;

        //Whether the music is currently playing
        public bool isplaying = false;


     
        public StateTitle()
        {
          
           
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(Fonts.font, title, new Vector2((GO.width/2)-15,(GO.height/2)-150), Color.White);
            //spriteBatch.DrawString(Fonts.font, "press any key to start the test", new Vector2((GO.width/2)-100, (GO.height / 2)), Color.White);

            if (selection == SELECTION_NEWGAME)
            {
                //Draw highlighted
                spriteBatch.DrawString(Fonts.font, newGame, new Vector2((GO.width / 2) - 100, (GO.height / 2)), Color.Red);
            }
            else
            {
                //Draw normally
                spriteBatch.DrawString(Fonts.font, newGame, new Vector2((GO.width / 2) - 100, (GO.height / 2)), Color.White);
            }

            if (selection == SELECTION_QUIT)
            {
                //Draw highlighted
                spriteBatch.DrawString(Fonts.font, quit, new Vector2((GO.width / 2) - 100, (GO.height / 2)-50), Color.Red);
            }
            else
            {
                //Draw normally
                spriteBatch.DrawString(Fonts.font, quit, new Vector2((GO.width / 2) - 100, (GO.height / 2)-50), Color.White);
            }
           
            spriteBatch.End();
        }

        public override void update()
        {
            if (Music.titleSong != null && !isplaying)
            {
                MediaPlayer.Play(Music.titleSong);
                isplaying = true;
            }
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
            if (selection > SELECTION_COUNT)
            {
                selection = 1;
            }
        }

        private void selectionDown()
        {
            selection--;
            if (selection < 1)
            {
                selection = SELECTION_COUNT;
            }
        }

        private void makeSelection()
        {
            if (selection == SELECTION_NEWGAME)
            {
                GO.get().state = new StateTest();
            }
            else if (selection == SELECTION_QUIT)
            {
                GO.get().Exit();
            }
        }
    }
}
