using UnityEngine;

namespace Managers
{
    public class AudioManager : MonoBehaviour {
    
        #region Variable Declarations
    
        public static AudioClip basicAttackSound;
        public static AudioClip[] hurtSounds = new AudioClip[15];
        public static AudioClip[] knightOpenerSounds = new AudioClip[5];
        public static AudioClip[] knightDeathSounds = new AudioClip[2];

        public static AudioSource audioSrc;
    
        #endregion
        private void Start () {
            basicAttackSound = Resources.Load<AudioClip> ("basicAttack");

            knightOpenerSounds[0] = Resources.Load<AudioClip> ("FallByMyHand1");
            knightOpenerSounds[1] = Resources.Load<AudioClip> ("TasteMySteel1");
            knightOpenerSounds[2] = Resources.Load<AudioClip> ("ByTheFury1");
            knightOpenerSounds[3] = Resources.Load<AudioClip> ("ByTheGods1");
            knightOpenerSounds[4] = Resources.Load<AudioClip> ("ByTheLight1");

            knightDeathSounds[0] = Resources.Load<AudioClip> ("KnightDeath");
            knightDeathSounds[1] = Resources.Load<AudioClip> ("KnightDeath2");

            hurtSounds[0] = Resources.Load<AudioClip> ("hurt0");
            hurtSounds[1] = Resources.Load<AudioClip> ("hurt1");
            hurtSounds[2] = Resources.Load<AudioClip> ("hurt2");
            hurtSounds[3] = Resources.Load<AudioClip> ("hurt3");
            hurtSounds[4] = Resources.Load<AudioClip> ("hurt4");
            hurtSounds[5] = Resources.Load<AudioClip> ("Hit1");
            hurtSounds[6] = Resources.Load<AudioClip> ("Hit2");
            hurtSounds[7] = Resources.Load<AudioClip> ("Hit3");
            hurtSounds[8] = Resources.Load<AudioClip> ("Hit4");
            hurtSounds[9] = Resources.Load<AudioClip> ("Hit5");
            hurtSounds[10] = Resources.Load<AudioClip> ("HitIntense1");
            hurtSounds[11] = Resources.Load<AudioClip> ("HitIntense2");
            hurtSounds[12] = Resources.Load<AudioClip> ("HitIntense3");
            hurtSounds[13] = Resources.Load<AudioClip> ("HitIntense4");
            hurtSounds[14] = Resources.Load<AudioClip> ("HitIntense5");

            audioSrc = GetComponent<AudioSource> ();
        }

        public static void PlaySound (string clip) {
            int random;
            switch (clip) {
                case "basicAttack":
                    audioSrc.PlayOneShot (basicAttackSound);
                    break;
                case "hurtSound":
                    random = Random.Range (0, hurtSounds.Length);
                    audioSrc.PlayOneShot (hurtSounds[random]);
                    break;
                case "KnightOpeners":
                    random = Random.Range (0, knightOpenerSounds.Length);
                    audioSrc.PlayOneShot (knightOpenerSounds[random]);
                    break;
                case "KnightDeath":
                    random = Random.Range (0, knightDeathSounds.Length);
                    audioSrc.PlayOneShot (knightOpenerSounds[random]);
                    break;

            }
        }
    }
}