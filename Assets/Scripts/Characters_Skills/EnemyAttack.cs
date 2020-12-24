using System.Collections;
using Managers;
using UnityEngine;

namespace Characters_Skills
{

	public class EnemyAttack : Unit
	{

#pragma warning disable 0649
		[SerializeField] private GameObject enemyGO;
#pragma warning restore 0649		
		private Enemy2 _enemy2Class;
		private Vector2 _enemyStartingPosition;
		
		protected override void Awake ()
		{
			base.Awake ();
			_enemy2Class = GetComponent<Enemy2> ();
			
		}

		public IEnumerator EnemyTurn ()
		{
			Debug.Log (unitData.currentHp);
			if (unitData.currentHp <= 0)
			{
				BattleSystemClass.gameState = GameState.ENEMYTURN;
				BattleSystemClass.unitState = UnitState.ENEMY2;
				StartCoroutine (_enemy2Class.Enemy2Turn ());
			}

			_enemyStartingPosition = enemyGO.transform.position;
			int randomPlayerUnitIndex = Random.Range (0, UIManager.playerUnitGOs.Count);
			GameObject attackedPlayerGO = UIManager.playerUnitGOs[randomPlayerUnitIndex];
			Unit attackedPlayerUnit = attackedPlayerGO.GetComponent<Unit> ();
			Vector3 playerPos = attackedPlayerGO.transform.position;

			AnimationManager.PlayAnim ("Dash", 4);

			enemyGO.transform.Translate (-Time.deltaTime * 1000, 0, 0, CameraTransform);
			if (Vector3.Distance (enemyGO.transform.position, playerPos) < unitData.errorDistance)
			{
				enemyGO.transform.position = new Vector3 (playerPos.x + 2f, playerPos.y, playerPos.z);
			}

			yield return new WaitForSeconds (0.5f);
			//StartCoroutine(CameraManager.MoveTowardsTarget(attackedPlayerGO));
			AnimationManager.PlayAnim ("Attack", 4);
			AudioManager.PlaySound ("basicAttack");
			AnimationManager.PlayAnim ("Hit", randomPlayerUnitIndex);

			//calculate damage
			float damageDone = CalculationManager.CalculateDamage (unitData);
			bool isDead = CalculationManager.DealDamage (damageDone, attackedPlayerUnit);

			//damagePopup
			GameObject cloneTextGO = Instantiate (unitData.floatingDamagePrefab, playerPos + unitData.damageOffset, Quaternion.identity);
			

			UIManager.DamagePopup (damageDone, unitData, cloneTextGO);

			UIManager.SetPlayerUnitHp (attackedPlayerUnit.unitData.currentHp, randomPlayerUnitIndex);
			AudioManager.PlaySound ("hurtSound");

			yield return new WaitForSeconds (0.5f);
			AnimationManager.PlayAnim ("Idle", randomPlayerUnitIndex);

			enemyGO.transform.position = _enemyStartingPosition;
			AnimationManager.PlayAnim ("Idle", 4);

			if (isDead)
			{
				AudioManager.PlaySound ("KnightDeath");
				AnimationManager.PlayAnim ("Dead", randomPlayerUnitIndex);

			}

			BattleSystemClass.gameState = GameState.ENEMYTURN;
			BattleSystemClass.unitState = UnitState.ENEMY2;
			yield return new WaitForSeconds (0.5f);
			StartCoroutine (_enemy2Class.Enemy2Turn ());

		}

	}
}