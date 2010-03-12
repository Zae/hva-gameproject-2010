using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using System.Threading;


namespace GO.Network
{
    public class ServerComponent 
    {
        NetConfiguration config;
        NetServer server;

        public ServerComponent()
        {
            config = new NetConfiguration("GO"); // needs to be same on client and server!
            config.MaxConnections = 32;
            config.Port = 12345;
            server = new NetServer(config);
            server.Start();
           
            


        }

        public void listen()
        {
            NetBuffer buffer = server.CreateBuffer();

            bool keepGoing = true;
           // while (keepGoing)
           // {
                NetMessageType type;
                NetConnection sender;
                while (server.ReadMessage(buffer, out type, out sender))
                    
                {
                    switch (type)
                    {
                        case NetMessageType.DebugMessage:
                            Console.WriteLine(buffer.ReadString());
                            break;

                        case NetMessageType.StatusChanged:
                            //Console.WriteLine("New status: " + server.s + " (Reason: " + buffer.ReadString() + ")");
                            break;

                        case NetMessageType.Data:
                            String message = buffer.ReadString();
                            Console.WriteLine("client zegt: " + message);

                            break;
                    }
                }
            //}
        }

        public void send(String message, NetConnection sender)
        {
            NetBuffer buffer = server.CreateBuffer();

            buffer.Write(message);

            server.SendMessage(buffer, sender, NetChannel.ReliableUnordered);
        }
 

    }
}
