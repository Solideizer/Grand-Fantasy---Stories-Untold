using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioClip basicAttackSound;
    public static AudioClip[] hurtSound = new AudioClip[15];
    public static AudioClip[] KnightOpenerSounds = new AudioClip[5];
    public static AudioClip[] KnightDeathSounds = new AudioClip[2];


    public static AudioSource audioSrc;
    
    
    private void Start()
    {
        basicAttackSound = Resources.Load<AudioClip>("basicAttack");

        KnightOpenerSounds[0] = Resources.Load<AudioClip>("FallByMyHand1");
        KnightOpenerSounds[1] = Resources.Load<AudioClip>("TasteMySteel1");
        KnightOpenerSounds[2] = Resources.Load<AudioClip>("ByTheFury1");
        KnightOpenerSounds[3] = Resources.Load<AudioClip>("ByTheGods1");
        KnightOpenerSounds[4] = Resources.Load<AudioClip>("ByTheLight1");

        KnightDeathSounds[0] = Resources.Load<AudioClip>("KnightDeath");
        KnightDeathSounds[1] = Resources.Load<AudioClip>("KnightDeath2");

        hurtSound[0] = Resources.Load<AudioClip>("hurt0");
        hurtSound[1] = Resources.Load<AudioClip>("hurt1");
        hurtSound[2] = Resources.Load<AudioClip>("hurt2");
        hurtSound[3] = Resources.Load<AudioClip>("hurt3");
        hurtSound[4] = Resources.Load<AudioClip>("hurt4");
        hurtSound[5] = Resources.Load<AudioClip>("Hit1");
        hurtSound[6] = Resources.Load<AudioClip>("Hit2");
        hurtSound[7] = Resources.Load<AudioClip>("Hit3");
        hurtSound[8] = Resources.Load<AudioClip>("Hit4");
        hurtSound[9] = Resources.Load<AudioClip>("Hit5");
        hurtSound[10] = Resources.Load<AudioClip>("HitIntense1");
        hurtSound[11] = Resources.Load<AudioClip>("HitIntense2");
        hurtSound[12] = Resources.Load<AudioClip>("HitIntense3");
        hurtSound[13] = Resources.Load<AudioClip>("HitIntense4");
        hurtSound[14] = Resources.Load<AudioClip>("HitIntense5");
        


        audioSrc = GetComponent<AudioSource>();
    }

    public static void PlaySound(string clip)
    {
        int random;
        switch (clip)
        {
            case "basicAttack":
                audioSrc.PlayOneShot(basicAttackSound);
                break;
            case "hurtSound":
                random = Random.Range(0, hurtSound.Length);
                audioSrc.PlayOneShot(hurtSound[random]);
                break;
            case "KnightOpeners":
                random = Random.Range(0, KnightOpenerSounds.Length);
                audioSrc.PlayOneShot(KnightOpenerSounds[random]);
                break;
            case "KnightDeath":
                random = Random.Range(0, KnightDeathSounds.Length);
                audioSrc.PlayOneShot(KnightOpenerSounds[random]);
                break;


        }
    }
}