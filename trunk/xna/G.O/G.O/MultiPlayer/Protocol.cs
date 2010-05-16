using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluorineFx;
using FluorineFx.Net;

namespace ION
{


    //example commands:
    //"SART"|<game timer>|<action time>|
    //"MOVE_UNIT_TO|<game timer>|<action time>|<unit nmbr>|<x in grid>|<y in grid>|"
    //"REMOVE_UNIT|<game timer>|<action time>|<unit nmbr>"
    //"ADD_UNIT|<game timer>|<action time>|<unit nmbr>(as test)|<x in grid>|<y in grid>|"
    //"MOVE_UNIT|<game timer>|<action time>|<unit nmbr>|<x in grid>|<y in grid>|"


    //int x = Int.Parse("23");
    
    
    public class Protocol
    {

        public static Protocol instance;
        private static String   start = "START",
                                addUnit = "ADD_UNIT", 
                                removeUnit = "REMOVE_UNIT", 
                                moveUnitTo = "MOVE_UNIT_TO",
                                syncGrid = "SYNC_GRID";

        public int timer = 0;
        //private String command;
        public static RemoteSharedObject CommandSO;
        private int player = 1;

        public Protocol()
        {
            if (instance == null) 
            {



                instance = this;
                if (ION.get().serverConnection != null && ION.get().serverConnection.GameConnection.Connected)
                {
                    
                    CommandSO = RemoteSharedObject.GetRemote("Command", ION.get().serverConnection.GameConnection.Uri.ToString(), false);
                    CommandSO.NetStatus += new NetStatusHandler(CommandSO_NetStatus);
                    CommandSO.OnConnect += new ConnectHandler(CommandSO_OnConnect);
                    CommandSO.OnDisconnect += new DisconnectHandler(CommandSO_OnDisconnect);
                    CommandSO.Sync += new SyncHandler(CommandSO_Sync);
                    CommandSO.Connect(ION.get().serverConnection.GameConnection);
                    Console.WriteLine("protocol constructor");

                }
                else Console.WriteLine("protocol cant find a gameConnection!!!!!!!!!!");
            }
            else instance=this;
        }



        // new commands get handled here
        // command = the whole string, commandPart is the part between the "|"
        void CommandSO_Sync(object sender, SyncEventArgs e)
        {
            
            

            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            Object[] commandObjects = (Object[])CommandSO.GetAttribute("Command");
            Byte[] commandBytes = commandBytes = Serializer.DeserializeObjectArray(commandObjects);
           

            String[] commandParts = new String[10];
            
            int partNumber=0;
            if (commandBytes != null)
            {

                Console.WriteLine("commandByteLength = " + commandBytes.Length);
                String commandPart = "";

                String command = enc.GetString(commandBytes);

                for (int i = 0; i < command.Length; i++)
                {
                    commandPart += command[i];


                    //Console.WriteLine("comPart=" + commandPart);
                    if (command[i] == '|')
                    {
                        //part without last char "|"
                        commandParts[partNumber] = commandPart.Substring(0, commandPart.Length - 1);
                        //Console.WriteLine("part = "+commandParts[partNumber]);
                        commandPart = "";
                        partNumber++;
                    }
                }

                foreach (String s in commandParts)
                {
                    //if(commandPart == "")
                    Console.WriteLine(s);
                }
                Console.WriteLine("-------------------");
                performeAction(commandParts);
            }
            
        }
        
           
        void CommandSO_OnDisconnect(object sender, EventArgs e)
        {
        }

        void CommandSO_OnConnect(object sender, EventArgs e)
        {

            //time to sync <todo>
           if(ION.instance.serverConnection.isHost==false)
               startGame(2);
        }

        void CommandSO_NetStatus(object sender, NetStatusEventArgs e)
        {


        }


        /// protocol interface
        /// 
        public void performeAction(String[] actionParts)
        {
            String action = actionParts[0];
            switch (action)
            {
                case "START":
                    Console.WriteLine("start Game message received");
                    //int player = Int32.Parse(actionParts[2]);
                    ION.instance.setState(new StateTest());
                    break;

                default :

                    Console.WriteLine("unknwon command recieved!!!");
                    break;
            }

        }
        public void declareAction(String command)
        {
           
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

            byte[] commandBytes = encoding.GetBytes(command);
            
            CommandSO.SetAttribute("Command", commandBytes);
            
            Console.WriteLine("command:" + command + " has been sent!!" + commandBytes.Length + "::" +CommandSO.GetAttribute("Command").ToString());
          

        }
        public void startGame(int playerNr)
        {
           
            //startGame(player);
            player = playerNr;
            declareAction("START|0|0|");

        }
        public void moveUnit (int UnitID)
        {

        }
        public void checkGrid(int checksum)
        {

        }
        public void update()
        {
            
            //166ms
            if (timer % 10 == 0)
            {
                timer++;
            }

            //timer++;
        }

        
    }
}
