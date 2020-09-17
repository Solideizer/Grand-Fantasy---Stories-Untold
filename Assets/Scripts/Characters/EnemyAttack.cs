using UnityEngine;
using System.Collections;
using TMPro;

public class EnemyAttack : Unit{
	
#pragma warning disable 0649
	[SerializeField] private GameObject enemyGO;
#pragma warning restore 0649
	private Enemy2 enemy2Class;
	private Vector2 _enemyStartingPosition;
	protected override void Awake()
	{
		base.Awake();
		enemy2Class = GetComponent<Enemy2>();
	}

	public IEnumerator EnemyTurn()
	{
		_enemyStartingPosition = enemyGO.transform.position;
		int randomPlayerUnitIndex = Random.Range(0, BattleSystemClass.playerUnitGOs.Count);
		GameObject _attackedPlayerGO = BattleSystemClass.playerUnitGOs[randomPlayerUnitIndex];

		AnimationManager.PlayAnim("Dash", 4);
		enemyGO.transform.Translate(-Vector3.right * 800f * Time.deltaTime);

		if (Vector3.Distance(_attackedPlayerGO.transform.position, enemyGO.transform.position) < unitData.errorDistance)
		{
			_enemyStartingPosition = _attackedPlayerGO.transform.position;
		}

		yield return new WaitForSeconds(0.5f);
		AnimationManager.PlayAnim("Attack", 4);
		AudioManager.PlaySound("basicAttack");
		AnimationManager.PlayAnim("Hit", randomPlayerUnitIndex);

		//calculate damage
		float damageDone = CalculationManager.CalculateDamage(unitData);
		bool isDead = CalculationManager.TakeDamage(damageDone, _attackedPlayerGO.GetComponent<Unit>());

		//damagePopup
		GameObject cloneTextGO = Instantiate(unitData.floatingDamagePrefab, _attackedPlayerGO.transform.position + unitData.damageOffset, Quaternion.identity);
		TextMeshPro damageText = unitData.floatingDamagePrefab.GetComponent<TextMeshPro>();

		UIManager.DamagePopup(damageDone, unitData, cloneTextGO);

		UIManager.SetPlayerUnitHP(_attackedPlayerGO.GetComponent<Unit>().currentHp, randomPlayerUnitIndex);
		AudioManager.PlaySound("hurtSound");

		yield return new WaitForSeconds(0.5f);
		AnimationManager.PlayAnim("Idle", randomPlayerUnitIndex);

		enemyGO.transform.position = _enemyStartingPosition;
		AnimationManager.PlayAnim("Idle", 4);

		if (isDead)
		{
			AudioManager.PlaySound("KnightDeath");
			BattleSystemClass.gameState = GameState.LOST;
			BattleSystemClass.EndBattle();
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