using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GameState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public enum UnitState { KNIGHT, WARRIOR, WIZARD, ENEMY1, ENEMY2 }

public class BattleSystem : MonoBehaviour {
    public GameState gameState;
    public UnitState unitState;
    private Unit enemyToAttack;
    private Unit attackedPlayerUnit;
    private int enemyID;
    private int playerUnitID;
    //************** Managers *****************
    private AnimationManager animationManager;
    private UIManager uiManager;

    //********** END Managers *****************
    [SerializeField] private GameObject Knight;
    [SerializeField] private GameObject Warrior;
    [SerializeField] private GameObject Wizard;
    [SerializeField] private GameObject Enemy1;
    [SerializeField] private GameObject Enemy2;

    [SerializeField] private GameObject explosionAnim;

    public List<Unit> PlayerUnits = new List<Unit> ();
    public List<Unit> EnemyUnits = new List<Unit> ();

    [SerializeField] private GameObject floatingDamage;
    [SerializeField] private Vector3 damageOffset = new Vector3 (0f, 3f, 0f);

    RaycastHit2D hit;
    Vector3 mousePos;
    Vector2 mousePos2D;

    private Knight knightClass;
    private Warrior warriorClass;
    private void Awake ()
    {        
        animationManager = GetComponent<AnimationManager> ();
        uiManager = GetComponent<UIManager> ();
        
        knightClass = GetComponent<Knight>();
        warriorClass = GetComponent<Warrior>();
    }
    private void Start () {
        //PlayerUnit 0 --> Knight --> UnitID 0
        //PlayerUnit 1 --> Warrior--> UnitID 1
        //PlayerUnit 2 --> Wizard --> UnitID 2
        //PlayerUnit 3 --> ****** --> UnitID 3

        //EnemyUnit0 --> Skeleton --> UnitID 4
        //EnemyUnit0 --> ******** --> UnitID 5

        gameState = GameState.START;
        uiManager.HideSkillHUD ();

        PlayerUnits.Add (Knight.GetComponent<Unit> ());
        PlayerUnits.Add (Warrior.GetComponent<Unit> ());
        PlayerUnits.Add (Wizard.GetComponent<Unit> ());

        EnemyUnits.Add (Enemy1.GetComponent<Unit> ());
        EnemyUnits.Add (Enemy2.GetComponent<Unit> ());
        
        gameState = GameState.PLAYERTURN; //players turn
        unitState = UnitState.KNIGHT; //-- KNIGHT STARTS
        
    }

    private void Update () {
        SelectTarget ();
    }

    private void SelectTarget () {
        if (Input.GetMouseButtonDown (0)) {
            if (gameState == GameState.PLAYERTURN) {
                mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
                mousePos2D = new Vector2 (mousePos.x, mousePos.y);
                hit = Physics2D.Raycast (mousePos2D, Vector2.zero);

                SelectEnemy (hit);                
            }

        }
    }


    public void SelectEnemy (RaycastHit2D hit) {
        if (gameState != GameState.PLAYERTURN) {
            return;
        }
        string enemyName = hit.collider.gameObject.name;  

        switch (unitState) {
            case UnitState.KNIGHT:
                if (enemyName == "Skeleton") {
                    knightClass.enemyToAttack = EnemyUnits[0];
                    enemyID = 4;
                }
                if (enemyName == "Undead") {
                    knightClass.enemyToAttack = EnemyUnits[1];
                    enemyID = 5;
                }
                knightClass.KnightAttack(enemyID);
                
                break;

            case UnitState.WARRIOR:
                if (enemyName == "Skeleton") {
                    warriorClass.enemyToAttack = EnemyUnits[0];
                    enemyID = 4;
                }
                if (enemyName == "Undead") {
                    warriorClass.enemyToAttack = EnemyUnits[1];
                    enemyID = 5;
                }
                warriorClass.WarriorAttack(enemyID);
                break;
        }
    }

    //************************************* Button Events ***********************************
    public void WizardAttack1Event () {
        if (gameState != GameState.PLAYERTURN) {
            return;
        }

        switch (unitState) {
            case UnitState.WIZARD:
                StartCoroutine (WizardAttack1 ());
                break;
        }
    }
    public void WizardAttack2Event () {
        if (gameState != GameState.PLAYERTURN) {
            return;
        }

        switch (unitState) {
            case UnitState.WIZARD:
                StartCoroutine (WizardAttack2 ());
                break;
        }
    }
    //************************************ Button Events END ********************************


    //************************************ Character Skills *********************************
    
    private IEnumerator WizardAttack1 () {

        animationManager.PlayAnim ("Attack1", 2);

        Vector3 enemyPos = new Vector3 (4.05f, -3.28f, 5f);
        GameObject explosionGO = Instantiate (explosionAnim, enemyPos, Quaternion.identity);
        Destroy (explosionGO, 1f);
        //explosionAnim.GetComponent<Animator>
        //play sound
        //AudioManager.PlaySound("basicAttack");

        //calculate damage        
        float damageDone = PlayerUnits[2].calculateDamage ();
        bool isDead = EnemyUnits[0].TakeDamage (damageDone);

        //damagePopup
        GameObject floatingDamageGO = Instantiate (floatingDamage, Enemy1.transform.position + damageOffset, Quaternion.identity);
        if (damageDone > PlayerUnits[2].baseDamage + (PlayerUnits[2].baseDamage / 10)) {
            Color newColor = new Color (0.8679f, 0.2941f, 0f, 1f);
            floatingDamageGO.GetComponent<TextMeshPro> ().fontSize = 250;
            floatingDamageGO.GetComponent<TextMeshPro> ().color = newColor;
            floatingDamageGO.GetComponent<TextMeshPro> ().text = damageDone.ToString ("F0");

            Destroy (floatingDamageGO, 2f);
        } else {
            floatingDamageGO.GetComponent<TextMeshPro> ().text = damageDone.ToString ("F0");
            Destroy (floatingDamageGO, 1f);
        }
        //**************************************************************************

        // update hp
        uiManager.SetEnemyHP (EnemyUnits[0].currentHP, 4);

        //enemy hit animation
        animationManager.PlayAnim ("Hit", 5);
        //enemy hurt sound
        AudioManager.PlaySound ("hurtSound");
        yield return new WaitForSeconds (0.5f);
        //enemy goes back to idle animation
        animationManager.PlayAnim ("Idle", 5);
        //unit goes back to idle animation
        animationManager.PlayAnim ("Idle", 2);

        uiManager.HideSkillHUD ();

        if (isDead) {
            animationManager.PlayAnim ("Dead", 5);
            gameState = GameState.WON;
            EndBattle ();
        } else {
            //Enemy turn
            uiManager.HideSkillHUD ();
            gameState = GameState.ENEMYTURN;
            unitState = UnitState.ENEMY1;
            yield return new WaitForSeconds (1f);
            StartCoroutine (EnemyTurn ());

        }
    }

    private IEnumerator WizardAttack2 () {
        animationManager.PlayAnim ("Attack2", 2);

        //play sound
        //AudioManager.PlaySound("basicAttack");

        //calculate damage        
        float damageDone = PlayerUnits[2].calculateDamage ();
        bool isDead = EnemyUnits[0].TakeDamage (damageDone);

        //damagePopup
        GameObject floatingDamageGO = Instantiate (floatingDamage, Enemy1.transform.position + damageOffset, Quaternion.identity);
        if (damageDone > PlayerUnits[2].baseDamage + (PlayerUnits[2].baseDamage / 10)) {
            Color newColor = new Color (0.8679f, 0.2941f, 0f, 1f);
            floatingDamageGO.GetComponent<TextMeshPro> ().fontSize = 250;
            floatingDamageGO.GetComponent<TextMeshPro> ().color = newColor;
            floatingDamageGO.GetComponent<TextMeshPro> ().text = damageDone.ToString ("F0");

            Destroy (floatingDamageGO, 2f);
        } else {
            floatingDamageGO.GetComponent<TextMeshPro> ().text = damageDone.ToString ("F0");
            Destroy (floatingDamageGO, 1f);
        }
        //**************************************************************************

        // update hp
        uiManager.SetEnemyHP (EnemyUnits[0].currentHP, 4);

        //enemy hit animation
        animationManager.PlayAnim ("Hit", 5);
        //enemy hurt sound
        AudioManager.PlaySound ("hurtSound");
        yield return new WaitForSeconds (0.5f);
        //enemy goes back to idle animation
        animationManager.PlayAnim ("Idle", 5);
        //unit goes back to its starting position
        //Knight.transform.position = startingPos;
        //unit is idling
        animationManager.PlayAnim ("Idle", 2);

        uiManager.HideSkillHUD ();

        if (isDead) {
            animationManager.PlayAnim ("Dead", 5);
            gameState = GameState.WON;
            EndBattle ();
        } else {
            //Enemy turn
            uiManager.HideSkillHUD ();
            gameState = GameState.ENEMYTURN;
            unitState = UnitState.ENEMY1;
            yield return new WaitForSeconds (1f);
            StartCoroutine (EnemyTurn ());

        }
    }
    //************************************ Character Skills END ******************************

    //*********************************** ENEMY TURNS *****************************************
    private IEnumerator EnemyTurn () {
        Vector2 startingPos = Enemy1.transform.position;

        int randomPlayerUnit = Random.Range (0, PlayerUnits.Count);
        attackedPlayerUnit = PlayerUnits[randomPlayerUnit];

        animationManager.PlayAnim ("Dash", 4);

        //hangi üniteye saldırcağını belirle
        Enemy1.transform.position += -attackedPlayerUnit.transform.right * (Time.deltaTime * 500);
        float errorDistance = 10f;

        if (Vector3.Distance (attackedPlayerUnit.transform.position, Enemy1.transform.position) < errorDistance) {
            Enemy1.transform.position = attackedPlayerUnit.transform.position;
        }

        yield return new WaitForSeconds (0.5f);
        animationManager.PlayAnim ("Attack", 4);
        AudioManager.PlaySound ("basicAttack");
        animationManager.PlayAnim ("Hit", randomPlayerUnit);

        //calculate damage
        float damageDone = EnemyUnits[0].calculateDamage ();
        bool isDead = attackedPlayerUnit.TakeDamage (damageDone);

        //damagePopup ******************************************************************************          

        GameObject floatingDamageGO = Instantiate (floatingDamage, attackedPlayerUnit.transform.position + damageOffset, Quaternion.identity);
        if (damageDone > EnemyUnits[0].baseDamage + (EnemyUnits[0].baseDamage / 10)) {
            Color newColor = new Color (0.8679f, 0.2941f, 0f, 1f);
            floatingDamageGO.GetComponent<TextMeshPro> ().fontSize = 250;
            floatingDamageGO.GetComponent<TextMeshPro> ().color = newColor;
            floatingDamageGO.GetComponent<TextMeshPro> ().text = damageDone.ToString ("F0");

            Destroy (floatingDamageGO, 2f);
        } else {
            floatingDamageGO.GetComponent<TextMeshPro> ().text = damageDone.ToString ("F0");
            Destroy (floatingDamageGO, 1f);
        }
        //******************************************************************************************

        uiManager.SetPlayerUnitHP (attackedPlayerUnit.currentHP, playerUnitID);
        AudioManager.PlaySound ("hurtSound");

        yield return new WaitForSeconds (0.5f);
        animationManager.PlayAnim ("Idle", randomPlayerUnit);

        Enemy1.transform.position = startingPos;
        animationManager.PlayAnim ("Idle", 4);

        if (isDead) {
            AudioManager.PlaySound ("KnightDeath");
            gameState = GameState.LOST;
            EndBattle ();
        } else {
            gameState = GameState.ENEMYTURN;
            unitState = UnitState.ENEMY2;
            yield return new WaitForSeconds (0.5f);
            StartCoroutine (Enemy2Turn ());

        }
    }

    private IEnumerator Enemy2Turn () {
        Vector2 startingPos = Enemy2.transform.position;

        int randomPlayerUnit = Random.Range (0, PlayerUnits.Count);
        attackedPlayerUnit = PlayerUnits[randomPlayerUnit];

        animationManager.PlayAnim ("Dash", 5);

        Enemy2.transform.position += -attackedPlayerUnit.transform.right * (Time.deltaTime * 500);
        float errorDistance = 10f;

        if (Vector3.Distance (attackedPlayerUnit.transform.position, Enemy2.transform.position) < errorDistance) {
            Enemy2.transform.position = attackedPlayerUnit.transform.position;
        }

        yield return new WaitForSeconds (0.5f);
        animationManager.PlayAnim ("Attack", 5);
        AudioManager.PlaySound ("basicAttack");
        animationManager.PlayAnim ("Hit", randomPlayerUnit);

        //calculate damage
        float damageDone = EnemyUnits[1].calculateDamage ();
        bool isDead = attackedPlayerUnit.TakeDamage (damageDone);

        //damagePopup ******************************************************************************          

        GameObject floatingDamageGO = Instantiate (floatingDamage, attackedPlayerUnit.transform.position + damageOffset, Quaternion.identity);
        if (damageDone > EnemyUnits[1].baseDamage + (EnemyUnits[1].baseDamage / 10)) {
            Color newColor = new Color (0.8679f, 0.2941f, 0f, 1f);
            floatingDamageGO.GetComponent<TextMeshPro> ().fontSize = 250;
            floatingDamageGO.GetComponent<TextMeshPro> ().color = newColor;
            floatingDamageGO.GetComponent<TextMeshPro> ().text = damageDone.ToString ("F0");

            Destroy (floatingDamageGO, 2f);
        } else {
            floatingDamageGO.GetComponent<TextMeshPro> ().text = damageDone.ToString ("F0");
            Destroy (floatingDamageGO, 1f);
        }
        //******************************************************************************************

        uiManager.SetPlayerUnitHP (attackedPlayerUnit.currentHP, playerUnitID);
        AudioManager.PlaySound ("hurtSound");

        yield return new WaitForSeconds (0.5f);
        animationManager.PlayAnim ("Idle", randomPlayerUnit);

        Enemy2.transform.position = startingPos;
        animationManager.PlayAnim ("Idle", 5);

        if (isDead) {
            AudioManager.PlaySound ("KnightDeath");
            gameState = GameState.LOST;
            EndBattle ();
        } else {
            gameState = GameState.PLAYERTURN;
            unitState = UnitState.KNIGHT;

        }
    }
    //*********************************** ENEMY TURNS END *************************************

    public void EndBattle () {
        if (gameState == GameState.WON) {
            //load next level
        } else if (gameState == GameState.LOST) {
            //reload this level
        }
    }
}