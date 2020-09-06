using System.Collections;
using TMPro;
using UnityEngine;
public class Warrior : MonoBehaviour {
    [SerializeField] private GameObject warriorGO;
    [SerializeField] private GameObject floatingDamage;
    [SerializeField] private Vector3 damageOffset = new Vector3 (0f, 3f, 0f);
    private AnimationManager animationManager;
    private UIManager uiManager;
    private Unit warriorUnit;
    private BattleSystem battleSystemClass;
    public Unit enemyToAttack;
    private void Awake () {
        warriorUnit = warriorGO.GetComponent<Unit> ();
        animationManager = GetComponent<AnimationManager> ();
        uiManager = GetComponent<UIManager> ();
        battleSystemClass = GetComponent<BattleSystem> ();

    }
    private IEnumerator WarriorBasicAttack (int enemyID) {

        Vector2 startingPos = warriorGO.transform.position;

        //play dashing animation
        animationManager.PlayAnim ("Dash", 1);

        warriorGO.transform.Translate (Vector3.right);
        float errorDistance = 15f;

        if (Vector3.Distance (warriorGO.transform.position, enemyToAttack.transform.position) < errorDistance) {
            warriorGO.transform.position = enemyToAttack.transform.position;
        }
        yield return new WaitForSeconds (0.5f);
        animationManager.PlayAnim ("Attack", 1);

        //play sound
        AudioManager.PlaySound ("basicAttack");

        //calculate damage        
        float damageDone = warriorUnit.calculateDamage ();
        bool isDead = enemyToAttack.TakeDamage (damageDone);

        //damagePopup
        GameObject floatingDamageGO = Instantiate (floatingDamage, enemyToAttack.transform.position + damageOffset, Quaternion.identity);
        TextMeshPro damageText = floatingDamage.GetComponent<TextMeshPro> ();
        uiManager.DamagePopup (damageDone, warriorUnit, damageText);

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
        warriorGO.transform.position = startingPos;
        //unit is idling
        animationManager.PlayAnim ("Idle", 1);

        uiManager.HideSkillHUD ();

        if (isDead) {
            animationManager.PlayAnim ("Dead", enemyID);
            battleSystemClass.gameState = GameState.WON;
            battleSystemClass.EndBattle ();
        } else {
            battleSystemClass.unitState = UnitState.WIZARD;
            uiManager.ShowSkillHUD ();
        }
    }
    public void WarriorAttack (int enemyID) {
        StartCoroutine (WarriorBasicAttack (enemyID));
    }

}