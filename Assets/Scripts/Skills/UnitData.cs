using UnityEngine;

namespace Characters
{
    [CreateAssetMenu (fileName = "Character", menuName = "Character")]
    public class UnitData : ScriptableObject
    {
        public float _maxHp;
        public float _currentHp;
        public float _baseDamage;
        public float _baseArmor;
        public float _criticalStrikeChance;
        public string[] criticalDialogue;

        public GameObject floatingDamagePrefab;
        public Vector3 damageOffset = new Vector3 (0f, 3f, 0f);
        public float errorDistance = 0.5f;
    }
}