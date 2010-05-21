using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ION.Tools
{
    public class StupidAI
    {
        private int counter = 0;
        private int actOn = 50;

        public int ai = -1;

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

                if (aiUnits.Count < 5)
                {

                }

                counter = 0;
            }           
        }

    }
}
