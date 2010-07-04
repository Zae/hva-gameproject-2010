using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace ION
{
    class StateIntro : State
    {
        private bool motion1 = true;
        private bool motion2 = true;
        private bool motion3 = true;
        private bool motion4 = true;
        private bool motion5 = true;
        private bool motion6 = true;
        private int now = 0;
        private int playTime = 4000;

        private float scale;
        private float fullScale;

        private int logoHeight;
        private int logoWidth;
        private int logoFullHeight;
        private int logoFullWidth;


        private int logoX;
        private int logoY;

        private int dX;
        private int dY;
        private float dS;

        private SoundEffectInstance logoSound = null;
        
        public StateIntro()
        {
            logoSound = Sounds.logoSound.CreateInstance();
            logoSound.IsLooped = false;
        }

        public override void draw()
        {
           
            ION.spriteBatch.Begin();

            if (now < 4200)
            {
                ION.get().GraphicsDevice.Clear(Color.Black);
                //logo
                ION.spriteBatch.Draw(Images.teamLogoImage, new Rectangle(logoX, logoY, logoWidth, logoHeight), Color.White);
                //ION.spriteBatch.Draw(Images.teamLogoImage, new Rectangle(0, 0, 200, 200), Color.White);
                int y = 0;
                //ION.spriteBatch.Begin();
                //ION.spriteBatch.DrawString(Fonts.font, "ellapsed: " + now, new Vector2(10, y += 15), Color.Green);
                //ION.spriteBatch.End();
            }
           

            if (now > 4200)
            {
                ION.spriteBatch.Draw(Images.white1px, new Rectangle(0, 0, ION.width, ION.height), new Color(0, 0, 0, 14));
            }

            



            ION.spriteBatch.End();
        }

        public override void update(int ellapsed)
        {
            now += ellapsed;
            
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed || now > playTime)
            {
                logoSound.Stop();
                ION.get().setState(new StateTitle());
            }

            if (now < 600)
            {
                if (motion1)
                {
                    motion1 = false;
                    
                    //play sound
                    logoSound.Play();

                    scale = ION.height / Images.teamLogoImage.Height;

                    logoHeight = (int)(Images.teamLogoImage.Height * scale);
                    logoWidth = (int)(Images.teamLogoImage.Width * scale);

                    logoX = -logoWidth;
                    logoY = (ION.height-logoHeight) / 2;

                    dX = (ION.width - logoX) / 100;
                    dY = 0;

                    return;
                }
                
                logoX += ellapsed * dX;
                logoY += ellapsed * dY;
            }
          
            else if (now < 1400)
            {
                if (motion4)
                {
                    motion4 = false;

               
                    logoX = (ION.width-logoWidth) /  2;
                    logoY = ION.height+logoHeight;

                    dX = 0;
                    dY = (-logoHeight - logoY) / 120;

                    return;
                }

                logoX += ellapsed * dX;
                logoY += ellapsed * dY;
            }
            else if (now < 1500)
            {
                if (motion5)
                {
                    motion5 = false;

                
                    fullScale = scale;
                    logoFullHeight = logoHeight;
                    logoFullWidth = logoWidth;

                    scale = 0.02f;
                    
                    logoHeight = (int)(Images.teamLogoImage.Height * scale);
                    logoWidth = (int)(Images.teamLogoImage.Width * scale);

                    logoX = -logoWidth;
                    logoY = (ION.height - logoHeight) / 2;

                    dX = (((ION.width-logoWidth)/2) - logoX) / 100;
                    //dX = ((ION.width) - logoX) / 200;
                    dY = 0;

                    return;
                }

                logoX += ellapsed * dX;
                logoY += ellapsed * dY;
            }
                 //else if (now > 2900 && now < 3500)
            else if (now > 1500 && now < 2100)
            {
                logoX = (ION.width - logoWidth) / 2;
                logoY = (ION.height - logoHeight) / 2;
            }
            else if (now > 2100 && now < 2300)
            {
                if (motion6)
                {
                    motion6 = false;
                    dS = (fullScale - scale) / 210;

                
                    //dX = 0;
                    //dY = 0;

                    //dX = ((((ION.width - logoFullWidth) / 2)) - logoX) / 140;
                    //dY = ((((ION.height - logoFullHeight) / 2 )) - logoY) / 140;

                   
                }
                scale += ellapsed * dS;
           
                logoHeight = (int)(Images.teamLogoImage.Height * scale);
                logoWidth = (int)(Images.teamLogoImage.Width * scale);

                logoX = (ION.width - logoWidth) / 2;
                logoY = (ION.height - logoHeight) / 2;
                


            }
            if ( now > 2300)  
            {
                logoWidth = logoFullWidth;
                logoHeight = logoFullHeight;

                logoX = (ION.width - logoWidth) / 2;
                logoY = (ION.height - logoHeight) / 2;
            }

      

            if (now > playTime)
            {
                ION.get().setState(new StateTitle());
            }

    
            

        }

        public override void focusGained()
        {
            //ION.get().IsMouseVisible = true;
            //MediaPlayer.Play(Sounds.titleSong);
            //MediaPlayer.IsRepeating = true;
        }

        public override void focusLost()
        {
            //ION.get().IsMouseVisible = false;
            //MediaPlayer.Stop();
        }

    }
}
