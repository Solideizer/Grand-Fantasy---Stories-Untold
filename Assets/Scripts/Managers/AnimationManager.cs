using UnityEngine;

namespace Managers
{
    public class AnimationManager : MonoBehaviour {
        
        #region Variable Declarations
        
        public Animator knightAnim;
        public Animator warriorAnim;
        public Animator wizardAnim;
        public Animator enemy1Anim;
        public Animator enemy2Anim;

        #endregion
        
        public void PlayAnim (string animName, int unitID) {

            switch (unitID) {
                case 0: //Knight Animations
                    knightAnim.SetTrigger (animName);
                    break;
                case 1: //Warrior Animations
                    warriorAnim.SetTrigger (animName);
                    break;
                case 2: //Wizard Animations
                    wizardAnim.SetTrigger (animName);
                    break;
                case 4: //Skeleton Animations
                    enemy1Anim.SetTrigger (animName);
                    break;
                case 5: //Undead Animations
                    enemy2Anim.SetTrigger (animName);
                    break;
            }
        }
    }
}