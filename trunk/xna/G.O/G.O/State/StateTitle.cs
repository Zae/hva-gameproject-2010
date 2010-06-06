using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using WindowSystem;

namespace ION
{
    public class StateTitle : State
    {
        private List<UIComponent> ComponentList; 
        private TextButton newGameButton;
        private TextButton mpButton;
        private TextButton quitButton;
        private TextButton optionsButton;
        private Rectangle background_overlay;
        private Rectangle Logo;
        private Rectangle background_starfield;

        private bool mousePressed = false;
        public bool upPressed = false;
        public bool downPressed = false;
        public bool enterPressed = false;

        public StateTitle()
        {
            ComponentList = new List<UIComponent>();

            newGameButton = new TextButton(ION.instance, ION.instance.gui);
            newGameButton.Text = "New Game";
            newGameButton.X = 125; newGameButton.Y = 125;
            newGameButton.Click += new ClickHandler(newGameButton_Click);
            mpButton = new TextButton(ION.instance, ION.instance.gui);
            mpButton.Text = "Multiplayer";
            mpButton.X = 125; mpButton.Y = 200;
            mpButton.Click += new ClickHandler(mpButton_Click);
            optionsButton = new TextButton(ION.instance, ION.instance.gui);
            optionsButton.Text = "Options";
            optionsButton.X = 125; optionsButton.Y = 275;
            optionsButton.Click += new ClickHandler(optionsButton_Click);
            quitButton = new TextButton(ION.instance, ION.instance.gui);
            quitButton.Text = "Quit";
            quitButton.X = 125; quitButton.Y = 350;
            quitButton.Click += new ClickHandler(quitButton_Click);
            //
            ComponentList.Add(newGameButton);
            ComponentList.Add(mpButton);
            ComponentList.Add(optionsButton);
            ComponentList.Add(quitButton);
            //
            background_overlay = new Rectangle(ION.width-Images.background_overlay.Width, 0, Images.background_overlay.Width, Images.background_overlay.Height);
            Logo = new Rectangle(ION.width / 100 * 10, ION.height - ION.height / 100 * 7 - Images.Logo.Height, Images.Logo.Width, Images.Logo.Height);
            background_starfield = new Rectangle(0, 0, Images.background_starfield.Width, Images.background_starfield.Height);
        }

        #region Button Handlers
        void quitButton_Click(UIComponent sender)
        {
            ION.get().Exit();
        }

        void optionsButton_Click(UIComponent sender)
        {
            throw new NotImplementedException();
        }

        void mpButton_Click(UIComponent sender)
        {
            ION.get().setState(new StateMP());
        }

        void newGameButton_Click(UIComponent sender)
        {
            ION.get().setState(new StateTest(1, 0, "MediumLevelTest.xml", false));
        }
        #endregion

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
            //Keyboard handling
            KeyboardState keyState = Keyboard.GetState();

            if(keyState.IsKeyDown(Keys.N)){
                ION.get().setState(new StateNetworkTest());
            }
            if (keyState.IsKeyDown(Keys.T))
            {
                ION.get().setState(new StateTicTacToe());
            }
            if (keyState.IsKeyDown(Keys.G))
            {
                ION.get().setState(new GuiTestState());
            }
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
