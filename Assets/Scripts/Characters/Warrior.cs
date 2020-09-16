using System.Collections;
using TMPro;
using UnityEngine;

public class Warrior : Unit
{
#pragma warning disable 0649
	[SerializeField] private GameObject warriorGO;
#pragma warning restore 0649

	
	private IEnumerator WarriorBasicAttack(int enemyID, GameObject enemyToAttackGO)
	{
		Vector2 startingPos = warriorGO.transform.position;
		//play dashing animation
		AnimationManager.PlayAnim("Dash", 1);

		warriorGO.transform.Translate(Vector3.right * 800f * Time.deltaTime);

		if (Vector3.Distance(warriorGO.transform.position, enemyToAttackGO.transform.position) < unitData.errorDistance)
		{
			warriorGO.transform.position = enemyToAttackGO.transform.position;
		}

		yield return new WaitForSeconds(0.5f);
		AnimationManager.PlayAnim("Attack", 1);
		AudioManager.PlaySound("basicAttack");

		//calculate damage        
		float damageDone = CalculationManager.CalculateDamage(unitData);
		bool isDead = CalculationManager.TakeDamage(damageDone, enemyToAttackGO.GetComponent<Unit>());

		//damagePopup
		GameObject floatingDamageGO = Instantiate(unitData.floatingDamagePrefab, enemyToAttackGO.transform.position + unitData.damageOffset,
			Quaternion.identity);
		TextMeshPro damageText = unitData.floatingDamagePrefab.GetComponent<TextMeshPro>();
		UIManager.DamagePopup(damageDone, unitData, floatingDamageGO);

		UIManager.SetEnemyHP(enemyToAttackGO.GetComponent<Unit>().currentHp, enemyID);

		AnimationManager.PlayAnim("Hit", enemyID);
		AudioManager.PlaySound("hurtSound");
		yield return new WaitForSeconds(1f);
		AnimationManager.PlayAnim("Idle", enemyID);
		warriorGO.transform.position = startingPos;
		AnimationManager.PlayAnim("Idle", 1);

		if (isDead)
		{
			AnimationManager.PlayAnim("Dead", enemyID);
			BattleSystemClass.gameState = GameState.WON;
			BattleSystemClass.EndBattle();
		}
		else
		{
			BattleSystemClass.unitState = UnitState.WIZARD;
		}
	}

	public void WarriorAttack(int enemyID, GameObject enemyToAttackGO)
	{
		StartCoroutine(WarriorBasicAttack(enemyID, enemyToAttackGO));
	}
}