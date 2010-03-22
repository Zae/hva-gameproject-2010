using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace ION
{
    public class StateTest : State
    {

        private Grid map;

        private int scrollValue;

        private static StateTest instance;

        private bool musicPaused = false;

        private int playqueue = 1;

        private string level = "Level1.xml";

        private bool actionOnScreen = false;

        private SoundEffectInstance actionOnScreenSound = null;
        //private float musicVolume = 1.0f;
        //private float actionSoundVolume = 0.0f;

        private int translationX = 0;
        private int translationY = 0;

        private int previousMouseX = 0;
        private int previousMouseY = 0;

        public StateTest()
        {
            instance = this;

            scrollValue = Mouse.GetState().ScrollWheelValue;


            map = new Grid(level);

            actionOnScreenSound = Music.actionSound1.CreateInstance();
            actionOnScreenSound.IsLooped = true;
        }

        public static StateTest get()
        {
            return instance;
        }

        public override void draw()
        {
            ION.get().GraphicsDevice.Clear(Color.Gray);

            map.draw(translationX, translationY);

            int y = 0;
            ION.spriteBatch.Begin();
            ION.spriteBatch.DrawString(Fonts.font, "Press Escape for Menu, F1 to quit directly", new Vector2(10, y += 15), Color.Red);
            ION.spriteBatch.DrawString(Fonts.font, "Space to trigger action sounds (now: " + actionOnScreen + ")(musicvolume:" + MediaPlayer.Volume + ")", new Vector2(10, y += 15), Color.Red);
            ION.spriteBatch.DrawString(Fonts.font, "Use the middle mouse button to drag the map around, press Left-Alt to recenter the map", new Vector2(10, y += 15), Color.Red);
            ION.spriteBatch.End();
        }

        public override void update(int ellapsed)
        {
            map.update(ellapsed);
            
            //Handles which background music to play
            if (MediaPlayer.State.Equals(MediaState.Stopped))
            {
                if (playqueue == 1)
                {
                    MediaPlayer.Play(Music.gameSong1);
                    playqueue = 2;
                }
                else if (playqueue == 2)
                {
                    MediaPlayer.Play(Music.gameSong2);
                    playqueue = 1;
                }
            }

            //Handle keyboard input
            KeyboardState keyState = Keyboard.GetState();

            //Handles mouse input
            MouseState mouseState = Mouse.GetState();

            
            
            if(keyState.IsKeyDown(Keys.Escape)) 
            {
                ION.get().setState(new StatePaused()); 
            }

            if (keyState.IsKeyDown(Keys.U))
            {
                map.createUnit(mouseState.X, mouseState.Y, translationX, translationY, Players.PLAYER1);
            }
            else if (keyState.IsKeyDown(Keys.I))
            {
                map.createUnit(mouseState.X, mouseState.Y, translationX, translationY, Players.PLAYER2);
            }
     

            if (keyState.IsKeyDown(Keys.LeftAlt))
            {
                translationX = 0;
                translationY = 0;
            }

            if (keyState.IsKeyDown(Keys.Space))
            {
                actionOnScreen = true;
            }
            else
            {
                actionOnScreen = false;
            }
            handleActionSound(ellapsed);

            //Handle mouse input

            if (mouseState.ScrollWheelValue > scrollValue)
            {
                Tile.zoomIn();
                //Debug.WriteLine("zoomIn");

            }
            else if (mouseState.ScrollWheelValue < scrollValue)
            {
                Tile.zoomOut();
               // Debug.WriteLine("zoomOut");
            }

            scrollValue = mouseState.ScrollWheelValue;

            

            //Debug.WriteLine("scrollvalue = " +mouseState.ScrollWheelValue);

            if (mouseState.MiddleButton == ButtonState.Pressed)
            {
                translationX += mouseState.X - previousMouseX;
                translationY += mouseState.Y - previousMouseY;
            }

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                map.mouseLeftPressed(mouseState.X, mouseState.Y, translationX, translationY);
            }
            else if (mouseState.LeftButton == ButtonState.Released)
            {
                map.mouseLeftReleased(mouseState.X, mouseState.Y, translationX, translationY);
            }

            if (mouseState.RightButton == ButtonState.Pressed)
            {
                map.mouseRightPressed(mouseState.X, mouseState.Y, translationX, translationY);
            }
            else if (mouseState.RightButton == ButtonState.Released)
            {
                map.mouseRightReleased(mouseState.X, mouseState.Y, translationX, translationY);
            }

            previousMouseX = mouseState.X;
            previousMouseY = mouseState.Y; 
        }

        //Used to fade in and fade out the action sound
        private void handleActionSound(int ellapsed)
        {

            if (actionOnScreen)
            {
                
                if(actionOnScreenSound.State == SoundState.Stopped) {
                    actionOnScreenSound.Volume = 0.0f;
                    actionOnScreenSound.Play();
                }

                if(MediaPlayer.Volume > 0.0f)
                {
                    MediaPlayer.Volume = MediaPlayer.Volume - (0.0003f * ellapsed);
                }
                if (actionOnScreenSound.Volume < 1.0f)
                {
                    float newVolume = actionOnScreenSound.Volume + (0.0003f * ellapsed);

                    if (newVolume > 1.0f)
                    {
                        actionOnScreenSound.Volume = 1.0f;
                    }
                    else
                    {
                        actionOnScreenSound.Volume = newVolume;
                    }

                } 
            }
            else
            {
                
                if (MediaPlayer.Volume < 1.0f)
                {
                    MediaPlayer.Volume = MediaPlayer.Volume + (0.0003f * ellapsed);
                }

                if (actionOnScreenSound.Volume > 0.0f)
                {
                    float newVolume = actionOnScreenSound.Volume - (0.0003f * ellapsed);
                    
                    if (newVolume < 0.0f)
                    {
                        actionOnScreenSound.Volume = 0.0f;
                    }
                    else
                    {
                        actionOnScreenSound.Volume = newVolume;
                    }
                }
                else if(actionOnScreenSound.State == SoundState.Playing) 
                {
                     actionOnScreenSound.Stop();
                }

            }
            
        }

        public override void focusGained()
        {
            ION.get().IsMouseVisible = true;
            
            //MediaPlayer.Play(Music.gameSong1);
            //MediaPlayer.IsRepeating = false;
            if (musicPaused)
            {
                MediaPlayer.Resume();
                musicPaused = false;
            }
        }

        public override void focusLost()
        {
            ION.get().IsMouseVisible = false;
            
            MediaPlayer.Pause();
            musicPaused = true;
        }
    }
}
