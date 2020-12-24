using UnityEngine;

namespace Characters_Skills
{
    [CreateAssetMenu (fileName = "Character", menuName = "Character")]
    public class UnitData : ScriptableObject
    {
        [HideInInspector] public float currentHp;
        public float maxHp;
        public float baseDamage;
        public float baseArmor;
        public float criticalStrikeChance;
        public string[] criticalDialogue;

        public GameObject floatingDamagePrefab;
        public Vector3 damageOffset = new Vector3 (0f, 3f, 0f);
        public float errorDistance = 0.5f;
    }
}