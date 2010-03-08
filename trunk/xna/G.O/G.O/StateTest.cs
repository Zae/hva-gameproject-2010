using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace G.O
{
    public class StateTest : State
    {

        private static StateTest instance;

        public StateTest()
        {
            instance = this;
        }

        public static StateTest get()
        {
            return instance;
        }

        public override void draw(SpriteBatch spritebatch)
        {
            GO.get().GraphicsDevice.Clear(Color.White);
        }

        public override void update()
        {

            KeyboardState keyState = Keyboard.GetState();
            
            if(keyState.IsKeyDown(Keys.Escape)) 
            {
                GO.get().state = new StatePaused(); 
            }


            //if (keyState.IsKeyDown(Keys.Up) && !upPressed)
            //{
            //    selectionDown();
            //    upPressed = true;
            //}
            //else if (keyState.IsKeyUp(Keys.Up) && upPressed)
            //{
            //    upPressed = false;
            //}


            //if (keyState.IsKeyDown(Keys.Down) && !downPressed)
            //{
            //    selectionDown();
            //    downPressed = true;
            //}
            //else if (keyState.IsKeyUp(Keys.Down) && downPressed)
            //{
            //    downPressed = false;
            //}

            //if (keyState.IsKeyDown(Keys.Enter))
            //{
            //    makeSelection();
            //}
            
        }
    }
}
