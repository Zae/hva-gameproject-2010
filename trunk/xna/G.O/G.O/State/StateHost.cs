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
using WindowSystem;



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

        private List<UIComponent> ComponentList;

        private Color fadeColor = Color.Orange;

        private Rectangle waitScreen;

        private TextButton backButton;
        private TextButton hostButton;
        private TextBox nameCaption;
        private TextBox nameField;

        private Rectangle background_overlay;
        private Rectangle Logo;
        private Rectangle background_starfield;

        private bool waitState = false;

        public StateHost()
        {
            ComponentList = new List<UIComponent>();
            //
            backButton = new TextButton(ION.instance, ION.instance.gui);
            backButton.Click += new ClickHandler(backButton_Click);
            backButton.X = 125; backButton.Y = 125;
            backButton.Text = "Back";
            nameCaption = new TextBox(ION.instance, ION.instance.gui);
            nameCaption.X = 125; nameCaption.Y = 300;
            nameCaption.Text = "Roomname:";
            nameCaption.IsEditable = false;
            nameCaption.Height = 50; nameCaption.Width = 150;
            nameField = new TextBox(ION.instance, ION.instance.gui);
            nameField.X = nameCaption.Width + nameCaption.X; nameField.Y = nameCaption.Y;
            nameField.Height = 50;
            hostButton = new TextButton(ION.instance, ION.instance.gui);
            hostButton.Click += new ClickHandler(hostButton_Click);
            hostButton.X = nameField.X + nameField.Width + 25; hostButton.Y = nameField.Y;
            hostButton.Text = "Host";
            //
            background_overlay = new Rectangle(ION.width - Images.background_overlay.Width, 0, Images.background_overlay.Width, Images.background_overlay.Height);
            Logo = new Rectangle(ION.width / 100 * 10, ION.height - ION.height / 100 * 7 - Images.Logo.Height, Images.Logo.Width, Images.Logo.Height);
            background_starfield = new Rectangle(0, 0, Images.background_starfield.Width, Images.background_starfield.Height);

            waitScreen = new Rectangle(100, 100, ION.width - 200, ION.height - 200);

            ComponentList.Add(backButton);
            ComponentList.Add(nameCaption);
            ComponentList.Add(nameField);
            ComponentList.Add(hostButton);
        }

        void backButton_Click(UIComponent sender)
        {
            ION.get().setState(new StateMP());
        }

        void hostButton_Click(UIComponent sender)
        {
            ION.instance.serverConnection.HostRoom(nameField.Text);
            waitState = true;
            wait();
        }

        public override void draw()
        {
            ION.get().GraphicsDevice.Clear(Color.Black);

            ION.spriteBatch.Begin();

            //logo
            Double w = Math.Ceiling((Double)ION.width / (Double)Images.background_overlay.Width);
            Double h = Math.Ceiling((Double)ION.height / (Double)Images.background_overlay.Height);
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    ION.spriteBatch.Draw(Images.background_starfield, new Rectangle(background_starfield.X + (i * background_starfield.Width), background_starfield.Y + (j * background_starfield.Height), background_starfield.Width, background_starfield.Height), Color.White);
                }
            }
            ION.spriteBatch.Draw(Images.background_overlay, background_overlay, Color.White);
            ION.spriteBatch.Draw(Images.Logo, Logo, Color.White);

            ION.spriteBatch.End();
            if (waitState)
            {
                wait();
                //ION.get().IsMouseVisible = false;
            }            
        }

        public override void update(int ellapsed)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                waitState = false;
            }
        }

        public void wait()
        {
            ION.spriteBatch.Begin();

            ION.spriteBatch.Draw(Images.waitScreen, waitScreen, Color.White);
            ION.spriteBatch.DrawString(Fonts.font, "Waiting for an opponent......", new Vector2(waitScreen.X + 100, waitScreen.Bottom - 100), Color.Black);
            ION.spriteBatch.DrawString(Fonts.font, "press 'escape' to cancel", new Vector2(waitScreen.X + 100, waitScreen.Bottom - 60), Color.Black);
            
            ION.spriteBatch.End();
        }

        public override void focusGained()
        {
            foreach (UIComponent uicomponent in ComponentList)
            {
                ION.instance.gui.Add(uicomponent);
            }
            //
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
            MediaPlayer.Stop();
        }
    }
}
