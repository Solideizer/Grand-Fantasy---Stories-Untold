using System.Collections;
using Managers;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Characters
{
	public class Enemy2 : Unit
	{
		#region Variable Declerations

#pragma warning disable 0649
		[SerializeField] private GameObject enemyGO;
#pragma warning restore 0649
		private Vector2 _enemyStartingPosition;
		private Transform _camTransform;

		#endregion

		private void Start ()
		{
			_camTransform = Camera.main.transform;
		}

		public IEnumerator Enemy2Turn ()
		{
			Debug.Log (unitData._currentHp);
			if (unitData._currentHp <= 0)
			{
				BattleSystemClass.gameState = GameState.PLAYERTURN;
				BattleSystemClass.unitState = UnitState.KNIGHT;
				UIManager.EnableKnightSkillBar ();
			}

			_enemyStartingPosition = enemyGO.transform.position;
			int randomPlayerUnitIndex = Random.Range (0, UIManager.playerUnitGOs.Count);
			GameObject attackedPlayerGO = UIManager.playerUnitGOs[randomPlayerUnitIndex];
			Unit attackedPlayerUnit = attackedPlayerGO.GetComponent<Unit> ();
			Vector3 playerPos = attackedPlayerGO.transform.position;

			AnimationManager.PlayAnim ("Dash", 5);
			enemyGO.transform.Translate (-Time.deltaTime * 1000, 0, 0, _camTransform);
			if (Vector3.Distance (enemyGO.transform.position, playerPos) < unitData.errorDistance)
			{
				enemyGO.transform.position = new Vector3 (playerPos.x + 2f, playerPos.y, playerPos.z);
			}

			yield return new WaitForSeconds (0.5f);
			//StartCoroutine(CameraManager.MoveTowardsTarget(attackedPlayerGO));
			AnimationManager.PlayAnim ("Attack", 5);
			AudioManager.PlaySound ("basicAttack");
			AnimationManager.PlayAnim ("Hit", randomPlayerUnitIndex);

			//calculate damage
			float damageDone = CalculationManager.CalculateDamage (unitData);
			bool isDead = CalculationManager.DealDamage (damageDone, attackedPlayerUnit);

			//damagePopup
			GameObject cloneTextGO = Instantiate (unitData.floatingDamagePrefab,
				attackedPlayerGO.transform.position + unitData.damageOffset, Quaternion.identity);

			UIManager.DamagePopup (damageDone, unitData, cloneTextGO);

			UIManager.SetPlayerUnitHp (attackedPlayerUnit.unitData._currentHp, randomPlayerUnitIndex);
			AudioManager.PlaySound ("hurtSound");

			yield return new WaitForSeconds (0.5f);
			AnimationManager.PlayAnim ("Idle", randomPlayerUnitIndex);

			enemyGO.transform.position = _enemyStartingPosition;
			AnimationManager.PlayAnim ("Idle", 5);

			if (isDead)
			{
				AudioManager.PlaySound ("KnightDeath");
				AnimationManager.PlayAnim ("Dead", randomPlayerUnitIndex);

			}
			BattleSystemClass.gameState = GameState.PLAYERTURN;
			BattleSystemClass.unitState = UnitState.KNIGHT;
			UIManager.EnableKnightSkillBar ();

		}

	}
}