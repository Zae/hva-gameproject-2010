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
using ION.GridStrategies;
using ION.UI;
using ION.Controls;
using ION.Tools;

namespace ION
{
    public class StateTest : State
    {

        public Grid grid;
        public GUIManager gui;
        public ControlState controls;

        public float scrollValue;

        private static StateTest instance;

        private bool musicPaused = false;
        private int playqueue = 1;

        public int level = 0;
        public string[] levels = { "MediumLevelTest.xml", "PathLevelTest.xml", "LargeLevelTest.xml", "WideLevelTest.xml", "WideLevelTest2.xml" };

        public GridStrategy[] strategies = { new ThunderStrategy(0), new CreepStrategy(), new FlowStrategy() };
        public int strategy = 0;

        public bool actionOnScreen = false;
        private SoundEffectInstance actionOnScreenSound = null;

        public float translationX = 0f;
        public float translationY = 0f;

        public static float previousMouseX = 0f;
        public static float previousMouseY = 0f;

        private bool nextMapDown = false;
        private bool previousMapDown = false;

        private bool nextStrategyDown = false;
        private bool previousStrategyDown = false;
   
        public float player1Control = 0.0f;
        public float player2Control = 0.0f;

        public bool showHelpFile = false;

        public List<Unit> selection = new List<Unit>();

        public bool online = false;

        //This is for use with multiplayer
        public StateTest(int player, int seed, string level, bool online)
        {
            instance = this;

            this.online = online;

            scrollValue = Mouse.GetState().ScrollWheelValue;

            SoundManager.init();

            grid = new Grid(level, new ThunderStrategy(seed), player);
            gui = new GUIManager();
            controls = new NeutralState();

            actionOnScreenSound = Sounds.actionSound1.CreateInstance();
            actionOnScreenSound.IsLooped = true;

           
        }

        public static StateTest get()
        {
            return instance;
        }

        public override void draw()
        {
            //ION.get().GraphicsDevice.Clear(Color.Black);

            ION.spriteBatch.Begin();
            ION.spriteBatch.Draw(Images.gameBackground, new Rectangle(0, 0, ION.width, ION.height), Color.White);
            ION.spriteBatch.End();

            grid.draw(translationX, translationY);

            controls.draw();
      
            gui.draw();

            if (showHelpFile)
            {
                ION.spriteBatch.Begin();
                ION.spriteBatch.Draw(Images.helpFile, new Rectangle(ION.halfWidth-(Images.helpFile.Width/2),ION.halfHeight-(Images.helpFile.Height/2), Images.helpFile.Width,Images.helpFile.Height), Color.White);
                ION.spriteBatch.End();
            }
        }

        public override void update(int ellapsed)
        {
            grid.update(ellapsed, grid.allUnits, translationX, translationY);

            //get keyboard input
            KeyboardState keyState = Keyboard.GetState();

            //get mouse input
            MouseState mouseState = Mouse.GetState();

           

            if (!gui.handleMouse(mouseState))
            {
                controls.handleInput(mouseState,keyState);
            }

            //Handles which background music to play
            if (MediaPlayer.State.Equals(MediaState.Stopped))
            {
                if (playqueue == 1)
                {
                    MediaPlayer.Play(Sounds.gameSong1);
                    playqueue = 2;
                }
                else if (playqueue == 2)
                {
                    MediaPlayer.Play(Sounds.gameSong2);
                    playqueue = 1;
                }
            }

            if (!online)
            {
                if (keyState.IsKeyDown(Keys.Escape))
                {
                    ION.get().setState(new StatePaused());
                }

                //MAP CHANGING CONTROLS
                if (keyState.IsKeyDown(Keys.M) && !nextMapDown)
                {
                    nextMapDown = true;
                    if (level < levels.Length - 1)
                    {
                        level++;
                    }
                    else
                    {
                        level = 0;
                    }
                    grid = new Grid(levels[level], strategies[strategy], Players.PLAYER1);
                    gui = new GUIManager();
                    controls = new NeutralState();
                }
                else if (keyState.IsKeyUp(Keys.M))
                {
                    nextMapDown = false;
                }

                if (keyState.IsKeyDown(Keys.N) && !previousMapDown)
                {
                    previousMapDown = true;
                    if (level > 0)
                    {
                        level--;
                    }
                    else
                    {
                        level = levels.Length - 1;
                    }
                    grid = new Grid(levels[level], strategies[strategy], Players.PLAYER1);
                    gui = new GUIManager();
                    controls = new NeutralState();
                }
                else if (keyState.IsKeyUp(Keys.N))
                {
                    previousMapDown = false;
                }

                //STRATEGY CHANGING CONTROLS
                if (keyState.IsKeyDown(Keys.K) && !nextStrategyDown)
                {
                    nextStrategyDown = true;
                    if (strategy < strategies.Length - 1)
                    {
                        strategy++;
                    }
                    else
                    {
                        strategy = 0;
                    }
                    grid = new Grid(levels[level], strategies[strategy], Players.PLAYER1);
                    gui = new GUIManager();
                    controls = new NeutralState();
                }
                else if (keyState.IsKeyUp(Keys.K))
                {
                    nextStrategyDown = false;
                }

                if (keyState.IsKeyDown(Keys.J) && !previousStrategyDown)
                {
                    previousStrategyDown = true;
                    if (strategy > 0)
                    {
                        strategy--;
                    }
                    else
                    {
                        strategy = strategies.Length - 1;
                    }
                    grid = new Grid(levels[level], strategies[strategy], Players.PLAYER1);
                    gui = new GUIManager();
                    controls = new NeutralState();
                }
                else if (keyState.IsKeyUp(Keys.J))
                {
                    previousStrategyDown = false;
                }

                //SPEED CHANGING
                if (keyState.IsKeyDown(Keys.O))
                {
                    grid.getUpdateStrategy().increaseSpeed();
                }
                if (keyState.IsKeyDown(Keys.I))
                {
                    grid.getUpdateStrategy().decreaseSpeed();
                }
            }
            

            //CENTER MAP
            if (keyState.IsKeyDown(Keys.LeftAlt))
            {
                translationX = 0;
                translationY = 0;
            }
    
            ////DEBUG MUSIC
            //if (keyState.IsKeyDown(Keys.Space))
            //{
            //    actionOnScreen = true;
            //}
            //else
            //{
            //    actionOnScreen = false;
            //}
            handleActionSound(ellapsed);

            //HELP FILE
            if (keyState.IsKeyDown(Keys.H))
            {
                showHelpFile = true;
            }
            else
            {
                showHelpFile = false;
            }
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

            actionOnScreenSound.Volume = 0.0f;
            MediaPlayer.Volume = 0.0f;
        }

        public override void focusGained()
        {
            ION.get().IsMouseVisible = true;

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
