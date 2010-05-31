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
        private TextButton textbutton;

        private List<UIComponent> ComponentList;

        public GuiTestState()
        {
            ComponentList = new List<UIComponent>();
            //
            textbox = new TextBox(ION.instance, ION.instance.gui);
            textbox.X = 100;
            textbox.Y = 100;
            textbutton = new TextButton(ION.instance, ION.instance.gui);
            textbutton.X = 200;
            textbutton.Y = 200;
            textbutton.Text = "Knopje";

            ComponentList.Add(textbox);
            ComponentList.Add(textbutton);
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
            foreach (UIComponent uicomponent in ComponentList)
            {
                ION.instance.gui.Add(uicomponent);
            }
            //
            ION.get().IsMouseVisible = true;
            MediaPlayer.Play(Sounds.titleSong);
            MediaPlayer.IsRepeating = true;
        }

        public override void focusLost()
        {
            foreach (UIComponent uicomponent in ComponentList)
            {
                ION.instance.gui.Remove(uicomponent);
            }
            //
            ION.get().IsMouseVisible = false;
            MediaPlayer.Stop();
        }
    }
}
