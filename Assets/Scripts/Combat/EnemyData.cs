using UnityEngine;
using UnityEngine.Serialization;


namespace Disjointed.Combat.Enemies
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "EnemyData")]
    public class EnemyData : ScriptableObject
    {
        public float health = 3f;
        public float acceleration = 10f;
        public float movementSpeed = 2f;
        public float detectionRadius = 5f;
        public int damage = 1;
        public float knockback = 5f;
        
        public Enemy.Aggro aggro;
        public Enemy.Movement movement;
    }
}