using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace ION
{
    class Sounds
    {
        //Sounds is loaded in ION.LoadContent()
        
        public static Song titleSong;
        public static Song gameSong1;
        public static Song gameSong2;

        public static SoundEffect logoSound;
        public static SoundEffect actionSound1;

        public static SoundEffect[] selectUnitSounds;
        public static SoundEffect orderUnit;


        public static SoundEffect[] fireSounds;
        public static SoundEffect[] explosionSounds;


        public static SoundEffect[,] alphabet;
        public static SoundEffect[,] numbers;
    }
}
