﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ION.MultiPlayer
{
    /**
    * by Michiel on 20100515:1710
    * This class is here to give an overview of those parts in the current StateTest and its various controls
    * that are supposed to become deferred until a later time so all clients/players have time to execute it at the same game-tick
    * 
    */
    class CommandDispatcher
    {

        private static List<Command> commandsQueue = new List<Command>();

        public static int latency = 300; //Latency to attach to commands to be safe all clients can execute the command in time
        private static int serial = 0;

        //Returns a good Tick to let the command process
        public static int getSupposedGameTick()
        {
            //Debug.WriteLine("Supposed tick: tcp=" + Grid.get().TCP + " supposed:" + (int)((float)Grid.get().TCP + ((float)latency / (float)Grid.TPT)));
            return (int)(Grid.get().TCP + ((float)latency / (float)Grid.TPT));  
        }

        public static int getSerial()
        {
            serial++;
            return serial;
        }

        public static bool executeCommand(int gameTick)
        {
       
            if (commandsQueue.Count == 0)
            {
                return false;
            }
            
            if (commandsQueue[0].supposedGameTick == gameTick)
            {
                commandsQueue[0].performCommand();
                commandsQueue.RemoveAt(0);
                Debug.WriteLine("EXECUTING COMMAND ON TICK " + gameTick);
            
                return true;
            }

            if (commandsQueue[0].supposedGameTick < gameTick)
            {
                Debug.WriteLine("ERROR IN COMMANDDISPATCHER: TCP="+gameTick+" SUPPOSED:"+commandsQueue[0].supposedGameTick);
                commandsQueue[0].performCommand();
                commandsQueue.RemoveAt(0);
                return true;
            }

            return false;
        }

        public static void issueCommand(Command command)
        {
            //Debug.WriteLine("ISSUED COMMAND:" + command._commandType);

            //push the command on the network
            if (Protocol.instance != null) Protocol.instance.declareAction(command);
            
            //send them back directly to put in your own queue
            sinkCommand(command);
        }

        //Command coming in over the network should be deserialized and then sent to this method
        public static void sinkCommand(Command command)
        {
            Debug.WriteLine("SINKING COMMAND: " + command.owner + " " + command.serial);
            if (command.supposedGameTick <= Grid.get().TCP)
            {
                //this is a insolvable situation, go resync 
                Debug.WriteLine("ERROR IN COMMANDDISPATCHER.SINKCOMMAND (COMMAND RECEIVED TOO LATE): TCP=" + Grid.get().TCP + " SUPPOSED:" + commandsQueue[0].supposedGameTick);
            }
            else
            {
                int queueLength = commandsQueue.Count;

                //When the list is empty we don't need to think about where to put it
                if (queueLength == 0 )
                {
                    Debug.WriteLine("COMMAND QUEUE EMPTY, ADDING");
                    commandsQueue.Add(command);
                    return;
                }

                //We start at the back of the list
                for (int i = queueLength - 1; i > -1; i--)
                {
         
                    if (commandsQueue[i].supposedGameTick < command.supposedGameTick)
                    {
                        commandsQueue.Insert(i + 1, command);
                        return;
                    }
                    else if (commandsQueue[i].supposedGameTick == command.supposedGameTick)
                    {
                        if (commandsQueue[i].owner == command.owner)
                        {
                            if (commandsQueue[i].serial < command.serial)
                            {
                                commandsQueue.Insert(i + 1, command);
                                return;
                            }
                        }
                        else if (commandsQueue[i].owner < command.owner)
                        {
                            commandsQueue.Insert(i + 1, command);
                            return;
                        }
                    }
                }

                //We need to add the command to the very front of the list appearantly
                commandsQueue.Insert(0, command);
                
                Debug.WriteLine("ADDED COMMAND TO BACK OF LIST");
            }

        }
    }
}
