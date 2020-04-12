using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GameState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public enum UnitState { KNIGHT, WARRIOR, ENEMY1 }

public class BattleSystem : MonoBehaviour
{
    private GameState gameState;
    private UnitState unitState;

    [SerializeField] private GameObject Knight;
    [SerializeField] private GameObject Warrior;
    [SerializeField] private GameObject Enemy1;

    private Animator KnightAnim;
    private Animator WarriorAnim;
    private Animator Enemy1Anim;

    private List<Unit> PlayerUnits = new List<Unit>();
    private List<Unit> EnemyUnits = new List<Unit>();

    //************** UI ********************
    [SerializeField] private Button KnightBasicAttackButton;
    [SerializeField] private Button WarriorBasicAttackButton;
    private CanvasGroup skillHUD;
    [SerializeField] private TextMeshProUGUI KnightNameText;
    [SerializeField] private TextMeshProUGUI WarriorNameText;
    [SerializeField] private TextMeshProUGUI Enemy1NameText;
    [SerializeField] private Slider KnightHPSlider;
    [SerializeField] private Slider WarriorHPSlider;
    [SerializeField] private Slider Enemy1HPSlider;
    [SerializeField] private GameObject floatingDamage;
    [SerializeField] private Vector3 damageOffset = new Vector3(0f, 3f, 0f);    
    //************** UI END ******************



    private void Start()
    {
        //PlayerUnit 0 --> Knight --> UnitID 0
        //PlayerUnit 1 --> Warrior --> UnitID 1
        //PlayerUnit 2 --> ****** --> UnitID 2
        //PlayerUnit 3 --> ****** --> UnitID 3

        //EnemyUnit0 --> Skeleton --> UnitID 5

        //starting state
        gameState = GameState.START;
        //hide skill huds
        HideSkillHUD();
        //get player unit animators
        KnightAnim = Knight.GetComponent<Animator>();
        WarriorAnim = Warrior.GetComponent<Animator>();

        //get enemy unit animators
        Enemy1Anim = Enemy1.GetComponent<Animator>();     

        //add Unit class to list
        PlayerUnits.Add(Knight.GetComponent<Unit>());
        PlayerUnits.Add(Warrior.GetComponent<Unit>());

        EnemyUnits.Add(Enemy1.GetComponent<Unit>());

        //damagePopup = floatingdamage.GetComponent<TextMeshProUGUI>();
        SetupBattle();
    }

    private void SetupBattle()
    {
        //Health Points
        SetEnemy1HP (EnemyUnits[0].maxHP);
        SetKnightHP(PlayerUnits[0].maxHP);
        SetWarriorHP(PlayerUnits[1].maxHP);

        //ui
        SetPlayerHUD();
        SetEnemyHUD();

        gameState = GameState.PLAYERTURN;       //players turn
        unitState = UnitState.KNIGHT;       //-- KNIGHT STARTS

        ShowSkillHUD();                     //-- KNIGHT SKILL HUD IS ACTIVE

        //WAITING FOR BUTTON CLICK EVENTS
    }

    private void PlayAnim(string animName, int UnitID)
    {
        Debug.Log("unit id: " + UnitID);

        switch (UnitID)
        {
            //Knight Animations
            case 0:
                KnightAnim.SetTrigger(animName);
                break;
            //Warrior Animations
            case 1:
                WarriorAnim.SetTrigger(animName);
                break;
            //Skeleton Animations
            case 5:
                Enemy1Anim.SetTrigger(animName);
                break;
        }
    }


    //*********************************** HP SETTERS **************************************
    public void SetKnightHP(float hp)
    {
        KnightHPSlider.value = hp;
    }

    public void SetWarriorHP(float hp)
    {
        WarriorHPSlider.value = hp;
    }

    public void SetEnemy1HP(float hp)
    {
        Enemy1HPSlider.value = hp;
    }

    //*********************************** HP SETTERS END ************************************



    //************************************* Skill HUDs & UI *********************************

    private void HideSkillHUD()
    {
        skillHUD = KnightBasicAttackButton.GetComponent<CanvasGroup>();
        skillHUD.alpha = 0f;
        skillHUD.interactable = false;
        skillHUD.blocksRaycasts = false;

        skillHUD = WarriorBasicAttackButton.GetComponent<CanvasGroup>();
        skillHUD.alpha = 0f;
        skillHUD.interactable = false;
        skillHUD.blocksRaycasts = false;
    }

    private void ShowSkillHUD()
    {
        switch (unitState)
        {
            case UnitState.KNIGHT:
                skillHUD = KnightBasicAttackButton.GetComponent<CanvasGroup>();
                skillHUD.alpha = 1f;
                skillHUD.interactable = true;
                skillHUD.blocksRaycasts = true;
                break;

            case UnitState.WARRIOR:
                skillHUD = WarriorBasicAttackButton.GetComponent<CanvasGroup>();
                skillHUD.alpha = 1f;
                skillHUD.interactable = true;
                skillHUD.blocksRaycasts = true;
                break;
        }
    }

    public void SetPlayerHUD()
    {
        //Knight
        KnightNameText.text = PlayerUnits[0].name;
        KnightHPSlider.maxValue = PlayerUnits[0].maxHP;
        KnightHPSlider.value = PlayerUnits[0].maxHP;

        //Warrior
        WarriorNameText.text = PlayerUnits[1].name;
        WarriorHPSlider.maxValue = PlayerUnits[1].maxHP;
        WarriorHPSlider.value = PlayerUnits[1].maxHP;
    }

    public void SetEnemyHUD()
    {
        // Enemy1
        Enemy1NameText.text = EnemyUnits[0].name;
        Enemy1HPSlider.maxValue = EnemyUnits[0].maxHP;
        Enemy1HPSlider.value = EnemyUnits[0].maxHP;
    }

    //************************************* Skill HUDs & UI END *****************************




    //************************************* Button Events ***********************************
    public void KnightBasicAttackEvent()
    {
        if (gameState != GameState.PLAYERTURN)
        {
            return;
        }        

        switch (unitState)
        {
            case UnitState.KNIGHT:
                StartCoroutine(KnightBasicAttack());
                break;
        }
    }

    public void WarriorBasicAttackEvent()
    {
        if (gameState != GameState.PLAYERTURN)
        {
            return;
        }
        Debug.Log("unitState from warriorButton: " + unitState);

        switch (unitState)
        {
            case UnitState.WARRIOR:
                StartCoroutine(WarriorBasicAttack());
                break;
        }
    }
    //************************************ Button Events END ********************************



    //************************************ Character Skills *********************************
    private IEnumerator KnightBasicAttack()
    {
        
        //starting position of the unit
        Vector2 startingPos = Knight.transform.position;        

        //play dashing animation
        PlayAnim("Dash", 0);
        
        Knight.transform.position += Knight.transform.right * (Time.deltaTime * 1000);
        float errorDistance = 15f;
        if (Vector3.Distance(Knight.transform.position, Enemy1.transform.position) < errorDistance)
        {
            //TO DO add option to choose different enemies
            Knight.transform.position = Enemy1.transform.position;
            
        }
        yield return new WaitForSeconds(0.5f);
        PlayAnim("Attack", 0);

        //play sound
        AudioManager.PlaySound("basicAttack");       

        //calculate damage        
        float damageDone = PlayerUnits[0].calculateDamage();
        bool isDead = EnemyUnits[0].TakeDamage(damageDone);

        // update hp
        SetEnemy1HP(EnemyUnits[0].currentHP);     
                
        //damagePopup            
        GameObject floatingDamageGO = Instantiate(floatingDamage, Enemy1.transform.position + damageOffset, Quaternion.identity);
        floatingDamageGO.GetComponent<TextMesh>().text = damageDone.ToString("F0");
        Destroy(floatingDamageGO, 1f);
        //**************************************************************************
        
        //enemy hit animation
        PlayAnim("Hit", 5);
        yield return new WaitForSeconds(0.5f);
        //enemy goes back to idle animation
        PlayAnim("Idle", 5);         
        //unit goes back to its starting position
        Knight.transform.position = startingPos;
        //unit is idling
        PlayAnim("Idle", 0);

        HideSkillHUD();      
        
        if (isDead)
        {
            PlayAnim("Dead",5);
            gameState = GameState.WON;
            EndBattle();
        }
        else
        {
            //Warrior's turn starts
            unitState = UnitState.WARRIOR;
            //show warrior's skills
            ShowSkillHUD();
        }
    }

    private IEnumerator WarriorBasicAttack()
    {
        Debug.Log("Warrior Basic attack");
        //starting position of the unit
        Vector2 startingPos = Warrior.transform.position;

        //play dashing animation
        PlayAnim("Dash", 1);

        Warrior.transform.position += Warrior.transform.right * (Time.deltaTime * 1000);
        float errorDistance = 15f;
        if (Vector3.Distance(Warrior.transform.position, Enemy1.transform.position) < errorDistance)
        {
            //TO DO add option to choose different enemies
            Warrior.transform.position = Enemy1.transform.position;
            
        }
        yield return new WaitForSeconds(0.5f);
        PlayAnim("Attack", 1);

        //play sound
        AudioManager.PlaySound("basicAttack");

        //calculate damage        
        float damageDone = PlayerUnits[1].calculateDamage();
        bool isDead = EnemyUnits[0].TakeDamage(damageDone);

        //damagePopup            
        GameObject floatingDamageGO = Instantiate(floatingDamage, Enemy1.transform.position + damageOffset, Quaternion.identity);
        floatingDamageGO.GetComponent<TextMesh>().text = damageDone.ToString("F0");
        Destroy(floatingDamageGO, 1f);

        // update hp
        SetEnemy1HP(EnemyUnits[0].currentHP);

        //enemy hit animation
        PlayAnim("Hit", 5);
        //enemy goes back to idle animation
        yield return new WaitForSeconds(1f);
        PlayAnim("Idle", 5);

        //unit goes back to its starting position
        Warrior.transform.position = startingPos;        
        //unit is idling
        PlayAnim("Idle", 1);                              

        HideSkillHUD();        

        if (isDead)
        {
            gameState = GameState.WON;
            EndBattle();
        }
        else
        {
            gameState = GameState.ENEMYTURN;
            yield return new WaitForSeconds(1f);
            StartCoroutine(EnemyTurn());
        }
    }
    //************************************ Character Skills END ******************************



    //*********************************** ENEMY TURN *****************************************
    private IEnumerator EnemyTurn()
    {
        Debug.Log("Enemy Turn");
        PlayAnim("Dash", 5);
        Vector2 startingPos = Enemy1.transform.position;

        //hangi üniteye saldırcağını belirle
        Enemy1.transform.position += -Knight.transform.right * (Time.deltaTime * 500);
        float errorDistance = 10f;

        if (Vector3.Distance(Knight.transform.position, Enemy1.transform.position) < errorDistance)
        {
            Enemy1.transform.position = Knight.transform.position;          
        }

        yield return new WaitForSeconds(0.5f);
        PlayAnim("Attack", 5);
        AudioManager.PlaySound("basicAttack");
        PlayAnim("Hit", 0);

        //calculate damage
        float damageDone = EnemyUnits[0].calculateDamage();
        bool isDead = PlayerUnits[0].TakeDamage(damageDone);

        
        //damagePopup ******************************************************************************           
        GameObject floatingDamageGO = Instantiate(floatingDamage, Knight.transform.position + damageOffset, Quaternion.identity);
        floatingDamageGO.GetComponent<TextMesh>().text = damageDone.ToString("F0");
        Destroy(floatingDamageGO, 1f);
        //******************************************************************************************

        SetKnightHP(PlayerUnits[0].currentHP);
        AudioManager.PlaySound("hurtSound");

        yield return new WaitForSeconds(0.5f);
        PlayAnim("Idle", 0);       
                
        
        Enemy1.transform.position = startingPos;
        PlayAnim("Idle", 5);        

        if (isDead)
        {
            gameState = GameState.LOST;
            EndBattle();
        }
        else
        {
            gameState = GameState.PLAYERTURN;
            unitState = UnitState.KNIGHT;
            ShowSkillHUD();
        }
    }
    //*********************************** ENEMY TURN END *************************************


    private void EndBattle()
    {
        if (gameState == GameState.WON)
        {
            //load next level
        }
        else if (gameState == GameState.LOST)
        {
            //reload this level
        }
    }
}