using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GameState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public enum UnitState { KNIGHT, WARRIOR, WIZARD, ENEMY1, ENEMY2 }

public class BattleSystem : MonoBehaviour
{
    private GameState gameState;
    private UnitState unitState;
    Unit enemyToAttack;
    Unit attackedPlayerUnit;
    int enemyID;
    int playerUnitID;

    [SerializeField] private GameObject Knight;
    [SerializeField] private GameObject Warrior;
    [SerializeField] private GameObject Wizard;
    [SerializeField] private GameObject Enemy1;
    [SerializeField] private GameObject Enemy2;

    private Animator KnightAnim;
    private Animator WarriorAnim;
    private Animator WizardAnim;
    private Animator Enemy1Anim;
    private Animator Enemy2Anim;

    private List<Unit> PlayerUnits = new List<Unit>();
    private List<Unit> EnemyUnits = new List<Unit>();

    //************** UI ********************    
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
    [SerializeField] private GameObject floatingDamage;
    [SerializeField] private Vector3 damageOffset = new Vector3(0f, 3f, 0f);
    private CanvasGroup skillHUD;
    //************** UI END ******************

    RaycastHit2D hit;
    Vector3 mousePos;
    Vector2 mousePos2D;

    private void Start()
    {
        //PlayerUnit 0 --> Knight --> UnitID 0
        //PlayerUnit 1 --> Warrior --> UnitID 1
        //PlayerUnit 2 --> Wizard --> UnitID 2
        //PlayerUnit 3 --> ****** --> UnitID 3

        //EnemyUnit0 --> Skeleton --> UnitID 4
        //EnemyUnit0 --> ******** --> UnitID 5

        //starting state
        gameState = GameState.START;
        //hide skill huds
        HideSkillHUD();
        //get player unit animators
        KnightAnim = Knight.GetComponent<Animator>();
        WarriorAnim = Warrior.GetComponent<Animator>();
        WizardAnim = Wizard.GetComponent<Animator>();

        //get enemy unit animators
        Enemy1Anim = Enemy1.GetComponent<Animator>();
        Enemy2Anim = Enemy2.GetComponent<Animator>();

        //add Unit class to list
        PlayerUnits.Add(Knight.GetComponent<Unit>());
        PlayerUnits.Add(Warrior.GetComponent<Unit>());
        PlayerUnits.Add(Wizard.GetComponent<Unit>());

        EnemyUnits.Add(Enemy1.GetComponent<Unit>());
        EnemyUnits.Add(Enemy2.GetComponent<Unit>());

        //damagePopup = floatingdamage.GetComponent<TextMeshProUGUI>();
        SetupBattle();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (gameState == GameState.PLAYERTURN)
            {
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos2D = new Vector2(mousePos.x, mousePos.y);
                hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

                SelectEnemy(hit);
                Debug.Log(hit.collider.gameObject.name);
            }
           
        }     
        

    }
   

    private void SetupBattle()
    {
        //Health Points
        SetEnemyHP (EnemyUnits[0].maxHP,4);
        SetEnemyHP (EnemyUnits[1].maxHP,5);

        SetPlayerUnitHP(PlayerUnits[0].maxHP,0);
        SetPlayerUnitHP(PlayerUnits[1].maxHP,1);
        SetPlayerUnitHP(PlayerUnits[2].maxHP,2);

        //ui
        SetPlayerHUD();
        SetEnemyHUD();

        gameState = GameState.PLAYERTURN;       //players turn
        unitState = UnitState.KNIGHT;       //-- KNIGHT STARTS

        ShowSkillHUD();                     //-- KNIGHT SKILL HUD IS ACTIVE

        //WAITING FOR PLAYER TO CLICK 
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
            //Wizard Animations
            case 2:
                WizardAnim.SetTrigger(animName);
                break;
            //Skeleton Animations
            case 4:
                Enemy1Anim.SetTrigger(animName);
                break;
            //Undead Animations
            case 5:
                Enemy2Anim.SetTrigger(animName);
                break;
        }
    }


    //*********************************** HP SETTERS **************************************
    public void SetPlayerUnitHP(float hp, int playerUnitID)
    {
        switch (playerUnitID)
        {
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

    public void SetEnemyHP(float hp,int enemyID)
    {
        switch (enemyID)
        {
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

    private void HideSkillHUD()
    {       

        skillHUD = WizardAttack1Button.GetComponent<CanvasGroup>();
        skillHUD.alpha = 0f;
        skillHUD.interactable = false;
        skillHUD.blocksRaycasts = false;

        skillHUD = WizardAttack2Button.GetComponent<CanvasGroup>();
        skillHUD.alpha = 0f;
        skillHUD.interactable = false;
        skillHUD.blocksRaycasts = false;
    }

    private void ShowSkillHUD()
    {
        switch (unitState)
        {            
            case UnitState.WIZARD:
                skillHUD = WizardAttack1Button.GetComponent<CanvasGroup>();
                skillHUD.alpha = 1f;
                skillHUD.interactable = true;
                skillHUD.blocksRaycasts = true;

                skillHUD = WizardAttack2Button.GetComponent<CanvasGroup>();
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

        //Wizard
        WizardNameText.text = PlayerUnits[2].name;
        WizardHPSlider.maxValue = PlayerUnits[2].maxHP;
        WizardHPSlider.value = PlayerUnits[2].maxHP;
    }

    public void SetEnemyHUD()
    {
        // Enemy1
        Enemy1NameText.text = EnemyUnits[0].name;
        Enemy1HPSlider.maxValue = EnemyUnits[0].maxHP;
        Enemy1HPSlider.value = EnemyUnits[0].maxHP;

        // Enemy2
        Enemy2NameText.text = EnemyUnits[1].name;
        Enemy2HPSlider.maxValue = EnemyUnits[1].maxHP;
        Enemy2HPSlider.value = EnemyUnits[1].maxHP;
    }

    //************************************* Skill HUDs & UI END *****************************
       
    
    public void SelectEnemy(RaycastHit2D hit)
    {
        if (gameState != GameState.PLAYERTURN)
        {
            return;
        }        

        switch (unitState)
        {
            case UnitState.KNIGHT:
                if (hit.collider.gameObject.name == "Skeleton")
                {
                    enemyToAttack = EnemyUnits[0];
                    enemyID = 4;
                }
                if (hit.collider.gameObject.name == "Undead")
                {
                    enemyToAttack = EnemyUnits[1];
                    enemyID = 5;
                }
                StartCoroutine(KnightBasicAttack(enemyID));
                break;

            case UnitState.WARRIOR:
                if (hit.collider.gameObject.name == "Skeleton")
                {
                    enemyToAttack = EnemyUnits[0];
                    enemyID = 4;
                }
                if (hit.collider.gameObject.name == "Undead")
                {
                    enemyToAttack = EnemyUnits[1];
                    enemyID = 5;
                }
                StartCoroutine(WarriorBasicAttack(enemyID));
                break;
        }
    }
    

    //************************************* Button Events ***********************************
    public void WizardAttack1Event()
    {
        if (gameState != GameState.PLAYERTURN)
        {
            return;
        }        

        switch (unitState)
        {
            case UnitState.WIZARD:
                StartCoroutine(WizardAttack1());
                break;
        }
    }
    public void WizardAttack2Event()
    {
        if (gameState != GameState.PLAYERTURN)
        {
            return;
        }

        switch (unitState)
        {
            case UnitState.WIZARD:
                StartCoroutine(WizardAttack2());
                break;
        }
    }
    //************************************ Button Events END ********************************



    //************************************ Character Skills *********************************
    private IEnumerator KnightBasicAttack(int enemyID)
    {
        AudioManager.PlaySound("KnightOpeners");
        //starting position of the unit
        Vector2 startingPos = Knight.transform.position;    
                                                    
       
        //play dashing animation
        PlayAnim("Dash", 0);
        
        Knight.transform.position += Knight.transform.right * (Time.deltaTime * 1000);
        float errorDistance = 15f;
        
        if (Vector3.Distance(Knight.transform.position,enemyToAttack.transform.position) < errorDistance)
        {            
            Knight.transform.position = enemyToAttack.transform.position;            
        }
        yield return new WaitForSeconds(0.5f);
        PlayAnim("Attack", 0);

        //play sound
        AudioManager.PlaySound("basicAttack");       

        //calculate damage        
        float damageDone = PlayerUnits[0].calculateDamage();
        bool isDead = enemyToAttack.TakeDamage(damageDone);
                          
        //damagePopup
        GameObject floatingDamageGO = Instantiate(floatingDamage, enemyToAttack.transform.position + damageOffset, Quaternion.identity);
        if (damageDone > PlayerUnits[0].baseDamage + (PlayerUnits[0].baseDamage / 10))
        {
            Color newColor = new Color(0.8679f, 0.2941f, 0f, 1f);
            floatingDamageGO.GetComponent<TextMesh>().fontSize = 250;
            floatingDamageGO.GetComponent<TextMesh>().color = newColor;
            floatingDamageGO.GetComponent<TextMesh>().text = damageDone.ToString("F0");

            Destroy(floatingDamageGO, 2f);
        }
        else
        {
            floatingDamageGO.GetComponent<TextMesh>().text = damageDone.ToString("F0");
            Destroy(floatingDamageGO, 1f);
        }
        //**************************************************************************

        // update hp
        SetEnemyHP(enemyToAttack.currentHP,enemyID);

        //enemy hit animation
        PlayAnim("Hit", enemyID);
        //enemy hurt sound
        AudioManager.PlaySound("hurtSound");
        yield return new WaitForSeconds(0.5f);
        //enemy goes back to idle animation
        PlayAnim("Idle", enemyID);         
        //unit goes back to its starting position
        Knight.transform.position = startingPos;
        //unit is idling
        PlayAnim("Idle", 0);

        HideSkillHUD();      
        
        if (isDead)
        {
            PlayAnim("Dead", enemyID);
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

    private IEnumerator WarriorBasicAttack(int enemyID)
    {
        Debug.Log("Warrior Basic attack");
        //starting position of the unit
        Vector2 startingPos = Warrior.transform.position;

        //play dashing animation
        PlayAnim("Dash", 1);

        Warrior.transform.position += Warrior.transform.right * (Time.deltaTime * 1000);
        float errorDistance = 15f;
        if (Vector3.Distance(Warrior.transform.position, enemyToAttack.transform.position) < errorDistance)
        {            
            Warrior.transform.position = enemyToAttack.transform.position;            
        }
        yield return new WaitForSeconds(0.5f);
        PlayAnim("Attack", 1);

        //play sound
        AudioManager.PlaySound("basicAttack");

        //calculate damage        
        float damageDone = PlayerUnits[1].calculateDamage();
        bool isDead = enemyToAttack.TakeDamage(damageDone);
        
        //damagePopup
        GameObject floatingDamageGO = Instantiate(floatingDamage, enemyToAttack.transform.position + damageOffset, Quaternion.identity);
        if (damageDone > PlayerUnits[1].baseDamage + (PlayerUnits[1].baseDamage/10))
        {
            Color newColor = new Color(0.8679f, 0.2941f, 0f, 1f);
            floatingDamageGO.GetComponent<TextMesh>().fontSize = 250;
            floatingDamageGO.GetComponent<TextMesh>().color = newColor;
            floatingDamageGO.GetComponent<TextMesh>().text = damageDone.ToString("F0");         
                        
            Destroy(floatingDamageGO, 2f);
        }
        else 
        {
            floatingDamageGO.GetComponent<TextMesh>().text = damageDone.ToString("F0");
            Destroy(floatingDamageGO, 1f);
        }        
        

        // update hp
        SetEnemyHP(enemyToAttack.currentHP,enemyID);

        //enemy hit animation
        PlayAnim("Hit", enemyID);
        //enemy hurt sound
        AudioManager.PlaySound("hurtSound");
        //enemy goes back to idle animation
        yield return new WaitForSeconds(1f);
        PlayAnim("Idle", enemyID);

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
            unitState = UnitState.WIZARD;
            ShowSkillHUD();
        }
    }

    private IEnumerator WizardAttack1()
    {
        
        PlayAnim("Attack1", 2);

        //play sound
        AudioManager.PlaySound("basicAttack");

        //calculate damage        
        float damageDone = PlayerUnits[2].calculateDamage();
        bool isDead = EnemyUnits[0].TakeDamage(damageDone);

        //damagePopup
        GameObject floatingDamageGO = Instantiate(floatingDamage, Enemy1.transform.position + damageOffset, Quaternion.identity);
        if (damageDone > PlayerUnits[2].baseDamage + (PlayerUnits[2].baseDamage / 10))
        {
            Color newColor = new Color(0.8679f, 0.2941f, 0f, 1f);
            floatingDamageGO.GetComponent<TextMesh>().fontSize = 250;
            floatingDamageGO.GetComponent<TextMesh>().color = newColor;
            floatingDamageGO.GetComponent<TextMesh>().text = damageDone.ToString("F0");

            Destroy(floatingDamageGO, 2f);
        }
        else
        {
            floatingDamageGO.GetComponent<TextMesh>().text = damageDone.ToString("F0");
            Destroy(floatingDamageGO, 1f);
        }
        //**************************************************************************

        // update hp
        SetEnemyHP(EnemyUnits[0].currentHP,4);

        //enemy hit animation
        PlayAnim("Hit", 5);
        //enemy hurt sound
        AudioManager.PlaySound("hurtSound");
        yield return new WaitForSeconds(0.5f);
        //enemy goes back to idle animation
        PlayAnim("Idle", 5);
        //unit goes back to its starting position
        //Knight.transform.position = startingPos;
        //unit is idling
        PlayAnim("Idle", 2);

        HideSkillHUD();

        if (isDead)
        {
            PlayAnim("Dead", 5);
            gameState = GameState.WON;
            EndBattle();
        }
        else
        {
            //Enemy turn
            HideSkillHUD();
            gameState = GameState.ENEMYTURN;
            unitState = UnitState.ENEMY1;
            yield return new WaitForSeconds(1f);
            StartCoroutine(EnemyTurn());
            

        }
    }

    private IEnumerator WizardAttack2()
    {
        PlayAnim("Attack2", 2);

        //play sound
        AudioManager.PlaySound("basicAttack");

        //calculate damage        
        float damageDone = PlayerUnits[2].calculateDamage();
        bool isDead = EnemyUnits[0].TakeDamage(damageDone);

        //damagePopup
        GameObject floatingDamageGO = Instantiate(floatingDamage, Enemy1.transform.position + damageOffset, Quaternion.identity);
        if (damageDone > PlayerUnits[2].baseDamage + (PlayerUnits[2].baseDamage / 10))
        {
            Color newColor = new Color(0.8679f, 0.2941f, 0f, 1f);
            floatingDamageGO.GetComponent<TextMesh>().fontSize = 250;
            floatingDamageGO.GetComponent<TextMesh>().color = newColor;
            floatingDamageGO.GetComponent<TextMesh>().text = damageDone.ToString("F0");

            Destroy(floatingDamageGO, 2f);
        }
        else
        {
            floatingDamageGO.GetComponent<TextMesh>().text = damageDone.ToString("F0");
            Destroy(floatingDamageGO, 1f);
        }
        //**************************************************************************

        // update hp
        SetEnemyHP(EnemyUnits[0].currentHP,4);

        //enemy hit animation
        PlayAnim("Hit", 5);
        //enemy hurt sound
        AudioManager.PlaySound("hurtSound");
        yield return new WaitForSeconds(0.5f);
        //enemy goes back to idle animation
        PlayAnim("Idle", 5);
        //unit goes back to its starting position
        //Knight.transform.position = startingPos;
        //unit is idling
        PlayAnim("Idle", 2);

        HideSkillHUD();

        if (isDead)
        {
            PlayAnim("Dead", 5);
            gameState = GameState.WON;
            EndBattle();
        }
        else
        {
            //Enemy turn
            HideSkillHUD();
            gameState = GameState.ENEMYTURN;
            unitState = UnitState.ENEMY1;
            yield return new WaitForSeconds(1f);
            StartCoroutine(EnemyTurn());


        }
    }
    //************************************ Character Skills END ******************************

    

    //*********************************** ENEMY TURNS *****************************************
    private IEnumerator EnemyTurn()
    {
        Vector2 startingPos = Enemy1.transform.position;

        int randomPlayerUnit = Random.Range(0, PlayerUnits.Count);
        attackedPlayerUnit = PlayerUnits[randomPlayerUnit];

        PlayAnim("Dash", 4);

        
        //hangi üniteye saldırcağını belirle
        Enemy1.transform.position += -attackedPlayerUnit.transform.right * (Time.deltaTime * 500);
        float errorDistance = 10f;

        if (Vector3.Distance(attackedPlayerUnit.transform.position, Enemy1.transform.position) < errorDistance)
        {
            Enemy1.transform.position = attackedPlayerUnit.transform.position;          
        }

        yield return new WaitForSeconds(0.5f);
        PlayAnim("Attack", 4);
        AudioManager.PlaySound("basicAttack");
        PlayAnim("Hit", randomPlayerUnit);

        //calculate damage
        float damageDone = EnemyUnits[0].calculateDamage();
        bool isDead = attackedPlayerUnit.TakeDamage(damageDone);


        //damagePopup ******************************************************************************          
        
        GameObject floatingDamageGO = Instantiate(floatingDamage, attackedPlayerUnit.transform.position + damageOffset, Quaternion.identity);
        if (damageDone > EnemyUnits[0].baseDamage + (EnemyUnits[0].baseDamage / 10))
        {
            Color newColor = new Color(0.8679f, 0.2941f, 0f, 1f);
            floatingDamageGO.GetComponent<TextMesh>().fontSize = 250;
            floatingDamageGO.GetComponent<TextMesh>().color = newColor;
            floatingDamageGO.GetComponent<TextMesh>().text = damageDone.ToString("F0");

            Destroy(floatingDamageGO, 2f);
        }
        else
        {
            floatingDamageGO.GetComponent<TextMesh>().text = damageDone.ToString("F0");
            Destroy(floatingDamageGO, 1f);
        }
        //******************************************************************************************

        SetPlayerUnitHP(attackedPlayerUnit.currentHP,playerUnitID);
        AudioManager.PlaySound("hurtSound");

        yield return new WaitForSeconds(0.5f);
        PlayAnim("Idle", randomPlayerUnit);       
                
        
        Enemy1.transform.position = startingPos;
        PlayAnim("Idle", 4);        

        if (isDead)
        {
            AudioManager.PlaySound("KnightDeath");
            gameState = GameState.LOST;
            EndBattle();
        }
        else
        {
            gameState = GameState.ENEMYTURN;
            unitState = UnitState.ENEMY2;
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Enemy2Turn());

        }
    }

    private IEnumerator Enemy2Turn()
    {
        Vector2 startingPos = Enemy2.transform.position;

        int randomPlayerUnit = Random.Range(0, PlayerUnits.Count);
        attackedPlayerUnit = PlayerUnits[randomPlayerUnit];

        PlayAnim("Dash", 5);
        
        Enemy2.transform.position += -attackedPlayerUnit.transform.right * (Time.deltaTime * 500);
        float errorDistance = 10f;

        if (Vector3.Distance(attackedPlayerUnit.transform.position, Enemy2.transform.position) < errorDistance)
        {
            Enemy2.transform.position = attackedPlayerUnit.transform.position;
        }

        yield return new WaitForSeconds(0.5f);
        PlayAnim("Attack", 5);
        AudioManager.PlaySound("basicAttack");
        PlayAnim("Hit", randomPlayerUnit);

        //calculate damage
        float damageDone = EnemyUnits[1].calculateDamage();
        bool isDead = attackedPlayerUnit.TakeDamage(damageDone);


        //damagePopup ******************************************************************************          

        GameObject floatingDamageGO = Instantiate(floatingDamage, attackedPlayerUnit.transform.position + damageOffset, Quaternion.identity);
        if (damageDone > EnemyUnits[1].baseDamage + (EnemyUnits[1].baseDamage / 10))
        {
            Color newColor = new Color(0.8679f, 0.2941f, 0f, 1f);
            floatingDamageGO.GetComponent<TextMesh>().fontSize = 250;
            floatingDamageGO.GetComponent<TextMesh>().color = newColor;
            floatingDamageGO.GetComponent<TextMesh>().text = damageDone.ToString("F0");

            Destroy(floatingDamageGO, 2f);
        }
        else
        {
            floatingDamageGO.GetComponent<TextMesh>().text = damageDone.ToString("F0");
            Destroy(floatingDamageGO, 1f);
        }
        //******************************************************************************************

        SetPlayerUnitHP(attackedPlayerUnit.currentHP, playerUnitID);
        AudioManager.PlaySound("hurtSound");

        yield return new WaitForSeconds(0.5f);
        PlayAnim("Idle", randomPlayerUnit);


        Enemy2.transform.position = startingPos;
        PlayAnim("Idle", 5);

        if (isDead)
        {
            AudioManager.PlaySound("KnightDeath");
            gameState = GameState.LOST;
            EndBattle();
        }
        else
        {
            gameState = GameState.PLAYERTURN;
            unitState = UnitState.KNIGHT;           
            
        }
    }
    //*********************************** ENEMY TURNS END *************************************


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