using System;
using System.Collections;
using Managers;
using UnityEngine;

namespace Characters
{
	public class Wizard : Unit
	{
		public GameObject explosionAnim;
		private EnemyAttack enemyAttackClass;
		public static event Action<int> UnitDied = delegate { };
		protected override void Awake ()
		{
			base.Awake ();
			enemyAttackClass = GetComponent<EnemyAttack> ();
		}

		public IEnumerator WizardBasicAttack (int enemyID, GameObject enemyToAttackGO)
		{
			Unit enemyToAttackUnit = enemyToAttackGO.GetComponent<Unit> ();
			Vector3 enemyPos = enemyToAttackGO.transform.position;

			BattleSystemClass.gameState = GameState.WAITING;
			AnimationManager.PlayAnim ("Attack1", 2);

			GameObject explosionGO = Instantiate (explosionAnim, enemyPos, Quaternion.identity);
			//StartCoroutine(CameraManager.MoveTowardsTarget(enemyToAttackGO));
			Destroy (explosionGO, 1f);

			//calculate damage        
			float damageDone = CalculationManager.CalculateDamage (unitData);
			bool isDead = CalculationManager.DealDamage (damageDone, enemyToAttackUnit);

			//damagePopup
			GameObject floatingDamageGO = Instantiate (unitData.floatingDamagePrefab, enemyPos + unitData.damageOffset,
				Quaternion.identity);
			UIManager.DamagePopup (damageDone, unitData, floatingDamageGO);

			UIManager.SetEnemyHp (enemyToAttackUnit.unitData._currentHp, enemyID);

			AnimationManager.PlayAnim ("Hit", enemyID);
			AudioManager.PlaySound ("hurtSound");
			yield return new WaitForSeconds (0.5f);
			AnimationManager.PlayAnim ("Idle", enemyID);
			AnimationManager.PlayAnim ("Idle", 2);

			if (isDead)
			{
				AnimationManager.PlayAnim ("Dead", enemyID);
				UnitDied (enemyID);

			}

			BattleSystemClass.gameState = GameState.ENEMYTURN;
			BattleSystemClass.unitState = UnitState.ENEMY1;
			UIManager.DisableWizardSkillBar ();
			yield return new WaitForSeconds (1f);
			StartCoroutine (enemyAttackClass.EnemyTurn ());

		}
		public IEnumerator WizardAreaAttack ()
		{
			Vector3[] enemyPositions = new Vector3[2];
			GameObject[] attackedEnemies = GameObject.FindGameObjectsWithTag ("EnemyUnit");
			for (int i = 0; i < attackedEnemies.Length; i++)
			{
				enemyPositions[i] = attackedEnemies[i].transform.position;
			}

			BattleSystemClass.gameState = GameState.WAITING;
			AnimationManager.PlayAnim ("Attack1", 2);
			for (int i = 0; i < attackedEnemies.Length; i++)
			{
				GameObject explosionGO = Instantiate (explosionAnim, enemyPositions[i], Quaternion.identity);
				Destroy (explosionGO, 1f);
			}

			//calculate damage        
			float damageDone = CalculationManager.CalculateDamage (unitData);
			bool isDead1 = CalculationManager.DealDamage (damageDone, attackedEnemies[0].GetComponent<Unit> ());
			bool isDead2 = CalculationManager.DealDamage (damageDone, attackedEnemies[1].GetComponent<Unit> ());

			//damagePopup
			for (int i = 0; i < attackedEnemies.Length; i++)
			{
				GameObject floatingDamageGO = Instantiate (unitData.floatingDamagePrefab, enemyPositions[i] + unitData.damageOffset,
					Quaternion.identity);

				UIManager.DamagePopup (damageDone, unitData, floatingDamageGO);
			}
			UIManager.SetEnemyHp (attackedEnemies[0].GetComponent<Unit> ().unitData._currentHp, 4);
			UIManager.SetEnemyHp (attackedEnemies[1].GetComponent<Unit> ().unitData._currentHp, 5);

			AnimationManager.PlayAnim ("Hit", 4);
			AnimationManager.PlayAnim ("Hit", 5);
			AudioManager.PlaySound ("hurtSound");
			yield return new WaitForSeconds (0.5f);
			AnimationManager.PlayAnim ("Idle", 4);
			AnimationManager.PlayAnim ("Idle", 5);

			AnimationManager.PlayAnim ("Idle", 2);

			if (isDead1)
			{
				AnimationManager.PlayAnim ("Dead", 4);
				UnitDied (4);
			}
			if (isDead2)
			{
				AnimationManager.PlayAnim ("Dead", 5);
				UnitDied (5);
			}
			else
			{
				BattleSystemClass.gameState = GameState.ENEMYTURN;
				BattleSystemClass.unitState = UnitState.ENEMY1;
				UIManager.DisableWizardSkillBar ();
				yield return new WaitForSeconds (1f);
				StartCoroutine (enemyAttackClass.EnemyTurn ());
			}
		}

	}
}