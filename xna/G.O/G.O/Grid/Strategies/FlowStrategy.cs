using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ION.GridStrategies
{
    public class FlowStrategy : GridStrategy
    {

        private int step = 0;

        public override void update(int ellapsed)
        {
            //do unit stuff
            step++;

            if (step == 5)
            {
                tileVersusTile();
            }
            else if (step == 10)
            {
                tileAidTile();

                step = 0;
            }

            //now tell all Tiles to update, we use the perspective map for that
            //because it might be faster?
            for (int i = 0; i < Grid.tileCount; i++)
            {
                Grid.perspectiveMap[i].update();
            }
        }

        private void tileVersusTile()
        {
            Tile todo;

            //This method looks at the grid from a overhead perspective where x increased in the
            //right direction and y increases in the downward direction.

            for (int i = 0; i < Grid.width; i++)
            {
                for (int j = 0; j < Grid.height; j++)
                {
                    todo = Grid.map[i, j];

                    //The tile to the right of this tile
                    if (isValid(i + 1, j))
                    {
                        tileVersusTile(todo,Grid.map[i + 1, j]);
                    }

                    ////The tile to the bottom-right of this tile
                    //if(isValid(i+1,j+1))
                    //{
                    //    todo.tileVersusTile(map[i+1,j+1]);
                    //}

                    //The tile to the bottom of this tile
                    if (isValid(i, j + 1))
                    {
                        tileVersusTile(todo,Grid.map[i, j + 1]);
                    }

                    ////The tile to the bottom left of this tile
                    //if (isValid(i-1,j+1))
                    //{
                    //    todo.tileVersusTile(map[i-1,j+1]);
                    //}

                }
            }
        }

        private bool isValid(int x, int y)
        {
            if (x >= 0 && x < Grid.width && y >= 0 && y < Grid.height)
            {
                return true;
            }
            return false;
        }

        private void tileAidTile()
        {
            Tile todo;
            Tile other;

            //This method looks at the grid from a overhead perspective where x increased in the
            //right direction and y increases in the downward direction.

            for (int i = 0; i < Grid.width; i++)
            {
                for (int j = 0; j < Grid.height; j++)
                {
                    todo = Grid.map[i, j];

                    //The tile to the right of this tile
                    if (isValid(i + 1, j))
                    {
                        other = Grid.map[i + 1, j];
                        tileAidTile(todo,other);
                    }

                    ////The tile to the bottom-right of this tile
                    //if (isValid(i + 1, j + 1))
                    //{
                    //    todo.tileAidTile(map[i + 1, j + 1]);
                    //}

                    //The tile to the bottom of this tile
                    if (isValid(i, j + 1))
                    {
                        other = Grid.map[i, j + 1];
                        tileAidTile(todo,other);
                    }

                    ////The tile to the bottom left of this tile
                    //if (isValid(i - 1, j + 1))
                    //{
                    //    todo.tileAidTile(map[i - 1, j + 1]);
                    //}

                }
            }

        }

        public void tileVersusTile(Tile a, Tile b)
        {
            ResourceTile resourceTileA;
            if (a is ResourceTile)
            {
                resourceTileA = (ResourceTile)a;
            }
            else
            {
                return;
            }

            //Check if the b tile is a resource tile else return from this method
            ResourceTile resourceTileB;
            if (b is ResourceTile)
            {
                resourceTileB = (ResourceTile)b;
            }
            else
            {
                return;
            }

            //if (b is ResourceTile)
            //{
            //    ResourceTile resourceTileB = (ResourceTile)b;
            //    if (resourceTileB.owner != owner && resourceTileB.owner != Players.NEUTRAL)
            //    {
            //        if (resourceTileB.charge > charge)
            //        {
            //            if (resourceTileB.charge - charge > minimumFlux)
            //            {
            //                removeCharge(minimumFlux);
            //                resourceTileB.removeCharge(minimumFlux);
            //            }

            //        }
            //        else if (charge > resourceTileB.charge)
            //        {
            //            if (charge - resourceTileB.charge > minimumFlux)
            //            {
            //                resourceTileB.removeCharge(minimumFlux);
            //                removeCharge(minimumFlux);
            //            }

            //        }

            //    }
            //}
        }

        public void tileAidTile(Tile a, Tile b)
        {
            ResourceTile resourceTileA;
            if (a is ResourceTile)
            {
                resourceTileA = (ResourceTile)a;
            }
            else
            {
                return;
            }
            
            //Check if the b tile is a resource tile else return from this method
            ResourceTile resourceTileB;
            if (b is ResourceTile)
            {
                resourceTileB = (ResourceTile)b;
            }
            else
            {
                return;
            }
 


            //Check if these tiles have the same owner
            if (resourceTileA.owner == resourceTileB.owner)
            {
                if (resourceTileA.charge > resourceTileB.charge)
                {
                    float diff = resourceTileA.charge - resourceTileB.charge;
                    //if (diff < 0.01f)
                    //{
                    //    return;
                    //}


                    float draw = 0.0f;
                    float maxRelease = resourceTileA.charge / 4.0f;
                    float maxDraw = (ResourceTile.MAX_CHARGE - resourceTileB.charge) / 4.0f;

                    if (maxDraw > maxRelease)
                    {
                        draw = maxRelease;
                    }
                    else if (maxRelease > maxDraw)
                    {
                        draw = maxDraw;
                    }

                    if (draw > diff)
                    {
                        draw = diff;
                    }

                    resourceTileA.removeCharge(draw);
                    resourceTileB.addCharge(draw, resourceTileA.owner);
                }
                else if (resourceTileB.charge > resourceTileA.charge)
                {
                    float diff = resourceTileB.charge - resourceTileA.charge;
                    //if (diff < 0.01f)
                    //{
                    //    return;
                    //}


                    float draw = 0.0f;
                    float maxRelease = resourceTileB.charge / 4.0f;
                    float maxDraw = (ResourceTile.MAX_CHARGE - resourceTileA.charge) / 4.0f;

                    if (maxDraw > maxRelease)
                    {
                        draw = maxRelease;
                    }
                    else if (maxRelease > maxDraw)
                    {
                        draw = maxDraw;
                    }

                    if (draw > diff)
                    {
                        draw = diff;
                    }

                    resourceTileB.removeCharge(draw);
                    resourceTileA.addCharge(draw, resourceTileB.owner);
                }
            }

            else if (resourceTileA.owner == Players.NEUTRAL && resourceTileB.owner != Players.NEUTRAL)
            {
                if (resourceTileB.charge > 0.95f)
                {
                    //resourceTileA.nextOwner = resourceTileB.owner;
                    resourceTileA.addCharge(1000000000.0f, resourceTileB.owner);
                }
            }
            else if (resourceTileA.owner != Players.NEUTRAL && resourceTileB.owner == Players.NEUTRAL)
            {
                if (resourceTileA.charge > 0.95f)
                {
                    //resourceTileB.nextOwner = resourceTileA.owner;
                    resourceTileB.addCharge(1000000000.0f, resourceTileA.owner);
                }
            }

        }
    }
}
