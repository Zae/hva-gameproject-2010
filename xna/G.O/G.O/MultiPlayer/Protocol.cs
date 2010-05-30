using System;
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluorineFx;
using FluorineFx.Net;
using ION.MultiPlayer;

namespace ION
{

    //example commands:
    //"START"|<game timer>|<action time>|
    //"MOVE_UNIT_TO|<game timer>|<action time>|<unit owner>|<unit ID>|<x in grid>|<y in grid>|"
    //"REMOVE_UNIT|<game timer>|<action time>|<unit nmbr>"
    //"CREATE_UNIT|<game timer>|<action time>|<unit owner>|<unit ID>(as test)|
    //"MOVE_UNIT|<game timer>|<action time>|<unit nmbr>|<x in grid>|<y in grid>|"


    //int x = Int.Parse("23");
    
    
    public class Protocol
    {
        private bool gameStarted = false;

        //private ArrayList sentCommands;
        //private ArrayList commands;

        public static Protocol instance;

        //private String command;
        public static RemoteSharedObject CommandSO;

        private static Char[] StringSplitToken;

        public Protocol()
        {
            ////initializing Timer
            //clock = new Timer();
            ////in ms
            //clock.Interval = 10;
            //clock.Start();
            
            //clock.Elapsed+= new ElapsedEventHandler(Timer_Tick);

            //commands = new ArrayList();
            //sentCommands = new ArrayList();
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
                }
                else Console.WriteLine("ERROR: Protocol cannot find a Game Connection");

                StringSplitToken = new char[1];
                Char.TryParse("|", out StringSplitToken[0]);
            }
        }

        void CommandSO_Sync(object sender, SyncEventArgs e)
        {
            //System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            //Object[] commandObjects = (Object[])CommandSO.GetAttribute("Command");
            
            //gets the object bytes
            //Byte[] commandBytes  = Serializer.DeserializeObjectArray(commandObjects);
                       
            //String[] commandParts = new String[10];

            //if (commandBytes != null)
            //{    
            //    //covert to string (original state of the command)
            //    String command = enc.GetString(commandBytes);
            //    Console.WriteLine("command received = " + command);

            //    //split the command into parts between the "|"
            //    //commandParts = splitCommand(command);

            //    //parseCommand(command);
            //    //performAction(commandParts);
            //    CommandDispatcher.sinkCommand(command);
            //}
            //else Console.WriteLine("empty command received :( " );
            Object[] objectArray = (Object[])CommandSO.GetAttribute("Command");
            if (objectArray != null)
            {
                Command command = Serializer.DeserializeCommand(objectArray);
                parseCommand(command);
            }
        }

        public void parseCommand(Command command)
        {
            switch (command._commandType)
            {
                //Can't call CommandDispatcher.sinkCommand because StartCommand has no timing.
                case Command.COMMANDTYPES.START_GAME:
                    command.performCommand();
                    break;
                default:
                    CommandDispatcher.sinkCommand(command);
                    break;
            }
        }
        public void parseCommand(String command)
        {
            String[] commandParts = splitCommand(command);

            int x, y;
            int tick;
            int serial;
            int unitOwner;
            int unitID;

            switch (commandParts[0])
            {
                case "START":
                    Console.WriteLine("start Game message received");
                    int seed = Int32.Parse(commandParts[1]);
                    if (ION.instance.serverConnection.isHost)
                        ION.instance.setState(new StateTest(1, seed, "MediumLevelTest.xml",true));
                    else
                        ION.instance.setState(new StateTest(2, seed, "MediumLevelTest.xml",true));

                    gameStarted = true;
                    break;

                case "MOVE_UNIT":
                    tick = Int32.Parse(commandParts[2]);
                    unitOwner = Int32.Parse(commandParts[3]);
                    unitID = Int32.Parse(commandParts[4]);
                    x = Int32.Parse(commandParts[5]);
                    y = Int32.Parse(commandParts[6]);
                    serial = Int32.Parse(commandParts[7]);
                    
                    CommandDispatcher.sinkCommand(new NewMoveCommand(tick,serial,unitOwner,unitID,x,y));
                    break;

                case "CREATE_UNIT":
                    tick = Int32.Parse(commandParts[2]);
                    unitOwner = Int32.Parse(commandParts[3]);
                    unitID = Int32.Parse(commandParts[4]);
                    serial = Int32.Parse(commandParts[5]);
                    
                    CommandDispatcher.sinkCommand(new NewUnitCommand(tick,serial,unitOwner,unitID));
                    break;

                case"ADD_MOVE_UNIT":

                    tick = Int32.Parse(commandParts[2]);
                    unitOwner = Int32.Parse(commandParts[3]);
                    unitID = Int32.Parse(commandParts[4]);
                    x = Int32.Parse(commandParts[5]);
                    y = Int32.Parse(commandParts[6]);
                    serial = Int32.Parse(commandParts[7]);

                    CommandDispatcher.sinkCommand(new AddMoveCommand(tick, serial, unitOwner, unitID, x, y));
                    break;


                default:
                    Console.WriteLine("ERROR: TRIED TO PARSE UNKNOWN COMMAND (Protocol.cs)");
                    break;
            }

        }
           
        void CommandSO_OnDisconnect(object sender, EventArgs e)
        {
        }

        void CommandSO_OnConnect(object sender, EventArgs e)
        {
            Console.WriteLine("Command SO connected");
            //time to sync <todo>
            if (ION.instance.serverConnection.isHost == false)
            {
                //DateTime zero = new DateTime(1985, 8, 6);
                //DateTime now = DateTime.Now;
                //TimeSpan passedTime = now - zero;
                //int secondsSince = (int)passedTime.TotalSeconds;
                //if (secondsSince < 0) secondsSince *= -1;
                //startGame(2, (int)passedTime.TotalSeconds);
                startGame(0);
            }
        }

        void CommandSO_NetStatus(object sender, NetStatusEventArgs e)
        {

        }
     
        private String[] splitCommand(String command)
        {
            //string[] commandParts = new String[10];
            //string commandPart = "";
            //int partNumber = 0;
            //for (int i = 0; i < command.Length; i++)
            //{
            //    commandPart += command[i];


            //    //Console.WriteLine("comPart=" + commandPart);
            //    if (command[i] == '|')
            //    {
            //        //part without last char "|"
            //        commandParts[partNumber] = commandPart.Substring(0, commandPart.Length - 1);
            //        //Console.WriteLine("part = "+commandParts[partNumber]);
            //        commandPart = "";
            //        partNumber++;
            //    }
            //}
            
            //return commandParts;
            return command.Split(StringSplitToken, StringSplitOptions.RemoveEmptyEntries);
        }

        /// protocol interface
        /// 
        public void declareAction(Command command)
        {
            CommandSO.BeginUpdate();
            CommandSO.SetAttribute("Command", Serializer.Serialize(command));
            CommandSO.EndUpdate();
        }
        public void declareAction(String command)
        {
           
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

            byte[] commandBytes = encoding.GetBytes(command);
            CommandSO.BeginUpdate();
            CommandSO.SetAttribute("Command",commandBytes);
            CommandSO.EndUpdate();
            
            Console.WriteLine("command:" + command + " has been sent!!" + commandBytes.Length );

            //parseCommand(command);
            //if(command[command.Length-1]!= 'E') sentCommands.Add(command+"E");
            //commands.Add(splitCommand(command));
        }
        public void startGame(int seed)
        {         
            //declareAction("START|"+seed+"|");
            StartGameCommand command = new StartGameCommand(0, 0, 0);
            declareAction(command);
            parseCommand(command);
            gameStarted = true;
        }        
    }
}
