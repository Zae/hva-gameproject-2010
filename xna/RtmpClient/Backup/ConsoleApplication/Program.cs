using System;
using FluorineFx;
using FluorineFx.Net;
using FluorineFx.Messaging;
using FluorineFx.Messaging.Rtmp;
using FluorineFx.Messaging.Api;
using FluorineFx.Messaging.Api.Service;

namespace ConsoleApplication
{
    class Program
    {
        NetConnection _netConnection;

        static void Main(string[] args)
        {
            //This sample connects to the HelloWorld FMS application
            Program program = new Program();
            program.Connect();

            System.Console.WriteLine("Connecting to server...");
            System.Console.ReadLine();
        }

        public void Connect()
        {
            // Create NetConnection client
            _netConnection = new NetConnection();
            _netConnection.OnConnect += new ConnectHandler(_netConnection_OnConnect);
            _netConnection.NetStatus += new NetStatusHandler(_netConnection_NetStatus);
            _netConnection.Connect("rtmp://localhost:1935/HelloWorld");
        }

        void _netConnection_OnConnect(object sender, EventArgs e)
        {
            System.Console.WriteLine("Connected. Calling server method 'serverHelloMsg'");
            _netConnection.Call("serverHelloMsg", new ServerHelloMsgHandler(), "some text");
        }

        void _netConnection_NetStatus(object sender, NetStatusEventArgs e)
        {
            string level = e.Info["level"] as string;
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

    }

    public class ServerHelloMsgHandler : IPendingServiceCallback
    {
        public void ResultReceived(IPendingServiceCall call)
        {
            object result = call.Result;
            System.Console.WriteLine("Server response: " + result);
            System.Console.WriteLine("Press 'Enter' to exit");
        }
    }
}
