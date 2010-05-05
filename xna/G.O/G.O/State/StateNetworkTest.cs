using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluorineFx.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ION
{
    class StateNetworkTest : State
    {
        private RemoteSharedObject rso;

        private static ulong counter = ulong.MinValue;

        long messagetime = 0;
        long localtime;
        long remotetimelong;

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
            remotetimelong = Serializer.DeserializeLong((Object[])rso.GetAttribute("Timer"));
            DateTime remotetime = DateTime.FromBinary(remotetimelong);
            messagetime = (time.Ticks - remotetime.Ticks) / TimeSpan.TicksPerMillisecond;
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
            ION.get().GraphicsDevice.Clear(Color.Black);
            ION.spriteBatch.Begin();
            if (ION.instance.serverConnection.isHost)
            {
                ION.spriteBatch.DrawString(Fonts.font, "WE ARE HOST, WE ONLY SEND TIME!", new Vector2(10, y + 15), Color.White);
                ION.spriteBatch.DrawString(Fonts.font, localtime.ToString(), new Vector2(10, y + 35), Color.White);
            }
            else
            {
                ION.spriteBatch.DrawString(Fonts.font, "Ping: " + messagetime.ToString() + " ms", new Vector2(10, y + 15), Color.White);
                ION.spriteBatch.DrawString(Fonts.font, remotetimelong.ToString(), new Vector2(10, y + 35), Color.White);
            }
            
            ION.spriteBatch.End();
        }

        public override void update(int ellapsed)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) ION.get().setState(new StateTitle());

            if (ION.instance.serverConnection.isHost)
            {
                if (counter >= ulong.MaxValue)
                {
                    counter = ulong.MinValue;
                }
                if (counter++ % 10 == 0) // slow the updating down a bit so we can actually SEE the ping :P
                {
                    DateTime time = DateTime.Now;
                    localtime = time.ToBinary();
                    rso.SetAttribute("Timer", Serializer.Serialize(localtime));
                }
            }
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
