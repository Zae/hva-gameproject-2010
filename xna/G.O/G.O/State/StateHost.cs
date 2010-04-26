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

        
   
        public SELECTION selection = SELECTION.BACK;

        private Color fadeColor = new Color(124, 124, 255, 220);

        private Rectangle waitScreen;
        private Rectangle backButton;
        private Rectangle startButton;
        private Rectangle nameField;

        private bool mousePressed = false;
        public bool upPressed = false;
        public bool downPressed = false;
        public bool enterPressed = false;
        public bool inTextField = false;
        private bool waitState = false;

        
        
        String name = "";
        String tempName=" ";
        bool[] pressedKeys = new bool[256];
        bool spacePressed = false;
        bool backPressed = false;

        




        public StateHost()
        {


            waitScreen = new Rectangle(100, 100, ION.width - 200, ION.height - 200);
            backButton = new Rectangle((ION.width / 2) - 500, (ION.height / 2) + 300, Images.buttonBack.Width, Images.buttonBack.Height);
            startButton = new Rectangle((ION.width / 2) - 125, (ION.height / 2) + 300, Images.buttonJoin.Width, Images.buttonJoin.Height);
            nameField = new Rectangle((ION.width / 2) - 125, (ION.height / 2) + 100, Images.buttonJoin.Width, Images.buttonJoin.Height);
           
        }



        public override void draw()
        {
            ION.get().GraphicsDevice.Clear(Color.Black);

            
           
                ION.spriteBatch.Begin();
                ION.spriteBatch.Draw(Images.ION_LOGO, new Rectangle((ION.width / 2) - 200, (ION.height / 2) - 170, Images.ION_LOGO.Width, Images.ION_LOGO.Height), Color.White);




                if (selection == SELECTION.NAMEFIELD)
                {
                    //Draw haaighlighted
                    ION.spriteBatch.Draw(Images.white1px, nameField, Color.White);
                    ION.spriteBatch.DrawString(Fonts.font, name, new Vector2(nameField.X + 15, nameField.Y + 15), Color.Black);
                    KeyboardState keyState = Keyboard.GetState();


                    if (keyState.GetPressedKeys().Length > 0)
                    {
                        foreach (Keys k in keyState.GetPressedKeys())
                        {
                            if (k.ToString().Length == 1)
                            {
                                pressedKeys[k.ToString()[0]] = true;
                            }
                            if (k.Equals(Keys.Back))
                            {
                                backPressed = true;
                            }
                            if (k.Equals(Keys.Space))
                            {
                                spacePressed = true;
                            }
                        }
                    }

                    if (keyState.GetPressedKeys().Length == 0)
                    {
                        int i = 0;
                        foreach (bool b in pressedKeys)
                        {
                            if (b)
                            {
                                name += (char)i;
                                // b = false;
                            }
                            i++;
                        }
                        pressedKeys = new bool[256];

                        if (spacePressed)
                        {
                            name += " ";
                            spacePressed = false;
                        }
                        if (backPressed && name.Length > 0)
                        {
                            name = name.Substring(0, name.Length - 1);
                            backPressed = false;
                        }

                    }




                }
                else
                {


                    //Draw normally
                    ION.spriteBatch.Draw(Images.white1px, nameField, fadeColor);
                    ION.spriteBatch.DrawString(Fonts.font, name, new Vector2(nameField.X + 15, nameField.Y + 15), Color.Black);
                }



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
                    ION.spriteBatch.Draw(Images.buttonNewGameF, startButton, Color.White);
                }
                else
                {
                    //Draw normally
                    ION.spriteBatch.Draw(Images.buttonNewGame, startButton, Color.White);
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
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                mousePressed = true;
            }

            if (mouseIn(mouseState.X, mouseState.Y, nameField))
            {
                selection = SELECTION.NAMEFIELD;
                if (mouseState.LeftButton == ButtonState.Released && mousePressed == true)
                {
                    makeSelection();
                    mousePressed = false;
                }

            }


            if (mouseIn(mouseState.X, mouseState.Y, startButton))
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

            if (selection == SELECTION.NAMEFIELD)
            {
                inTextField = true;
            }
            if (selection == SELECTION.START)
            {
                ION.instance.serverConnection.HostRoom(name);
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

            ION.spriteBatch.Draw(Images.white1px, waitScreen, fadeColor);
            ION.spriteBatch.DrawString(Fonts.font, "Waiting for an opponent......", new Vector2(waitScreen.X + 100, waitScreen.Bottom - 100), Color.Black);
            ION.spriteBatch.DrawString(Fonts.font, "press 'escape' to cancel", new Vector2(waitScreen.X + 100, waitScreen.Bottom - 60), Color.Black);
            
            ION.spriteBatch.End();

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
