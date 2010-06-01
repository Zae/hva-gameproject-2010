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
    class StateLobby : State
    {

        public enum SELECTION
        {
            JOIN = 1,
            BACK
        }
        private Rectangle backButton;
        private Rectangle joinButton;

        public StateLobby()
        {
            joinButton = new Rectangle(125, 125, Images.buttonJoin.Width, Images.buttonJoin.Height);
            backButton = new Rectangle(125, 200, Images.buttonBack.Width, Images.buttonBack.Height);

        }
        public override void draw()
        {
        }

        public override void update(int ellapsed)
        {
        }

        public override void focusLost()
        {
        }

        public override void focusGained()
        {
        }
    }


    

}
