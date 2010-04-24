using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluorineFx.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ION
{
    class StateNetworkTest : State
    {
        private RemoteSharedObject rso;

        private static UInt64 counter=0;

        private int messagetime = 0;

        private int y = 0;

        public StateNetworkTest()
        {
            if (ION.instance.serverConnection != null && ION.instance.serverConnection.GameConnection != null && ION.instance.serverConnection.GameConnection.Connected)
            {
                rso = RemoteSharedObject.GetRemote("timer", ION.instance.serverConnection.GameConnection.Uri.ToString(), false);
                rso.NetStatus += new NetStatusHandler(rso_NetStatus);
                rso.OnConnect += new ConnectHandler(rso_OnConnect);
                rso.OnDisconnect += new DisconnectHandler(rso_OnDisconnect);
                rso.Sync += new SyncHandler(rso_Sync);
                rso.Connect(ION.instance.serverConnection.GameConnection);
            }
        }

        void rso_Sync(object sender, SyncEventArgs e)
        {
            DateTime time = DateTime.Now;
            long remotetimelong = (long)rso.GetAttribute("Timer");
            DateTime remotetime = DateTime.FromBinary(remotetimelong);
            messagetime = time.CompareTo(remotetime);
        }

        void rso_OnDisconnect(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void rso_OnConnect(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void rso_NetStatus(object sender, NetStatusEventArgs e)
        {
            throw new NotImplementedException();
        }
        public override void draw()
        {
            ION.spriteBatch.Begin();
            ION.spriteBatch.DrawString(Fonts.font, "MessageTime:" + messagetime.ToString(), new Vector2(10, y += 15), Color.White);
        }

        public override void update(int ellapsed)
        {
            DateTime time = DateTime.Now;
            rso.SetAttribute("Timer", time.ToBinary());
        }

        public override void focusLost()
        {
            ION.get().IsMouseVisible = true;
        }

        public override void focusGained()
        {
            ION.get().IsMouseVisible = false;
        }
    }
}
