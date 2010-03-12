using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace GO.Network
{
    public class ClientComponent
    {
        NetConfiguration config;
        NetClient client;

        public ClientComponent()
        {
            config = new NetConfiguration("GO"); // needs to be same on client and server!
            client = new NetClient(config);
            client.Connect("127.0.0.1", 12345);
         
            

        }

        public void listen()
        {
            NetBuffer buffer = client.CreateBuffer();

            bool keepGoing = true;
            while (keepGoing)
            {
                NetMessageType type;
                while (client.ReadMessage(buffer, out type))
                {
                    switch (type)
                    {
                        case NetMessageType.DebugMessage:
                            Console.WriteLine(buffer.ReadString());
                            break;

                        case NetMessageType.StatusChanged:
                            Console.WriteLine("New status: " + client.Status + " (Reason: " + buffer.ReadString() + ")");
                            break;

                        case NetMessageType.Data:
                            // Handle data in buffer here
                            break;
                    }
                }
            }

        }


        public void send(String message)
        {
            NetBuffer buffer = client.CreateBuffer();

            buffer.Write(message);

            client.SendMessage(buffer, NetChannel.ReliableUnordered);
        }
 


    }
}
