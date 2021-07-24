using UnityEngine;

namespace Disjointed
{
    [CreateAssetMenu(fileName = "EnemyTemplate", menuName = "EnemyTemplate")]
    public class EnemyTemplate : ScriptableObject
    {
        public float health = 3f;
        public float acceleration = 10f;
        public float movementSpeed = 2f;
        public float detectionRadius = 5f;
        public int damage = 1;
        public float knockback = 5f;
        
        public Aggro aggro;
        public MovementType movementType;
        
        public enum Aggro
        {
            Ignore,
            Homing, // Moves straight towards player with no regard for safety
            Intelligent, //Backs off when low on health, stays at optimal fighting distance
        }
        public enum MovementType
        {
            Walking,
            Flying,
            Stationary,
        }
    }
}