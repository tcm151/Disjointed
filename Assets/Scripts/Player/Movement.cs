using UnityEngine;
using UnityEngine.InputSystem;


namespace OGAM.Player
{
    public class Movement : MonoBehaviour, Controls.IPlatformingActions
    {
        private Keyboard keyboard;
        new private Rigidbody rigidbody;

        public Controls controls;
        
        private void Awake()
        {
            keyboard = Keyboard.current;
            rigidbody = GetComponent<Rigidbody>();
            
            controls.Platforming.Jump.performed += OnJump;
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            
        }
    }
}
