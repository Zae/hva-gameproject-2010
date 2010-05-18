﻿using System;
using System.Timers;
using System.Collections;
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
        private int commandNr;
        private Timer clock;

        private ArrayList sentCommands;
        private ArrayList commands;

        public static Protocol instance;
  
        public int timer = 0;
        private int tik = 0;
        //private String command;
        public static RemoteSharedObject CommandSO;
        private int player = 1;

        public Protocol()
        {
            //initializing Timer
            clock = new Timer();
            //in ms
            clock.Interval = 10;
            clock.Start();
            
            clock.Elapsed+= new ElapsedEventHandler(Timer_Tick);

            commands = new ArrayList();
            sentCommands = new ArrayList();
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

        //update every interval
        void Timer_Tick(Object source, EventArgs e)
        {
            if (timer % 20 == 0)
            {
                Console.WriteLine("-" + timer + " commands waiting for echo:" + sentCommands.ToArray().Length);
                Console.WriteLine("commands waiting for execution:" + commands.ToArray().Length);
            }
            update();
        }

        // new commands get handled here
        // command = the whole string, commandPart is the part between the "|"
        void CommandSO_Sync(object sender, SyncEventArgs e)
        {

            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            Object[] commandObjects = (Object[])CommandSO.GetAttribute("Command");
            
            //gets the object bytes
            Byte[] commandBytes  = Serializer.DeserializeObjectArray(commandObjects);
                       
            String[] commandParts = new String[10];

            if (commandBytes != null)
            {

             
                //covert to string (original state of the command)
                String command = enc.GetString(commandBytes);
                Console.WriteLine("command received = " + command);

                //split the command into parts between the "|"
                commandParts = splitCommand(command);
                
               

                //cheks if the command is not an echo command
                if (!sentCommands.Contains(command))
                {
                    //if this is the first message the timer will sync
                    if (commandParts[0] == "START")
                        timer = Int32.Parse(commandParts[1]);

                    //send an echo back
                    declareAction(command+"E");
                    Console.WriteLine("echo of: " + command + " is sent");
                }

                else
                {
                    //remove it and dont send an echo
                    sentCommands.Remove(command);

                }

                //add command to commandlist, in the update is resposible for excution
                commands.Add(commandParts);
            }
            else Console.WriteLine("empty command received :( " );
        }


           
        void CommandSO_OnDisconnect(object sender, EventArgs e)
        {
        }

        void CommandSO_OnConnect(object sender, EventArgs e)
        {
            Console.WriteLine("command so connected!");
            //time to sync <todo>
           if(ION.instance.serverConnection.isHost==false)
               startGame(2);
        }

        void CommandSO_NetStatus(object sender, NetStatusEventArgs e)
        {


        }


      
        private String[] splitCommand(String command)
        {
            string[] commandParts = new String[10];
            string commandPart = "";
            int partNumber = 0;
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
            return commandParts;
           
        }


        /// protocol interface
        /// 

        public void performAction(String[] actionParts)
        {
            String action = actionParts[0];
            switch (action)
            {
                case "START":
                    Console.WriteLine("start Game message received");
                    //timer = Int32.Parse(actionParts[1]);
                    
                    ION.instance.setState(new StateTest());
                    break;

                case "MOVE_UNIT_TO":
                    Console.WriteLine("move unit message received");
                    //int player = Int32.Parse(actionParts[2]);
                    //ION.instance.setState(new StateTest());
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
            CommandSO.BeginUpdate();
            CommandSO.SetAttribute("Command",commandBytes);
            CommandSO.EndUpdate();
            
            Console.WriteLine("command:" + command + " has been sent!!" + commandBytes.Length );
            
            if(command[command.Length-1]!= 'E') sentCommands.Add(command+"E");

            //commands.Add(splitCommand(command));
        }
        public void startGame(int playerNr)
        {

           
           
            player = playerNr;
            declareAction("START|"+timer+"|"+(timer+50)+"|");

        }
        public void moveUnit (int UnitID, int x, int y)
        {

        }
        public void checkGrid(int checksum)
        {

        }
        public void update()
        {
            timer++;

            
            if(commands.Count>0)
            for(int i = 0; i<commands.Count; i++)
            {

                string[] commandParts = (String[])commands[i];
                if (commandParts != null)
                {
                    
                    
                    if (Int32.Parse(commandParts[2]) == timer)
                    {
                        Console.WriteLine("command: " + commandParts[0] + " will be performed");
                        performAction(commandParts);
                        commands.RemoveAt(i);
                        
                        Console.Write("commandsSize = "  + commands.Count);
                        break;
                        
                        return;
                    }
                    else if (Int32.Parse(commandParts[2]) < timer)
                    {
                        Console.WriteLine("command: " + commandParts[0] + " is out of date:: timer = "+timer+ " execution time =" + Int32.Parse(commandParts[2]) );
                    }
                }
            }
            
        }

        
    }
}