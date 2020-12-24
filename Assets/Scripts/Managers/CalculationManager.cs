using Characters;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
	public class CalculationManager : MonoBehaviour
	{
		public float CalculateDamage (UnitData unitData)
		{
			float damage = Random.Range (unitData.baseDamage - (unitData.baseDamage / 10),
				unitData.baseDamage + (unitData.baseDamage / 10));
			damage += CalculateCrit (damage, unitData);
			return damage;
		}
		private static float CalculateCrit (float baseDamage, UnitData unitData)
		{
			float criticalMultiplier = Random.Range (1.5f, 3f);

			if (Random.value <= unitData.criticalStrikeChance)
			{
				float criticalDamage = baseDamage * criticalMultiplier;
				return criticalDamage;
			}
			else
			{
				return 0f;
			}
		}
		public bool DealDamage (float damage, Unit unit)
		{
			unit.unitData.currentHp -= (damage * (100 / (100 + unit.unitData.baseArmor)));
			return unit.unitData.currentHp <= 0f;
		}

	}
}