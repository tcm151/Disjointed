using Disjointed.Tools.Extensions;
using UnityEngine;


namespace Disjointed.Combat.Enemies
{
    public class FlyingEnemy : Enemy
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
            if (target)
            {
                var targetDirection = (target.position - data.position).normalized;
                desiredVelocity.MoveTowards(targetDirection * data.movementSpeed, data.acceleration * Time.deltaTime);
            }
            else
            {
                var targetDirection = data.position.DirectionTo(initialPosition);
                if (Vector2.Distance(data.position, initialPosition) < 0.1f) transform.position = initialPosition;
                else desiredVelocity.MoveTowards(targetDirection * data.movementSpeed, data.acceleration * Time.deltaTime);
            }
        }
    }
}