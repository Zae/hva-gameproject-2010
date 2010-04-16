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
           
        }
    }
}
