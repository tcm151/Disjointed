using UnityEngine;


namespace OGAM.Player
{
    public class Movement : MonoBehaviour
    {
        new private Rigidbody2D rigidbody;
        private bool jumping;


        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            jumping |= Input.GetKeyDown(KeyCode.Space);

            if (jumping)
            {
                jumping = false;
                rigidbody.velocity += 5f * Vector2.up;
            }
            
            var velocity = rigidbody.velocity;
            velocity.x = 4f * Input.GetAxis("Horizontal");
            rigidbody.velocity = velocity;



        }
    }
}
