using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioClip basicAttackSound;
    public static AudioClip[] hurtSound = new AudioClip[5];
    
    
    public static AudioSource audioSrc;
    
    private void Start()
    {
        basicAttackSound = Resources.Load<AudioClip>("basicAttack");
        hurtSound[0] = Resources.Load<AudioClip>("hurt0");
        hurtSound[1] = Resources.Load<AudioClip>("hurt1");
        hurtSound[2] = Resources.Load<AudioClip>("hurt2");
        hurtSound[3] = Resources.Load<AudioClip>("hurt3");
        hurtSound[4] = Resources.Load<AudioClip>("hurt4");
        

        audioSrc = GetComponent<AudioSource>();
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "basicAttack":
            audioSrc.PlayOneShot(basicAttackSound);
            break;

            case "hurtSound":
                int randomInd = Random.Range(0, hurtSound.Length);
                audioSrc.PlayOneShot(hurtSound[randomInd]);
                break;

            
        }
    }
}