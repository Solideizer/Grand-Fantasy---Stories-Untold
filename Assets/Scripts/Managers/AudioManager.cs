﻿using UnityEngine;

namespace Managers
{
    public class AudioManager : MonoBehaviour {
    
        #region Variable Declarations
    
        private static AudioClip _basicAttackSound;
        private static readonly AudioClip[] HurtSounds = new AudioClip[15];
        private static readonly AudioClip[] KnightOpenerSounds = new AudioClip[5];
        private static readonly AudioClip[] KnightDeathSounds = new AudioClip[2];

        private static AudioSource _audioSource;
    
        #endregion
        private void Start () {
            _basicAttackSound = Resources.Load<AudioClip> ("basicAttack");

            KnightOpenerSounds[0] = Resources.Load<AudioClip> ("FallByMyHand1");
            KnightOpenerSounds[1] = Resources.Load<AudioClip> ("TasteMySteel1");
            KnightOpenerSounds[2] = Resources.Load<AudioClip> ("ByTheFury1");
            KnightOpenerSounds[3] = Resources.Load<AudioClip> ("ByTheGods1");
            KnightOpenerSounds[4] = Resources.Load<AudioClip> ("ByTheLight1");

            KnightDeathSounds[0] = Resources.Load<AudioClip> ("KnightDeath");
            KnightDeathSounds[1] = Resources.Load<AudioClip> ("KnightDeath2");

            HurtSounds[0] = Resources.Load<AudioClip> ("hurt0");
            HurtSounds[1] = Resources.Load<AudioClip> ("hurt1");
            HurtSounds[2] = Resources.Load<AudioClip> ("hurt2");
            HurtSounds[3] = Resources.Load<AudioClip> ("hurt3");
            HurtSounds[4] = Resources.Load<AudioClip> ("hurt4");
            HurtSounds[5] = Resources.Load<AudioClip> ("Hit1");
            HurtSounds[6] = Resources.Load<AudioClip> ("Hit2");
            HurtSounds[7] = Resources.Load<AudioClip> ("Hit3");
            HurtSounds[8] = Resources.Load<AudioClip> ("Hit4");
            HurtSounds[9] = Resources.Load<AudioClip> ("Hit5");
            HurtSounds[10] = Resources.Load<AudioClip> ("HitIntense1");
            HurtSounds[11] = Resources.Load<AudioClip> ("HitIntense2");
            HurtSounds[12] = Resources.Load<AudioClip> ("HitIntense3");
            HurtSounds[13] = Resources.Load<AudioClip> ("HitIntense4");
            HurtSounds[14] = Resources.Load<AudioClip> ("HitIntense5");

            _audioSource = GetComponent<AudioSource> ();
        }

        public static void PlaySound (string clip) {
            int random;
            switch (clip) {
                case "basicAttack":
                    _audioSource.PlayOneShot (_basicAttackSound);
                    break;
                case "hurtSound":
                    random = Random.Range (0, HurtSounds.Length);
                    _audioSource.PlayOneShot (HurtSounds[random]);
                    break;
                case "KnightOpeners":
                    random = Random.Range (0, KnightOpenerSounds.Length);
                    _audioSource.PlayOneShot (KnightOpenerSounds[random]);
                    break;
                case "KnightDeath":
                    random = Random.Range (0, KnightDeathSounds.Length);
                    _audioSource.PlayOneShot (KnightOpenerSounds[random]);
                    break;

            }
        }
    }
}