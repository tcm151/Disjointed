using UnityEngine;


namespace Disjointed.Combat.Enemies
{
    public class Bat : FlyingEnemy
    {
        override protected void Update()
        {
            base.Update();

            SetAnimationState("Flying", IsMoving);
        }
    }
}