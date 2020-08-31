using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public enum UnitState { KNIGHT, WARRIOR, WIZARD, ENEMY1, ENEMY2 }

public class BattleSystem : MonoBehaviour {
    private GameState gameState;
    public UnitState unitState;
    Unit enemyToAttack;
    Unit attackedPlayerUnit;
    int enemyID;
    int playerUnitID;
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
    private void Awake () {
        //get managers
        animationManager = GetComponent<AnimationManager> ();
        uiManager = GetComponent<UIManager> ();
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
                Debug.Log (hit.collider.gameObject.name);
            }

        }
    }


    public void SelectEnemy (RaycastHit2D hit) {
        if (gameState != GameState.PLAYERTURN) {
            return;
        }

        switch (unitState) {
            case UnitState.KNIGHT:
                if (hit.collider.gameObject.name == "Skeleton") {
                    enemyToAttack = EnemyUnits[0];
                    enemyID = 4;
                }
                if (hit.collider.gameObject.name == "Undead") {
                    enemyToAttack = EnemyUnits[1];
                    enemyID = 5;
                }
                StartCoroutine (KnightBasicAttack (enemyID));
                break;

            case UnitState.WARRIOR:
                if (hit.collider.gameObject.name == "Skeleton") {
                    enemyToAttack = EnemyUnits[0];
                    enemyID = 4;
                }
                if (hit.collider.gameObject.name == "Undead") {
                    enemyToAttack = EnemyUnits[1];
                    enemyID = 5;
                }
                StartCoroutine (WarriorBasicAttack (enemyID));
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
    private IEnumerator KnightBasicAttack (int enemyID) {
        AudioManager.PlaySound ("KnightOpeners");
        //starting position of the unit
        Vector2 startingPos = Knight.transform.position;

        //play dashing animation
        animationManager.PlayAnim ("Dash", 0);

        Knight.transform.position += Knight.transform.right * (Time.deltaTime * 1000);
        float errorDistance = 15f;

        if (Vector3.Distance (Knight.transform.position, enemyToAttack.transform.position) < errorDistance) {
            Knight.transform.position = enemyToAttack.transform.position;
        }
        yield return new WaitForSeconds (0.5f);
        animationManager.PlayAnim ("Attack", 0);

        //play sound
        AudioManager.PlaySound ("basicAttack");

        //calculate damage        
        float damageDone = PlayerUnits[0].calculateDamage ();
        bool isDead = enemyToAttack.TakeDamage (damageDone);

        //damagePopup
        GameObject floatingDamageGO = Instantiate (floatingDamage, enemyToAttack.transform.position + damageOffset, Quaternion.identity);
        if (damageDone > PlayerUnits[0].baseDamage + (PlayerUnits[0].baseDamage / 10)) {
            Color newColor = new Color (0.8679f, 0.2941f, 0f, 1f);
            floatingDamageGO.GetComponent<TextMesh> ().fontSize = 250;
            floatingDamageGO.GetComponent<TextMesh> ().color = newColor;
            floatingDamageGO.GetComponent<TextMesh> ().text = damageDone.ToString ("F0");

            Destroy (floatingDamageGO, 2f);
        } else {
            floatingDamageGO.GetComponent<TextMesh> ().text = damageDone.ToString ("F0");
            Destroy (floatingDamageGO, 1f);
        }
        //**************************************************************************

        // update hp
        uiManager.SetEnemyHP (enemyToAttack.currentHP, enemyID);

        //enemy hit animation
        animationManager.PlayAnim ("Hit", enemyID);
        //enemy hurt sound
        AudioManager.PlaySound ("hurtSound");
        yield return new WaitForSeconds (0.5f);
        //enemy goes back to idle animation
        animationManager.PlayAnim ("Idle", enemyID);
        //unit goes back to its starting position
        Knight.transform.position = startingPos;
        //unit is idling
        animationManager.PlayAnim ("Idle", 0);

        uiManager.HideSkillHUD ();

        if (isDead) {
            animationManager.PlayAnim ("Dead", enemyID);
            gameState = GameState.WON;
            EndBattle ();
        } else {
            //Warrior's turn starts
            unitState = UnitState.WARRIOR;
            //show warrior's skills
            uiManager.ShowSkillHUD ();
        }
    }

    private IEnumerator WarriorBasicAttack (int enemyID) {
        Debug.Log ("Warrior Basic attack");
        //starting position of the unit
        Vector2 startingPos = Warrior.transform.position;

        //play dashing animation
        animationManager.PlayAnim ("Dash", 1);

        Warrior.transform.position += Warrior.transform.right * (Time.deltaTime * 1000);
        float errorDistance = 15f;
        if (Vector3.Distance (Warrior.transform.position, enemyToAttack.transform.position) < errorDistance) {
            Warrior.transform.position = enemyToAttack.transform.position;
        }
        yield return new WaitForSeconds (0.5f);
        animationManager.PlayAnim ("Attack", 1);

        //play sound
        AudioManager.PlaySound ("basicAttack");

        //calculate damage        
        float damageDone = PlayerUnits[1].calculateDamage ();
        bool isDead = enemyToAttack.TakeDamage (damageDone);

        //damagePopup
        GameObject floatingDamageGO = Instantiate (floatingDamage, enemyToAttack.transform.position + damageOffset, Quaternion.identity);
        if (damageDone > PlayerUnits[1].baseDamage + (PlayerUnits[1].baseDamage / 10)) {
            Color newColor = new Color (0.8679f, 0.2941f, 0f, 1f);
            floatingDamageGO.GetComponent<TextMesh> ().fontSize = 250;
            floatingDamageGO.GetComponent<TextMesh> ().color = newColor;
            floatingDamageGO.GetComponent<TextMesh> ().text = damageDone.ToString ("F0");

            Destroy (floatingDamageGO, 2f);
        } else {
            floatingDamageGO.GetComponent<TextMesh> ().text = damageDone.ToString ("F0");
            Destroy (floatingDamageGO, 1f);
        }

        // update hp
        uiManager.SetEnemyHP (enemyToAttack.currentHP, enemyID);

        //enemy hit animation
        animationManager.PlayAnim ("Hit", enemyID);
        //enemy hurt sound
        AudioManager.PlaySound ("hurtSound");
        //enemy goes back to idle animation
        yield return new WaitForSeconds (1f);
        animationManager.PlayAnim ("Idle", enemyID);

        //unit goes back to its starting position
        Warrior.transform.position = startingPos;
        //unit is idling
        animationManager.PlayAnim ("Idle", 1);

        uiManager.HideSkillHUD ();

        if (isDead) {
            gameState = GameState.WON;
            EndBattle ();
        } else {
            unitState = UnitState.WIZARD;
            uiManager.ShowSkillHUD ();
        }
    }

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
            floatingDamageGO.GetComponent<TextMesh> ().fontSize = 250;
            floatingDamageGO.GetComponent<TextMesh> ().color = newColor;
            floatingDamageGO.GetComponent<TextMesh> ().text = damageDone.ToString ("F0");

            Destroy (floatingDamageGO, 2f);
        } else {
            floatingDamageGO.GetComponent<TextMesh> ().text = damageDone.ToString ("F0");
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
            floatingDamageGO.GetComponent<TextMesh> ().fontSize = 250;
            floatingDamageGO.GetComponent<TextMesh> ().color = newColor;
            floatingDamageGO.GetComponent<TextMesh> ().text = damageDone.ToString ("F0");

            Destroy (floatingDamageGO, 2f);
        } else {
            floatingDamageGO.GetComponent<TextMesh> ().text = damageDone.ToString ("F0");
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
            floatingDamageGO.GetComponent<TextMesh> ().fontSize = 250;
            floatingDamageGO.GetComponent<TextMesh> ().color = newColor;
            floatingDamageGO.GetComponent<TextMesh> ().text = damageDone.ToString ("F0");

            Destroy (floatingDamageGO, 2f);
        } else {
            floatingDamageGO.GetComponent<TextMesh> ().text = damageDone.ToString ("F0");
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
            floatingDamageGO.GetComponent<TextMesh> ().fontSize = 250;
            floatingDamageGO.GetComponent<TextMesh> ().color = newColor;
            floatingDamageGO.GetComponent<TextMesh> ().text = damageDone.ToString ("F0");

            Destroy (floatingDamageGO, 2f);
        } else {
            floatingDamageGO.GetComponent<TextMesh> ().text = damageDone.ToString ("F0");
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

    private void EndBattle () {
        if (gameState == GameState.WON) {
            //load next level
        } else if (gameState == GameState.LOST) {
            //reload this level
        }
    }
}