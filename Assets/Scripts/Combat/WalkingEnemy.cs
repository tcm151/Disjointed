using Disjointed.Tools.Extensions;
using UnityEngine;


namespace Disjointed.Combat.Enemies
{
    public class WalkingEnemy : Enemy
    {
        private void FixedUpdate()
        {
            desiredVelocity = rigidbody.velocity;
            data.position = transform.position;

            DetectPlayer();
            Move();

            rigidbody.velocity = desiredVelocity;
        }

        override protected void Move()
        {
            if (!target) return;

            var hit = Physics2D.Raycast(data.position, Vector2.down, groundedDistance, groundMask);
            onGround = hit.collider is { };

            var targetDirection = (target.position - data.position).normalized;
            desiredVelocity.x.MoveTowards(targetDirection.x * data.movementSpeed, data.acceleration * Time.deltaTime);
        }
    }
}