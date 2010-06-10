using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using ION.Tools;
using System.IO;

namespace ION.MultiPlayer
{
    /// <summary>
    /// Base class for all commands used by the game.
    /// </summary>
    public class Command : Serializable
    {
        /// <summary>
        /// All the command types are listed here.
        /// </summary>
        /// <remarks>Changing lists on others versions could introduce strange errors, try not to change
        /// the order and only add new command on the end of the list to minimize errors.</remarks>
        public enum COMMANDTYPES
        {
            NEW_ROBOT,
            MOVE_UNIT,
            START_GAME,
            NEW_TOWER,
            ADD_MOVE_UNIT,
            ATTACK_UNIT,
            STOP_UNIT,
            ATTACK_BASE
        }

        public int supposedGameTick = -1;
        public int serial = -1;
        public int owner = -1;

        public COMMANDTYPES _commandType;

        /// <summary>
        /// Constructor with 0 arguments to return an empty Command on deserializing and for other uses.
        /// </summary>
        public Command()
        {
        }
        /// <summary>
        /// Constructor for the base Command class, would usually not be called directly but called by
        /// the constructor of the class that inherits the class.
        /// </summary>
        /// <param name="commandType">The type of the class, <see cref="COMMANDTYPES"/>see COMMANDTYPES</param>
        /// <param name="suppposedGameTick">The tick when the command should be executued by the <see cref="CommandDispatcher">CommandDispatcher</see></param>
        /// <param name="owner">The owner of the command, usually the playernumber.</param>
        /// <param name="serial">The command counter for the player, adds up every command.</param>
        public Command(COMMANDTYPES commandType, int suppposedGameTick, int owner, int serial)
        {
            this.supposedGameTick = suppposedGameTick;
            this.owner = owner;
            this.serial = serial;
            this._commandType = commandType;
        }
        /// <summary>
        /// CommandDispatcher calls this function to perform the command. It's defined virtual so the subclasses can
        /// define their own functionality.
        /// </summary>
        public virtual void performCommand() {
        }
        /// <summary>
        /// Static function to Deserialize all the commands.
        /// </summary>
        /// <param name="inData">The memorystream that has the data to deserialize the command.</param>
        /// <returns>The command deserialized from the MemoryStream</returns>
        /// <see cref="Serializer.DeserializeCommand">Serializer.DeserializeCommand</see>
        public static Command DeserializeCommand(MemoryStream inData)
        {
            BinaryReader br = new BinaryReader(inData);
            COMMANDTYPES ct = (COMMANDTYPES)br.ReadInt32();
#if DEBUG
            int sgt,serial,owner,unitID,targetx,targety,seed,targetOwner,targetID,towerID;
#endif

            switch (ct)
            {
                case COMMANDTYPES.NEW_ROBOT:
#if DEBUG
                    sgt=br.ReadInt32();
                    serial = br.ReadInt32();
                    owner = br.ReadInt32();
                    unitID = br.ReadInt32();
                    Console.WriteLine("command received of type:" + ct + ", spt=" + sgt + ", owner=" + owner + ", unitID=" + unitID); 
                    return new NewUnitCommand(sgt,serial, owner, unitID);
#else
                    return new NewUnitCommand(br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadInt32());
#endif
                case COMMANDTYPES.MOVE_UNIT:
#if DEBUG
                    sgt = br.ReadInt32();
                    serial = br.ReadInt32();
                    owner = br.ReadInt32();
                    unitID = br.ReadInt32();
                    targetx = br.ReadInt32();
                    targety = br.ReadInt32();
                    Console.WriteLine("new move umnit-- command received of type:" + ct + ", spt=" + sgt + ", owner=" + owner + ", unitID=" + unitID + ", targetX="+targetx+", targetY="+targety);
                    return new NewMoveCommand(sgt, serial, owner, unitID, targetx, targety);
#else
                    return new NewMoveCommand(br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadInt32());
#endif
                case COMMANDTYPES.START_GAME:
#if DEBUG
                    seed = br.ReadInt32();
                    sgt = br.ReadInt32();
                    serial = br.ReadInt32();
                    Console.WriteLine("command received of type:" + ct + ", seed=" + seed + ", spt=" + sgt + ", serial=" + serial);
                    return new StartGameCommand(sgt, sgt, serial);
#else
                    return new StartGameCommand(br.ReadInt32(), br.ReadInt32(), br.ReadInt32());
#endif
                case COMMANDTYPES.NEW_TOWER:
#if DEBUG
                    sgt = br.ReadInt32();
                    serial = br.ReadInt32();
                    owner = br.ReadInt32();
                    towerID = br.ReadInt32();
                    unitID = br.ReadInt32();
                    Console.WriteLine("command received of type:" + ct + ", spt=" + sgt + ", serial=" + serial + ", owner=" + owner + ", towerID=" + towerID + ", unitID=" + unitID);
                    return new NewTowerUnitCommand(sgt, serial, owner, towerID, unitID);
#else
                    return new NewTowerUnitCommand(br.ReadInt32(), br.ReadInt32(), br.ReadInt32(),br.ReadInt32(), br.ReadInt32());
#endif
                case COMMANDTYPES.ADD_MOVE_UNIT:
#if DEBUG
                    sgt = br.ReadInt32();
                    serial = br.ReadInt32();
                    owner = br.ReadInt32();
                    unitID = br.ReadInt32();
                    targetx = br.ReadInt32();
                    targety = br.ReadInt32();
                    Console.WriteLine("addMoveUnit-- command received of type:" + ct + ", spt=" + sgt + ", owner=" + owner + ", unitID=" + unitID + ", targetX=" + targetx + ", targetY=" + targety);
                    return new AddMoveCommand(sgt, serial, owner, unitID, targetx, targety);
#else
                    return new AddMoveCommand(br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadInt32());
#endif
                case COMMANDTYPES.ATTACK_UNIT:
#if DEBUG
                    sgt = br.ReadInt32();
                    serial = br.ReadInt32();
                    owner = br.ReadInt32();
                    unitID = br.ReadInt32();
                    targetOwner = br.ReadInt32();
                    targetID = br.ReadInt32();
                    Console.WriteLine("attackUnit-- command received of type:" + ct + ", spt=" + sgt + ", owner=" + owner + ", unitID=" + unitID + ", targetOwner=" + targetOwner + ", targetID=" + targetID);
                    return new AttackUnitCommand(sgt, serial, owner, unitID, targetOwner, targetID);
#else
                     return new AttackUnitCommand(br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadInt32());
#endif
                case COMMANDTYPES.ATTACK_BASE:
#if DEBUG
                    sgt = br.ReadInt32();
                    serial = br.ReadInt32();
                    owner = br.ReadInt32();
                    unitID = br.ReadInt32();
                    targetOwner = br.ReadInt32();
                    Console.WriteLine("attackUnit-- command received of type:" + ct + ", spt=" + sgt + ", owner=" + owner + ", unitID=" + unitID + ", targetOwner=" + targetOwner);
                    return new AttackBaseCommand(sgt, serial, owner, unitID, targetOwner);
#else
                     return new AttackBaseCommand(br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadInt32());
#endif
                default:
                    return new Command();
            }
        }


        #region Serializable Members

        /// <summary>
        /// Serialize declared as virtual so the classes that inherit from command can define their own
        /// version of the serializer for Serializable.
        /// </summary>
        /// <returns>MemoryStream with the data of the command.</returns>
        public virtual MemoryStream Serialize()
        {
            return new MemoryStream();
        }
        /// <summary>
        /// Deserialize declared as virtual so the classes that inherit from command can define their own
        /// version of the deserializer for Serializable.
        /// </summary>
        /// <param name="inData">MemoryStream with teh data of the command.</param>
        public virtual void Deserialize(MemoryStream inData)
        {
        }

        #endregion
    }
    /// <summary>
    /// StartGameCommand version of Command.
    /// 
    /// This command is sent when the game should start online.
    /// </summary>
    public class StartGameCommand : Command
    {
        /// <summary>
        /// Seed is used to seed the random number generators in the online games, so we can use
        /// random numbers and still be synchronized.
        /// </summary>
        /// <remarks>This is only usable with a Pseudo-Random number generator.</remarks>
        public int seed;
        /// <summary>
        /// Constructor for the StartGameCommand command
        /// </summary>
        /// <param name="seed">Seed is used to seed the random number generators in the online games, so we can use
        /// random numbers and still be synchronized.</param>
        /// <param name="supposedGameTick">The tick on which the game should start so all games start at the same time.</param>
        /// <param name="serial">The command serial.</param>
        public StartGameCommand(int seed, int supposedGameTick, int serial)
            : base(COMMANDTYPES.START_GAME, supposedGameTick, 0, serial)
        {
            this.seed = seed;
        }
        /// <summary>
        /// CommandDispatcher calls this function to perform the command. It overrides the performCommand of the base
        /// class so this class can define it's own functionality.
        /// </summary>
        public override void performCommand()
        {
            if (ION.instance.serverConnection.isHost)
                ION.instance.setState(new StateTest(1, seed, "MediumLevelTest.xml", true));
            else
                ION.instance.setState(new StateTest(2, seed, "MediumLevelTest.xml", true));
        }

        #region Serializable Members
        /// <summary>
        /// Serialize serializes the important data of the command so it can be deserialized.
        /// </summary>
        /// <returns>MemoryStream with the data of the command.</returns>
        public override MemoryStream Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write((Int32)_commandType);
            bw.Write((Int32)seed);
            bw.Write((Int32)supposedGameTick);
            bw.Write((Int32)serial);

            return ms;
        }

        #endregion
    }
    /// <summary>
    /// NewMoveCommand version of Command.
    /// 
    /// This command is sent when a unit is moved.
    /// </summary>
    public class NewMoveCommand : Command
    {
        public int xTarget;
        public int yTarget;

        public int unitId;
        /// <summary>
        /// Constructor for the NewMoveCommand command.
        /// </summary>
        /// <param name="supposedGameTick">The tick on which the game should start so al games start at the same time.</param>
        /// <param name="serial">The command serial.</param>
        /// <param name="owner">The owner of the unit. (PlayerNumber)</param>
        /// <param name="unitId">The id of the unit (index of the list)</param>
        /// <param name="xTarget">The x index of the grid.</param>
        /// <param name="yTarget">The y index of the grid.</param>
        public NewMoveCommand(int supposedGameTick, int serial, int owner, int unitId, int xTarget, int yTarget)
            : base(COMMANDTYPES.MOVE_UNIT, supposedGameTick, owner, serial)
        {
            this.xTarget = xTarget;
            this.yTarget = yTarget;
            this.unitId = unitId;
        }
        /// <summary>
        /// CommandDispatcher calls this function to perform the command. It overrides the performCommand of the base
        /// class so this class can define it's own functionality.
        /// </summary>
        public override void performCommand()
        {
            //Debug.WriteLine("commanding unit id=" + unitId + " owner=" + unitOwner);
            Unit u = Grid.get().getUnit(owner, unitId);
            if (u == null)
            {
                //This is no problem, we just catch it here
                //Debug.WriteLine("TRIED TO FIND UNIT FOR COMMAND BUT UNIT WAS NOT PRESENT (ANYMORE)");
            }
            else if(Grid.map[xTarget,yTarget] is ResourceTile == false) 
            {
                //@TODO Deny this for now
            }
            else
            {
                u.EmptyWayPoints();

                int fromX; int fromY;
                //find out the starting point

                if (u.targetPosition == null)
                {
                    fromX = u.position.indexX; fromY = u.position.indexY;
                }
                else if (!u.moving && u.targetPosition != null)
                {
                    fromX = u.position.indexX; fromY = u.position.indexY;
                }
                else
                {
                    fromX = u.targetPosition.indexX; fromY = u.targetPosition.indexY;
                }



                List<ResourceTile> path = FloodFill.getPath((ResourceTile)Grid.map[fromX,fromY], (ResourceTile)Grid.map[xTarget, yTarget]);

                foreach (ResourceTile rt in path)
                {
                    u.AddDestination(rt);
                }
                //u.SetTarget(Grid.map[xTarget, yTarget].GetPos(StateTest.get().translationX, StateTest.get().translationY));

                if (u.owner == Grid.playerNumber)
                {
                    //SoundManager.orderUnitSound();
                    if (path.Count != 0)
                    {
                        SoundManager.setCoordinate(CoordinateTool.coordinateSound(path.Last<ResourceTile>().indexX, path.Last<ResourceTile>().indexY));

                    }
                }
              
            }
            //u.AddDestination(Grid.map[xTarget,yTarget]);
        }

        #region Serializable Members
        /// <summary>
        /// Serialize serializes the important data of the command so it can be deserialized.
        /// </summary>
        /// <returns>MemoryStream with the data of the command.</returns>
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

        #endregion
    }

    /// <summary>
    /// NewMoveCommand version of Command.
    /// 
    /// This command is sent when a unit is moved.
    /// </summary>
    public class AddMoveCommand : Command
    {
        public int xTarget;
        public int yTarget;

        public int unitId;
        /// <summary>
        /// Constructor for the NewMoveCommand command.
        /// </summary>
        /// <param name="supposedGameTick">The tick on which the game should start so al games start at the same time.</param>
        /// <param name="serial">The command serial.</param>
        /// <param name="owner">The owner of the unit. (PlayerNumber)</param>
        /// <param name="unitId">The id of the unit (index of the list)</param>
        /// <param name="xTarget">The x index of the grid.</param>
        /// <param name="yTarget">The y index of the grid.</param>
        public AddMoveCommand(int supposedGameTick, int serial, int owner, int unitId, int xTarget, int yTarget)
            : base(COMMANDTYPES.ADD_MOVE_UNIT, supposedGameTick, owner, serial)
        {
            this.xTarget = xTarget;
            this.yTarget = yTarget;
            this.unitId = unitId;
        }
        /// <summary>
        /// CommandDispatcher calls this function to perform the command. It overrides the performCommand of the base
        /// class so this class can define it's own functionality.
        /// </summary>
        public override void performCommand()
        {
            //Debug.WriteLine("commanding unit id=" + unitId + " owner=" + unitOwner);
            Unit u = Grid.get().getUnit(owner, unitId);
            if (u == null)
            {
                Debug.WriteLine("TRIED TO FIND UNIT FOR COMMAND BUT UNIT WAS NOT PRESENT (ANYMORE)");
            }
            else if (Grid.map[xTarget, yTarget] is ResourceTile == false)
            {
                //@TODO Deny this for now
            }
            else
            {
                //get the last waypoint of this unit
                ResourceTile last;
                if (u.destination.Count > 0)
                {
                    last = (ResourceTile)u.destination.Last<Tile>();
                }
                else
                {
                    last = (ResourceTile)Grid.map[u.inTileX, u.inTileY];
                }


                //u.AddDestination(Grid.map[xTarget,yTarget]);

                List<ResourceTile> path = FloodFill.getPath(last, (ResourceTile)Grid.map[xTarget, yTarget]);

                foreach (ResourceTile rt in path)
                {
                 u.AddDestination(rt);
                }

                //If this unit belonges to the player, make a sound
                if (u.owner == Grid.playerNumber)
                {
                    //SoundManager.orderUnitSound();
                }

            }
            
        }

        #region Serializable Members
        /// <summary>
        /// Serialize serializes the important data of the command so it can be deserialized.
        /// </summary>
        /// <returns>MemoryStream with the data of the command.</returns>
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

        #endregion
    }

    /// <summary>
    /// NewUnitCommand version of Command
    /// 
    /// This command is sent when a new unit should be created.
    /// </summary>
    public class NewUnitCommand : Command, Serializable
    {
        int unitId;
        /// <summary>
        /// Constructor for the NewUnitCommand command.
        /// </summary>
        /// <param name="supposedGameTick">The tick on which the game should stat so all games start at the same time.</param>
        /// <param name="serial">The command serial.</param>
        /// <param name="owner">The owner of the unit (playerNumber)</param>
        /// <param name="unitId">The id of the unit (index of the list)</param>
        public NewUnitCommand(int supposedGameTick, int serial, int owner, int unitId) 
            : base(COMMANDTYPES.NEW_ROBOT, supposedGameTick, owner, serial)
        {
            this.unitId = unitId;
        }
        /// <summary>
        /// CommandDispatcher calls this function to perform the command. It overrides the performCommand of the base
        /// class so this class can define it's own functionality.
        /// </summary>
        public override void performCommand()
        {
            Grid.get().createUnit(owner, unitId);
        }

        #region Serializable Members
        /// <summary>
        /// Serialize serializes the important data of the command so it can be deserialized.
        /// </summary>
        /// <returns>MemoryStream with the data of the command.</returns>
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

        #endregion
    }
    /// <summary>
    /// NewTowerUnitCommand version of Command.
    /// 
    /// This command is sent when the game creates a tower out of a unit.
    /// </summary>
    public class NewTowerUnitCommand : Command
    {
        int towerId;
        int robotId;
        /// <summary>
        /// Constructor for the NewTowerUnitCommand command.
        /// </summary>
        /// <param name="supposedGameTick">The tick in which the game shoudl start so all games start ath the same time.</param>
        /// <param name="serial">The command serial.</param>
        /// <param name="unitOwner">The owner of the unit (playerNumber)</param>
        public NewTowerUnitCommand(int supposedGameTick, int serial, int owner, int towerId, int robotId)
            : base(COMMANDTYPES.NEW_TOWER, supposedGameTick, owner, serial)
        {
            this.towerId = towerId;
            this.robotId = robotId;
        }
        //emmet Shouldn't this also include the ID of the unit that is converted into a tower?
        public override void performCommand()
        {
            Debug.WriteLine("net tower command, pars: woner="+owner+" towerId="+towerId + " robotID="+robotId);
            Grid.get().createTowerUnit(owner,towerId,robotId);
        }
        #region Serializable Members
        /// <summary>
        /// Serialize serializes the important data of the command so it can be deserialized.
        /// </summary>
        /// <returns>MemoryStream with the data of the command.</returns>
        public override MemoryStream Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write((Int32)_commandType);
            bw.Write((Int32)supposedGameTick);
            bw.Write((Int32)serial);
            bw.Write((Int32)owner);
            bw.Write((Int32)towerId);
            bw.Write((Int32)robotId);

            return ms;
        }
        #endregion
    }

    /// <summary>
    /// AttackUnitCommand version of Command.
    /// 
    /// This command is sent when a unit is moved.
    /// </summary>
    public class AttackUnitCommand : Command
    {
        public int targetOwner;
        public int targetID;

        public int unitId;
        /// <summary>
        /// Constructor for the NewMoveCommand command.
        /// </summary>
        /// <param name="supposedGameTick">The tick on which the game should start so al games start at the same time.</param>
        /// <param name="serial">The command serial.</param>
        /// <param name="owner">The owner of the unit. (PlayerNumber)</param>
        /// <param name="unitId">The id of the unit (index of the list)</param>
        /// <param name="xTarget">The x index of the grid.</param>
        /// <param name="yTarget">The y index of the grid.</param>
        public AttackUnitCommand(int supposedGameTick, int serial, int owner, int unitId, int targetOwner, int targetID)
            : base(COMMANDTYPES.ATTACK_UNIT, supposedGameTick, owner, serial)
        {
            this.targetOwner = targetOwner;
            this.targetID = targetID;

            this.unitId = unitId;
        }
        /// <summary>
        /// CommandDispatcher calls this function to perform the command. It overrides the performCommand of the base
        /// class so this class can define it's own functionality.
        /// </summary>
        public override void performCommand()
        {
<<<<<<< .mine

            Unit attacker = Grid.get().getUnit(owner, unitId);
            Unit target = Grid.get().getUnit(targetOwner, targetID);
            attacker.attackTarget = target;




            ////Debug.WriteLine("commanding unit id=" + unitId + " owner=" + unitOwner);
            //Unit u = Grid.get().getUnit(owner, unitId);
            //if (u == null)
            //{
            //    Debug.WriteLine("TRIED TO FIND UNIT FOR COMMAND BUT UNIT WAS NOT PRESENT (ANYMORE)");
            //}
            //else if (Grid.map[xTarget, yTarget] is ResourceTile == false)
            //{
            //    //@TODO Deny this for now
            //}
            //else
            //{
            //    //get the last waypoint of this unit
            //    ResourceTile last;
            //    if (u.destination.Count > 0)
            //    {
            //        last = (ResourceTile)u.destination.Last<Tile>();
            //    }
            //    else
            //    {
            //        last = (ResourceTile)Grid.map[u.inTileX, u.inTileY];
            //    }
=======
            Unit attacker = Grid.get().getUnit(owner, unitId);
            Unit target = Grid.get().getUnit(targetOwner, targetID);
>>>>>>> .r297

            if (attacker != null && target != null)
            {
                attacker.setAttackTarget(target);
            }
            
        }

        #region Serializable Members
        /// <summary>
        /// Serialize serializes the important data of the command so it can be deserialized.
        /// </summary>
        /// <returns>MemoryStream with the data of the command.</returns>
        public override MemoryStream Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write((Int32)_commandType);
            bw.Write((Int32)supposedGameTick);
            bw.Write((Int32)serial);
            bw.Write((Int32)owner);
            bw.Write((Int32)unitId);
            bw.Write((Int32)targetOwner);
            bw.Write((Int32)targetID);

            return ms;
        }

        #endregion
    }

    /// <summary>
    /// AttackUnitCommand version of Command.
    /// 
    /// This command is sent when a unit is moved.
    /// </summary>
    public class StopUnitCommand : Command
    {

        public int unitId;
        /// <summary>
        /// Constructor for the NewMoveCommand command.
        /// </summary>
        /// <param name="supposedGameTick">The tick on which the game should start so al games start at the same time.</param>
        /// <param name="serial">The command serial.</param>
        /// <param name="owner">The owner of the unit. (PlayerNumber)</param>
        /// <param name="unitId">The id of the unit (index of the list)</param>
        /// <param name="xTarget">The x index of the grid.</param>
        /// <param name="yTarget">The y index of the grid.</param>
        public StopUnitCommand(int supposedGameTick, int serial, int owner, int unitId)
            : base(COMMANDTYPES.STOP_UNIT, supposedGameTick, owner, serial)
        {

            this.unitId = unitId;
        }
        /// <summary>
        /// CommandDispatcher calls this function to perform the command. It overrides the performCommand of the base
        /// class so this class can define it's own functionality.
        /// </summary>
        public override void performCommand()
        {
            Unit u = Grid.get().getUnit(owner, unitId);
            if (u != null)
            {
                u.stop();
            }           
        }

        #region Serializable Members
        /// <summary>
        /// Serialize serializes the important data of the command so it can be deserialized.
        /// </summary>
        /// <returns>MemoryStream with the data of the command.</returns>
        public override MemoryStream Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write((Int32)_commandType);
            bw.Write((Int32)supposedGameTick);
            bw.Write((Int32)serial);
            bw.Write((Int32)owner);
            bw.Write((Int32)unitId);

            return ms;
        }

        #endregion
    }

    /// <summary>
    /// AttackBaseCommand version of Command.
    /// 
    /// This command is sent when a unit is moved.
    /// </summary>
    public class AttackBaseCommand : Command
    {
        public int targetOwner;

        public int unitId;

        /// <summary>
        /// Constructor for the AttackBaseCommand command.
        /// </summary>
        /// <param name="supposedGameTick">The tick on which the game should start so al games start at the same time.</param>
        /// <param name="serial">The command serial.</param>
        /// <param name="owner">The owner of the unit. (PlayerNumber)</param>
        /// <param name="unitId">The id of the unit (index of the list)</param>
        /// <param name="xTarget">The x index of the grid.</param>
        /// <param name="yTarget">The y index of the grid.</param>
        public AttackBaseCommand(int supposedGameTick, int serial, int owner, int unitId, int targetOwner)
            : base(COMMANDTYPES.ATTACK_BASE, supposedGameTick, owner, serial)
        {
            this.targetOwner = targetOwner;
            

            this.unitId = unitId;
        }
        /// <summary>
        /// CommandDispatcher calls this function to perform the command. It overrides the performCommand of the base
        /// class so this class can define it's own functionality.
        /// </summary>
        public override void performCommand()
        {
            Unit u = Grid.get().getUnit(owner, unitId);

            if (u != null)
            {
                u.setAttackTarget(Grid.getPlayerBase(targetOwner));
            }
        }

        #region Serializable Members
        /// <summary>
        /// Serialize serializes the important data of the command so it can be deserialized.
        /// </summary>
        /// <returns>MemoryStream with the data of the command.</returns>
        public override MemoryStream Serialize()
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write((Int32)_commandType);
            bw.Write((Int32)supposedGameTick);
            bw.Write((Int32)serial);
            bw.Write((Int32)owner);
            bw.Write((Int32)unitId);
            bw.Write((Int32)targetOwner);

            return ms;
        }

        #endregion
    }
}