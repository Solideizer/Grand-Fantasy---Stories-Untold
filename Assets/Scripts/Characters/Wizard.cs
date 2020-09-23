using System.Collections;
using Managers;
using UnityEngine;

namespace Characters
{
	public class Wizard : Unit
	{
#pragma warning disable 0649
		[SerializeField] private GameObject wizardGO;
#pragma warning restore 0649

		public GameObject explosionAnim;
		private EnemyAttack enemyAttackClass;
		protected override void Awake()
		{
			base.Awake();
			enemyAttackClass = GetComponent<EnemyAttack>();
		}

		private IEnumerator WizardAttack1(int enemyID, GameObject enemyToAttackGO)
		{
			Unit enemyToAttackUnit = enemyToAttackGO.GetComponent<Unit>();
			Vector3 enemyPos = enemyToAttackGO.transform.position;
		
			BattleSystemClass.gameState = GameState.WAITING;
			AnimationManager.PlayAnim("Attack1", 2);

			GameObject explosionGO = Instantiate(explosionAnim, enemyPos, Quaternion.identity);
			Destroy(explosionGO, 1f);

			//calculate damage        
			float damageDone = CalculationManager.CalculateDamage(unitData);
			bool isDead = CalculationManager.TakeDamage(damageDone, enemyToAttackUnit);

			//damagePopup
			GameObject floatingDamageGO = Instantiate(unitData.floatingDamagePrefab, enemyPos + unitData.damageOffset,
				Quaternion.identity);
			UIManager.DamagePopup(damageDone, unitData, floatingDamageGO,enemyToAttackGO);

			UIManager.SetEnemyHp(enemyToAttackUnit.currentHp, enemyID);

			AnimationManager.PlayAnim("Hit", enemyID);
			AudioManager.PlaySound("hurtSound");
			yield return new WaitForSeconds(0.5f);
			AnimationManager.PlayAnim("Idle", enemyID);
			AnimationManager.PlayAnim("Idle", 2);

			if (isDead)
			{
				AnimationManager.PlayAnim("Dead", enemyID);
			
			}
			else
			{
				BattleSystemClass.gameState = GameState.ENEMYTURN;
				BattleSystemClass.unitState = UnitState.ENEMY1;
				yield return new WaitForSeconds(1f);
				StartCoroutine(enemyAttackClass.EnemyTurn());
			}
		}

		public void WizardAttack(int enemyID, GameObject enemyToAttackGO)
		{
			StartCoroutine(WizardAttack1(enemyID, enemyToAttackGO));
		}
	}
}