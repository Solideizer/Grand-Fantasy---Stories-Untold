using Managers;
using UnityEngine;

namespace Characters
{
    public class Unit : MonoBehaviour
    {
        #region Variable Declarations
    
        public UnitData unitData;
        public float currentHp;

        protected AnimationManager AnimationManager;
        protected UIManager UIManager;
        protected BattleSystem BattleSystemClass;
        protected CalculationManager CalculationManager;
        //protected CameraManager CameraManager;
    
        #endregion
    
        protected virtual void Awake()
        {
            AnimationManager = GetComponent<AnimationManager>();
            UIManager = GetComponent<UIManager>();
            BattleSystemClass = GetComponent<BattleSystem>();
            CalculationManager = GetComponent<CalculationManager>();
            //CameraManager = GetComponent<CameraManager>();
        }
    }
}
