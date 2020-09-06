using UnityEngine;
public class AnimationManager : MonoBehaviour {

    public Animator KnightAnim;
    public Animator WarriorAnim;
    public Animator WizardAnim;
    public Animator Enemy1Anim;
    public Animator Enemy2Anim;
    public void PlayAnim (string animName, int UnitID) {

        switch (UnitID) {
            case 0: //Knight Animations
                KnightAnim.SetTrigger (animName);
                break;
            case 1: //Warrior Animations
                WarriorAnim.SetTrigger (animName);
                break;
            case 2: //Wizard Animations
                WizardAnim.SetTrigger (animName);
                break;
            case 4: //Skeleton Animations
                Enemy1Anim.SetTrigger (animName);
                break;
            case 5: //Undead Animations
                Enemy2Anim.SetTrigger (animName);
                break;
        }
    }
}