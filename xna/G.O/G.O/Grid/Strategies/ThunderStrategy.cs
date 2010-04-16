using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ION.GridStrategies
{
    class ThunderStrategy : GridStrategy
    {
        private Random random;

        private int seed = 0;

        private ResourceTile[] neighbours = new ResourceTile[8];


        //Speed control
        private int step = -1;
        private int maxStep = 50;

        //Timing control
        private int iterations = 0;

        
        public ThunderStrategy()
        {
            name = "ThunderStrategy";
            random = new Random(seed);
            
        }


        public override void reset()
        {
        }

        public override void drawDebug()
        {
            //int y = 200;
            //ION.spriteBatch.Begin();
            //ION.spriteBatch.DrawString(Fonts.font, "Average time spent on calculations: " + avgTimeSpent, new Vector2(10, y += 15), Color.Black);
            //ION.spriteBatch.DrawString(Fonts.font, "Last time spent on calculations: " + lastTimeSpent, new Vector2(10, y += 15), Color.Black);
            //ION.spriteBatch.DrawString(Fonts.font, "Total time spent on calculations: " + totalTimeSpent, new Vector2(10, y += 15), Color.Black);

            //ION.spriteBatch.End();
        }

        public override void increaseSpeed()
        {
            if (maxStep > 0)
            {
                maxStep--;
                speed++;
                step = -2;
            }
        }
        public override void decreaseSpeed()
        {
            maxStep++;
            speed--;
            step = -2;
        }

        public override void update(int ellapsed)
        {
            //do unit stuff
            step++;
            if (step == 0)
            {

                recalculateGrid();

                iterations++;
                
                //now tell all Tiles to update
                //We do this every step because unit interactions might have happened
                for (int i = 0; i < Grid.tileCount; i++)
                {
                    Grid.perspectiveMap[i].update();
                }


            }
            if (step == maxStep)
            {

                step = -1;
            }


        }

        private void recalculateGrid()
        {
            Tile todo;

            //We first do the "middle tiles" which can be checked without concern for probing outside the arrays
            for (int i = 1; i < Grid.width - 1; i++)
            {
                for (int j = 1; j < Grid.height - 1; j++)
                {
                    todo = Grid.map[i, j];
                    recalculateTiles(todo);
                }
            }

            //Now we run along the sides where probing the surrounding tiles might go outside the boundaries
            int lastRow = Grid.height - 1;
            for (int i = 0; i < Grid.width; i++)
            {
                todo = Grid.map[i, 0];
                recalculateTilesSafeMode(todo);
                todo = Grid.map[i, lastRow];
                recalculateTilesSafeMode(todo);
            }

            int lastColum = Grid.width - 1;
            for (int i = 1; i < Grid.height - 1; i++)
            {
                todo = Grid.map[0, i];
                recalculateTilesSafeMode(todo);
                todo = Grid.map[lastColum, i];
                recalculateTilesSafeMode(todo);
            }


        }


        private void recalculateTile(ResourceTile todo, ResourceTile[] neighbours, int validCount)
        {

            float friendlyCharge = 0.0f;
            int friendlyTileCount = 0;
            float enemyCharge = 0.0f;
            int enemyTileCount = 0;
            int neutralTiles = 0;
            ResourceTile other;

            for (int i = 0; i < validCount; i++)
            {
                other = neighbours[i];

                //If the owners are the same
                if (todo.owner == other.owner)
                {
                    friendlyTileCount++;
                    friendlyCharge += other.charge;
                }
                //If the owner is a other player, not neutral
                else if (other.owner > 0)
                {
                    enemyTileCount++;
                    enemyCharge += other.charge;
                }
                //Finally handle the neutral tiles
                else
                {
                    neutralTiles++;
                }

            }

            if (todo.owner > 0)
            {
                //float fAvg = friendlyCharge / (friendlyTileCount + (neutralTiles / 8) + 1);
                //float eAvg = enemyCharge / (enemyTileCount + (neutralTiles / 8) + 1);

                friendlyCharge += todo.charge;
                float fAvg = (friendlyCharge / (friendlyTileCount));
                float eAvg = enemyCharge / (enemyTileCount);

                fAvg += (float)random.NextDouble();
                fAvg = fAvg / 2;

                eAvg += (float)random.NextDouble();
                eAvg = eAvg / 2;

                if (eAvg > fAvg)
                {
                    if (todo.charge == 0.0f)
                    {
                        todo.sustain(0.05f, getWinningPlayer(neighbours, validCount));
                    }
                    todo.donate(0.05f);

                }

                else if (fAvg > todo.charge - 0.0f)
                {
                    float toGet = fAvg - todo.charge;
                    if (toGet > 0.05f)
                    {
                        toGet = 0.05f;
                    }
                    ////else if (toGet < 0.05f)
                    ////{
                    ////    toGet = 0.05f;
                    ////}

                    todo.receive(toGet);
                    //todo.receive(0.1f);
                }
                else if (fAvg < todo.charge)
                {
                    float toLose = todo.charge - fAvg;
                    if (toLose > 0.05f)
                    {
                        toLose = 0.05f;
                    }
                    //else if (toLose < 0.05f)
                    //{
                    //    toLose = 0.0f;
                    //}
                    //todo.receive(fAvg - todo.charge);

                    todo.donate(toLose);
                }


                //float chargeDiff = friendlyCharge - enemyCharge;

                //if (chargeDiff > 0 && chargeDiff > todo.charge)
                //{
                //    todo.receive(0.1f);
                //}
                //else
                //{
                //    todo.donate(0.1f);
                //}
            }

            ////Handle the result for a tile owned by a player
            //if (todo.owner > 0)
            //{
            //    float chargeDiff = friendlyCharge - enemyCharge;

            //    if (chargeDiff > 0 && chargeDiff > todo.charge)
            //    {
            //        todo.receive(0.1f);
            //    }
            //    else
            //    {
            //        todo.donate(0.1f);
            //    }
            //}

            //Handle the result for a neutral tile
            else
            {
                if (enemyCharge > 0.0f)
                {
                    int winningPlayer = 0;

                    //Set for two players
                    float[] playerCharges = new float[3];

                    //Add the player's charges together
                    for (int i = 0; i < validCount; i++)
                    {
                        playerCharges[neighbours[i].owner] += neighbours[i].charge;
                    }

                    float highest = 0.0f;

                    //Look for the player with the highest charge
                    for (int i = 0; i < 3; i++)
                    {
                        if (playerCharges[i] > highest)
                        {
                            highest = playerCharges[i];
                            winningPlayer = i;
                        }
                    }

                    //The tile becomes owned by the winning player
                    if (highest >= 0.9f)
                    {
                        todo.nextOwner = winningPlayer;
                        todo.nextCharge = 0.0f;
                    }


                }
            }

            if (todo.charge > ResourceTile.spikeCharge)
            {
                todo.isSpiking = true;
                todo.spikeCount = ResourceTile.spikeDuration;
                todo.charge = 2.0f;
            }
        }

        private int getWinningPlayer(ResourceTile[] neighbours, int validCount)
        {
            int winningPlayer = 0;

            //Set for two players
            float[] playerCharges = new float[3];

            //Add the player's charges together
            for (int i = 0; i < validCount; i++)
            {
                playerCharges[neighbours[i].owner] += neighbours[i].charge;
            }

            float highest = 0.0f;

            //Look for the player with the highest charge
            for (int i = 0; i < 3; i++)
            {
                if (playerCharges[i] > highest)
                {
                    highest = playerCharges[i];
                    winningPlayer = i;
                }
            }

            return winningPlayer;
        }

        private void recalculateTiles(Tile t)
        {
            if (!(t is ResourceTile))
            {
                return;
            }

            ResourceTile todo = (ResourceTile)t;

            int x = t.indexX;
            int y = t.indexY;

            int validCount = 0;


            //Horizontal and Vertical
            if (Grid.map[x, y - 1] is ResourceTile)
            {
                neighbours[validCount] = (ResourceTile)Grid.map[x, y - 1];
                validCount++;
            }

            if (Grid.map[x + 1, y] is ResourceTile)
            {
                neighbours[validCount] = (ResourceTile)Grid.map[x + 1, y];
                validCount++;
            }



            if (Grid.map[x, y + 1] is ResourceTile)
            {
                neighbours[validCount] = (ResourceTile)Grid.map[x, y + 1];
                validCount++;
            }



            if (Grid.map[x - 1, y] is ResourceTile)
            {
                neighbours[validCount] = (ResourceTile)Grid.map[x - 1, y];
                validCount++;
            }

            ////Diagonal
            //if (Grid.map[x - 1, y - 1] is ResourceTile)
            //{
            //    neighbours[validCount] = (ResourceTile)Grid.map[x - 1, y - 1];
            //    validCount++;
            //}

            //if (Grid.map[x + 1, y - 1] is ResourceTile)
            //{
            //    neighbours[validCount] = (ResourceTile)Grid.map[x + 1, y - 1];
            //    validCount++;
            //}

            //if (Grid.map[x + 1, y + 1] is ResourceTile)
            //{
            //    neighbours[validCount] = (ResourceTile)Grid.map[x + 1, y + 1];
            //    validCount++;
            //}

            //if (Grid.map[x - 1, y + 1] is ResourceTile)
            //{
            //    neighbours[validCount] = (ResourceTile)Grid.map[x - 1, y + 1];
            //    validCount++;
            //}

            recalculateTile(todo, neighbours, validCount);

        }



        private void recalculateTilesSafeMode(Tile t)
        {
            if (!(t is ResourceTile))
            {
                return;
            }

            ResourceTile todo = (ResourceTile)t;


            int validCount = 0;

            int x = t.indexX;
            int y = t.indexY;
       
            //Horizontal and Vertical
            if (isValid(x, y - 1) && Grid.map[x, y - 1] is ResourceTile)
            {
                neighbours[validCount] = (ResourceTile)Grid.map[x, y - 1];
                validCount++;

            }
            
            if (isValid(x + 1, y) && Grid.map[x + 1, y] is ResourceTile)
            {

                neighbours[validCount] = (ResourceTile)Grid.map[x + 1, y];
                validCount++;
            }
           
            if (isValid(x, y + 1) && Grid.map[x, y + 1] is ResourceTile)
            {

                neighbours[validCount] = (ResourceTile)Grid.map[x, y + 1];
                validCount++;
            }
           
            if (isValid(x - 1, y) && Grid.map[x - 1, y] is ResourceTile)
            {

                neighbours[validCount] = (ResourceTile)Grid.map[x - 1, y];
                validCount++;
            }

            //////Diagonal
            //if (isValid(x - 1, y - 1) && Grid.map[x - 1, y - 1] is ResourceTile)
            //{

            //    neighbours[validCount] = (ResourceTile)Grid.map[x - 1, y - 1];
            //    validCount++;

            //}
            //if (isValid(x + 1, y - 1) && Grid.map[x + 1, y - 1] is ResourceTile)
            //{

            //    neighbours[validCount] = (ResourceTile)Grid.map[x + 1, y - 1];
            //    validCount++;
            //}
            //if (isValid(x + 1, y + 1) && Grid.map[x + 1, y + 1] is ResourceTile)
            //{

            //    neighbours[validCount] = (ResourceTile)Grid.map[x + 1, y + 1];
            //    validCount++;
            //}
            //if (isValid(x - 1, y + 1) && Grid.map[x - 1, y + 1] is ResourceTile)
            //{

            //    neighbours[validCount] = (ResourceTile)Grid.map[x - 1, y + 1];
            //    validCount++;
            //}




            recalculateTile(todo, neighbours, validCount);

        }

        private bool isValid(int x, int y)
        {
            if (x >= 0 && x < Grid.width && y >= 0 && y < Grid.height)
            {
                return true;
            }
            return false;
        }

    }
}
