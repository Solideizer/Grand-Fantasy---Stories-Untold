using Managers;
using UnityEngine;

namespace Characters_Skills
{
    public class Unit : MonoBehaviour
    {
        #region Variable Declarations

        public UnitData unitData;

        protected AnimationManager AnimationManager;
        protected UIManager UIManager;
        protected BattleSystem BattleSystemClass;
        protected CalculationManager CalculationManager;
        protected TurnManager TurnManager;
        protected Transform CameraTransform; 
        //protected CameraManager CameraManager;

        #endregion

        protected virtual void Awake ()
        {
            AnimationManager = GetComponent<AnimationManager> ();
            UIManager = GetComponent<UIManager> ();
            BattleSystemClass = GetComponent<BattleSystem> ();
            CalculationManager = GetComponent<CalculationManager> ();
            TurnManager = GetComponent<TurnManager> ();
            if (!(Camera.main is null)) CameraTransform = Camera.main.transform;
            
            //CameraManager = GetComponent<CameraManager>();
            unitData.currentHp = unitData.maxHp;

        }
    }
}