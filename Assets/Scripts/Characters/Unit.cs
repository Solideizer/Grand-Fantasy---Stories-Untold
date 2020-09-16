using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitData unitData;
    public float currentHp;
    
    //*********** MANAGERS ***********************
    protected AnimationManager AnimationManager;
    protected UIManager UIManager;
    protected BattleSystem BattleSystemClass;
    protected CalculationManager CalculationManager;
    //*********** END MANAGERS ********************
    
    protected virtual void Awake()
    {
        AnimationManager = GetComponent<AnimationManager>();
        UIManager = GetComponent<UIManager>();
        BattleSystemClass = GetComponent<BattleSystem>();
        CalculationManager = GetComponent<CalculationManager>();
    }
}
