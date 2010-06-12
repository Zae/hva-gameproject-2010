using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ION.MultiPlayer;

namespace ION.Tools
{
    public class StupidAI
    {
        private int counter = 0;
        private int actOn = 5;

        public int ai = -1;

        public int newId = 0;

        public int serial = 0;

        public int maxUnits = 32;

        public Random r = new Random(123);

        public bool managePlayer = false;

        public StupidAI(int ai)
        {
            this.ai = ai;
        }

        public void act()
        {
            counter++;
            if (counter == actOn)
            {
                //do things
                List<Unit> aiUnits = Grid.get().getPlayerUnits(ai);

                int towers = 0;
                foreach (Unit u in aiUnits)
                {
                    if (u is Tower)
                    {
                        towers++;
                    }
                }

                if (aiUnits.Count < 2 + towers && maxUnits > 0)
                {
                    serial++;
                    maxUnits--;
                    CommandDispatcher.issueCommand(new NewUnitCommand(CommandDispatcher.getSupposedGameTick(), serial,ai, newId++));
                }

                foreach (Unit u in aiUnits)
                {

                    
                    if (u is Robot && u.destination.Count == 0 && !u.moving)
                    {
                        int x = (int)(Grid.width * r.NextDouble());
                        int y = (int)(Grid.height * r.NextDouble());

                        serial++;
                        CommandDispatcher.issueCommand(new NewMoveCommand(CommandDispatcher.getSupposedGameTick()
                                                                            , serial
                                                                            , u.owner
                                                                            , u.id
                                                                            , x
                                                                            , y));
                        
                    }
                }

                if (managePlayer)
                {
                    //do things
                    List<Unit> playerUnits = Grid.get().getPlayerUnits(Grid.playerNumber);

                    if (playerUnits.Count < 3 + towers)
                    {
 
                        CommandDispatcher.issueCommand(new NewUnitCommand(CommandDispatcher.getSupposedGameTick()
                                                                    , CommandDispatcher.getSerial()
                                                                    , Grid.playerNumber
                                                                    , Grid.getNewId()));
                    }

                    foreach (Unit u in playerUnits)
                    {
                        if (u is Robot && u.destination.Count == 0 && !u.moving)
                        {
                            int x = (int)(Grid.width * r.NextDouble());
                            int y = (int)(Grid.height * r.NextDouble());

                            CommandDispatcher.issueCommand(new NewMoveCommand(CommandDispatcher.getSupposedGameTick()
                                                                                , CommandDispatcher.getSerial()
                                                                                , u.owner
                                                                                , u.id
                                                                                , x
                                                                                , y));

                        }
                    }
                }

                counter = 0;
            }           
        }

    }
}
