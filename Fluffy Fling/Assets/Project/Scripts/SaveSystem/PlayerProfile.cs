using System;
using System.Collections;
using UnityEngine;

namespace tomi.SaveSystem
{
    [System.Serializable]
    public class PlayerProfile
    {
        public int[] scores;
        public int[] stars;


        public int masterVolume;
        public int musicVolume;
        public int effectsVolume;

        public bool musicOn;
        public bool effectsOn;
    }
}