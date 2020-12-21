using Characters;
using UnityEngine;

namespace Managers
{
    public class TurnManager : MonoBehaviour
    {
        private BattleSystem BattleSystem;
        private UIManager UIManager;
        private void Awake ()
        {
            UIManager = GetComponent<UIManager> ();
            BattleSystem = GetComponent<BattleSystem> ();

        }
        private void OnEnable ()
        {
            Knight.UnitDied += TurnManager_UnitDied;
            Warrior.UnitDied += TurnManager_UnitDied;
            Wizard.UnitDied += TurnManager_UnitDied;
        }
        private void OnDisable ()
        {
            Knight.UnitDied -= TurnManager_UnitDied;
            Warrior.UnitDied -= TurnManager_UnitDied;
            Wizard.UnitDied -= TurnManager_UnitDied;
        }

        private void TurnManager_UnitDied (int unitID)
        {
            switch (unitID)
            {
                case 0: // knight is dead
                    Debug.Log ("knight is dead");
                    UIManager.DisableKnightUI ();
                    if (BattleSystem.gameState == GameState.PLAYERTURN &&
                        BattleSystem.unitState == UnitState.KNIGHT)
                    {
                        BattleSystem.unitState++;
                    }
                    break;
                case 1: // warrior is dead
                    Debug.Log ("warrior is dead");
                    UIManager.DisableWarriorUI ();
                    if (BattleSystem.gameState == GameState.PLAYERTURN &&
                        BattleSystem.unitState == UnitState.WARRIOR)
                    {
                        BattleSystem.unitState++;
                    }
                    break;
                case 2: // wizard is dead
                    Debug.Log ("wizard is dead");
                    UIManager.DisableWizardUI ();
                    if (BattleSystem.gameState == GameState.PLAYERTURN &&
                        BattleSystem.unitState == UnitState.WIZARD)
                    {
                        BattleSystem.unitState++;
                        BattleSystem.gameState = GameState.ENEMYTURN;
                    }
                    break;
                case 4: // enemy1 is dead
                    Debug.Log ("enemy1 is dead");
                    UIManager.DisableEnemy1UI ();
                    if (BattleSystem.gameState == GameState.ENEMYTURN &&
                        BattleSystem.unitState == UnitState.ENEMY1)
                    {
                        BattleSystem.unitState++;
                        BattleSystem.gameState = GameState.ENEMYTURN;
                    }
                    break;
                case 5: // enemy2 is dead
                    Debug.Log ("enemy2 is dead");
                    UIManager.DisableEnemy2UI ();
                    if (BattleSystem.gameState == GameState.ENEMYTURN &&
                        BattleSystem.unitState == UnitState.ENEMY2)
                    {
                        BattleSystem.unitState = UnitState.KNIGHT;
                        BattleSystem.gameState = GameState.PLAYERTURN;
                    }
                    break;
            }
        }

    }
}