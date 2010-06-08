using System;
using FluorineFx.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FluorineFx.Messaging.Api.Service;

namespace ION
{
    /// <summary>
    /// With this state we test the network, stuff like ping and bandwidth.
    /// </summary>
    class StateNetworkTest : State
    {
        private RemoteSharedObject rso;

        private static ulong counter = ulong.MinValue;

        long messagetime = 0;
        long totaltime=0;
        long avgtime = 0;
        long maxtime = 0;
        long testsdone = 0;
        long localtime;
        long remotetimelong;

        DateTime beginBWTest;
        float bandwidth;

        private int y = 0;

        enum Tests
        {
            PING,
            BANDWIDTH
        }
        Tests runningTest = Tests.PING;

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
            totaltime += messagetime;
            if (messagetime > maxtime) maxtime = messagetime;
            testsdone++;
            avgtime = totaltime / testsdone;
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
                ION.spriteBatch.DrawString(Fonts.font, "WE ARE HOST, WE ONLY SEND TIME! (Press P to enable)", new Vector2(10, y + 15), Color.White);
                ION.spriteBatch.DrawString(Fonts.font, localtime.ToString(), new Vector2(10, y + 35), Color.White);
            }
            else
            {
                ION.spriteBatch.DrawString(Fonts.font, "Ping: " + messagetime.ToString() + " ms / average: "+avgtime.ToString() + " ms / max: "+maxtime.ToString()+" ms", new Vector2(10, y + 15), Color.White);
                ION.spriteBatch.DrawString(Fonts.font, remotetimelong.ToString(), new Vector2(10, y + 35), Color.White);
            }

            ION.spriteBatch.DrawString(Fonts.font, "BW: " + bandwidth.ToString() + " kB/s (Press B to enable)", new Vector2(10, y + 75), Color.White);
            
            ION.spriteBatch.End();
        }

        public override void update(int ellapsed)
        {
            //Keyboard handling
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Escape))
            {
                ION.get().setState(new StateTitle());
            }
            if (keyState.IsKeyDown(Keys.P))
            {
                runningTest = Tests.PING;
            }
            if (keyState.IsKeyDown(Keys.B))
            {
                runningTest = Tests.BANDWIDTH;
            }

            if (counter >= ulong.MaxValue)
            {
                counter = ulong.MinValue;
            }

            if (ION.instance.serverConnection.isHost)
            { 
                if (counter % 10 == 0 && runningTest == Tests.PING) // slow the updating down a bit so we can actually SEE the ping :P
                {
                    DateTime time = DateTime.Now;
                    localtime = time.ToBinary();
                    rso.SetAttribute("Timer", Serializer.Serialize(localtime));
                }
            }
            if (counter % 100 == 0 && runningTest == Tests.BANDWIDTH) //slow the updating down even more
            {
                beginBWTest = DateTime.Now;

                //create some data to send.
                Byte[] bytearr = new Byte[100000];
                Random rand = new Random();
                rand.NextBytes(bytearr);

                ION.instance.serverConnection.GameConnection.Call("BWCheck", new getHostsMsgHandler(this), bytearr);
            }
            
            counter++;
        }

        public class getHostsMsgHandler : IPendingServiceCallback
        {
            StateNetworkTest _state;
            public getHostsMsgHandler(StateNetworkTest state)
            {
                _state = state;
            }
            public void ResultReceived(IPendingServiceCall call)
            {
                _state.bwcheck(DateTime.Now);
            }
        }

        public void bwcheck(DateTime datetime)
        {
            float milliseconds = (datetime.Ticks - beginBWTest.Ticks) / TimeSpan.TicksPerMillisecond;
            bandwidth = (1 / (milliseconds / 1000)) * 100;
        }

        public override void focusLost()
        {
            //ION.get().IsMouseVisible = true;
        }

        public override void focusGained()
        {
            //ION.get().IsMouseVisible = false;
        }
    }
}
