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

namespace ION
{
    public class StateTest : State
    {

        private Grid map;

        private float scrollValue;

        private static StateTest instance;

        private bool musicPaused = false;

        private int playqueue = 1;

        private int level = 0;
        private string[] levels = { "MediumLevelTest.xml", "Level1.xml", "LargeLevelTest.xml"}; //also available ,"BigLevelTest.xml"

        private GridStrategy[] strategies = { new ThunderStrategy(), new CreepStrategy(), new FlowStrategy(), new BleedStrategy() };
        private int strategy = 0;

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

     
        //we need to create a shared object blueUnits


        static int unitCounter = 0;

        public StateTest()
        {
            instance = this;

            scrollValue = Mouse.GetState().ScrollWheelValue;


            map = new Grid(levels[level], strategies[strategy]);


            //blueArmy = new List<Unit>();
            map.blueArmy.Add(new BallUnit());

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

            map.draw(translationX, translationY);

    

            int y = 0;
            ION.spriteBatch.Begin();
            ION.spriteBatch.DrawString(Fonts.font, "Press Escape for Menu, F1 to quit directly", new Vector2(10, y += 15), Color.White);
            ION.spriteBatch.DrawString(Fonts.font, "Space to trigger action sounds (now: " + actionOnScreen + ")(musicvolume:" + MediaPlayer.Volume + ")", new Vector2(10, y += 15), Color.White);
            ION.spriteBatch.DrawString(Fonts.font, "Use the middle mouse button to drag the map around, press Left-Alt to recenter the map", new Vector2(10, y += 15), Color.White);
            ION.spriteBatch.DrawString(Fonts.font, "N - M change Level (now: " + levels[level] + ") J - K change GridStrategy (now: " + strategies[strategy].name + ")", new Vector2(10, y += 15), Color.White);
            ION.spriteBatch.DrawString(Fonts.font, "I - O change game speed (now: "+map.getUpdateStrategy().speed, new Vector2(10, y += 15), Color.White);
            ION.spriteBatch.End();
        }

        public override void update(int ellapsed)
        {

            map.update(ellapsed, map.blueArmy, translationX, translationY);
            
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
                map = new Grid(levels[level],strategies[strategy]);   
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
                map = new Grid(levels[level],strategies[strategy]);  
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
                map = new Grid(levels[level],strategies[strategy]);
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
                map = new Grid(levels[level],strategies[strategy]);
            }
            else if (keyState.IsKeyUp(Keys.J))
            {
                previousStrategyDown = false;
            }

            //SPEED CHANGING
            if (keyState.IsKeyDown(Keys.O) && !increaseSpeedDown)
            {
                //increaseSpeedDown = true;
                map.getUpdateStrategy().increaseSpeed();
            }
            else if (keyState.IsKeyUp(Keys.O))
            {
                //increaseSpeedDown = false;
            }

            if (keyState.IsKeyDown(Keys.I) && !decreaseSpeedDown)
            {
               // decreaseSpeedDown = true;
                map.getUpdateStrategy().decreaseSpeed();
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
                for (int i = 0; i < map.getPerspectiveMap().Length; i++)
                {
                    if (map.getPerspectiveMap()[i] is ResourceTile)
                    {
                        ((ResourceTile)map.getPerspectiveMap()[i]).nextCharge = 0.01f;
                    }
                    if (map.getPerspectiveMap()[i] is BaseTile)
                    {
                        ((ResourceTile)map.getPerspectiveMap()[i]).nextCharge = 999.0f;
                    }
                }
            }

            if (keyState.IsKeyDown(Keys.G))
            {
                //soldier = new BallUnit(map.GetBlueBlueBase());
                //blueArmy.Add(new BallUnit(new Vector2(ION.halfWidth - (blueArmy[0].GetScale() / 2), -(blueArmy[0].GetScale() / 4)), blueArmy[0].GetVirtualPos()));
                map.blueArmy.Add(new BallUnit(map.GetTileScreenPos(new Vector2(12, 12), translationX, translationY), map.GetTileScreenPos(new Vector2(11, 13), translationX, translationY)));
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

            

            //Debug.WriteLine("scrollvalue = " +mouseState.ScrollWheelValue);

            if (mouseState.MiddleButton == ButtonState.Pressed)
            {
                translationX += mouseState.X - previousMouseX;
                translationY += mouseState.Y - previousMouseY;
            }

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                //map.mouseLeftPressed(mouseState.X, mouseState.Y, translationX, translationY);
                //blueArmy[0].SetTarget(new Vector2(mouseState.X, mouseState.Y));
                map.mouseLeftPressed(mouseState.X, mouseState.Y, translationX, translationY, map.blueArmy);// pass the currently selected unit
                
            }
            else if (mouseState.LeftButton == ButtonState.Released)
            {
                map.mouseLeftReleased(mouseState.X, mouseState.Y, translationX, translationY);
            }

            if (mouseState.RightButton == ButtonState.Pressed)
            {
                map.mouseRightPressed(mouseState.X, mouseState.Y, translationX, translationY, map.blueArmy);
            }
            else if (mouseState.RightButton == ButtonState.Released)
            {
                map.mouseRightReleased(mouseState.X, mouseState.Y, translationX, translationY);
            }

            previousMouseX = mouseState.X;
            previousMouseY = mouseState.Y;






            unitCounter++;

            if (unitCounter > 1000)
            {
                unitCounter = 0;
                map.CreateBlueUnit(translationX,translationY);
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
