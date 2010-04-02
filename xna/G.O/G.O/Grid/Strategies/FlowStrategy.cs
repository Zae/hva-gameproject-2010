using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ION.GridStrategies
{
    public class FlowStrategy : GridStrategy
    {

        private int step = -1;

        private int tileAidStep = 1;
        private int tileVersusStep = 0;

        //SETTINGS

        private bool spread = true;
        private float spreadValue = 7.0f;

        private bool useMomentum = true;

        public FlowStrategy()
        {
            name = "FlowStrategy";
        }

        public override void increaseSpeed()
        {
            if (tileVersusStep > 0)
            {
                tileAidStep--;
                tileVersusStep--;
                speed++;

                step = -1;
            }
        }
        public override void decreaseSpeed()
        {
            tileAidStep++;
            tileVersusStep++;
            speed--;

            step = -1;
        }

        public override void update(int ellapsed)
        {
            //do unit stuff
            step++;
            if (step == tileVersusStep)
            {
                tileVersusTile();
            }
            else if (step == tileAidStep)
            {
                tileAidTile();

                step = -1;
            }


            //if (step == tileAidStep)
            //{
            //    tileVersusTile();
            //}
            //else if (step == tileVersusStep)
            //{
            //    tileAidTile();

            //    step = -1;
            //}

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
                    //if (isValid(i + 1, j + 1))
                    //{
                    //    tileVersusTile(other,Grid.map[i + 1, j + 1]);
                    //}

                    //The tile to the bottom of this tile
                    if (isValid(i, j + 1))
                    {
                        tileVersusTile(todo,Grid.map[i, j + 1]);
                    }

                    ////The tile to the bottom left of this tile
                    //if (isValid(i - 1, j + 1))
                    //{
                    //    tileVersusTile(other,Grid.map[i - 1, j + 1]);
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

            for (int i = 0; i < Grid.tileCount; i++)
            {
                Grid.perspectiveMap[i].releaseMomentum();
            }

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
                        tileAidTile(todo,other,1,0);
                    }

                    //otherX = i + 1;
                    //otherY = j + 1;
                    ////The tile to the bottom-right of this tile
                    //if (isValid(i + 1, j + 1))
                    //{
                    //    other = Grid.map[otherX, otherY];
                    //    tileAidTile(other, other, 1, 1);
                    //}

                    otherX = i;
                    otherY = j + 1;
                    //The tile to the bottom of this tile
                    if (isValid(i, j + 1))
                    {
                        other = Grid.map[otherX, otherY];
                        tileAidTile(todo,other,0,1);
                    }

                    //otherX = i - 1;
                    //otherY = j + 1;
                    ////The tile to the bottom left of this tile
                    //if (isValid(i - 1, j + 1))
                    //{
                    //    other = Grid.map[otherX, otherY];
                    //    tileAidTile(other, other, -1, 1);
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
                 
                            resourceTileA.sustain(0.002f,resourceTileB.owner);
                            resourceTileB.donate(0.001f);
                        

                    }
                    else if (resourceTileA.charge > resourceTileB.charge)
                    {
                       
                            resourceTileB.sustain(0.002f,resourceTileA.owner);
                            resourceTileA.donate(0.001f);
                        

                    }

                }
            //}
        }

        public void tileAidTile(Tile a, Tile b, int directionX, int directionY)
        {
            //Console.WriteLine("**********");
            //Console.WriteLine("directionX = " + directionX);
            //Console.WriteLine("directionY = " + directionY);
            //Console.WriteLine("INVdirectionX = " + -directionX);
            //Console.WriteLine("INVdirectionY = " + -directionY);
            

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

            //if (a.id == b.id)
            //{
            //    Console.WriteLine("**********");
            //    Console.WriteLine("a.id = " + a.id);
            //    Console.WriteLine("b.id = " + b.id);

            //    //Console.WriteLine("EEEEEROOOOOOOOOOOOOOOOOOOOOOOR!");

            //    //Console.WriteLine("EEEEEROOOOOOOOOOOOOOOOOOOOOOOR!");

            //    //Console.WriteLine("EEEEEROOOOOOOOOOOOOOOOOOOOOOOR!");

            //}
 


            //Check if these tiles have the same owner
            if (resourceTileA.owner == resourceTileB.owner)
            {
                if (resourceTileA.charge > resourceTileB.charge)
                {
                    if(!canDonate(resourceTileA,directionX,directionY)) {
                        return;
                    }
                    
             


                    float draw = 0.0f;
                    float maxRelease = resourceTileA.charge / 4.0f;
                    float maxDraw = (ResourceTile.MAX_CHARGE - resourceTileB.charge) / 4.0f;

                    if (maxDraw >= maxRelease)
                    {
                        draw = maxRelease;
                    }
                    else if (maxRelease >= maxDraw)
                    {
                        draw = maxDraw;
                    }

                    float diff = resourceTileA.charge - resourceTileB.charge;
                    //if (diff < 0.01f)
                    //{
                    //    return;
                    //}
                    if (draw >= (diff/4))
                    {
                        //return;
                        draw = (diff/4);
                    }

                    resourceTileA.donate(draw);
                    resourceTileB.receive(draw);

                    addMomentum(resourceTileB,-directionX,-directionY);
                }
                else if (resourceTileA.charge < resourceTileB.charge)
                {
                    if(!canDonate(resourceTileB,-directionX,-directionY)) {
                        return;
                    }
                    
                    
                 


                    float draw = 0.01f;
                    float maxRelease = resourceTileB.charge / 4.0f;
                    float maxDraw = (ResourceTile.MAX_CHARGE - resourceTileA.charge) / 4.0f;

                    if (maxDraw >= maxRelease)
                    {
                        draw = maxRelease;
                    }
                    else if (maxRelease >= maxDraw)
                    {
                        draw = maxDraw;
                    }


                    float diff = resourceTileB.charge - resourceTileA.charge;
                    //if (diff < 0.0006f)
                    //{
                    //    return;
                    //}
                    if (draw >= (diff / 4))
                    {
                        //return;
                        draw = (diff / 4);
                    }

                    resourceTileB.donate(draw);
                    resourceTileA.receive(draw);

                   addMomentum(resourceTileA,directionX,directionY);
                }
            }

            else if(spread) {

            //This bit looks if a neutral tile can be claimed
            if (resourceTileA.owner == Players.NEUTRAL && resourceTileB.owner != Players.NEUTRAL)
            {
                if (resourceTileB.charge > 0.80f)
                {
                    //resourceTileA.nextOwner = resourceTileB.owner;
                    resourceTileA.sustain(1000000000.0f, resourceTileB.owner);
                }
            }
            else if (resourceTileA.owner != Players.NEUTRAL && resourceTileB.owner == Players.NEUTRAL)
            {
                if (resourceTileA.charge > 0.80f)
                {
                    //resourceTileB.nextOwner = resourceTileA.owner;
                    resourceTileB.sustain(1000000000.0f, resourceTileA.owner);
                }
            }

            }

        }

        //public bool getDirectionalBool(ResourceTile target, ResourceTile subject)
        //{
        //    //int xDirection = target.

        //    return false;
        //}

        private bool canDonate(ResourceTile donator, int directionX, int directionY)
        {
            
            if(directionX == 1 && directionY == 0) 
            {
                if(donator.canDonateE == 0) {
                    return true;
                }
                return false;
            }
            else if(directionX == 1 && directionY == 1) 
            {
                if(donator.canDonateSE == 0) {
                    return true;
                }
                return false;
            }
            else if(directionX == 0 && directionY == 1) 
            {
                if(donator.canDonateS == 0) {
                    return true;
                }
                return false;
            }
            else if(directionX == -1 && directionY == 1) 
            {
                if(donator.canDonateSW == 0) {
                    return true;
                }
                return false;
            }
            else if(directionX == -1 && directionY == 0) 
            {
                if(donator.canDonateW == 0) {
                    return true;
                }
                return false;
            }
            else if(directionX == -1 && directionY == -1) 
            {
                if(donator.canDonateNW == 0) {
                    return true;
                }
                return false;
            }
            else if(directionX == 0 && directionY == -1) 
            {
                if(donator.canDonateN == 0) {
                    return true;
                }
                return false;
            }
            else if(directionX == 1 && directionY == -1) 
            {
                if(donator.canDonateNE == 0) {
                    return true;
                }
                return false;
            }

            return false;
        }

        private void addMomentum(ResourceTile receiver, int directionX, int directionY)
        {
            int direct = 2;
            int indirect1 = 0;
            int indirect2 = 0;

            if (directionX == 1 && directionY == 0)
            {
                receiver.nextCanDonateE += direct;
                receiver.nextCanDonateNE += indirect1;
                receiver.nextCanDonateSE += indirect1;

                receiver.nextCanDonateN += indirect2;
                receiver.nextCanDonateS += indirect2;
            }
            else if (directionX == 1 && directionY == 1)
            {
                receiver.nextCanDonateSE += direct;
                receiver.nextCanDonateE += indirect1;
                receiver.nextCanDonateS += indirect1;

                receiver.nextCanDonateNE += indirect2;
                receiver.nextCanDonateSW += indirect2;
            }
            else if (directionX == 0 && directionY == 1)
            {
                receiver.nextCanDonateS += direct;
                receiver.nextCanDonateSE += indirect1;
                receiver.nextCanDonateSW += indirect1;

                receiver.nextCanDonateE += indirect2;
                receiver.nextCanDonateW += indirect2;
            }
            else if (directionX == -1 && directionY == 1)
            {
                receiver.nextCanDonateSW += direct;
                receiver.nextCanDonateW += indirect1;
                receiver.nextCanDonateS += indirect1;

                receiver.nextCanDonateSE += indirect2;
                receiver.nextCanDonateNW += indirect2;
            }
            else if (directionX == -1 && directionY == 0)
            {
                receiver.nextCanDonateW += direct;
                receiver.nextCanDonateNW += indirect1;
                receiver.nextCanDonateSW += indirect1;

                receiver.nextCanDonateN += indirect2;
                receiver.nextCanDonateS += indirect2;
            }
            else if (directionX == -1 && directionY == -1)
            {
                receiver.nextCanDonateNW += direct;
                receiver.nextCanDonateN += indirect1;
                receiver.nextCanDonateW += indirect1;

                receiver.nextCanDonateNE += indirect2;
                receiver.nextCanDonateSW += indirect2;

            }
            else if (directionX == 0 && directionY == -1)
            {
                receiver.nextCanDonateN += direct;
                receiver.nextCanDonateNE += indirect1;
                receiver.nextCanDonateNW += indirect1;

                receiver.nextCanDonateW += indirect2;
                receiver.nextCanDonateE += indirect2;
            }
            else if (directionX == 1 && directionY == -1)
            {
                receiver.nextCanDonateNE += direct;
                receiver.nextCanDonateN += indirect1;
                receiver.nextCanDonateE += indirect1;

                receiver.nextCanDonateNW += indirect2;
                receiver.nextCanDonateSE += indirect2;
            }

            
            
            //int direct = 2;
            //int indirect = 1;
            
            //if(directionX == 1 && directionY == 0) 
            //{
            //    receiver.nextCanDonateE += direct;
            //    receiver.nextCanDonateNE += indirect;
            //    receiver.nextCanDonateSE += indirect;
            //}
            //else if(directionX == 1 && directionY == 1) 
            //{
            //    receiver.nextCanDonateSE += direct;
            //    receiver.nextCanDonateE += indirect;
            //    receiver.nextCanDonateS += indirect;
            //}
            //else if(directionX == 0 && directionY == 1) 
            //{
            //    receiver.nextCanDonateS += direct;
            //    receiver.nextCanDonateSE += indirect;
            //    receiver.nextCanDonateSW += indirect;
            //}
            //else if(directionX == -1 && directionY == 1) 
            //{
            //    receiver.nextCanDonateSW += direct;
            //    receiver.nextCanDonateW += indirect;
            //    receiver.nextCanDonateS += indirect;
            //}
            //else if(directionX == -1 && directionY == 0) 
            //{
            //    receiver.nextCanDonateW += direct;
            //    receiver.nextCanDonateNW += indirect;
            //    receiver.nextCanDonateSW += indirect;
            //}
            //else if(directionX == -1 && directionY == -1) 
            //{
            //    receiver.nextCanDonateNW += direct;
            //    receiver.nextCanDonateN += indirect;
            //    receiver.nextCanDonateW += indirect;
            //}
            //else if(directionX == 0 && directionY == -1) 
            //{
            //    receiver.nextCanDonateN += direct;
            //    receiver.nextCanDonateNE += indirect;
            //    receiver.nextCanDonateNW += indirect;
            //}
            //else if(directionX == 1 && directionY == -1) 
            //{
            //    receiver.nextCanDonateNE += direct;
            //    receiver.nextCanDonateN += indirect;
            //    receiver.nextCanDonateE += indirect;
            //}

         
        }
    }
}
