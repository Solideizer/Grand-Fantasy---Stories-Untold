using System.Collections.Generic;
using Characters;
using UnityEngine;

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

#pragma warning disable 0649
		[SerializeField] private GameObject explosionAnim;
		[SerializeField] private GameObject floatingDamage;
		[SerializeField] private Vector3 damageOffset = new Vector3(0f, 3f, 0f);
#pragma warning restore 0649

		public GameObject Knight;
		public GameObject Warrior;
		public GameObject Wizard;
		public GameObject Enemy1;
		public GameObject Enemy2;

		public GameState gameState;
		public UnitState unitState;

		[HideInInspector] public List<GameObject> playerUnitGOs = new List<GameObject>();
		[HideInInspector] public List<GameObject> enemyUnitGOs = new List<GameObject>();
		[HideInInspector] public List<Unit> playerUnits = new List<Unit>();
		[HideInInspector] public List<Unit> enemyUnits = new List<Unit>();

		private RaycastHit2D _hit;
		private Vector3 _mousePos;
		private Vector2 _mousePos2D;
		private UnitData _attackedPlayerUnitData;

		private AnimationManager _animationManager;
		private UIManager _uiManager;
		private CalculationManager _calculationManager;

		private Knight _knightClass;
		private Warrior _warriorClass;
		private Wizard _wizardClass;
		private Camera _cameraMain;

		#endregion

		#region Awake-Start-Update

		private void Awake()
		{
			_cameraMain = Camera.main;
			_animationManager = GetComponent<AnimationManager>();
			_uiManager = GetComponent<UIManager>();
			_calculationManager = GetComponent<CalculationManager>();

			_knightClass = GetComponent<Knight>();
			_warriorClass = GetComponent<Warrior>();
			_wizardClass = GetComponent<Wizard>();
		}

		private void Start()
		{
			gameState = GameState.START;
			PopulateUnitList();
			PopulateGameobjetList();
			gameState = GameState.PLAYERTURN;
			unitState = UnitState.KNIGHT;
		}

		private void Update()
		{
			SelectTarget();
		}

		#endregion

		private void PopulateUnitList()
		{
			playerUnits.Add(Knight.GetComponent<Unit>());
			playerUnits.Add(Warrior.GetComponent<Unit>());
			playerUnits.Add(Wizard.GetComponent<Unit>());
			enemyUnits.Add(Enemy1.GetComponent<Unit>());
			enemyUnits.Add(Enemy2.GetComponent<Unit>());
		}

		private void PopulateGameobjetList()
		{
			playerUnitGOs.Add(Knight);
			playerUnitGOs.Add(Warrior);
			playerUnitGOs.Add(Wizard);
		}

		private void SelectTarget()
		{
			if (!Input.GetMouseButtonDown(0) || gameState != GameState.PLAYERTURN) return;

			_mousePos = _cameraMain.ScreenToWorldPoint(Input.mousePosition);
			_mousePos2D = new Vector2(_mousePos.x, _mousePos.y);
			_hit = Physics2D.Raycast(_mousePos2D, Vector2.zero);
			SelectEnemy(_hit);
		}

		private void SelectEnemy(RaycastHit2D hit)
		{
			if (gameState != GameState.PLAYERTURN)
			{
				return;
			}

			string enemyName = hit.collider.gameObject.name;
			int enemyId;

			switch (unitState)
			{
				case UnitState.KNIGHT:
					if (enemyName == "Skeleton")
					{
						enemyId = 4;
						_knightClass.KnightAttack(enemyId, Enemy1);
					}

					if (enemyName == "Undead")
					{
						enemyId = 5;
						_knightClass.KnightAttack(enemyId, Enemy2);
					}

					break;

				case UnitState.WARRIOR:
					if (enemyName == "Skeleton")
					{
						enemyId = 4;
						_warriorClass.WarriorAttack(enemyId, Enemy1);
					}

					if (enemyName == "Undead")
					{
						enemyId = 5;
						_warriorClass.WarriorAttack(enemyId, Enemy2);
					}

					break;

				case UnitState.WIZARD:
					if (enemyName == "Skeleton")
					{
						enemyId = 4;
						_wizardClass.WizardAttack(enemyId, Enemy1);
					}

					if (enemyName == "Undead")
					{
						enemyId = 5;
						_wizardClass.WizardAttack(enemyId, Enemy2);
					}

					break;
			}
		}

		public void EndBattle()
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