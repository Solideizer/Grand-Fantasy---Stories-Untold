using Characters;
using UnityEngine;

namespace Managers
{
    public class TurnManager : MonoBehaviour
    {
        private BattleSystem _battleSystem;
        private UIManager _uiManager;
        private void Awake ()
        {
            _uiManager = GetComponent<UIManager> ();
            _battleSystem = GetComponent<BattleSystem> ();

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
                    _uiManager.DisableKnightUI ();
                    if (_battleSystem.gameState == GameState.PLAYERTURN &&
                        _battleSystem.unitState == UnitState.KNIGHT)
                    {
                        _battleSystem.unitState++;
                    }
                    break;
                case 1: // warrior is dead
                    Debug.Log ("warrior is dead");
                    _uiManager.DisableWarriorUI ();
                    if (_battleSystem.gameState == GameState.PLAYERTURN &&
                        _battleSystem.unitState == UnitState.WARRIOR)
                    {
                        _battleSystem.unitState++;
                    }
                    break;
                case 2: // wizard is dead
                    Debug.Log ("wizard is dead");
                    _uiManager.DisableWizardUI ();
                    if (_battleSystem.gameState == GameState.PLAYERTURN &&
                        _battleSystem.unitState == UnitState.WIZARD)
                    {
                        _battleSystem.unitState++;
                        _battleSystem.gameState = GameState.ENEMYTURN;
                    }
                    break;
                case 4: // enemy1 is dead
                    Debug.Log ("enemy1 is dead");
                    _uiManager.DisableEnemy1UI ();
                    if (_battleSystem.gameState == GameState.ENEMYTURN &&
                        _battleSystem.unitState == UnitState.ENEMY1)
                    {
                        _battleSystem.unitState++;
                        _battleSystem.gameState = GameState.ENEMYTURN;
                    }
                    break;
                case 5: // enemy2 is dead
                    Debug.Log ("enemy2 is dead");
                    _uiManager.DisableEnemy2UI ();
                    if (_battleSystem.gameState == GameState.ENEMYTURN &&
                        _battleSystem.unitState == UnitState.ENEMY2)
                    {
                        _battleSystem.unitState = UnitState.KNIGHT;
                        _battleSystem.gameState = GameState.PLAYERTURN;
                    }
                    break;
            }
        }

    }
}