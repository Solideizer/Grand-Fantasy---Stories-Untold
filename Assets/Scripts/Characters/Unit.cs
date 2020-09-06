using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {    
     
    public float maxHP;
    public float currentHP;
    public float baseDamage; 
    public float baseArmor;
    public float criticalStrikeChance;
    private float _damage;
    private float _criticalMultiplier;
    private float _criticalDamage;

    public bool TakeDamage (float damage) 
    {
        currentHP = currentHP - (damage * (100 / (100 + baseArmor)));

        if (currentHP <= 0f) 
        {
            return true;
        }else{
            return false;
        }
    }

    public float calculateDamage () 
    {
        _damage = Random.Range (baseDamage - (baseDamage / 10), baseDamage + (baseDamage / 10));
        _damage += calculateCrit ();
        return _damage;
    }
    public float calculateCrit () {

        _criticalMultiplier = Random.Range (1.5f, 3f);

        if (Random.value <= criticalStrikeChance)
        {
            _criticalDamage = _damage * _criticalMultiplier;
            return _criticalDamage;            
        }else{
            return 0f;
        }

    }
}