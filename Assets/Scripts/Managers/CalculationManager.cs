using Characters;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
	public class CalculationManager : MonoBehaviour
	{
		public static bool TakeDamage(float damage, Unit unit)
		{
			unit.currentHp -= (damage * (100 / (100 + unit.unitData._baseArmor)));
			return unit.currentHp <= 0f;
		}

		public float CalculateDamage(UnitData unitData)
		{
			float damage = Random.Range(unitData._baseDamage - (unitData._baseDamage / 10),
				unitData._baseDamage + (unitData._baseDamage / 10));
			damage += CalculateCrit(damage, unitData);
			return damage;
		}

		private static float CalculateCrit(float baseDamage, UnitData unitData)
		{
			float criticalMultiplier = Random.Range(1.5f, 3f);

			if (Random.value <= unitData._criticalStrikeChance)
			{
				float criticalDamage = baseDamage * criticalMultiplier;
				return criticalDamage;
			}
			else
			{
				return 0f;
			}
		}
	}
}