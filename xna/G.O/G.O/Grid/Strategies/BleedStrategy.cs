using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ION.GridStrategies
{
    public class BleedStrategy : GridStrategy
    {
        private int step = 0;

        public BleedStrategy()
        {
            name = "BleedStrategy";
        }

        public override void reset()
        {
           
        }

        public override void drawDebug()
        {
          
        }

        public override void increaseSpeed()
        {
            
        }
        public override void decreaseSpeed()
        {

        }

        
        public override void update(int ellapsed)
        {
            //do unit stuff
            step++;

            if (step == 20)
            {
                tileVersusTile();
            }
            else if (step == 40)
            {
                tileAidTile();

                step = -1;
            }

            //now tell all Tiles to update, we use the perspective grid for that
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
                    //    other.tileVersusTile(grid[i+1,j+1]);
                    //}

                    //The tile to the bottom of this tile
                    if (isValid(i, j + 1))
                    {
                        tileVersusTile(todo,Grid.map[i, j + 1]);
                    }

                    ////The tile to the bottom left of this tile
                    //if (isValid(i-1,j+1))
                    //{
                    //    other.tileVersusTile(grid[i-1,j+1]);
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
            int otherX = 0;
            int otherY = 0;

            //This method looks at the grid from a overhead perspective where x increased in the
            //right direction and y increases in the downward direction.

            for (int i = 0; i < Grid.width; i++)
            {
                for (int j = 0; j < Grid.height; j++)
                {
                    todo = Grid.map[i, j];


                    otherX = i + 1;
                    otherY = j;
                    //The tile to the right of this tile
                    if (isValid(otherX, otherY))
                    {
                        other = Grid.map[otherX,otherY];
                        tileAidTile(todo,other);
                    }

                    //otherX = i + 1;
                    //otherY = j + 1;
                    ////The tile to the bottom-right of this tile
                    //if (isValid(i + 1, j + 1))
                    //{
                    //    other = Grid.grid[otherX, otherY];
                    //    tileAidTile(other, other);
                    //}

                    otherX = i;
                    otherY = j + 1;
                    //The tile to the bottom of this tile
                    if (isValid(i, j + 1))
                    {
                        other = Grid.map[otherX, otherY];
                        tileAidTile(todo,other);
                    }

                    //otherX = i - 1;
                    //otherY = j + 1;
                    ////The tile to the bottom left of this tile
                    //if (isValid(i - 1, j + 1))
                    //{
                    //    other = Grid.grid[otherX, otherY];
                    //    tileAidTile(other, other);
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

                if (resourceTileA.owner != resourceTileB.owner && resourceTileA.owner != Players.NEUTRAL && resourceTileB.owner != Players.NEUTRAL)
                {
                    if (resourceTileB.charge > resourceTileA.charge)
                    {
                 
                            resourceTileA.sustain(0.003f,resourceTileB.owner);
                            resourceTileB.donate(0.005f);
                        

                    }
                    else if (resourceTileA.charge > resourceTileB.charge)
                    {
                       
                            resourceTileB.sustain(0.003f,resourceTileA.owner);
                            resourceTileA.donate(0.005f);
                        

                    }

                }
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

            if (a.id == b.id)
            {
                Console.WriteLine("**********");
                Console.WriteLine("a.id = " + a.id);
                Console.WriteLine("b.id = " + b.id);

                //Console.WriteLine("EEEEEROOOOOOOOOOOOOOOOOOOOOOOR!");

                //Console.WriteLine("EEEEEROOOOOOOOOOOOOOOOOOOOOOOR!");

                //Console.WriteLine("EEEEEROOOOOOOOOOOOOOOOOOOOOOOR!");

            }
 


            //Check if these tiles have the same owner
            if (resourceTileA.owner == resourceTileB.owner)
            {
                if (resourceTileA.charge > resourceTileB.charge)
                {
                    float diff = resourceTileA.charge - resourceTileB.charge;
                    if (diff < 0.06f)
                    {
                        return;
                    }


                    float draw = 0.0f;
                    float maxRelease = resourceTileA.charge / 16.0f;
                    float maxDraw = (ResourceTile.MAX_CHARGE - resourceTileB.charge) / 16.0f;

                    if (maxDraw >= maxRelease)
                    {
                        draw = maxRelease;
                    }
                    else if (maxRelease >= maxDraw)
                    {
                        draw = maxDraw;
                    }

                    if (draw >= diff)
                    {
                        //return;
                        draw = diff;
                    }

                    resourceTileA.donate(draw);
                    resourceTileB.receive(draw);
                }
                else if (resourceTileA.charge < resourceTileB.charge)
                {
                    float diff = resourceTileB.charge - resourceTileA.charge;
                    if (diff < 0.06f)
                    {
                        return;
                    }


                    float draw = 0.0f;
                    float maxRelease = resourceTileB.charge / 16.0f;
                    float maxDraw = (ResourceTile.MAX_CHARGE - resourceTileA.charge) / 16.0f;

                    if (maxDraw >= maxRelease)
                    {
                        draw = maxRelease;
                    }
                    else if (maxRelease >= maxDraw)
                    {
                        draw = maxDraw;
                    }

                    if (draw >= diff)
                    {
                        //return;
                        draw = diff;
                    }

                    resourceTileB.donate(draw);
                    resourceTileA.receive(draw);
                }
            }

            else if (resourceTileA.owner == Players.NEUTRAL && resourceTileB.owner != Players.NEUTRAL)
            {
                if (resourceTileB.charge > 0.93f)
                {
                    //resourceTileA.nextOwner = resourceTileB.owner;
                    resourceTileA.sustain(1000000000.0f, resourceTileB.owner);
                }
            }
            else if (resourceTileA.owner != Players.NEUTRAL && resourceTileB.owner == Players.NEUTRAL)
            {
                if (resourceTileA.charge > 0.93f)
                {
                    //resourceTileB.nextOwner = resourceTileA.owner;
                    resourceTileB.sustain(1000000000.0f, resourceTileA.owner);
                }
            }

        }

        public bool getDirectionalBool(ResourceTile target, ResourceTile subject)
        {
            //int xDirection = target.

            return false;
        }
    }
}