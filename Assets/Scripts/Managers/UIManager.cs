using System;
using System.Collections;
using Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Random = UnityEngine.Random;

namespace Managers
{
	public class UIManager : MonoBehaviour
	{
		#region Variable Declarations

		public BattleSystem battleSystemScript;
		public static event Action<GameObject> CriticalStrikeReceived = delegate { };

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
		[SerializeField] private GameObject floatingDialogue;
#pragma warning restore 0649

		private CanvasGroup _skillHud;

		#endregion

		#region Hp Setters

		public void SetPlayerUnitHp(float hp, int playerUnitID)
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

		public void SetEnemyHp(float hp, int enemyID)
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

		#endregion

		#region UI

		public void SetPlayerHud()
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

		private void SetEnemyHud()
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

		#endregion

		private void Start()
		{
			StartCoroutine(SetupBattle());
		}

		IEnumerator SetupBattle()
		{
			yield return new WaitForSeconds(0.1f); // waiting for BattleSystem class
			SetPlayerUnitHp(battleSystemScript.playerUnits[0].unitData._maxHp, 0);
			SetPlayerUnitHp(battleSystemScript.playerUnits[1].unitData._maxHp, 1);
			SetPlayerUnitHp(battleSystemScript.playerUnits[2].unitData._maxHp, 2);

			SetEnemyHp(battleSystemScript.enemyUnits[0].unitData._maxHp, 4);
			SetEnemyHp(battleSystemScript.enemyUnits[1].unitData._maxHp, 5);

			SetPlayerHud();
			SetEnemyHud();
		}

		public void DamagePopup(float damageDone, UnitData unitData, GameObject cloneTextGO, GameObject enemyGo)
		{
			TextMeshPro damageText = cloneTextGO.GetComponent<TextMeshPro>();

			if (damageDone > (unitData._baseDamage + (unitData._baseDamage / 10)))
			{
				CriticalStrikeReceived(enemyGo);
				Color newColor = new Color(1f, 0.1949452f, 0.145098f, 1f);
				damageText.fontSize = 500f;
				damageText.color = newColor;
				damageText.text = damageDone.ToString("F0");
				CameraShake.Instance.ShakeCamera(3f, 0.5f);
				Destroy(cloneTextGO, 2f);
			}
			else
			{
				Color newColor = new Color(1f, 0.8182157f, 0.145098f, 1f);
				damageText.color = newColor;
				CameraShake.Instance.ShakeCamera(1f, 0.5f);
				damageText.text = damageDone.ToString("F0");
				Destroy(cloneTextGO, 1f);
			}
		}

		private void OnEnable()
		{
			CriticalStrikeReceived += OnCriticalStrikeReceived;
		}

		private void OnCriticalStrikeReceived(GameObject obj)
		{
			UnitData unitData = obj.GetComponent<Unit>().unitData;
			DialoguePopup(obj, unitData);
		}

		private void DialoguePopup(GameObject obj, UnitData unitData)
		{
			GameObject floatingDialogueGO = Instantiate(floatingDialogue, obj.transform.position + new Vector3(0f, 6f, 0f),
				Quaternion.identity);
			TextMeshPro dialogueText = floatingDialogue.GetComponent<TextMeshPro>();

			Color newColor = new Color(1f, 0.8182157f, 0.145098f, 1f);
			dialogueText.color = newColor;

			int random = Random.Range(0, unitData.criticalDialogue.Length);
			dialogueText.text = unitData.criticalDialogue[random];
			Destroy(floatingDialogueGO, 3f);
		}

		private void OnDisable()
		{
			CriticalStrikeReceived -= OnCriticalStrikeReceived;
		}
	}
}