using UnityEngine;


namespace Disjointed.Combat.Enemies
{
    public class Rat : WalkingEnemy
    {
        override protected void Update()
        {
            base.Update();

            SetAnimationState("Walking", IsMoving);
        }
    }
}