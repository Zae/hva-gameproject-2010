using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Audio;

namespace ION.Tools
{
    class CoordinateTool
    {

        //private int[] horizontal = new int[26] {
        
        private const int horizontal = 26;
        private const int vertical = 10;

        public static string[] alphabet = new string[26] { "alpha", "bravo", "charlie", "delta", "echo", "foxtrot", "golf", "hotel", "india", "juliet", "kilo", "lima", "mike", "november", "oscar", "papa", "quebec", "romeo", "sierra", "tango", "uniform", "victor", "whiskey", "xray", "yankee", "zulu" };
        ////private static string[] alphabet = new string[26] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        public static string[] decimals = new string[10] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", };
        
        public static Coordinate coordinateSound(int x, int y)
        {
            //Debug.WriteLine("***Coordinate("+x+":"+y+"***");
            
            int[] xIndices = new int[2];
            int[] yIndices = new int[3];

            int xValid = 0;
            int yValid = 0;

            int[] xCoordinate;
            int[] yCoordinate;

            if (x == 0)
            {
                xIndices[xValid] = x % horizontal;
                xValid++;
            }
            if (y == 0)
            {
                yIndices[yValid] = y % vertical;
                yValid++;
            }

            while (x > 0)
            {
                xIndices[xValid] = x % horizontal;
                xValid++;
                x /= horizontal;
            }
            while (y > 0)
            {
                yIndices[yValid] = y % vertical;
                yValid++;
                y /= vertical;
            }

            xCoordinate = new int[xValid];
            yCoordinate = new int[yValid];

            int j = 0;
            for (int i = xValid-1; i > -1; i--)
            {
                xCoordinate[j] = xIndices[i];
                j++;
            }

            j = 0;

            for (int i = yValid - 1; i > -1; i--)
            {
                yCoordinate[j] = yIndices[i];
                j++;
            }

            Coordinate c = new Coordinate(xCoordinate, yCoordinate);
            return c;
        }




    }

    public class Coordinate
    {
        public int[] xCoordinate;
        public int[] yCoordinate;

        public SoundEffect[] sounds;

        public int atSound = 0;

        private SoundEffectInstance current;

        public Coordinate(int[] xCoordinate, int[] yCoordinate)
        {
            this.xCoordinate = xCoordinate;
            this.yCoordinate = yCoordinate;

            sounds = new SoundEffect[(xCoordinate.Length + yCoordinate.Length)];

            int soundsCount = 0;

            for (int i = 0; i < xCoordinate.Length; i++)
            {
                sounds[soundsCount] = Sounds.alphabet[SoundManager.getVoiceCount(),xCoordinate[i]];
                soundsCount++;
            }

            for (int i = 0; i < yCoordinate.Length; i++)
            {
                sounds[soundsCount] = Sounds.numbers[SoundManager.getVoiceCount(),yCoordinate[i]];
                soundsCount++;
            }

            current = sounds[atSound].CreateInstance();
        }

        public bool update()
        {
            if (current.State == SoundState.Stopped && atSound < sounds.Length)
            {
                current = sounds[atSound].CreateInstance();
                current.Play();
                atSound++;
                
            }
            //finished all sounds
            else if (atSound == sounds.Length)
            {
                return false;
            }
            return true;
        }
    }

}
