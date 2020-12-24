using System;
using System.Collections;
using Managers;
using UnityEngine;

namespace Characters_Skills
{
	public class Warrior : Unit
	{
#pragma warning disable 0649
		[SerializeField] private GameObject warriorGO;
#pragma warning restore 0649

		public static event Action<int> UnitDied = delegate { };

		public IEnumerator WarriorBasicAttack (int enemyID, GameObject enemyToAttackGO)
		{
			Unit enemyToAttackUnit = enemyToAttackGO.GetComponent<Unit> ();
			Vector3 enemyPos = enemyToAttackGO.transform.position;

			BattleSystemClass.gameState = GameState.WAITING;
			Vector2 startingPos = warriorGO.transform.position;
			AnimationManager.PlayAnim ("Dash", 1);

			warriorGO.transform.Translate (Time.deltaTime * 1000, 0, 0, CameraTransform);
			if (Vector3.Distance (warriorGO.transform.position, enemyPos) < unitData.errorDistance)
			{
				warriorGO.transform.position = new Vector3 (enemyPos.x - 2f, enemyPos.y, enemyPos.z);
			}

			yield return new WaitForSeconds (0.5f);
			AnimationManager.PlayAnim ("Attack", 1);
			AudioManager.PlaySound ("basicAttack");
			//StartCoroutine(CameraManager.MoveTowardsTarget(enemyToAttackGO));
			//calculate damage        
			float damageDone = CalculationManager.CalculateDamage (unitData);
			bool isDead = CalculationManager.DealDamage (damageDone, enemyToAttackUnit);

			//damagePopup
			GameObject floatingDamageGO = Instantiate (unitData.floatingDamagePrefab, enemyPos + unitData.damageOffset,
				Quaternion.identity);
			UIManager.DamagePopup (damageDone, unitData, floatingDamageGO);
			UIManager.SetEnemyHp (enemyToAttackUnit.unitData.currentHp, enemyID);

			AnimationManager.PlayAnim ("Hit", enemyID);
			AudioManager.PlaySound ("hurtSound");
			yield return new WaitForSeconds (1f);
			AnimationManager.PlayAnim ("Idle", enemyID);
			warriorGO.transform.position = startingPos;
			AnimationManager.PlayAnim ("Idle", 1);

			if (isDead)
			{
				AnimationManager.PlayAnim ("Dead", enemyID);
				UnitDied (enemyID);

			}

			BattleSystemClass.gameState = GameState.PLAYERTURN;
			BattleSystemClass.unitState = UnitState.WIZARD;
			UIManager.DisableWarriorSkillBar ();
			UIManager.EnableWizardSkillBar ();

		}

	}
}