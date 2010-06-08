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

        private static SoundEffectInstance[,] firesounds;
        private static int firesoundsCount;
        private static int firesoundsPointer = 0;

        private static SoundEffectInstance[] explosionsounds;
        private static int explosionsoundsCount;
        private static int explosionsoundsPointer = 0;


        private static int levelOfAction = 0;

        public static int blabla = 2;

        private static SoundEffectInstance[] selectUnit;
        private static int selectUnitCount;
        private static int selectUnitPointer = 0;

        private static SoundEffectInstance orderUnit;

        private static int voiceCount = -1;
        private static int voiceMaxCount = 3;

        private static Coordinate coordinate = null;

        public static void init()
        {
            firesoundsCount = Sounds.fireSounds.Length;
            firesounds = new SoundEffectInstance[blabla,firesoundsCount];
            for (int j = 0; j < blabla; j++)
            {

                for (int i = 0; i < firesoundsCount; i++)
                {
                    firesounds[j,i] = Sounds.fireSounds[i].CreateInstance();
                    firesounds[j,i].IsLooped = false;
                    firesounds[j,i].Volume = 0.4f;
                }

            }

            explosionsoundsCount = Sounds.explosionSounds.Length;
            explosionsounds = new SoundEffectInstance[explosionsoundsCount];
            for (int i = 0; i < explosionsoundsCount; i++)
            {
                explosionsounds[i] = Sounds.explosionSounds[i].CreateInstance();
                explosionsounds[i].IsLooped = false;
                //explosionsounds[i].Volume = 0.2f;
            }

            selectUnitCount = Sounds.selectUnitSounds.Length;
            selectUnit = new SoundEffectInstance[selectUnitCount];
            for (int i = 0; i < selectUnitCount; i++)
            {
                selectUnit[i] = Sounds.selectUnitSounds[i].CreateInstance();
                selectUnit[i].IsLooped = false;
                //explosionsounds[i].Volume = 0.2f;
            }

            //selectUnit = Sounds.selectUnit.CreateInstance();
            //selectUnit.IsLooped = false;
            
            orderUnit = Sounds.orderUnit.CreateInstance();
            orderUnit.IsLooped = false;
           
        }

        public static void playCoordinate()
        {
            if (coordinate != null)
            {
                if (!coordinate.update())
                {
                    coordinate = null;
                }
            }
        }

        public static void setCoordinate(Coordinate c)
        {
            if (coordinate == null)
            {
                coordinate = c;
            }
        }

        public static int getVoiceCount()
        {
            voiceCount++;
            if (voiceCount < voiceMaxCount)
            {
                return voiceCount;
            }
            else
            {
                voiceCount = 0;
                return voiceCount;
            }
        }

        public static void update()
        {
           //Debug.WriteLine("SM: loa=" + levelOfAction);
            if (levelOfAction > 0)
            {
                levelOfAction--;
            }

            if (levelOfAction > 15)
            {
                StateTest.get().actionOnScreen = true;
            }
            else
            {
                StateTest.get().actionOnScreen = false;
            }

            playCoordinate();
        }

        public static void fireSound()
        {
            levelOfAction+=10;

            firesoundsPointer++;
            if (firesoundsPointer == firesoundsCount)
            {
                firesoundsPointer = 0;
            }

            for (int i = 0; i < blabla; i++)
            {
                if (firesounds[i,firesoundsPointer].State == SoundState.Stopped)
                {
                    firesounds[i,firesoundsPointer].Play();
                    return;
                }
            }

        }

        public static void explosionSound()
        {
            explosionsoundsPointer++;
            if (explosionsoundsPointer == explosionsoundsCount)
            {
                explosionsoundsPointer = 0;
            }


            explosionsounds[explosionsoundsPointer].Play();

        }


        public static void selectUnitSound()
        {
            selectUnitPointer++;
            if (selectUnitPointer == selectUnitCount)
            {
                selectUnitPointer = 0;
            }


            selectUnit[selectUnitPointer].Play();
        }

        //public static void selectUnitSound()
        //{
        //    if (selectUnit.State != SoundState.Playing)
        //    {
        //        selectUnit.Play();
        //    }
        //}

        //public static void orderUnitSound()
        //{
        //    //if (orderUnit.State != SoundState.Playing)
        //    //{
        //    //    orderUnit.Play();
        //    //}
        //}


    }
}
