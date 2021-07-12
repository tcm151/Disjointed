using UnityEngine;

namespace OGAM
{
    [CreateAssetMenu(fileName = "EnemyStatsObject", menuName = "EnemyStatsObject")]
    public class EnemyStatsObject : ScriptableObject
    {
        public float health = 3f, speed = 2f, knockbackMultiplier = 1f;
        public enum movementType
        {
            Walking,
            Flying,
            Stationary
        }
        public enum behaviorTowardsPlayer
        {
            Homing, //Moves straight towards player with no regard for safety
            Intelligent, //Backs off when low on health, stays at optimal fighting distance
            Ignore
        }
    }
}