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
        public ThunderStrategy()
        {
            name = "ThunderStrategy";
        }


        public override void reset()
        {
  
        }

        public override void draw()
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
            //if (maxStep > 0)
            //{
            //    maxStep--;
            //    speed++;
            //    step = -2;
            //}
        }
        public override void decreaseSpeed()
        {
            //maxStep++;
            //speed--;
            //step = -2;
        }

        public override void update(int ellapsed)
        {
            ////do unit stuff
            //step++;
            //if (step == 0)
            //{
            //    DateTime timeBefore;
            //    DateTime timeAfter;
            //    TimeSpan timeTaken;

            //    timeBefore = DateTime.Now;
            //    recalculateGrid();
            //    timeAfter = DateTime.Now;

            //    iterations++;
            //    timeTaken = timeAfter - timeBefore;
            //    lastTimeSpent = timeTaken.Milliseconds;
            //    totalTimeSpent += lastTimeSpent;
            //    avgTimeSpent = totalTimeSpent / iterations;

            //    //now tell all Tiles to update
            //    //We do this every step because unit interactions might have happened
            //    for (int i = 0; i < Grid.tileCount; i++)
            //    {
            //        Grid.perspectiveMap[i].update();
            //    }


            //}
            //if (step == maxStep)
            //{

            //    step = -1;
            //}


        }
    }
}
