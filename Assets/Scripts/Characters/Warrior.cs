using System.Collections;
using Managers;
using TMPro;
using UnityEngine;

namespace Characters
{
	public class Warrior : Unit
	{
#pragma warning disable 0649
		[SerializeField] private GameObject warriorGO;
#pragma warning restore 0649
		private Transform camTransform;
		private void Start()
		{
			camTransform = Camera.main.transform;
		}

		private IEnumerator WarriorBasicAttack(int enemyID, GameObject enemyToAttackGO)
		{
			Unit enemyToAttackUnit = enemyToAttackGO.GetComponent<Unit>();
			Vector3 enemyPos = enemyToAttackGO.transform.position;
		
			BattleSystemClass.gameState = GameState.WAITING;
			Vector2 startingPos = warriorGO.transform.position;
			AnimationManager.PlayAnim("Dash", 1);

			warriorGO.transform.Translate(Time.deltaTime * 1000, 0, 0, camTransform);
			if (Vector3.Distance(warriorGO.transform.position, enemyPos) < unitData.errorDistance)
			{
				warriorGO.transform.position = new Vector3(enemyPos.x - 2f,enemyPos.y,enemyPos.z);
			}

			yield return new WaitForSeconds(0.5f);
			AnimationManager.PlayAnim("Attack", 1);
			AudioManager.PlaySound("basicAttack");

			//calculate damage        
			float damageDone = CalculationManager.CalculateDamage(unitData);
			bool isDead = CalculationManager.TakeDamage(damageDone, enemyToAttackUnit);

			//damagePopup
			GameObject floatingDamageGO = Instantiate(unitData.floatingDamagePrefab, enemyPos + unitData.damageOffset,
				Quaternion.identity);
			TextMeshPro damageText = unitData.floatingDamagePrefab.GetComponent<TextMeshPro>();
			UIManager.DamagePopup(damageDone, unitData, floatingDamageGO, enemyToAttackGO);

			UIManager.SetEnemyHp(enemyToAttackUnit.currentHp, enemyID);

			AnimationManager.PlayAnim("Hit", enemyID);
			AudioManager.PlaySound("hurtSound");
			yield return new WaitForSeconds(1f);
			AnimationManager.PlayAnim("Idle", enemyID);
			warriorGO.transform.position = startingPos;
			AnimationManager.PlayAnim("Idle", 1);

			if (isDead)
			{
				AnimationManager.PlayAnim("Dead", enemyID);
			}
			else
			{
				BattleSystemClass.gameState = GameState.PLAYERTURN;
				BattleSystemClass.unitState = UnitState.WIZARD;
			}
		}

		public void WarriorAttack(int enemyID, GameObject enemyToAttackGO)
		{
			StartCoroutine(WarriorBasicAttack(enemyID, enemyToAttackGO));
		}
	}
}