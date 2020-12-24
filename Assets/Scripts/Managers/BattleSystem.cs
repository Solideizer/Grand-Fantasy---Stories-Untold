using Characters_Skills;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Managers
{
	public enum GameState
	{
		START,
		PLAYERTURN,
		ENEMYTURN,
		WAITING,
		WON,
		LOST
	}

	public enum UnitState
	{
		KNIGHT,
		WARRIOR,
		WIZARD,
		ENEMY1,
		ENEMY2
	}

	public class BattleSystem : MonoBehaviour
	{
		#region Variable Declarations		
		public GameObject enemy1;
		public GameObject enemy2;

		public GameState gameState;
		public UnitState unitState;

		private RaycastHit2D _hit;
		private Vector3 _mousePos;
		private Vector2 _mousePos2D;

		private Knight _knightClass;
		private Warrior _warriorClass;
		private Wizard _wizardClass;
		private Camera _cameraMain;

		#endregion

		#region Awake-Start-Update

		private void Awake ()
		{
			_cameraMain = Camera.main;
			_knightClass = GetComponent<Knight> ();
			_warriorClass = GetComponent<Warrior> ();
			_wizardClass = GetComponent<Wizard> ();
			
			gameState = GameState.START;
			gameState = GameState.PLAYERTURN;
			unitState = UnitState.KNIGHT;
		}

		private void Update ()
		{
			SelectTarget ();
		}

		#endregion

		private void SelectTarget ()
		{
			if (!Input.GetMouseButtonDown (0) || EventSystem.current.IsPointerOverGameObject () || gameState != GameState.PLAYERTURN) return;

			_mousePos = _cameraMain.ScreenToWorldPoint (Input.mousePosition);
			_mousePos2D = new Vector2 (_mousePos.x, _mousePos.y);
			_hit = Physics2D.Raycast (_mousePos2D, Vector2.zero);
		}
		public void CastSingleTargetSkill (int skillIndex)
		{
			if (!EventSystem.current.IsPointerOverGameObject () || gameState != GameState.PLAYERTURN) return;
			SelectEnemy (_hit, skillIndex);
		}
		public void CastTargetlessSkill ()
		{
			if (!EventSystem.current.IsPointerOverGameObject () || gameState != GameState.PLAYERTURN) return;

			switch (unitState)
			{
				case UnitState.KNIGHT:
					StartCoroutine (_knightClass.KnightSkill2 ());
					break;

			}
			switch (unitState)
			{
				case UnitState.WIZARD:
					StartCoroutine (_wizardClass.WizardAreaAttack ());
					break;

			}
		}

		private void SelectEnemy (RaycastHit2D hit, int skillIndex)
		{
			if (gameState != GameState.PLAYERTURN) return;

			string enemyName = hit.collider.gameObject.name;
			int enemyId;

			switch (unitState)
			{
				case UnitState.KNIGHT:

					if (enemyName == "Skeleton")
					{
						enemyId = 4;
						switch (skillIndex)
						{
							case 0:
								StartCoroutine (_knightClass.KnightBasicAttack (enemyId, enemy1));
								break;

						}

					}

					if (enemyName == "Undead")
					{
						enemyId = 5;
						switch (skillIndex)
						{
							case 0:
								StartCoroutine (_knightClass.KnightBasicAttack (enemyId, enemy2));
								break;

						}

					}

					break;

				case UnitState.WARRIOR:
					if (enemyName == "Skeleton")
					{
						enemyId = 4;
						StartCoroutine (_warriorClass.WarriorBasicAttack (enemyId, enemy1));

					}

					if (enemyName == "Undead")
					{
						enemyId = 5;
						StartCoroutine (_warriorClass.WarriorBasicAttack (enemyId, enemy2));

					}

					break;

				case UnitState.WIZARD:
					if (enemyName == "Skeleton")
					{
						enemyId = 4;
						switch (skillIndex)
						{
							case 0:
								StartCoroutine (_wizardClass.WizardBasicAttack (enemyId, enemy1));
								break;

						}

					}

					if (enemyName == "Undead")
					{
						enemyId = 5;
						switch (skillIndex)
						{
							case 0:
								StartCoroutine (_wizardClass.WizardBasicAttack (enemyId, enemy2));
								break;

						}

					}

					break;
			}
		}

		public void EndBattle ()
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
}