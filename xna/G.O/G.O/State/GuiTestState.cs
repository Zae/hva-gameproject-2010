using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowSystem;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace ION
{
    class GuiTestState  : State
    {
        private TextBox textbox;

        public GuiTestState()
        {
            //ION.instance.gui = new GUIManager(ION.instance);
            textbox = new TextBox(ION.instance, ION.instance.gui);
            textbox.X = 100;
            textbox.Y = 100;

            ION.instance.gui.Add(textbox);
        }
        public override void draw()
        {
            ION.get().GraphicsDevice.Clear(Color.Black);
        }

        public override void update(int ellapsed)
        {
            
        }

        public override void focusGained()
        {
            ION.get().IsMouseVisible = true;
            MediaPlayer.Play(Sounds.titleSong);
            MediaPlayer.IsRepeating = true;
        }

        public override void focusLost()
        {
            ION.get().IsMouseVisible = false;
            MediaPlayer.Stop();
        }
    }
}
