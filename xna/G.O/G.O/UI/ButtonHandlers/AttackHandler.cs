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

            List<Unit> playerUnits = Grid.get().getSelection();

            foreach (Unit u in playerUnits)
            {

                if (u is Robot && Grid.get().resources >= Tower.cost)
                {

                    Grid.get().resources -= Tower.cost;
                    
                    CommandDispatcher.issueCommand(new NewTowerUnitCommand(CommandDispatcher.getSupposedGameTick()
                                                      , CommandDispatcher.getSerial()
                                                      , Grid.playerNumber
                                                      , Grid.getNewId()
                                                      , u.id));
                }
   
            }
           
        }
    }
}
