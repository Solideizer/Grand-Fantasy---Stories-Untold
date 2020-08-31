using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class UIManager : MonoBehaviour {
    public BattleSystem battleSystemScript;

    //*********************************** UI ******************************************
    [SerializeField] private Button WizardAttack1Button;
    [SerializeField] private Button WizardAttack2Button;
    [SerializeField] private TextMeshProUGUI KnightNameText;
    [SerializeField] private TextMeshProUGUI WarriorNameText;
    [SerializeField] private TextMeshProUGUI WizardNameText;
    [SerializeField] private TextMeshProUGUI Enemy1NameText;
    [SerializeField] private TextMeshProUGUI Enemy2NameText;
    [SerializeField] private Slider KnightHPSlider;
    [SerializeField] private Slider WarriorHPSlider;
    [SerializeField] private Slider WizardHPSlider;
    [SerializeField] private Slider Enemy1HPSlider;
    [SerializeField] private Slider Enemy2HPSlider;
    private CanvasGroup skillHUD;
    //*********************************** END UI ******************************************

    //*********************************** HP SETTERS **************************************
    public void SetPlayerUnitHP (float hp, int playerUnitID) {
        switch (playerUnitID) {
            case 0:
                KnightHPSlider.value = hp;
                break;
            case 1:
                WarriorHPSlider.value = hp;
                break;
            case 2:
                WizardHPSlider.value = hp;
                break;
        }

    }
    public void SetEnemyHP (float hp, int enemyID) {
        switch (enemyID) {
            case 4:
                Enemy1HPSlider.value = hp;
                break;
            case 5:
                Enemy2HPSlider.value = hp;
                break;
        }

    }
    //*********************************** HP SETTERS END ************************************

    //************************************* Skill HUDs & UI *********************************
    public void HideSkillHUD () {

        skillHUD = WizardAttack1Button.GetComponent<CanvasGroup> ();
        skillHUD.alpha = 0f;
        skillHUD.interactable = false;
        skillHUD.blocksRaycasts = false;

        skillHUD = WizardAttack2Button.GetComponent<CanvasGroup> ();
        skillHUD.alpha = 0f;
        skillHUD.interactable = false;
        skillHUD.blocksRaycasts = false;
    }

    public void ShowSkillHUD () {
        switch (battleSystemScript.unitState) {
            case UnitState.WIZARD:
                skillHUD = WizardAttack1Button.GetComponent<CanvasGroup> ();
                skillHUD.alpha = 1f;
                skillHUD.interactable = true;
                skillHUD.blocksRaycasts = true;

                skillHUD = WizardAttack2Button.GetComponent<CanvasGroup> ();
                skillHUD.alpha = 1f;
                skillHUD.interactable = true;
                skillHUD.blocksRaycasts = true;
                break;
        }
    }

    public void SetPlayerHUD () {
        //Knight
        KnightNameText.text = battleSystemScript.PlayerUnits[0].name;
        KnightHPSlider.maxValue = battleSystemScript.PlayerUnits[0].maxHP;
        KnightHPSlider.value = battleSystemScript.PlayerUnits[0].currentHP;

        //Warrior
        WarriorNameText.text = battleSystemScript.PlayerUnits[1].name;
        WarriorHPSlider.maxValue = battleSystemScript.PlayerUnits[1].maxHP;
        WarriorHPSlider.value = battleSystemScript.PlayerUnits[1].currentHP;

        //Wizard
        WizardNameText.text = battleSystemScript.PlayerUnits[2].name;
        WizardHPSlider.maxValue = battleSystemScript.PlayerUnits[2].maxHP;
        WizardHPSlider.value = battleSystemScript.PlayerUnits[2].currentHP;
    }

    public void SetEnemyHUD () {
        // Enemy1
        Enemy1NameText.text = battleSystemScript.EnemyUnits[0].name;
        Enemy1HPSlider.maxValue = battleSystemScript.EnemyUnits[0].maxHP;
        Enemy1HPSlider.value = battleSystemScript.EnemyUnits[0].maxHP;

        // Enemy2
        Enemy2NameText.text = battleSystemScript.EnemyUnits[1].name;
        Enemy2HPSlider.maxValue = battleSystemScript.EnemyUnits[1].maxHP;
        Enemy2HPSlider.value = battleSystemScript.EnemyUnits[1].maxHP;
    }

    //************************************* Skill HUDs & UI END *****************************

    private void Start () {

        StartCoroutine(SetupBattle ());
    }

    IEnumerator SetupBattle () {
              
        yield return new WaitForSeconds(0.1f); // waiting for BattleSystem class
        SetPlayerUnitHP (battleSystemScript.PlayerUnits[0].maxHP, 0);
        SetPlayerUnitHP (battleSystemScript.PlayerUnits[1].maxHP, 1);
        SetPlayerUnitHP (battleSystemScript.PlayerUnits[2].maxHP, 2);

        SetEnemyHP (battleSystemScript.EnemyUnits[0].maxHP, 4);
        SetEnemyHP (battleSystemScript.EnemyUnits[1].maxHP, 5);

        SetPlayerHUD ();
        SetEnemyHUD ();

        ShowSkillHUD (); //-- KNIGHT SKILL HUD IS ACTIVE

        //WAITING FOR PLAYER TO CLICK 
    }
}