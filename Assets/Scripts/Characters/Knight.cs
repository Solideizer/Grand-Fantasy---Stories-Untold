using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Knight : MonoBehaviour
{
#pragma warning disable 0649
	[SerializeField] private GameObject knightGO;
	[SerializeField] private GameObject floatingDamage;
	[SerializeField] private Vector3 damageOffset = new Vector3(0f, 3f, 0f);
#pragma warning restore 0649

	//*********** MANAGERS ***********************
	private AnimationManager _animationManager;
	private UIManager _uiManager;
	private BattleSystem _battleSystemClass;

	private CalculationManager _calculationManager;
	//*********** END MANAGERS ********************

	private UnitData _knightUnitData;
	private float errorDistance = 15f;

	private void Awake()
	{
		_knightUnitData = knightGO.GetComponent<Unit>().unitData;
		_animationManager = GetComponent<AnimationManager>();
		_uiManager = GetComponent<UIManager>();
		_battleSystemClass = GetComponent<BattleSystem>();
		_calculationManager = GetComponent<CalculationManager>();
	}

	public IEnumerator KnightBasicAttack(int enemyID, GameObject enemyToAttackGO)
	{
		AudioManager.PlaySound("KnightOpeners");

		Vector2 startingPos = knightGO.transform.position;
		_animationManager.PlayAnim("Dash", 0);

		//Knight.transform.position += Knight.transform.right * (Time.deltaTime * 1000);
		knightGO.transform.Translate(Vector3.right);

		if (Vector3.Distance(knightGO.transform.position, enemyToAttackGO.transform.position) < errorDistance)
		{
			knightGO.transform.position = enemyToAttackGO.transform.position;
		}

		yield return new WaitForSeconds(0.5f);
		_animationManager.PlayAnim("Attack", 0);
		AudioManager.PlaySound("basicAttack");

		//calculate damage        
		float damageDone = _calculationManager.CalculateDamage(_knightUnitData);
		Debug.Log(damageDone);
		bool isDead = _calculationManager.TakeDamage(damageDone, enemyToAttackGO.GetComponent<Unit>());

		//damagePopup
		Instantiate(floatingDamage, enemyToAttackGO.transform.position + damageOffset, Quaternion.identity);
		TextMeshPro damageText = floatingDamage.GetComponent<TextMeshPro>();

		_uiManager.DamagePopup(damageDone, _knightUnitData, damageText);
		Debug.Log(damageText.text);

		_uiManager.SetEnemyHP(enemyToAttackGO.GetComponent<Unit>().currentHp, enemyID);


		_animationManager.PlayAnim("Hit", enemyID);
		AudioManager.PlaySound("hurtSound");
		yield return new WaitForSeconds(0.5f);
		_animationManager.PlayAnim("Idle", enemyID);
		knightGO.transform.position = startingPos;
		_animationManager.PlayAnim("Idle", 0);

		if (isDead)
		{
			_animationManager.PlayAnim("Dead", enemyID);
			_battleSystemClass.gameState = GameState.WON;
			_battleSystemClass.EndBattle();
		}
		else
		{
			//Warrior's turn starts
			_battleSystemClass.unitState = UnitState.WARRIOR;
		}
	}

	public void KnightAttack(int enemyID, GameObject enemyToAttackGO)
	{
		StartCoroutine(KnightBasicAttack(enemyID, enemyToAttackGO));
	}
}