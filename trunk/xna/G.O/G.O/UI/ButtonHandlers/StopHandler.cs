using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ION.MultiPlayer;

namespace ION.UI
{
    class StopHandler : ButtonHandler
    {

        public override void run()
        {
            //get the selected units
            //make them stop
            foreach (Unit u in Grid.get().getSelection())
            {
                CommandDispatcher.issueCommand(new StopUnitCommand(CommandDispatcher.getSupposedGameTick()
                                                                   , CommandDispatcher.getSerial()
                                                                   , u.owner,u.id));
            }
        }

    }
}
