using System.Collections;
using TMPro;
using UnityEngine;

public class Warrior : MonoBehaviour
{
#pragma warning disable 0649
	[SerializeField] private GameObject warriorGO;
	[SerializeField] private GameObject floatingDamage;
	[SerializeField] private Vector3 damageOffset = new Vector3(0f, 3f, 0f);
#pragma warning restore 0649

	private AnimationManager _animationManager;
	private UIManager _uiManager;
	private BattleSystem _battleSystemClass;
	private CalculationManager _calculationManager;

	private UnitData _warriorUnitData;
	private const float ErrorDistance = 15f;

	private void Awake()
	{
		_warriorUnitData = warriorGO.GetComponent<Unit>().unitData;
		_animationManager = GetComponent<AnimationManager>();
		_uiManager = GetComponent<UIManager>();
		_battleSystemClass = GetComponent<BattleSystem>();
		_calculationManager = GetComponent<CalculationManager>();
	}

	private IEnumerator WarriorBasicAttack(int enemyID, GameObject enemyToAttackGO)
	{
		Vector2 startingPos = warriorGO.transform.position;
		//play dashing animation
		_animationManager.PlayAnim("Dash", 1);

		warriorGO.transform.Translate(Vector3.right);

		if (Vector3.Distance(warriorGO.transform.position, enemyToAttackGO.transform.position) < ErrorDistance)
		{
			warriorGO.transform.position = enemyToAttackGO.transform.position;
		}

		yield return new WaitForSeconds(0.5f);
		_animationManager.PlayAnim("Attack", 1);
		AudioManager.PlaySound("basicAttack");

		//calculate damage        
		float damageDone = _calculationManager.CalculateDamage(_warriorUnitData);
		bool isDead = _calculationManager.TakeDamage(damageDone, enemyToAttackGO.GetComponent<Unit>());

		//damagePopup
		GameObject floatingDamageGO = Instantiate(floatingDamage, enemyToAttackGO.transform.position + damageOffset,
			Quaternion.identity);
		TextMeshPro damageText = floatingDamage.GetComponent<TextMeshPro>();
		_uiManager.DamagePopup(damageDone, _warriorUnitData, damageText);

		_uiManager.SetEnemyHP(enemyToAttackGO.GetComponent<Unit>().currentHp, enemyID);

		_animationManager.PlayAnim("Hit", enemyID);
		AudioManager.PlaySound("hurtSound");
		yield return new WaitForSeconds(1f);
		_animationManager.PlayAnim("Idle", enemyID);
		warriorGO.transform.position = startingPos;
		_animationManager.PlayAnim("Idle", 1);

		if (isDead)
		{
			_animationManager.PlayAnim("Dead", enemyID);
			_battleSystemClass.gameState = GameState.WON;
			_battleSystemClass.EndBattle();
		}
		else
		{
			_battleSystemClass.unitState = UnitState.WIZARD;
			_uiManager.ShowSkillHUD();
		}
	}

	public void WarriorAttack(int enemyID, GameObject enemyToAttackGO)
	{
		StartCoroutine(WarriorBasicAttack(enemyID, enemyToAttackGO));
	}
}