using System;
using System.Collections;
using Managers;
using TMPro;
using UnityEngine;

namespace Characters_Skills
{
	public class Knight : Unit
	{
		#region Variable Declarations		

#pragma warning disable 0649
		[SerializeField] private GameObject knightGO;
#pragma warning restore 0649

		public static event Action<int> UnitDied = delegate { };

		#endregion

		public IEnumerator KnightBasicAttack (int enemyID, GameObject enemyToAttackGO)
		{
			Unit attackedEnemyUnit = enemyToAttackGO.GetComponent<Unit> ();
			Vector3 enemyPos = enemyToAttackGO.transform.position;

			BattleSystemClass.gameState = GameState.WAITING;
			AudioManager.PlaySound ("KnightOpeners");

			Vector2 startingPos = knightGO.transform.position;
			AnimationManager.PlayAnim ("Dash", 0);

			knightGO.transform.Translate (Time.deltaTime * 1000, 0, 0, CameraTransform);
			if (Vector3.Distance (knightGO.transform.position, enemyPos) < unitData.errorDistance)
			{
				knightGO.transform.position = new Vector3 (enemyPos.x - 2f, enemyPos.y, enemyPos.z);
			}
			//StartCoroutine(CameraManager.MoveTowardsTarget(enemyToAttackGO));
			yield return new WaitForSeconds (0.5f);
			AnimationManager.PlayAnim ("Attack", 0);
			AudioManager.PlaySound ("basicAttack");

			//calculate damage        
			float damageDone = CalculationManager.CalculateDamage (unitData);
			bool isDead = CalculationManager.DealDamage (damageDone, attackedEnemyUnit);

			//damagePopup
			GameObject cloneTextGO = Instantiate (unitData.floatingDamagePrefab, enemyPos + unitData.damageOffset,
				Quaternion.identity);

			UIManager.DamagePopup (damageDone, unitData, cloneTextGO);
			UIManager.SetEnemyHp (attackedEnemyUnit.unitData.currentHp, enemyID);

			AnimationManager.PlayAnim ("Hit", enemyID);
			AudioManager.PlaySound ("hurtSound");
			yield return new WaitForSeconds (0.5f);
			AnimationManager.PlayAnim ("Idle", enemyID);
			knightGO.transform.position = startingPos;
			AnimationManager.PlayAnim ("Idle", 0);

			if (isDead)
			{
				//play animation on attacked enemy
				AnimationManager.PlayAnim ("Dead", enemyID);
				//observer.NotifyUnitDeath (enemyID);
				UnitDied (enemyID);
				//check if there is an alive enemy

			}
			//Warrior's turn starts
			BattleSystemClass.gameState = GameState.PLAYERTURN;
			BattleSystemClass.unitState = UnitState.WARRIOR;
			UIManager.DisableKnightSkillBar ();
			UIManager.EnableWarriorSkillBar ();

		}

		public IEnumerator KnightSkill2 ()
		{
			BattleSystemClass.gameState = GameState.WAITING;

			AnimationManager.PlayAnim ("Hit", 0);
			Debug.Log (unitData.baseArmor);
			unitData.baseArmor += 30;
			Instantiate (unitData.floatingDamagePrefab, transform.position, Quaternion.identity);
			TextMeshPro damageText = unitData.floatingDamagePrefab.GetComponent<TextMeshPro> ();
			damageText.SetText ("Defence ++");
			Debug.Log (unitData.baseArmor);
			yield return new WaitForSeconds (0.5f);
			AnimationManager.PlayAnim ("Idle", 0);

			BattleSystemClass.gameState = GameState.PLAYERTURN;
			BattleSystemClass.unitState = UnitState.WARRIOR;
			UIManager.DisableKnightSkillBar ();
			UIManager.EnableWarriorSkillBar ();

		}

	}
}