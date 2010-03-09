﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace G.O
{
    public class StateTest : State
    {

        private Grid map;

        private static StateTest instance;

        private bool musicPaused = false;

        private int playqueue = 1;

        private string level = "Level1.xml";

        private bool actionOnScreen = false;

        private SoundEffectInstance actionOnScreenSound = null;
        private float musicVolume = 1.0f;
        private float actionSoundVolume = 0.0f;

        public StateTest()
        {
            instance = this;

            map = new Grid(level);

            actionOnScreenSound = Music.actionSound1.CreateInstance();
            actionOnScreenSound.IsLooped = true;
        }

        public static StateTest get()
        {
            return instance;
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            GO.get().GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            spriteBatch.DrawString(Fonts.font, "Press Escape for Menu, F1 to quit directly, Space to trigger action sounds (now: "+actionOnScreen+")(musicvolume:"+MediaPlayer.Volume+")", new Vector2(10,10), Color.Red);
            spriteBatch.End();

            map.draw(spriteBatch);
        }

        public override void update(int ellapsed)
        {
            //if (!isplaying)
            //{
            //    MediaPlayer.Play(Music.gameSong1);
            //    MediaPlayer.IsRepeating = false;
            //    isplaying = true;
            //}

            if (MediaPlayer.State.Equals(MediaState.Stopped))
            {
                if (playqueue == 1)
                {
                    MediaPlayer.Play(Music.gameSong1);
                    playqueue = 2;
                }
                else if (playqueue == 2)
                {
                    MediaPlayer.Play(Music.actionSong1);
                   // MediaPlayer.Play(Music.gameSong2);
                    playqueue = 3;
                }
                else if (playqueue == 3)
                {
                    MediaPlayer.Play(Music.gameSong2);
                    playqueue = 1;
                }
            }

            KeyboardState keyState = Keyboard.GetState();
            
            if(keyState.IsKeyDown(Keys.Escape)) 
            {
                GO.get().setState(new StatePaused()); 
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
            GO.get().IsMouseVisible = true;
            
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
            GO.get().IsMouseVisible = false;
            
            MediaPlayer.Pause();
            musicPaused = true;
        }
    }
}
