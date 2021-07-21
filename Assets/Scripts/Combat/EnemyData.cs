using UnityEngine;

namespace Disjointed
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "EnemyData")]
    public class EnemyData : ScriptableObject
    {
        public float health = 3f;
        public float acceleration = 10f;
        public float topSpeed = 2f;
        public float knockbackMultiplier = 1f;
        
        public enum MovementType
        {
            Walking,
            Flying,
            Stationary,
        }
        public enum Aggro
        {
            Ignore,
            Homing, // Moves straight towards player with no regard for safety
            Intelligent, //Backs off when low on health, stays at optimal fighting distance
        }
    }
}