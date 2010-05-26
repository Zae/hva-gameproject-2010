using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using ION.Tools;

namespace ION.MultiPlayer
{
    public class Command
    {
        public int supposedGameTick = -1;
        public int serial = -1; 

        public Command(int suppposedGameTick, int serial)
        {
            this.supposedGameTick = suppposedGameTick;
            this.serial = serial;
        }
        
        public virtual void performCommand() {
        }

        public virtual String toCommandParts()
        {
            return "";
        }

    }

    public class NewMoveCommand : Command
    {
        public int xTarget;
        public int yTarget;

        public int unitOwner;
        public int unitId;

        public NewMoveCommand(int supposedGameTick, int serial,int unitOwner, int unitId, int xTarget, int yTarget)
            : base(supposedGameTick, serial)
        {
            this.xTarget = xTarget;
            this.yTarget = yTarget;

            this.unitOwner = unitOwner;
            this.unitId = unitId;
        }

        public override void performCommand()
        {
            //Debug.WriteLine("commanding unit id=" + unitId + " owner=" + unitOwner);
            Unit u = Grid.get().getUnit(unitOwner, unitId);
            if (u == null)
            {
                Debug.WriteLine("TRIED TO FIND UNIT FOR COMMAND BUT UNIT WAS NOT PRESENT (ANYMORE)");
            }
            else
            {
                u.EmptyWayPoints();
                u.SetTarget(Grid.map[xTarget, yTarget].GetPos(StateTest.get().translationX, StateTest.get().translationY));

                if (u.owner == Grid.playerNumber)
                {
                    SoundManager.orderUnitSound();
                }
              
            }
            //u.AddDestination(Grid.map[xTarget,yTarget]);
        }

        public override String toCommandParts()
        {
            return "MOVE_UNIT_TO|0|"+supposedGameTick+"|"+unitOwner+"|"+unitId+"|"+xTarget+"|"+yTarget+"|"+serial+"|";
        }

    }

    //public class AddMoveCommand : Command
    //{
    //    public int[,] targetPositions;

    //    int unitOwner;
    //    int unitId;

    //    public AddMoveCommand(int supposedGameTick, int serial,int unitOwner, int unitId, int[,] targetPositions, int positionsCount)
    //        : base(supposedGameTick, serial)
    //    {
    //        this.unitOwner = unitOwner;
    //        this.unitId = unitId;
    //    }

    //    public override void performCommand()
    //    {
    //        Unit u = Grid.get().getUnit(unitOwner, unitId);
    //        //u.AddDestination(Grid.get().getT
    //    }


    //    public override String toCommandParts()
    //    {
    //        return "blabla";
    //    }


    
    //}

    public class NewUnitCommand : Command
    {
        int unitOwner;
        int unitId;
    
        public NewUnitCommand(int supposedGameTick, int serial,int unitOwner, int unitId) 
            : base(supposedGameTick, serial)
        {
            this.unitOwner = unitOwner;
            this.unitId = unitId;
        }

        public override void performCommand()
        {
            Grid.get().createUnit(unitOwner, unitId);
        }

        public override String toCommandParts()
        {
            return "CREATE_UNIT|0|"+supposedGameTick+"|"+unitOwner+"|"+unitId+"|"+serial+"|";
        }

    }

}
