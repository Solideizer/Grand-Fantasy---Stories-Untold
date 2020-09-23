using System.Collections;
using Managers;
using TMPro;
using UnityEngine;

namespace Characters
{
	public class EnemyAttack : Unit{
	
#pragma warning disable 0649
		[SerializeField] private GameObject enemyGO;
#pragma warning restore 0649
		private Enemy2 enemy2Class;
		private Vector2 _enemyStartingPosition;
		private Transform camTransform;
		protected override void Awake()
		{
			base.Awake();
			enemy2Class = GetComponent<Enemy2>();
			camTransform = Camera.main.transform;
		}

		public IEnumerator EnemyTurn()
		{
			_enemyStartingPosition = enemyGO.transform.position;
			int randomPlayerUnitIndex = Random.Range(0, BattleSystemClass.playerUnitGOs.Count);
			GameObject attackedPlayerGO = BattleSystemClass.playerUnitGOs[randomPlayerUnitIndex];
			Unit attackedPlayerUnit = attackedPlayerGO.GetComponent<Unit>();
			Vector3 playerPos = attackedPlayerGO.transform.position;

			AnimationManager.PlayAnim("Dash", 4);
		
			enemyGO.transform.Translate(- Time.deltaTime * 1000, 0, 0, camTransform);
			if (Vector3.Distance(enemyGO.transform.position, playerPos) < unitData.errorDistance)
			{
				enemyGO.transform.position = new Vector3(playerPos.x + 2f,playerPos.y,playerPos.z);
			}

			yield return new WaitForSeconds(0.5f);
			AnimationManager.PlayAnim("Attack", 4);
			AudioManager.PlaySound("basicAttack");
			AnimationManager.PlayAnim("Hit", randomPlayerUnitIndex);

			//calculate damage
			float damageDone = CalculationManager.CalculateDamage(unitData);
			bool isDead = CalculationManager.TakeDamage(damageDone, attackedPlayerUnit);

			//damagePopup
			GameObject cloneTextGO = Instantiate(unitData.floatingDamagePrefab, playerPos + unitData.damageOffset, Quaternion.identity);
			TextMeshPro damageText = unitData.floatingDamagePrefab.GetComponent<TextMeshPro>();

			UIManager.DamagePopup(damageDone, unitData, cloneTextGO,attackedPlayerGO);

			UIManager.SetPlayerUnitHp(attackedPlayerUnit.currentHp, randomPlayerUnitIndex);
			AudioManager.PlaySound("hurtSound");

			yield return new WaitForSeconds(0.5f);
			AnimationManager.PlayAnim("Idle", randomPlayerUnitIndex);

			enemyGO.transform.position = _enemyStartingPosition;
			AnimationManager.PlayAnim("Idle", 4);

			if (isDead)
			{
				AudioManager.PlaySound("KnightDeath");
				AnimationManager.PlayAnim("Dead",randomPlayerUnitIndex);
			
			}
			else
			{
				BattleSystemClass.gameState = GameState.ENEMYTURN;
				BattleSystemClass.unitState = UnitState.ENEMY2;
				yield return new WaitForSeconds(0.5f);
				StartCoroutine(enemy2Class.Enemy2Turn());
			}
		}

	}
}