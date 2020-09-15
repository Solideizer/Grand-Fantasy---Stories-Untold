using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName="Character")]
public class UnitData : ScriptableObject {    
     
    public float _maxHp;
    public float _baseDamage; 
    public float _baseArmor;
    public float _criticalStrikeChance;

}