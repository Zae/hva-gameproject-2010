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
        private List<UIComponent> ComponentList;

        private CheckBox check;

        public GuiTestState()
        {
            ComponentList = new List<UIComponent>();
            //
            TextBox textbox = new TextBox(ION.instance, ION.instance.gui);
            textbox.X = 100;
            textbox.Y = 200;
            TextButton textbutton = new TextButton(ION.instance, ION.instance.gui);
            textbutton.X = 200;
            textbutton.Y = 100;
            textbutton.Text = "Knopje";
            check = new CheckBox(ION.instance, ION.instance.gui);
            check.X = 300;
            check.Y = 100;
            ComboBox combo = new ComboBox(ION.instance, ION.instance.gui);
            combo.X = 400;
            combo.Y = 100;
            combo.AddEntry("Testing");
            combo.AddEntry("the");
            combo.AddEntry("combo");
            combo.AddEntry("box");
            RadioButton radio = new RadioButton(ION.instance, ION.instance.gui);
            radio.X = 200;
            radio.Y = 400;
            //
            textbutton.Click += new ClickHandler(textbutton_Click);
            //
            ComponentList.Add(textbox);
            ComponentList.Add(textbutton);
            ComponentList.Add(check);
            ComponentList.Add(combo);
            ComponentList.Add(radio);
        }

        void textbutton_Click(UIComponent sender)
        {
            check.IsChecked = !check.IsChecked;
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
            ION.get().IsMouseVisible = false;
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
