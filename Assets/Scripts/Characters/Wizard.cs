using System;
using TMPro;
using System.Collections;
using UnityEngine;

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
		AnimationManager.PlayAnim("Attack1", 2);

		Vector3 enemyPos = new Vector3(4.05f, -3.28f, 5f);
		GameObject explosionGO = Instantiate(explosionAnim, enemyPos, Quaternion.identity);
		Destroy(explosionGO, 1f);

		//calculate damage        
		float damageDone = CalculationManager.CalculateDamage(unitData);
		bool isDead = CalculationManager.TakeDamage(damageDone, enemyToAttackGO.GetComponent<Unit>());

		//damagePopup
		GameObject floatingDamageGO = Instantiate(unitData.floatingDamagePrefab, enemyToAttackGO.transform.position + unitData.damageOffset,
			Quaternion.identity);
		UIManager.DamagePopup(damageDone, unitData, floatingDamageGO);

		UIManager.SetEnemyHP(enemyToAttackGO.GetComponent<Unit>().currentHp, enemyID);

		AnimationManager.PlayAnim("Hit", enemyID);
		AudioManager.PlaySound("hurtSound");
		yield return new WaitForSeconds(0.5f);
		AnimationManager.PlayAnim("Idle", enemyID);
		AnimationManager.PlayAnim("Idle", 2);

		if (isDead)
		{
			AnimationManager.PlayAnim("Dead", enemyID);
			BattleSystemClass.gameState = GameState.WON;
			BattleSystemClass.EndBattle();
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