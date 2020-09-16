using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GameState
{
	START,
	PLAYERTURN,
	ENEMYTURN,
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

	public List<GameObject> playerUnitGOs = new List<GameObject>();
	public List<GameObject> enemyUnitGOs = new List<GameObject>();
	public List<Unit> playerUnits = new List<Unit>();
	public List<Unit> enemyUnits = new List<Unit>();

	private RaycastHit2D _hit;
	private Vector3 _mousePos;
	private Vector2 _mousePos2D;

	private UnitData _attackedPlayerUnitData;

	//************** Managers *****************
	private AnimationManager _animationManager;
	private UIManager _uiManager;
	private CalculationManager _calculationManager;
	//********** END Managers *****************

	private Knight _knightClass;
	private Warrior _warriorClass;
	private Wizard _wizardClass;

	//PlayerUnit 0 --> Knight --> UnitID 0
	//PlayerUnit 1 --> Warrior--> UnitID 1
	//PlayerUnit 2 --> Wizard --> UnitID 2
	//PlayerUnit 3 --> ****** --> UnitID 3

	//EnemyUnit0 --> Skeleton --> UnitID 4
	//EnemyUnit1 --> ******** --> UnitID 5
	
	private void Awake()
	{
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
		if (Input.GetMouseButtonDown(0) && gameState == GameState.PLAYERTURN)
		{
			_mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			_mousePos2D = new Vector2(_mousePos.x, _mousePos.y);
			_hit = Physics2D.Raycast(_mousePos2D, Vector2.zero);
			SelectEnemy(_hit);
		}
	}


	public void SelectEnemy(RaycastHit2D hit)
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

	//*********************************** ENEMY TURNS *****************************************
	
	public IEnumerator Enemy2Turn()
	{
		Vector2 startingPos = Enemy2.transform.position;

		int randomPlayerUnitIndex = Random.Range(0, playerUnitGOs.Count);
		GameObject _attackedPlayerGO = playerUnitGOs[randomPlayerUnitIndex];

		_animationManager.PlayAnim("Dash", 5);

		Enemy2.transform.position += -_attackedPlayerGO.transform.right * (Time.deltaTime * 500);
		float errorDistance = 10f;

		if (Vector3.Distance(_attackedPlayerGO.transform.position, Enemy2.transform.position) < errorDistance)
		{
			Enemy2.transform.position = _attackedPlayerGO.transform.position;
		}

		yield return new WaitForSeconds(0.5f);
		_animationManager.PlayAnim("Attack", 5);
		AudioManager.PlaySound("basicAttack");
		_animationManager.PlayAnim("Hit", randomPlayerUnitIndex);

		//calculate damage
		float damageDone = _calculationManager.CalculateDamage(enemyUnits[1].unitData);
		bool isDead = _calculationManager.TakeDamage(damageDone, _attackedPlayerGO.GetComponent<Unit>());

		//damagePopup ******************************************************************************          

		GameObject floatingDamageGO = Instantiate(floatingDamage, _attackedPlayerGO.transform.position + damageOffset,
			Quaternion.identity);
		if (damageDone > enemyUnits[1].unitData._baseDamage + (enemyUnits[1].unitData._baseDamage / 10))
		{
			Color newColor = new Color(0.8679f, 0.2941f, 0f, 1f);
			floatingDamageGO.GetComponent<TextMeshPro>().fontSize = 250;
			floatingDamageGO.GetComponent<TextMeshPro>().color = newColor;
			floatingDamageGO.GetComponent<TextMeshPro>().text = damageDone.ToString("F0");

			Destroy(floatingDamageGO, 2f);
		}
		else
		{
			floatingDamageGO.GetComponent<TextMeshPro>().text = damageDone.ToString("F0");
			Destroy(floatingDamageGO, 1f);
		}
		//******************************************************************************************

		_uiManager.SetPlayerUnitHP(_attackedPlayerGO.GetComponent<Unit>().currentHp, randomPlayerUnitIndex);
		AudioManager.PlaySound("hurtSound");

		yield return new WaitForSeconds(0.5f);
		_animationManager.PlayAnim("Idle", randomPlayerUnitIndex);

		Enemy2.transform.position = startingPos;
		_animationManager.PlayAnim("Idle", 5);

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