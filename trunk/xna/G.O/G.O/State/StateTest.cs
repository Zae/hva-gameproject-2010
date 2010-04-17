﻿using System;
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

namespace ION
{
    public class StateTest : State
    {

        private Grid grid;
        private GUIManager gui;
        private ControlState controls;

        private float scrollValue;

        private static StateTest instance;

        private bool musicPaused = false;

        private int playqueue = 1;

        public int level = 0;
        public string[] levels = { "MediumLevelTest.xml", "PathLevelTest.xml","LargeLevelTest.xml"}; //also available ,"BigLevelTest.xml","Level1.xml"

        public GridStrategy[] strategies = { new ThunderStrategy(), new CreepStrategy(), new FlowStrategy(), new BleedStrategy() };
        public int strategy = 0;

        private bool actionOnScreen = false;

        private SoundEffectInstance actionOnScreenSound = null;

        private float translationX = 0f;
        private float translationY = 0f;

        private float previousMouseX = 0f;
        private float previousMouseY = 0f;

        private bool nextMapDown = false;
        private bool previousMapDown = false;

        private bool nextStrategyDown = false;
        private bool previousStrategyDown = false;

        private bool increaseSpeedDown = false;
        private bool decreaseSpeedDown = false;

        public int playerEnergy = 0;
        public float player1Control = 0.0f;
        public float player2Control = 0.0f;

        Vector2 oldMousePos, mousePos;
        bool leftMouseDown = false;

        public bool showHelpFile = false;

     
        //we need to create a shared object blueUnits


        static int unitCounter = 999;

        public StateTest()
        {
            instance = this;

            scrollValue = Mouse.GetState().ScrollWheelValue;

            grid = new Grid(levels[level], strategies[strategy]);
            gui = new GUIManager();
            controls = new NeutralState();

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

            ION.spriteBatch.Begin();
            ION.spriteBatch.Draw(Images.starfieldImage, new Rectangle(0, 0, ION.width, ION.height), Color.White);
            ION.spriteBatch.End();

            grid.draw(translationX, translationY);



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

            grid.update(ellapsed, grid.blueArmy, translationX, translationY);

            playerEnergy++;

            if (!gui.handleMouse(ellapsed))
            {
                controls.handleMouse(ellapsed);
            }
            controls.handleKeyboard(ellapsed);
            
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
                grid = new Grid(levels[level],strategies[strategy]);   
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
                grid = new Grid(levels[level],strategies[strategy]);  
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
                grid = new Grid(levels[level],strategies[strategy]);
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
                grid = new Grid(levels[level],strategies[strategy]);
            }
            else if (keyState.IsKeyUp(Keys.J))
            {
                previousStrategyDown = false;
            }

            //SPEED CHANGING
            if (keyState.IsKeyDown(Keys.O) && !increaseSpeedDown)
            {
                //increaseSpeedDown = true;
                grid.getUpdateStrategy().increaseSpeed();
            }
            else if (keyState.IsKeyUp(Keys.O))
            {
                //increaseSpeedDown = false;
            }

            if (keyState.IsKeyDown(Keys.I) && !decreaseSpeedDown)
            {
               // decreaseSpeedDown = true;
                grid.getUpdateStrategy().decreaseSpeed();
            }
            else if (keyState.IsKeyUp(Keys.I))
            {
                //decreaseSpeedDown = false;
            }
     

            if (keyState.IsKeyDown(Keys.LeftAlt))
            {
                translationX = 0;
                translationY = 0;
            }

            if (keyState.IsKeyDown(Keys.RightAlt))
            {
                for (int i = 0; i < grid.getPerspectiveMap().Length; i++)
                {
                    if (grid.getPerspectiveMap()[i] is ResourceTile)
                    {
                        ((ResourceTile)grid.getPerspectiveMap()[i]).nextCharge = 0.01f;
                    }
                    if (grid.getPerspectiveMap()[i] is BaseTile)
                    {
                        ((ResourceTile)grid.getPerspectiveMap()[i]).nextCharge = 999.0f;
                    }
                }
            }

            if (keyState.IsKeyDown(Keys.G))
            {
                //soldier = new BallUnit(grid.GetBlueBlueBase());
                //blueArmy.Add(new BallUnit(new Vector2(ION.halfWidth - (blueArmy[0].GetScale() / 2), -(blueArmy[0].GetScale() / 4)), blueArmy[0].GetVirtualPos()));
                grid.blueArmy.Add(new BallUnit(grid.GetTileScreenPos(new Vector2(12, 12), translationX, translationY), grid.GetTileScreenPos(new Vector2(11, 13), translationX, translationY)));
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

            if (keyState.IsKeyDown(Keys.H))
            {
                showHelpFile = true;
            }
            else
            {
                showHelpFile = false;
            }

            //Handle mouse input

            if (mouseState.ScrollWheelValue > scrollValue)
            {
                Tile.zoomIn();
                Unit.zoomIn();
                //Debug.WriteLine("zoomIn");

            }
            else if (mouseState.ScrollWheelValue < scrollValue)
            {
                Tile.zoomOut();
                Unit.zoomOut();
               // Debug.WriteLine("zoomOut");
            }

            scrollValue = mouseState.ScrollWheelValue;

            if (mouseState.MiddleButton == ButtonState.Pressed)
            {
                translationX += mouseState.X - previousMouseX;
                translationY += mouseState.Y - previousMouseY;
            }

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                //grid.mouseLeftPressed(mouseState.X, mouseState.Y, translationX, translationY);
                //blueArmy[0].SetTarget(new Vector2(mouseState.X, mouseState.Y));
                
                grid.mouseLeftPressed(mouseState.X, mouseState.Y, translationX, translationY, grid.blueArmy, oldMousePos);// pass the currently selected unit

                if (!leftMouseDown)
                {
                    oldMousePos.X = (mouseState.X);
                    oldMousePos.Y = (mouseState.Y);
                }
                leftMouseDown = true;
                mousePos.X = (mouseState.X);
                mousePos.Y = (mouseState.Y);
            }
            else if (mouseState.LeftButton == ButtonState.Released)
            {
                grid.mouseLeftReleased(mouseState.X, mouseState.Y, translationX, translationY);
                leftMouseDown = false;
            }

            if (mouseState.RightButton == ButtonState.Pressed)
            {
                grid.mouseRightPressed(mouseState.X, mouseState.Y, translationX, translationY, grid.blueArmy);
            }
            else if (mouseState.RightButton == ButtonState.Released)
            {
                grid.mouseRightReleased(mouseState.X, mouseState.Y, translationX, translationY);
            }

            previousMouseX = mouseState.X;
            previousMouseY = mouseState.Y;






            unitCounter++;

            if (unitCounter > 1000)
            {
                unitCounter = 0;
                grid.CreateBlueUnit(translationX,translationY);
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
