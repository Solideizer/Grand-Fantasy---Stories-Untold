using UnityEngine;

namespace Managers
{
    public class AnimationManager : MonoBehaviour {
        
        #region Variable Declarations
        
        public Animator KnightAnim;
        public Animator WarriorAnim;
        public Animator WizardAnim;
        public Animator Enemy1Anim;
        public Animator Enemy2Anim;

        #endregion
        
        public void PlayAnim (string animName, int unitID) {

            switch (unitID) {
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
}