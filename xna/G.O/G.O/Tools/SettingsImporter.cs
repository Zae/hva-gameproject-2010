using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Reflection;

namespace ION.Tools
{
    class SettingsImporter
    {

        public void run()
        {
           
//<Settings
//  RobotHealth="32"
//  RobotCost="32"
//  RobotMoveSpeed="0.5" 

//  RobotFireRate="-1"
//  RobotMinDamage="2"
//  RobotMaxDamage="4"
  
//  TurretHealth="32"
//  TurretCost="32"
//  TurretFireRate="-1"
//  TurretMinDamage="6"
//  TurretMaxDamage="9"
  
//  BaseHealth="500"

//  StartingMoney="500"
//  ResourceRate="1.0"
//  SpikeBonus="1.0"
  
//  VictoryCondition="0"
  
//  AmountToCollect="3000"
//>
//</Settings>
//<!-- Important info:
//MoveSpeed is the time in seconds it takes to move from one tile to the next (always use a float (e.g. 0.2 1.3)

//Possible Victory Conditions 
//0 = Anihilation
//1 = Resource Collection
//-->

            //load the xml file into the XmlTextReader object. 
            try
            {
                string execPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
                //Debug.WriteLine("Execution path is: "+execPath);

                //Read the level file using the execution path we got earlier
                XmlTextReader XmlRdr = new System.Xml.XmlTextReader(execPath + "\\Content\\gameSettings\\gamesettings.xml");

                //Read the first Node
                XmlRdr.Read();

                //If it is a Settings Node we can continue
                if (XmlRdr.NodeType == XmlNodeType.Element && XmlRdr.Name == "Settings")
                {

                    int RobotHealth = int.Parse(XmlRdr.GetAttribute("RobotHealth"));
                    int RobotCost = int.Parse(XmlRdr.GetAttribute("RobotCost"));
                    float RobotMoveSpeed = float.Parse(XmlRdr.GetAttribute("RobotMoveSpeed"), System.Globalization.NumberStyles.Any);
                    int RobotFireRange = int.Parse(XmlRdr.GetAttribute("RobotFireRange"));
                    int RobotFireRate = int.Parse(XmlRdr.GetAttribute("RobotFireRate"));
                    int RobotMinDamage = int.Parse(XmlRdr.GetAttribute("RobotMinDamage"));
                    int RobotMaxDamage = int.Parse(XmlRdr.GetAttribute("RobotMaxDamage"));

                    Robot.maxHealth = RobotHealth;
                    Robot.cost = RobotCost;
                    Robot.firingRange = RobotFireRange;
                    Robot.tileToTileTicks = (int)(Grid.TPS * RobotMoveSpeed);
                    Robot.minDamage = RobotMinDamage;
                    Robot.maxDamage = RobotMaxDamage;

                    int TurretHealth = int.Parse(XmlRdr.GetAttribute("TurretHealth"));
                    int TurretCost = int.Parse(XmlRdr.GetAttribute("TurretCost"));
                    int TurretFireRange = int.Parse(XmlRdr.GetAttribute("TurretFireRange"));
                    int TurretFireRate = int.Parse(XmlRdr.GetAttribute("TurretFireRate"));
                    int TurretMinDamage = int.Parse(XmlRdr.GetAttribute("TurretMinDamage"));
                    int TurretMaxDamage = int.Parse(XmlRdr.GetAttribute("TurretMaxDamage"));

                    Tower.maxHealth = TurretHealth;
                    Tower.cost = TurretCost;
                    Tower.firingRange = TurretFireRange;
                    Tower.minDamage = TurretMinDamage;
                    Tower.maxDamage = TurretMaxDamage;

                    //  BaseHealth="500"

                    //  StartingMoney="500.0"
                    //  ResourceRate="1.0"
                    //  SpikeBonus="1.0"

                    //  VictoryCondition="0"

                    //  AmountToCollect="3000.0"

                    int BaseHealth = int.Parse(XmlRdr.GetAttribute("BaseHealth"));
                    float StartingMoney = float.Parse(XmlRdr.GetAttribute("StartingMoney"));
                    float ResourceRate = float.Parse(XmlRdr.GetAttribute("ResourceRate"));
                    float SpikeBonus = float.Parse(XmlRdr.GetAttribute("SpikeBonus"));
                    int VictoryCondition = int.Parse(XmlRdr.GetAttribute("VictoryCondition"));
                    float AmountToCollect = float.Parse(XmlRdr.GetAttribute("AmountToCollect"));

                    int TicksPerSecond = int.Parse(XmlRdr.GetAttribute("TicksPerSecond"));


                    BaseTile.maxHealth = BaseHealth;
                    Grid.resources = StartingMoney;
                    Grid.resourceRate = ResourceRate;
                    Grid.setTPS(TicksPerSecond);

     
                    
                }
                else
                {
                    Debug.WriteLine("FATAL ERROR: Settings file did not contain a valid Settings Node");
                    ION.get().Exit();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception while parsing Settings file: " + e.ToString());
            }
        }
    }
}
