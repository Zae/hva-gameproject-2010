using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace ION.Tools
{
    class SoundManager
    {

        private static SoundEffectInstance[] firesounds;
        private static int firesoundsCount;

        private static int firesoundsPointer = 0;

        private static int levelOfAction = 0;

        private static SoundEffectInstance selectUnit;
        private static SoundEffectInstance orderUnit;

        public static void init()
        {
            firesoundsCount = Sounds.fireSounds.Length;
            firesounds = new SoundEffectInstance[firesoundsCount];
            for (int i = 0; i < firesoundsCount; i++)
            {
                firesounds[i] = Sounds.fireSounds[i].CreateInstance();
                firesounds[i].IsLooped = false;
                firesounds[i].Volume = 0.2f;
            }

            selectUnit = Sounds.selectUnit.CreateInstance();
            selectUnit.IsLooped = false;
            
            orderUnit = Sounds.orderUnit.CreateInstance();
            orderUnit.IsLooped = false;
           
        }

        public static void update()
        {
           //Debug.WriteLine("SM: loa=" + levelOfAction);
            
            if (levelOfAction > 0)
            {
                StateTest.get().actionOnScreen = true;
                levelOfAction--;
            }
            else
            {
                StateTest.get().actionOnScreen = false;
            }
        }

        public static void fireSound(int ticks)
        {
            levelOfAction+=2;
            
            firesoundsPointer++;
            if (firesoundsPointer == firesoundsCount)
            {
                firesoundsPointer = 0;
            }
            firesounds[firesoundsPointer].Play();
           
        }

        public static void selectUnitSound()
        {
            if (selectUnit.State != SoundState.Playing)
            {
                selectUnit.Play();
            }
        }

        public static void orderUnitSound()
        {
            if (orderUnit.State != SoundState.Playing)
            {
                orderUnit.Play();
            }
        }


    }
}
