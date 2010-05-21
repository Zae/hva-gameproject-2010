using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                return true;
            }
            return false;
        }

        public static void issueCommand(Command command)
        {
            //push the command on the network
            Protocol.instance.declareAction(command);
           //string commandParts = command.ToString();
            
            //send them back directly,
            sinkCommand(command);
        }

        //Command coming in over the network should be deserialized and then sent to this method
        public static void sinkCommand(Command command)
        {
            if (command.supposedGameTick < Grid.get().TCP)
            {
                //this is a insolvable situation, go resync 
            }
            else
            {
                int queueLength = commandsQueue.Count;

                if (queueLength == 0 || commandsQueue[queueLength - 1].supposedGameTick <= command.supposedGameTick)
                {
                    commandsQueue.Add(command);
                }

                for (int i = queueLength - 2; i > -1; i--)
                {
                    if (commandsQueue[i].supposedGameTick <= command.supposedGameTick)
                    {
                        commandsQueue.Insert(i + 1, command);
                    }
                }
                
            }

        }
    }
}
