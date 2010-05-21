using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ION.MultiPlayer
{
    public class Command
    {
        public int supposedGameTick = -1;

        public Command(int supposedGameTick)
        {
            this.supposedGameTick = supposedGameTick;
        }

        public virtual void performCommand() {
        }

        public virtual string toCommandParts()
        {

        }

    }

    public class NewMoveCommand : Command
    {
        public int xTarget;
        public int yTarget;

        public int unitOwner;
        public int unitId;

        public NewMoveCommand(int supposedGameTick, int unitOwner, int unitId, int xTarget, int yTarget)
            : base(supposedGameTick)
        {
            this.xTarget = xTarget;
            this.yTarget = yTarget;

            this.unitOwner = unitOwner;
            this.unitId = unitId;
        }

        public override void performCommand()
        {
            Unit u = Grid.get().getUnit(unitOwner, unitId);
            u.EmptyWayPoints();
            u.SetTarget(Grid.map[xTarget,yTarget].GetPos(StateTest.translationX,StateTest.translationY));
        }

        public override string toCommandParts()
        {

        }

    }

    public class AddMoveCommand : Command
    {
        public int[,] targetPositions;

        int unitOwner;
        int unitId;

        public AddMoveCommand(int supposedGameTick, int unitOwner, int unitId, int[,] targetPositions, int positionsCount)
            : base(supposedGameTick)
        {
            this.unitOwner = unitOwner;
            this.unitId = unitId;
        }

        public override void performCommand()
        {
            Unit u = Grid.get().getUnit(unitOwner, unitId);
            //u.AddDestination(Grid.get().getT
        }

        public override string toCommandParts()
        {

        }
    }

    public class NewUnitCommand : Command
    {
        int unitOwner;
        int unitId;
    
        public NewUnitCommand(int supposedGameTick, int unitOwner, int unitId) 
            : base(supposedGameTick)
        {
            this.unitOwner = unitOwner;
            this.unitId = unitId;
        }

        public override void performCommand()
        {
            Grid.get().createUnit(unitOwner, unitId);
        }

        public override string toCommandParts()
        {

        }
    }

}
