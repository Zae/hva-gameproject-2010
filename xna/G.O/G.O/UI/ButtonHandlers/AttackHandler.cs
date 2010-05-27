using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ION.MultiPlayer;

namespace ION.UI
{
    class AttackHandler : ButtonHandler
    {

        public override void run()
        {
                CommandDispatcher.issueCommand(new NewTowerUnitCommand(CommandDispatcher.getSupposedGameTick()
                                                                    , CommandDispatcher.getSerial()
                                                                    , Grid.playerNumber));
        }
    }
}
