using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ION
{
    class StateIntro : State
    {

        int lastTime = 0;

        
        public StateIntro()
        {
          

        }

        public override void draw()
        {
            ION.get().GraphicsDevice.Clear(Color.Black);

            ION.spriteBatch.Begin();

            //logo
            ION.spriteBatch.Draw(Images.ION_LOGO, new Rectangle((ION.width / 2) - 200, (ION.height / 2) - 170, Images.ION_LOGO.Width, Images.ION_LOGO.Height), Color.White);
            


            



            ION.spriteBatch.End();
        }

        public override void update(int ellapsed)
        {

           
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                //mousePressed = true;
            }

    
            

        }

        

        

        public override void focusGained()
        {
            ION.get().IsMouseVisible = true;
            //MediaPlayer.Play(Music.titleSong);
            //MediaPlayer.IsRepeating = true;
        }

        public override void focusLost()
        {
            ION.get().IsMouseVisible = false;
            //MediaPlayer.Stop();
        }

    }
}
