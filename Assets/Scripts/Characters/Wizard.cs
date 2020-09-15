using TMPro;
using System.Collections;
using UnityEngine;

public class Wizard : MonoBehaviour
{
#pragma warning disable 0649
	[SerializeField] private GameObject wizardGO;
	[SerializeField] private GameObject floatingDamage;
	[SerializeField] private Vector3 damageOffset = new Vector3(0f, 3f, 0f);
#pragma warning restore 0649

	private AnimationManager _animationManager;
	private UIManager uiManager;
	private UnitData _wizardUnitData;
	private BattleSystem battleSystemClass;
	private CalculationManager _calculationManager;

	public GameObject explosionAnim;

	private void Awake()
	{
		_animationManager = GetComponent<AnimationManager>();
		uiManager = GetComponent<UIManager>();
		_wizardUnitData = wizardGO.GetComponent<Unit>().unitData;
		battleSystemClass = GetComponent<BattleSystem>();
		_calculationManager = GetComponent<CalculationManager>();
	}

	private IEnumerator WizardAttack1(int enemyID, GameObject enemyToAttackGO)
	{
		_animationManager.PlayAnim("Attack1", 2);

		Vector3 enemyPos = new Vector3(4.05f, -3.28f, 5f);
		GameObject explosionGO = Instantiate(explosionAnim, enemyPos, Quaternion.identity);
		Destroy(explosionGO, 1f);

		//calculate damage        
		float damageDone = _calculationManager.CalculateDamage(_wizardUnitData);
		bool isDead = _calculationManager.TakeDamage(damageDone, enemyToAttackGO.GetComponent<Unit>());

		//damagePopup
		GameObject floatingDamageGO = Instantiate(floatingDamage, enemyToAttackGO.transform.position + damageOffset,
			Quaternion.identity);
		TextMeshPro damageText = floatingDamage.GetComponent<TextMeshPro>();
		uiManager.DamagePopup(damageDone, _wizardUnitData, damageText);

		// update hp
		uiManager.SetEnemyHP(enemyToAttackGO.GetComponent<Unit>().currentHp, enemyID);

		//enemy hit animation
		_animationManager.PlayAnim("Hit", enemyID);
		//enemy hurt sound
		AudioManager.PlaySound("hurtSound");
		yield return new WaitForSeconds(0.5f);
		//enemy goes back to idle animation
		_animationManager.PlayAnim("Idle", enemyID);
		//unitData goes back to idle animation
		_animationManager.PlayAnim("Idle", 2);

		uiManager.HideSkillHUD();

		if (isDead)
		{
			_animationManager.PlayAnim("Dead", enemyID);
			battleSystemClass.gameState = GameState.WON;
			battleSystemClass.EndBattle();
		}
		else
		{
			//Enemy turn
			uiManager.HideSkillHUD();
			battleSystemClass.gameState = GameState.ENEMYTURN;
			battleSystemClass.unitState = UnitState.ENEMY1;
			yield return new WaitForSeconds(1f);
			StartCoroutine(battleSystemClass.EnemyTurn());
		}
	}

	public void WizardAttack(int enemyID, GameObject enemyToAttackGO)
	{
		StartCoroutine(WizardAttack1(enemyID, enemyToAttackGO));
	}
}