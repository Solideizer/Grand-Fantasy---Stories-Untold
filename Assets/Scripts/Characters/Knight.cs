using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Knight : Unit
{
#pragma warning disable 0649
	[SerializeField] private GameObject knightGO;
#pragma warning restore 0649
	

	private IEnumerator KnightBasicAttack(int enemyID, GameObject enemyToAttackGO)
	{
		BattleSystemClass.gameState = GameState.WAITING;
		AudioManager.PlaySound("KnightOpeners");

		Vector2 startingPos = knightGO.transform.position;
		AnimationManager.PlayAnim("Dash", 0);

		//Knight.transform.position += Knight.transform.right * (Time.deltaTime * 1000);
		knightGO.transform.Translate(Vector3.right * 800f * Time.deltaTime);

		if (Vector3.Distance(knightGO.transform.position, enemyToAttackGO.transform.position) < unitData.errorDistance)
		{
			knightGO.transform.position = enemyToAttackGO.transform.position;
		}

		yield return new WaitForSeconds(0.5f);
		AnimationManager.PlayAnim("Attack", 0);
		AudioManager.PlaySound("basicAttack");

		//calculate damage        
		float damageDone = CalculationManager.CalculateDamage(unitData);
		bool isDead = CalculationManager.TakeDamage(damageDone, enemyToAttackGO.GetComponent<Unit>());

		//damagePopup
		GameObject cloneTextGO = Instantiate(unitData.floatingDamagePrefab, enemyToAttackGO.transform.position + unitData.damageOffset, Quaternion.identity);
		TextMeshPro damageText = unitData.floatingDamagePrefab.GetComponent<TextMeshPro>();

		UIManager.DamagePopup(damageDone, unitData, cloneTextGO);
		UIManager.SetEnemyHP(enemyToAttackGO.GetComponent<Unit>().currentHp, enemyID);

		AnimationManager.PlayAnim("Hit", enemyID);
		AudioManager.PlaySound("hurtSound");
		yield return new WaitForSeconds(0.5f);
		AnimationManager.PlayAnim("Idle", enemyID);
		knightGO.transform.position = startingPos;
		AnimationManager.PlayAnim("Idle", 0);

		if (isDead)
		{
			AnimationManager.PlayAnim("Dead", enemyID);
			BattleSystemClass.gameState = GameState.WON;
			BattleSystemClass.EndBattle();
		}
		else
		{
			//Warrior's turn starts
			BattleSystemClass.gameState = GameState.PLAYERTURN;
			BattleSystemClass.unitState = UnitState.WARRIOR;
		}
		
	}

	public void KnightAttack(int enemyID, GameObject enemyToAttackGO)
	{
		StartCoroutine(KnightBasicAttack(enemyID, enemyToAttackGO));
	}
}