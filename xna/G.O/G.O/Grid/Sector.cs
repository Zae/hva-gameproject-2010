using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ION
{
    public class Sector
    {

        public int indexX;
        public int indexY;

        public const int MAX_SECTOR_ON_AXIS = 26;
        public const int TILES_PER_SECTOR_AXIS = 4;

        public string coordinate;

        public Sector(int indexX, int indexY, int tileX, int tileY)
        {
            this.indexX = indexX;
            this.indexY = indexY;

            satisfyCoordinates();
        }

        private void satisfyCoordinates()
        {
           
            
            ////This is the goal amount of Sectors but I'm lazy and don't want to make the graphics
            string[] xCoordinates = new string[MAX_SECTOR_ON_AXIS] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            coordinate = xCoordinates[indexX];

            coordinate += "-";

            string[] yCoordinates = new string[MAX_SECTOR_ON_AXIS] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26" };
            coordinate += yCoordinates[indexY];
        }
    }
}
