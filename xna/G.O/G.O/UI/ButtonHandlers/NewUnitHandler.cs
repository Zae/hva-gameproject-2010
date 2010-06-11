using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ION.MultiPlayer;

namespace ION.UI
{
    class NewUnitHandler : ButtonHandler
    {

        public override void run()
        {
            //if (keyState.IsKeyDown(Keys.G))
            //{
            //    //soldier = new Robot(grid.GetBlueBlueBase());
            //    //allUnits.Add(new Robot(new Vector2(ION.halfWidth - (allUnits[0].GetScale() / 2), -(allUnits[0].GetScale() / 4)), allUnits[0].GetVirtualPos()));
            //    grid.allUnits.Add(new Robot(grid.GetTileScreenPos(new Vector2(12, 12), translationX, translationY), grid.GetTileScreenPos(new Vector2(11, 13), translationX, translationY)));
            //}

            if(Grid.resources >= Robot.cost) 
            {
                Grid.resources -= Robot.cost;

                //if (Protocol.instance != null)
                //    Protocol.instance.createUnit(Grid.playerNumber, Grid.getNewId());
                   
                //else
                CommandDispatcher.issueCommand(new NewUnitCommand(CommandDispatcher.getSupposedGameTick()
                                                                    ,CommandDispatcher.getSerial()
                                                                    ,Grid.playerNumber
                                                                    ,Grid.getNewId()));

                GUIManager.statusBar.add(new CashFlowDisplay(70, 30, Robot.cost));
            }
        }
    }
}
