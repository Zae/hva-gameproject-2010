using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ION.MultiPlayer;

namespace ION.UI
{
    class TowerHandler : ButtonHandler
    {
        public override void run()
        {
            List<Unit> playerUnits = Grid.get().getSelection();

            int cost = 0;

            foreach (Unit u in playerUnits)
            {
                if (u is Robot && Grid.resources >= Tower.cost)
                {
                    Grid.resources -= Tower.cost;

                    CommandDispatcher.issueCommand(new NewTowerUnitCommand(CommandDispatcher.getSupposedGameTick()
                                                      , CommandDispatcher.getSerial()
                                                      , Grid.playerNumber
                                                      , Grid.getNewId()
                                                      , u.id));

                    cost += Tower.cost;
                }
            }

            if(cost > 0)
                GUIManager.statusBar.add(new CashFlowDisplay(70, 30, cost));
        }
    }
}
