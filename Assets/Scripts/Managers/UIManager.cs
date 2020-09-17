using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
	public BattleSystem battleSystemScript;

#pragma warning disable 0649
	[SerializeField] private TextMeshProUGUI knightNameText;
	[SerializeField] private TextMeshProUGUI warriorNameText;
	[SerializeField] private TextMeshProUGUI wizardNameText;
	[SerializeField] private TextMeshProUGUI enemy1NameText;
	[SerializeField] private TextMeshProUGUI enemy2NameText;
	[SerializeField] private Slider knightHpSlider;
	[SerializeField] private Slider warriorHpSlider;
	[SerializeField] private Slider wizardHpSlider;
	[SerializeField] private Slider enemy1HpSlider;
	[SerializeField] private Slider enemy2HpSlider;
#pragma warning restore 0649

	private CanvasGroup _skillHud;
	
	//*********************************** HP SETTERS **************************************
	public void SetPlayerUnitHP(float hp, int playerUnitID)
	{
		switch (playerUnitID)
		{
			case 0:
				knightHpSlider.value = hp;
				break;
			case 1:
				warriorHpSlider.value = hp;
				break;
			case 2:
				wizardHpSlider.value = hp;
				break;
		}
	}

	public void SetEnemyHP(float hp, int enemyID)
	{
		switch (enemyID)
		{
			case 4:
				enemy1HpSlider.value = hp;
				break;
			case 5:
				enemy2HpSlider.value = hp;
				break;
		}
	}
	//*********************************** HP SETTERS END ************************************

	
	public void SetPlayerHUD()
	{
		//Knight
		knightNameText.text = battleSystemScript.playerUnits[0].name;
		knightHpSlider.maxValue = battleSystemScript.playerUnits[0].unitData._maxHp;
		knightHpSlider.value = battleSystemScript.playerUnits[0].currentHp;

		//Warrior
		warriorNameText.text = battleSystemScript.playerUnits[1].name;
		warriorHpSlider.maxValue = battleSystemScript.playerUnits[1].unitData._maxHp;
		warriorHpSlider.value = battleSystemScript.playerUnits[1].currentHp;

		//Wizard
		wizardNameText.text = battleSystemScript.playerUnits[2].name;
		wizardHpSlider.maxValue = battleSystemScript.playerUnits[2].unitData._maxHp;
		wizardHpSlider.value = battleSystemScript.playerUnits[2].currentHp;
	}

	private void SetEnemyHUD()
	{
		// Enemy1
		enemy1NameText.text = battleSystemScript.enemyUnits[0].name;
		enemy1HpSlider.maxValue = battleSystemScript.enemyUnits[0].unitData._maxHp;
		enemy1HpSlider.value = battleSystemScript.enemyUnits[0].currentHp;

		// Enemy2
		enemy2NameText.text = battleSystemScript.enemyUnits[1].name;
		enemy2HpSlider.maxValue = battleSystemScript.enemyUnits[1].unitData._maxHp;
		enemy2HpSlider.value = battleSystemScript.enemyUnits[1].currentHp;
	}

	//************************************* Skill HUDs & UI END *****************************
	public void DamagePopup(float damageDone, UnitData unitData, GameObject cloneTextGO)
	{
		TextMeshPro damageText = cloneTextGO.GetComponent<TextMeshPro>();

		if (damageDone > (unitData._baseDamage + (unitData._baseDamage / 10)))
		{
			Color newColor = new Color(1f, 0.1949452f, 0.145098f, 1f);
			damageText.fontSize = 500f;
			damageText.color = newColor;
			damageText.text = damageDone.ToString("F0");
			CameraShake.Instance.ShakeCamera(3f,0.5f);
			Destroy (cloneTextGO, 2f);
		}
		else
		{
			Color newColor = new Color(1f,0.8182157f,0.145098f,1f);
			damageText.color = newColor;
			CameraShake.Instance.ShakeCamera(1f,0.5f);
			damageText.text = damageDone.ToString("F0");
			Destroy (cloneTextGO, 1f);
		}
	}

	private void Start()
	{
		StartCoroutine(SetupBattle());
	}

	IEnumerator SetupBattle()
	{
		yield return new WaitForSeconds(0.1f); // waiting for BattleSystem class
		SetPlayerUnitHP(battleSystemScript.playerUnits[0].unitData._maxHp, 0);
		SetPlayerUnitHP(battleSystemScript.playerUnits[1].unitData._maxHp, 1);
		SetPlayerUnitHP(battleSystemScript.playerUnits[2].unitData._maxHp, 2);

		SetEnemyHP(battleSystemScript.enemyUnits[0].unitData._maxHp, 4);
		SetEnemyHP(battleSystemScript.enemyUnits[1].unitData._maxHp, 5);

		SetPlayerHUD();
		SetEnemyHUD();

	}
}