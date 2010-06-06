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
using WindowSystem;



namespace ION
{
    class StateMP : State
    {
        private List<UIComponent> ComponentList;

        private TextButton newGameButton;
        private TextButton mpButton;
        private TextButton quitButton;
        private TextButton optionsButton;
        private TextButton hostButton;
        private TextButton joinButton;
        private TextButton backButton;
        
        private Rectangle background_overlay;
        private Rectangle Logo;
        private Rectangle background_starfield;

        public StateMP()
        {
            ComponentList = new List<UIComponent>();

            newGameButton = new TextButton(ION.instance, ION.instance.gui);
            newGameButton.X = 125; newGameButton.Y = 125;
            newGameButton.Text = "New Game";
            newGameButton.Enabled = false;
            mpButton = new TextButton(ION.instance, ION.instance.gui);
            mpButton.X = 125; mpButton.Y = 200;
            mpButton.Text = "Multiplayer";
            optionsButton = new TextButton(ION.instance, ION.instance.gui);
            optionsButton.X = 125; optionsButton.Y = 275;
            optionsButton.Text = "Options";
            quitButton = new TextButton(ION.instance, ION.instance.gui);
            quitButton.X = 125; quitButton.Y = 350;
            quitButton.Text = "Quit";

            hostButton = new TextButton(ION.instance, ION.instance.gui);
            hostButton.Click += new ClickHandler(hostButton_Click);
            hostButton.X = 375; hostButton.Y = 200;
            hostButton.Text = "Host";
            joinButton = new TextButton(ION.instance, ION.instance.gui);
            joinButton.Click += new ClickHandler(joinButton_Click);
            joinButton.X = 375; joinButton.Y = 275;
            joinButton.Text = "Join";
            backButton = new TextButton(ION.instance, ION.instance.gui);
            backButton.Click += new ClickHandler(backButton_Click);
            backButton.X = 375; backButton.Y = 350;
            backButton.Text = "Back";

            ComponentList.Add(newGameButton);
            ComponentList.Add(mpButton);
            ComponentList.Add(optionsButton);
            ComponentList.Add(quitButton);
            ComponentList.Add(hostButton);
            ComponentList.Add(joinButton);
            ComponentList.Add(backButton);

            background_overlay = new Rectangle(ION.width - Images.background_overlay.Width, 0, Images.background_overlay.Width, Images.background_overlay.Height);
            Logo = new Rectangle(ION.width / 100 * 10, ION.height - ION.height / 100 * 7 - Images.Logo.Height, Images.Logo.Width, Images.Logo.Height);
            background_starfield = new Rectangle(0, 0, Images.background_starfield.Width, Images.background_starfield.Height);

            if (ION.instance.serverConnection == null)
            {
                ION.instance.serverConnection = new ServerConnection();
            }
        }

        void backButton_Click(UIComponent sender)
        {
            ION.get().setState(new StateTitle());
        }

        void joinButton_Click(UIComponent sender)
        {
            ION.get().setState(new StateJoin());
        }

        void hostButton_Click(UIComponent sender)
        {
            ION.get().setState(new StateHost());
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
