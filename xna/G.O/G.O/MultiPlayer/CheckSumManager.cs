using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ION.MultiPlayer
{
    public class CheckSumManager
    {
        public const int SUCCES = 1;
        public const int FAIL = 0;
        public const int UNKNOWN = -1;
        
        private List<CheckSumProduct> checksums = new List<CheckSumProduct>();

        public int compareCheckSum(CheckSumProduct toCheck)
        {
            foreach (CheckSumProduct csp in checksums)
            {
                if (csp.gameTick == toCheck.gameTick)
                {
                    if (csp.sum != toCheck.sum)
                    {
                        return FAIL;
                    }
                    else
                    {
                        return SUCCES;
                    }
                }
            }
            return UNKNOWN;
        }
    }

    public class CheckSumProduct
    {
        public int gameTick;
        public float sum;

        public CheckSumProduct(int gameTick, float sum)
        {
            this.gameTick = gameTick;
            this.sum = sum;
        }

        public static CheckSumProduct getCheckSum()
        {
            Grid g = Grid.get();

            float sum = 0;

            foreach(ResourceTile rt in Grid.resourceTiles) 
            {
                sum += rt.charge;
                sum += rt.spikeCount;
            }

            foreach (Unit u in Grid.get().allUnits)
            {
                sum += u.inTileX; sum += u.inTileY;
                sum += u.health; sum += u.id;
                sum += u.owner;
            }

            CheckSumProduct csp = new CheckSumProduct(g.gameTick,sum);
            return csp;
        }
    }
}
