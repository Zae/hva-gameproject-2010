using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Net;
using System.IO;
using FluorineFx.Net;
using FluorineFx.Messaging.Api.Service;
using WindowSystem;



namespace ION
{


    class StateHost : State
    {

        public enum SELECTION
        {
            BACK = 1,
            START,
            NAMEFIELD

        }

        private List<UIComponent> ComponentList;
   
        public SELECTION selection = SELECTION.BACK;

        private Color fadeColor = Color.Orange;

        private Rectangle waitScreen;
        private Rectangle backButton;
        private Rectangle hostButton;
        private Rectangle nameCaption;
        private TextBox nameField;

        private Rectangle background_overlay;
        private Rectangle Logo;
        private Rectangle background_starfield;

        private bool mousePressed = false;
        public bool upPressed = false;
        public bool downPressed = false;
        public bool enterPressed = false;
        public bool inTextField = false;
        private bool waitState = false;

        bool[] pressedKeys = new bool[256];

        public StateHost()
        {
            background_overlay = new Rectangle(ION.width - Images.background_overlay.Width, 0, Images.background_overlay.Width, Images.background_overlay.Height);
            Logo = new Rectangle(ION.width / 100 * 10, ION.height - ION.height / 100 * 7 - Images.Logo.Height, Images.Logo.Width, Images.Logo.Height);
            background_starfield = new Rectangle(0, 0, Images.background_starfield.Width, Images.background_starfield.Height);

            waitScreen = new Rectangle(100, 100, ION.width - 200, ION.height - 200);
            backButton = new Rectangle(125, 125, Images.buttonBack.Width, Images.buttonBack.Height);
            nameCaption = new Rectangle(125, 300, Images.roomCaption.Width, Images.roomCaption.Height);
            nameField = new TextBox(ION.instance, ION.instance.gui);
            nameField.X = nameCaption.Right+25;
            nameField.Y = nameCaption.Top;
            hostButton = new Rectangle(nameField.X + nameField.Width + 25, nameField.Y, Images.buttonJoin.Width, Images.buttonJoin.Height);

            ComponentList = new List<UIComponent>();
            ComponentList.Add(nameField);
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
            ION.spriteBatch.Draw(Images.Logo, Logo, Color.White);

            ION.spriteBatch.Draw(Images.roomCaption, nameCaption, Color.White);

            if (selection == SELECTION.BACK)
            {
                //Draw highlighted
                ION.spriteBatch.Draw(Images.buttonBackF, backButton, Color.White);
            }
            else
            {
                //Draw normally
                ION.spriteBatch.Draw(Images.buttonBack, backButton, Color.White);
            }
            if (selection == SELECTION.START)
            {
                //Draw highlighted
                ION.spriteBatch.Draw(Images.buttonHostF, hostButton, Color.White);
            }
            else
            {
                //Draw normally
                ION.spriteBatch.Draw(Images.buttonHost, hostButton, Color.White);
            }

            ION.spriteBatch.End();
            if (waitState)
            {
                wait();
                ION.get().IsMouseVisible = false;
            }            
        }

        public override void update(int ellapsed)
        {

            //mouse handling
            Microsoft.Xna.Framework.Input.MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                mousePressed = true;
            }

            if (mouseIn(mouseState.X, mouseState.Y, hostButton))
            {
                selection = SELECTION.START;
                if (mouseState.LeftButton == ButtonState.Released && mousePressed == true)
                {
                    makeSelection();
                    mousePressed = false;
                }

            }
            if (mouseIn(mouseState.X, mouseState.Y, backButton))
            {
                selection = SELECTION.BACK;
                if (mouseState.LeftButton == ButtonState.Released && mousePressed == true)
                {
                    makeSelection();
                    mousePressed = false;
                }

            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                waitState = false;
                ION.get().IsMouseVisible = true;
             
            }

        }

      

        private void makeSelection()
        {
            if (selection == SELECTION.START)
            {
                ION.instance.serverConnection.HostRoom(nameField.Text);
                waitState = true;
                wait();
                
            }
            else if (selection == SELECTION.BACK)
            {
                StateMP st = new StateMP();

                ION.get().setState(st);
            }

          
        }

        public void wait()
        {
            ION.spriteBatch.Begin();

            ION.spriteBatch.Draw(Images.waitScreen, waitScreen, Color.White);
            ION.spriteBatch.DrawString(Fonts.font, "Waiting for an opponent......", new Vector2(waitScreen.X + 100, waitScreen.Bottom - 100), Color.Black);
            ION.spriteBatch.DrawString(Fonts.font, "press 'escape' to cancel", new Vector2(waitScreen.X + 100, waitScreen.Bottom - 60), Color.Black);
            
            ION.spriteBatch.End();

        }

        public override void focusGained()
        {
            //push all gui components of this state to the gui manager
            foreach (UIComponent uic in ComponentList)
            {
                ION.instance.gui.Add(uic);
            }
            //
            ION.get().IsMouseVisible = true;
            //MediaPlayer.Play(Sounds.titleSong);
            //MediaPlayer.IsRepeating = true;
        }

        public override void focusLost()
        {
            //remove all the gui components of this state from the gui manager
            foreach (UIComponent uic in ComponentList)
            {
                ION.instance.gui.Remove(uic);
            }
            //
            ION.get().IsMouseVisible = false;
            //MediaPlayer.Stop();
        }



    }


   




}
