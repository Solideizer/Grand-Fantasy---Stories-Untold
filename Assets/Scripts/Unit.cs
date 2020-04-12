using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    //public int unitLevel;

    public float baseDamage;
    private float damage;
    public float maxHP;
    public float currentHP;
    public float baseArmor;
    public float criticalStrikeChance;

    private float criticalMultiplier;
    private float criticalDamage;
    
    public bool TakeDamage(float damage)
    {
        
       currentHP = currentHP - (damage * (100 / (100 + baseArmor)));

       if(currentHP <= 0f)
       {           
           return true;
       }else
       {
           return false;
       }
    }    

    public float calculateDamage()
    {
        damage = Random.Range(baseDamage - 5f, baseDamage + 5f);
        damage += calculateCrit();
        return damage;
        
    }
    public float calculateCrit()
    {
        criticalMultiplier = Random.Range(1.5f, 3f);
        if (Random.value <= criticalStrikeChance)
        {
            criticalDamage = damage * criticalMultiplier;
            return criticalDamage;
        }
        else
            return 0f;
    }
}
