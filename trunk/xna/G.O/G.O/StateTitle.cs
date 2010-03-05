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
        //public int playerX = 0;
        //public int playerY = 0;

        public bool isplaying = false;
     
        public StateTitle()
        {
          
           
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(Fonts.font, "G.O.", new Vector2((GO.width/2)-15,(GO.height/2)-150), Color.White);
            spriteBatch.DrawString(Fonts.font, "press any key to start the test", new Vector2((GO.width/2)-100, (GO.height / 2)), Color.White);
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

        
            if (keyState.GetPressedKeys().Length > 0)
            {
                GO.get().state = new StateTest();
            }
            //if (keyState.IsKeyDown(Keys.Left))
            //    playerX -= 1;

            //if (keyState.IsKeyDown(Keys.Right))
            //    playerX += 1;


            //if (keyState.IsKeyDown(Keys.Up))
            //{
            //    playerY -= 1;
            //}


            //if (keyState.IsKeyDown(Keys.Down))
            //{
            //    playerY += 1;
            //}
        }
    }
}
