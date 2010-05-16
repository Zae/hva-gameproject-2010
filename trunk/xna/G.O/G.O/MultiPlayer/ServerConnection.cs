using System;
using FluorineFx.Net;
using FluorineFx.Messaging.Api.Service;

namespace ION{
    /// <summary>
    /// This class is the main connection to the server.
    /// </summary>
    public class ServerConnection
    {
        public static ServerConnection instance;
        public NetConnection LobbyConnection;
        public RemoteSharedObject LobbyRSObject;

        public NetConnection GameConnection;

        public Protocol protocol;
        private Boolean _isHost;

        public Boolean isHost
        {
            get
            {
                return _isHost;
            }
        }

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
            Console.WriteLine("LobbyConnection lost, ");
            //LobbyConnection.Connect("rtmp://127.0.0.1:1935/gameserver", true);
        }

        void LobbyConnection_OnConnect(object sender, EventArgs e)
        {
            Console.WriteLine("Connected to Lobby Server");
            //Console.WriteLine("Querying Serverlist...");

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

        /// <summary>
        /// Object to handle the asynchronous callback from the server.
        /// </summary>
        public class getHostsMsgHandler : IPendingServiceCallback
        {
            public void ResultReceived(IPendingServiceCall call)
            {
                
               Object[] callRes = (Object[])call.Result;
               String[] result = new String[callRes.Length];
               System.Console.WriteLine("number of rooms: " +callRes.Length);
               for (int i = 0; i < callRes.Length; i++)
               {
                    
                    result[i]=callRes[i].ToString();
                    System.Console.WriteLine("result " + i + ": " + result[i].ToString());

               }
               

               StateJoin.get().showHosts(result);

               // System.Console.WriteLine("number of rooms: "+result.Length);
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

        /// <summary>
        /// Get the hosts from the server.
        /// </summary>
        public void getHosts()
        {
           // LobbyConnection.Call("getHosts", new getHostsMsgHandler());
            Console.WriteLine("servercon.gethosts() in servercon");
            LobbyConnection.Call("getHosts", new getHostsMsgHandler());
        }

        /// <summary>
        /// Join a room on the server
        /// </summary>
        /// <param name="roomname">the name of the room</param>
        public void JoinRoom(String roomname)
        {
            this._isHost = false;
            createRoom(roomname);
        }
        /// <summary>
        /// Create a room on the server
        /// </summary>
        /// <param name="roomname">the name of the room</param>
        public void HostRoom(String roomname)
        {
            this._isHost = true;
            createRoom(roomname);
        }
        /// <summary>
        /// Actually create the real room on the server.
        /// </summary>
        /// <param name="roomname">the name of the room</param>
        private void createRoom(String roomname)
        {
            GameConnection = new NetConnection();
            GameConnection.ObjectEncoding = FluorineFx.ObjectEncoding.AMF0;
            GameConnection.NetStatus += new NetStatusHandler(GameConnection_NetStatus);
            GameConnection.OnConnect += new ConnectHandler(GameConnection_OnConnect);
            GameConnection.OnDisconnect += new DisconnectHandler(GameConnection_OnDisconnect);
            GameConnection.Connect("rtmp://red5.dooping.nl:1935/ion/" + roomname, true);
        }
        void GameConnection_OnDisconnect(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            int a = 2;
        }

        void GameConnection_OnConnect(object sender, EventArgs e)
        {
            //throw new NotImplementedException();\
            int a = 2;
            protocol = new Protocol();
        }

        void GameConnection_NetStatus(object sender, NetStatusEventArgs e)
        {
            //throw new NotImplementedException();
            int a = 2;
        }

        public static ServerConnection get()
        {
            return instance;
        }

    }
}
