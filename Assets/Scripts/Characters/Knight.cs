using System.Collections;
using Managers;
using TMPro;
using UnityEngine;

namespace Characters
{
	public class Knight : Unit
	{
		#region Variable Declarations

#pragma warning disable 0649
		[SerializeField] private GameObject knightGO;
#pragma warning restore 0649

		private Transform camTransform;

		#endregion

		private void Start()
		{
			camTransform = Camera.main.transform;
		}

		public IEnumerator KnightBasicAttack(int enemyID, GameObject enemyToAttackGO)
		{
			Unit enemyToAttackUnit = enemyToAttackGO.GetComponent<Unit>();
			Vector3 enemyPos = enemyToAttackGO.transform.position;

			BattleSystemClass.gameState = GameState.WAITING;
			AudioManager.PlaySound("KnightOpeners");

			Vector2 startingPos = knightGO.transform.position;
			AnimationManager.PlayAnim("Dash", 0);

			knightGO.transform.Translate(Time.deltaTime * 1000, 0, 0, camTransform);
			if (Vector3.Distance(knightGO.transform.position, enemyPos) < unitData.errorDistance)
			{
				knightGO.transform.position = new Vector3(enemyPos.x - 2f, enemyPos.y, enemyPos.z);
			}
			//StartCoroutine(CameraManager.MoveTowardsTarget(enemyToAttackGO));
			yield return new WaitForSeconds(0.5f);
			AnimationManager.PlayAnim("Attack", 0);
			AudioManager.PlaySound("basicAttack");

			//calculate damage        
			float damageDone = CalculationManager.CalculateDamage(unitData);
			bool isDead = CalculationManager.TakeDamage(damageDone, enemyToAttackUnit);

			//damagePopup
			GameObject cloneTextGO = Instantiate(unitData.floatingDamagePrefab, enemyPos + unitData.damageOffset,
				Quaternion.identity);
			TextMeshPro damageText = unitData.floatingDamagePrefab.GetComponent<TextMeshPro>();

			UIManager.DamagePopup(damageDone, unitData, cloneTextGO, enemyToAttackGO);
			UIManager.SetEnemyHp(enemyToAttackUnit.currentHp, enemyID);

			AnimationManager.PlayAnim("Hit", enemyID);
			AudioManager.PlaySound("hurtSound");
			yield return new WaitForSeconds(0.5f);
			AnimationManager.PlayAnim("Idle", enemyID);
			knightGO.transform.position = startingPos;
			AnimationManager.PlayAnim("Idle", 0);

			if (isDead)
			{
				AnimationManager.PlayAnim("Dead", enemyID);
			}
			else
			{
				//Warrior's turn starts
				BattleSystemClass.gameState = GameState.PLAYERTURN;
				BattleSystemClass.unitState = UnitState.WARRIOR;
			}
		}

	}
}