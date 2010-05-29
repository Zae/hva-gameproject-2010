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
        private int actOn = 200;

        public int ai = -1;

        public int newId = 0;

        public int serial = 0;

        public Random r = new Random(123);

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

                if (aiUnits.Count < 26)
                {
                    serial++;
                    CommandDispatcher.issueCommand(new NewUnitCommand(CommandDispatcher.getSupposedGameTick(), serial,ai, newId++));
                }

                foreach (Unit u in aiUnits)
                {
                    if (u.destination.Count == 0)
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

                counter = 0;
            }           
        }

    }
}
