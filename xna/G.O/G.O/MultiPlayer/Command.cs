using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using ION.Tools;
using System.IO;

namespace ION.MultiPlayer
{
    public class Command : Serializable
    {
        public enum COMMANDTYPES
        {
            NEW_UNIT,
            MOVE_UNIT,
            START_GAME,
            NEW_TOWER
        }

        public int supposedGameTick = -1;
        public int serial = -1;
        public int owner = -1;

        public COMMANDTYPES _commandType;

        /// <summary>
        /// This constructor is here solely for the purpose of Deserialization
        /// </summary>
        /// <example>new Command().Deserialze();</example>
        /// <seealso cref="Serializer.DeserializeCommand"/>
        public Command()
        {
        }
        public Command(COMMANDTYPES commandType, int suppposedGameTick, int owner, int serial)
        {
            this.supposedGameTick = suppposedGameTick;
            this.owner = owner;
            this.serial = serial;
            this._commandType = commandType;
        }
        
        public virtual void performCommand() {
        }

        public virtual String toCommandParts()
        {
            return "";
        }

        public static Command DeserializeCommand(MemoryStream inData)
        {
            BinaryReader br = new BinaryReader(inData);
            COMMANDTYPES ct = (COMMANDTYPES)br.ReadInt32();

            int sgt,serial,owner,unitID,targetx,targety;

            switch (ct)
            {
                case COMMANDTYPES.NEW_UNIT:
                    sgt=br.ReadInt32();
                    serial = br.ReadInt32();
                    owner = br.ReadInt32();
                    unitID = br.ReadInt32();
                    Console.WriteLine("command received of type:" + ct + ", spt=" + sgt + ", owner=" + owner + ", unitID=" + unitID); 
                    return new NewUnitCommand(sgt,serial, owner, unitID);
                case COMMANDTYPES.MOVE_UNIT:
                    sgt = br.ReadInt32();
                    serial = br.ReadInt32();
                    owner = br.ReadInt32();
                    unitID = br.ReadInt32();
                    targetx = br.ReadInt32();
                    targety = br.ReadInt32();
                    Console.WriteLine("command received of type:" + ct + ", spt=" + sgt + ", owner=" + owner + ", unitID=" + unitID + ", targetX="+targetx+", targetY="+targety);
                    return new NewMoveCommand(sgt, serial, owner, unitID, targetx, targety);
                case COMMANDTYPES.START_GAME:
                    return new StartGameCommand(br.ReadInt32(), br.ReadInt32(), br.ReadInt32());
                default:
                    return new Command();
            }
        }


        #region Serializable Members

        public virtual MemoryStream Serialize()
        {
            return new MemoryStream();
        }

        public virtual void Deserialize(MemoryStream inData)
        {
        }

        #endregion
    }
    public class StartGameCommand : Command
    {
        public int seed;

        public StartGameCommand(int seed, int supposedGameTick, int serial)
            : base(COMMANDTYPES.START_GAME, supposedGameTick, 0, serial)
        {
            this.seed = seed;
        }

        public override void performCommand()
        {
            if (ION.instance.serverConnection.isHost)
                ION.instance.setState(new StateTest(1, seed, "MediumLevelTest.xml", true));
            else
                ION.instance.setState(new StateTest(2, seed, "MediumLevelTest.xml", true));
        }

        #region Serializable Members

        public override MemoryStream Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write((Int32)_commandType);
            bw.Write(seed);
            bw.Write(supposedGameTick);
            bw.Write(serial);

            return ms;
        }

        public override void Deserialize(MemoryStream inData)
        {
            throw new NotImplementedException("Please use static Command.Deserialize");
        }

        #endregion
    }
    public class NewMoveCommand : Command
    {
        public int xTarget;
        public int yTarget;

        public int unitId;

        public NewMoveCommand(int supposedGameTick, int serial, int owner, int unitId, int xTarget, int yTarget)
            : base(COMMANDTYPES.MOVE_UNIT, supposedGameTick, owner, serial)
        {
            this.xTarget = xTarget;
            this.yTarget = yTarget;
            this.unitId = unitId;
        }

        public override void performCommand()
        {
            //Debug.WriteLine("commanding unit id=" + unitId + " owner=" + unitOwner);
            Unit u = Grid.get().getUnit(owner, unitId);
            if (u == null)
            {
                Debug.WriteLine("TRIED TO FIND UNIT FOR COMMAND BUT UNIT WAS NOT PRESENT (ANYMORE)");
            }
            else if(Grid.map[xTarget,yTarget] is ResourceTile == false) 
            {
                //@TODO Deny this for now
            }
            else
            {
                u.EmptyWayPoints();

                List<ResourceTile> path = FloodFill.getPath((ResourceTile)Grid.map[u.inTileX, u.inTileY], (ResourceTile)Grid.map[xTarget, yTarget]);

                foreach (ResourceTile rt in path)
                {
                    u.AddDestination(rt);
                }
                //u.SetTarget(Grid.map[xTarget, yTarget].GetPos(StateTest.get().translationX, StateTest.get().translationY));

                if (u.owner == Grid.playerNumber)
                {
                    SoundManager.orderUnitSound();
                }
              
            }
            //u.AddDestination(Grid.map[xTarget,yTarget]);
        }

        public override String toCommandParts()
        {
            return "MOVE_UNIT_TO|0|"+supposedGameTick+"|"+owner+"|"+unitId+"|"+xTarget+"|"+yTarget+"|"+serial+"|";
        }


        #region Serializable Members

        public override MemoryStream Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write((Int32)_commandType);
            bw.Write((Int32)supposedGameTick);
            bw.Write((Int32)serial);
            bw.Write((Int32)owner);
            bw.Write((Int32)unitId);
            bw.Write((Int32)xTarget);
            bw.Write((Int32)yTarget);

            return ms;
        }

        public override void Deserialize(MemoryStream inData)
        {
            throw new NotImplementedException("Please use static Command.Deserialize");
        }

        #endregion
    }

    public class NewUnitCommand : Command, Serializable
    {
        int unitId;
    
        public NewUnitCommand(int supposedGameTick, int serial, int owner, int unitId) 
            : base(COMMANDTYPES.NEW_UNIT, supposedGameTick, owner, serial)
        {
            this.unitId = unitId;
        }

        public override void performCommand()
        {
            Grid.get().createUnit(owner, unitId);
        }

        public override String toCommandParts()
        {
            return "CREATE_UNIT|0|"+supposedGameTick+"|"+owner+"|"+unitId+"|"+serial+"|";
        }


        #region Serializable Members

        public override MemoryStream Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write((Int32)_commandType);
            bw.Write(supposedGameTick);
            bw.Write(serial);
            bw.Write(owner);
            bw.Write(unitId);

            return ms;
        }
        public override void Deserialize(MemoryStream inData)
        {
            throw new NotImplementedException("Please use static Command.Deserialize");
        }

        #endregion
    }

    public class NewTowerUnitCommand : Command
    {
        int unitOwner;

        public NewTowerUnitCommand(int supposedGameTick, int serial, int unitOwner)
            : base(COMMANDTYPES.NEW_TOWER, supposedGameTick, unitOwner, serial)
        {
            this.unitOwner = unitOwner;
        }

        public override void performCommand()
        {
            Grid.get().createTowerUnit(unitOwner);
        }

        //public override String toCommandParts()
        //{
        //    return "CREATE_UNIT|0|" + supposedGameTick + "|" + unitOwner + "|" + unitId + "|" + serial + "|";
        //}

    }

}
