using System.Collections;
using TMPro;
using UnityEngine;
public class Knight : MonoBehaviour {
    [SerializeField] private GameObject knightGO;
    [SerializeField] private GameObject floatingDamage;
    [SerializeField] private Vector3 damageOffset = new Vector3 (0f, 3f, 0f);
    private AnimationManager animationManager;
    private UIManager uiManager;
    private Unit knightUnit;
    private BattleSystem battleSystemClass;
    public Unit enemyToAttack;
    private void Awake () {
        knightUnit = knightGO.GetComponent<Unit> ();
        animationManager = GetComponent<AnimationManager> ();
        uiManager = GetComponent<UIManager> ();
        battleSystemClass = GetComponent<BattleSystem> ();

    }
    public IEnumerator KnightBasicAttack (int enemyID) {

        AudioManager.PlaySound ("KnightOpeners");

        Vector2 startingPos = knightGO.transform.position;
        //play dashing animation
        animationManager.PlayAnim ("Dash", 0);

        //Knight.transform.position += Knight.transform.right * (Time.deltaTime * 1000);
        knightGO.transform.Translate (Vector3.right);
        float errorDistance = 15f;

        if (Vector3.Distance (knightGO.transform.position, enemyToAttack.transform.position) < errorDistance) {
            knightGO.transform.position = enemyToAttack.transform.position;
        }
        yield return new WaitForSeconds (0.5f);
        animationManager.PlayAnim ("Attack", 0);

        //play sound
        AudioManager.PlaySound ("basicAttack");

        //calculate damage        
        float damageDone = knightUnit.calculateDamage ();
        bool isDead = enemyToAttack.TakeDamage (damageDone);

        //damagePopup
        GameObject floatingDamageGO = Instantiate (floatingDamage, enemyToAttack.transform.position + damageOffset, Quaternion.identity);
        TextMeshPro damageText = floatingDamage.GetComponent<TextMeshPro> ();
        uiManager.DamagePopup (damageDone, knightUnit, damageText);

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
        knightGO.transform.position = startingPos;
        //unit is idling
        animationManager.PlayAnim ("Idle", 0);

        uiManager.HideSkillHUD ();

        if (isDead) {
            animationManager.PlayAnim ("Dead", enemyID);
            battleSystemClass.gameState = GameState.WON;
            battleSystemClass.EndBattle ();
        } else {
            //Warrior's turn starts
            battleSystemClass.unitState = UnitState.WARRIOR;
            //show warrior's skills
            uiManager.ShowSkillHUD ();
        }
    }
    public void KnightAttack (int enemyID) {
        StartCoroutine (KnightBasicAttack (enemyID));
    }

}