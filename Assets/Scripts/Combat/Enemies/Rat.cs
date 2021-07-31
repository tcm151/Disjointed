using UnityEngine;


namespace Disjointed.Combat.Enemies
{
    public class Rat : Enemy
    {
        override protected void Update()
        {
            base.Update();

            SetAnimationState("Walking", IsMoving);
        }
    }
}