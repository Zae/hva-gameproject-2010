using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using FluorineFx.Net;
using FluorineFx.Messaging.Api.Service;


namespace ION{

    public class ServerConnection
    {
        public static ServerConnection instance;
        public NetConnection LobbyConnection;
        public RemoteSharedObject LobbyRSObject;

        public NetConnection GameConnection;

        public ServerConnection()
            
        {
            instance = this;
            LobbyConnection = new NetConnection();
            LobbyConnection.ObjectEncoding = FluorineFx.ObjectEncoding.AMF0;
            LobbyConnection.NetStatus += new NetStatusHandler(LobbyConnection_NetStatus);
            LobbyConnection.OnConnect += new ConnectHandler(LobbyConnection_OnConnect);
            LobbyConnection.OnDisconnect += new DisconnectHandler(LobbyConnection_OnDisconnect);
            LobbyConnection.Connect("rtmp://red5.dooping.nl:1935/ion", true);
        }

        void LobbyConnection_OnDisconnect(object sender, EventArgs e)
        {
            Console.WriteLine("LobbyConnection lost, reconnecting...");
            //LobbyConnection.Connect("rtmp://127.0.0.1:1935/gameserver", true);
        }

        void LobbyConnection_OnConnect(object sender, EventArgs e)
        {
            Console.WriteLine("Connected to Lobby Server");
            Console.WriteLine("Querying Serverlist...");

            LobbyRSObject = RemoteSharedObject.GetRemote("Chat", LobbyConnection.Uri.ToString(), false);
            LobbyRSObject.NetStatus += new NetStatusHandler(LobbyRSObject_NetStatus);
            LobbyRSObject.OnConnect += new ConnectHandler(LobbyRSObject_OnConnect);
            LobbyRSObject.OnDisconnect += new DisconnectHandler(LobbyRSObject_OnDisconnect);
            LobbyRSObject.Sync += new SyncHandler(LobbyRSObject_Sync);
            LobbyRSObject.Connect(LobbyConnection);

            
        }

        void LobbyRSObject_Sync(object sender, SyncEventArgs e)
        {
            int a = 2;
            String message = (String)this.LobbyRSObject.GetAttribute("SystemMessage");
            //throw new NotImplementedException();
        }


        public class getHostsMsgHandler : IPendingServiceCallback
        {
            public void ResultReceived(IPendingServiceCall call)
            {
               // object result = ;
                String[] result = (String[])call.Result;
                for (int i = 0; i < result.Length; i++)
                {
                    System.Console.WriteLine("result " + i + ": " + result[i].ToString());
                }

                //StateJoin.get().showHosts(result);


                System.Console.WriteLine("Press 'Enter' to exit");
            }

           
        }


        void LobbyRSObject_OnDisconnect(object sender, EventArgs e) 
        {
            Console.WriteLine("LobbyObject lost, reconnecting...");
            //LobbyRSObject.Connect(LobbyConnection);
        }

        void LobbyRSObject_OnConnect(object sender, EventArgs e)
        {
            Console.WriteLine("LobbyObject connected.");
        }

        void LobbyRSObject_NetStatus(object sender, NetStatusEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void LobbyConnection_NetStatus(object sender, NetStatusEventArgs e)
        {
            int a = 2;
            //throw new NotImplementedException();
        }

        public void getHosts()
        {
           // LobbyConnection.Call("getHosts", new getHostsMsgHandler());
            LobbyConnection.Call("getHosts", new getHostsMsgHandler());
        }

        public void JoinRoom(String roomname)
        {
            GameConnection = new NetConnection();
            GameConnection.ObjectEncoding = FluorineFx.ObjectEncoding.AMF0;
            GameConnection.NetStatus += new NetStatusHandler(GameConnection_NetStatus);
            GameConnection.OnConnect += new ConnectHandler(GameConnection_OnConnect);
            GameConnection.OnDisconnect += new DisconnectHandler(GameConnection_OnDisconnect);
            GameConnection.Connect("rtmp://red5.dooping.nl:1935/ion/"+roomname, true);
        }

        void GameConnection_OnDisconnect(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void GameConnection_OnConnect(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void GameConnection_NetStatus(object sender, NetStatusEventArgs e)
        {
            throw new NotImplementedException();
        }

        public static ServerConnection get()
        {
            return instance;
        }

    }
}
