using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace G.O
{
    public class StateTest : State
    {

        private static StateTest instance;

        private bool musicPaused = false;

        private int playqueue = 1;

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
