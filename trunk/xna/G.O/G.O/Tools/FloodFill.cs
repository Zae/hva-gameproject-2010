using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ION.Tools
{
    public class FloodFill
    {
        
        //Tells us if we have spotted our target
        private static bool foundTarget = false;
 
        private static ResourceTile target; 

        public static List<ResourceTile> getPath(ResourceTile begin, ResourceTile end)
        {
            foundTarget = false;
            
            target = end; 
            
            //To hold the final path
            List<ResourceTile> path = new List<ResourceTile>();

            //Every step away from the beginning is stored in a new List
            List<List<ResourceTile>> steps = new List<List<ResourceTile>>();           

            List<ResourceTile> firstStep = new List<ResourceTile>();
            firstStep.Add(begin);
            steps.Add(firstStep);

            bool findingTarget = true;
            //As long as the target has not been encountered
            while (findingTarget)
            {
                List<ResourceTile> nextStep = new List<ResourceTile>();
                
                foreach (ResourceTile rt in steps[steps.Count - 1])
                {
                     nextStep.AddRange(getValidNeighbours(rt));
                }

                //Tells us if the target can't be reached
                if (!foundTarget && nextStep.Count == 0)
                {
                    return path;
                }  

                if (foundTarget)
                {
                    findingTarget = false;
                }
                else
                {
                    steps.Add(nextStep);
                }
            }

            path.Add(end);

            //The situation is now that the target tile is not in steps.
            //Only the layer before the target tile is in steps.
            for (int i = steps.Count - 1; i >= 0; i--)
            {
                foreach (ResourceTile rt in steps[i])
                {
                    if(isNeighbour(rt,path[path.Count-1])) 
                    {
                        path.Add(rt);
                        break;
                    }
                }
            }

            //This could be more elegant
            foreach (ResourceTile rt in Grid.resourceTiles)
            {
                rt.floofFillFlag = false;
            }

            path.Reverse();

            return path;
        }

        private static bool isNeighbour(ResourceTile a, ResourceTile b)
        {
            int diffX = a.indexX - b.indexX;
            int diffY = a.indexY - b.indexY;

            if (diffX < 2 && diffX > -2 && diffY < 2 && diffY > -2)
            {
                return true;
            }
            return false;
        }

        private static List<ResourceTile> getValidNeighbours(ResourceTile rt)
        {
            List<ResourceTile> result = new List<ResourceTile>();
            
            int x = rt.indexX;
            int y = rt.indexY;

            ResourceTile temp;

            //while (!foundTarget)
            //{

            //Horizontal/Vertical
                temp = getValidNeighbour(x, y - 1);
                if (temp != null) result.Add(temp);
   
                temp = getValidNeighbour(x - 1, y);
                if (temp != null) result.Add(temp);

                temp = getValidNeighbour(x + 1, y);
                if (temp != null) result.Add(temp);

                temp = getValidNeighbour(x, y + 1);
                if (temp != null) result.Add(temp);


            //Diagonal
                temp = getValidNeighbour(x - 1, y - 1);
                if (temp != null) result.Add(temp);

                temp = getValidNeighbour(x + 1, y - 1);
                if (temp != null) result.Add(temp);

                temp = getValidNeighbour(x - 1, y + 1);
                if (temp != null) result.Add(temp);

                temp = getValidNeighbour(x + 1, y + 1);
                if (temp != null) result.Add(temp);
            //}
            return result;
        }

        private static ResourceTile getValidNeighbour(int x, int y)
        {
            if (Grid.get().isValid(x, y))
            {
                Tile t = Grid.map[x, y];
                if (t is ResourceTile)
                {
                    ResourceTile rt = (ResourceTile)t;

                    if (rt.indexX == target.indexX && rt.indexY == target.indexY)
                    {
                        foundTarget = true;
                        return null;
                    }

                    if (rt.floofFillFlag == false)
                    {
                        rt.floofFillFlag = true;

                        return rt;
                    }
                }
            }
            return null;
        }
    }
}
