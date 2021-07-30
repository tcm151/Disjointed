using UnityEngine;


namespace Disjointed.Combat.Enemies
{
    public class Bat : Enemy
    {
        override protected void Update()
        {
            base.Update();

            SetAnimationState("Flying", IsMoving);
        }
    }
}