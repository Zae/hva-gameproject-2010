using System;
using FluorineFx;
using FluorineFx.Net;

namespace SharedObjectTest
{
    //
    //Another FMS application deploy sotest.asc to FMS 
    //

    class Program
    {
        NetConnection _netConnection;

        public NetConnection NetConnection
        {
            get { return _netConnection; }
        }
        RemoteSharedObject _sharedObject;

        public RemoteSharedObject SharedObject
        {
            get { return _sharedObject; }
        }

        static void Main(string[] args)
        {
            Program program = new Program();
            program.Connect();

            System.Console.WriteLine("Connecting to server...");
            while (true)
            {
                string input = System.Console.ReadLine();
                if (input == "exit" || input == string.Empty)
                {
                    program.SharedObject.Close();
                    program.NetConnection.Close();
                    break;
                }
                if (program.SharedObject.Connected)
                {
                    program.NetConnection.Call("msgFromClient", null, input);
                }
                else
                    break;
            }
        }

        public void Connect()
        {
            // Create NetConnection client
            _netConnection = new NetConnection();
            _netConnection.OnConnect += new ConnectHandler(_netConnection_OnConnect);
            _netConnection.OnDisconnect += new DisconnectHandler(_netConnection_OnDisconnect);
            _netConnection.NetStatus += new NetStatusHandler(_netConnection_NetStatus);
            _netConnection.Client = this;
            //FMS test
            _netConnection.Connect("rtmp://localhost:1935/sotest");
        }

        void _netConnection_OnConnect(object sender, EventArgs e)
        {
            System.Console.WriteLine("Connected to server. Connecting to RSO...");
            //_sharedObject = RemoteSharedObject.GetRemote("users", _netConnection.Uri.ToString(), false);
            //Our custom UsersRSO will handle "ChatMsg" messages
            _sharedObject = RemoteSharedObject.GetRemote(typeof(UsersRSO), "users", _netConnection.Uri.ToString(), false);

            _sharedObject.OnConnect += new ConnectHandler(_sharedObject_OnConnect);
            _sharedObject.OnDisconnect += new DisconnectHandler(_sharedObject_OnDisconnect);
            _sharedObject.NetStatus += new NetStatusHandler(_sharedObject_NetStatus);
            _sharedObject.Sync += new SyncHandler(_sharedObject_Sync);
            _sharedObject.Connect(_netConnection);
        }

        void _netConnection_OnDisconnect(object sender, EventArgs e)
        {
            System.Console.WriteLine("Connection disconnected.");
        }

        void _netConnection_NetStatus(object sender, NetStatusEventArgs e)
        {
            string level = null;
            if(e.Info.ContainsKey("level"))
                level = e.Info["level"] as string;
            if (level == "error")
            {
                //received an error
                System.Console.WriteLine("Error: " + e.Info["code"] as string);
                System.Console.WriteLine("Client not connected. Press 'Enter' to exit");
            }
            if (level == "status")
            {
                System.Console.WriteLine("Status: " + e.Info["code"] as string);
            }
        }

        void _sharedObject_OnConnect(object sender, EventArgs e)
        {
            System.Console.WriteLine("Connected to RSO.");
        }

        void _sharedObject_OnDisconnect(object sender, EventArgs e)
        {
            System.Console.WriteLine("Disconnected RSO.");
        }

        void _sharedObject_Sync(object sender, SyncEventArgs e)
        {
            ASObject[] changeList = e.ChangeList;
            for (int i = 0; i < changeList.Length; i++)
            {
                ASObject info = changeList[i];
                if (info.ContainsKey("name") && info["name"] != null)
                    System.Console.WriteLine(info["name"].ToString() + " - " + info["code"].ToString());
                else
                    System.Console.WriteLine(info["code"].ToString());
            }
            //Send some message when you are ready
            //_sharedObject.Send("msgFromClient", "test message");
        }

        void _sharedObject_NetStatus(object sender, NetStatusEventArgs e)
        {
            string level = e.Info["level"] as string;
            if (level == "error")
            {
                //received an error
                System.Console.WriteLine("Error: " + e.Info["code"] as string);
            }
            if (level == "status")
            {
                System.Console.WriteLine("Status: " + e.Info["code"] as string);
            }
        }

        public void setHistory(string history)
        {
            System.Console.Write(history);
        }
    }
}
