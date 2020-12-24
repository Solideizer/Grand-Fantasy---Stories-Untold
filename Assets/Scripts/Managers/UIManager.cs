using System.Collections.Generic;
using Characters_Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Managers
{
	public class UIManager : MonoBehaviour
	{
		#region Variable Declarations		
		//public static event Action<GameObject> CriticalStrikeReceived = delegate { };

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
		//[SerializeField] private GameObject floatingDialogue;
		[SerializeField] private Canvas knightSkillBar;
		[SerializeField] private Canvas warriorSkillBar;
		[SerializeField] private Canvas wizardSkillBar;
		[SerializeField] private GameObject knight;
		[SerializeField] private GameObject warrior;
		[SerializeField] private GameObject wizard;
		[SerializeField] private GameObject enemy1;
		[SerializeField] private GameObject enemy2;
		[SerializeField] private Image enemy1Background;
		[SerializeField] private Image enemy2Background;
		[SerializeField] private Image knightBackground;
		[SerializeField] private Image warriorBackground;
		[SerializeField] private Image wizardBackground;
#pragma warning restore 0649
		[HideInInspector] public List<GameObject> playerUnitGOs = new List<GameObject> ();
		[HideInInspector] public List<Unit> playerUnits = new List<Unit> ();
		[HideInInspector] public List<Unit> enemyUnits = new List<Unit> ();
		#endregion
		#region Hp Setters
		public void SetPlayerUnitHp (float hp, int playerUnitID)
		{
			switch (playerUnitID)
			{
				case 0:
					knightHpSlider.value = hp;
					playerUnits[0].unitData.currentHp = knightHpSlider.value;
					break;
				case 1:
					warriorHpSlider.value = hp;
					playerUnits[1].unitData.currentHp = warriorHpSlider.value;
					break;
				case 2:
					wizardHpSlider.value = hp;
					playerUnits[2].unitData.currentHp = wizardHpSlider.value;
					break;
			}
		}

		public void SetEnemyHp (float hp, int enemyID)
		{
			switch (enemyID)
			{
				case 4:
					enemy1HpSlider.value = hp;
					enemyUnits[0].unitData.currentHp = enemy1HpSlider.value;
					break;
				case 5:
					enemy2HpSlider.value = hp;
					enemyUnits[1].unitData.currentHp = enemy2HpSlider.value;
					break;
			}
		}

		#endregion 
		#region Skill Bars

		public void EnableKnightSkillBar ()
		{
			knightSkillBar.enabled = true;
		}
		public void DisableKnightSkillBar ()
		{
			knightSkillBar.enabled = false;
		}
		public void EnableWarriorSkillBar ()
		{
			warriorSkillBar.enabled = true;
		}
		public void DisableWarriorSkillBar ()
		{
			warriorSkillBar.enabled = false;
		}
		public void EnableWizardSkillBar ()
		{
			wizardSkillBar.enabled = true;
		}
		public void DisableWizardSkillBar ()
		{
			wizardSkillBar.enabled = false;
		}
		#endregion
		#region PopUps
		public void DamagePopup (float damageDone, UnitData unitData, GameObject cloneTextGO)
		{
			TextMeshPro damageText = cloneTextGO.GetComponent<TextMeshPro> ();

			if (damageDone > (unitData.baseDamage + (unitData.baseDamage / 10)))
			{
				Color newColor = new Color (1f, 0.1949452f, 0.145098f, 1f);
				damageText.fontSize = 500f;
				damageText.color = newColor;
				damageText.text = damageDone.ToString ("F0");
				CameraShake.Instance.ShakeCamera (3f, 0.5f);
				Destroy (cloneTextGO, 2f);
			}
			else
			{
				Color newColor = new Color (1f, 0.8182157f, 0.145098f, 1f);
				damageText.color = newColor;
				CameraShake.Instance.ShakeCamera (1f, 0.5f);
				damageText.text = damageDone.ToString ("F0");
				Destroy (cloneTextGO, 1f);
			}
		}

		// private void DialoguePopup (GameObject attackedGo, UnitData unitData)
		// {
		// 	GameObject floatingDialogueGO = Instantiate (
		// 		floatingDialogue, attackedGo.transform.position + new Vector3 (0f, 6f, 0f), Quaternion.identity);
		// 	TextMeshPro dialogueText = floatingDialogue.GetComponent<TextMeshPro> ();
		// 	floatingDialogueGO.SetActive (false);
		// 	Color newColor = new Color (1f, 0.8182157f, 0.145098f, 1f);
		// 	dialogueText.color = newColor;

		// 	int random = Random.Range (0, unitData.criticalDialogue.Length);
		// 	dialogueText.text = unitData.criticalDialogue[random];
		// 	floatingDialogueGO.SetActive (true);
		// 	Destroy (floatingDialogueGO, 3f);
		// }
		#endregion
		private void SetPlayerHud ()
		{
			//Knight
			knightNameText.text = playerUnits[0].name;
			knightHpSlider.maxValue = playerUnits[0].unitData.maxHp;
			knightHpSlider.value = playerUnits[0].unitData.currentHp;

			//Warrior
			warriorNameText.text = playerUnits[1].name;
			warriorHpSlider.maxValue = playerUnits[1].unitData.maxHp;
			warriorHpSlider.value = playerUnits[1].unitData.currentHp;

			//Wizard
			wizardNameText.text = playerUnits[2].name;
			wizardHpSlider.maxValue = playerUnits[2].unitData.maxHp;
			wizardHpSlider.value = playerUnits[2].unitData.currentHp;
		}

		private void SetEnemyHud ()
		{
			// Enemy1
			enemy1NameText.text = enemyUnits[0].name;
			enemy1HpSlider.maxValue = enemyUnits[0].unitData.maxHp;
			enemy1HpSlider.value = enemyUnits[0].unitData.currentHp;

			// Enemy2
			enemy2NameText.text = enemyUnits[1].name;
			enemy2HpSlider.maxValue = enemyUnits[1].unitData.maxHp;
			enemy2HpSlider.value = enemyUnits[1].unitData.currentHp;
		}

		private void Awake()
		{
			DisableWarriorSkillBar ();
			DisableWizardSkillBar ();
			EnableKnightSkillBar ();
			PopulateUnitList ();
			PopulateGameobjetList ();
			SetupBattle ();
		}

		private void PopulateUnitList ()
		{
			playerUnits.Add (knight.GetComponent<Unit> ());
			playerUnits.Add (warrior.GetComponent<Unit> ());
			playerUnits.Add (wizard.GetComponent<Unit> ());
			enemyUnits.Add (enemy1.GetComponent<Unit> ());
			enemyUnits.Add (enemy2.GetComponent<Unit> ());
		}

		private void PopulateGameobjetList ()
		{
			playerUnitGOs.Add (knight);
			playerUnitGOs.Add (warrior);
			playerUnitGOs.Add (wizard);
		}
		private void SetupBattle ()
		{
			SetPlayerUnitHp (playerUnits[0].unitData.maxHp, 0);
			SetPlayerUnitHp (playerUnits[1].unitData.maxHp, 1);
			SetPlayerUnitHp (playerUnits[2].unitData.maxHp, 2);

			SetEnemyHp (enemyUnits[0].unitData.maxHp, 4);
			SetEnemyHp (enemyUnits[1].unitData.maxHp, 5);

			SetPlayerHud ();
			SetEnemyHud ();
		}
		#region UI Disablers
		public void DisableKnightUI ()
		{
			DisableKnightSkillBar ();
			knightNameText.GetComponentInParent<RectTransform> ().localScale = new Vector3 (0, 0, 0);
			Destroy (knightHpSlider.gameObject);
			Destroy (knightBackground.gameObject);
		}
		public void DisableWarriorUI ()
		{
			DisableWarriorSkillBar ();
			warriorNameText.GetComponentInParent<RectTransform> ().localScale = new Vector3 (0, 0, 0);
			Destroy (warriorHpSlider.gameObject);
			Destroy (warriorBackground.gameObject);
		}
		public void DisableWizardUI ()
		{
			DisableWizardSkillBar ();
			wizardNameText.GetComponentInParent<RectTransform> ().localScale = new Vector3 (0, 0, 0);
			Destroy (wizardHpSlider.gameObject);
			Destroy (wizardBackground.gameObject);
		}
		public void DisableEnemy1UI ()
		{
			enemy1NameText.GetComponentInParent<RectTransform> ().localScale = new Vector3 (0, 0, 0);
			Destroy (enemy1HpSlider.gameObject);
			Destroy (enemy1Background.gameObject);
		}
		public void DisableEnemy2UI ()
		{
			enemy2NameText.GetComponentInParent<RectTransform> ().localScale = new Vector3 (0, 0, 0);
			Destroy (enemy2HpSlider.gameObject);
			Destroy (enemy2Background.gameObject);
		}
		#endregion
		// private void OnCriticalStrikeReceived (GameObject attackedGo)
		// {
		// 	UnitData unitData = attackedGo.GetComponent<Unit> ().unitData;
		// 	DialoguePopup (attackedGo, unitData);
		// }
		// private void OnEnable ()
		// {
		// 	CriticalStrikeReceived += OnCriticalStrikeReceived;
		// }
		// private void OnDisable ()
		// {
		// 	CriticalStrikeReceived -= OnCriticalStrikeReceived;
		// }
	}
}