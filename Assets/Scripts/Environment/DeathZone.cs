using Disjointed.Player;
using UnityEngine;
using Disjointed.Tools.Extensions;


namespace Disjointed.Environment
{
    public class DeathZone : MonoBehaviour
    {
        public LayerMask playerMask;
        
        private void OnTriggerEnter2D(Collider2D collider)
        {
            // ignore anything except the thePlayer
            if (!playerMask.Contains(collider.gameObject.layer)) return;

            var player = collider.GetComponent<ThePlayer>();
            if (player is null) return;
            
            player.Die();
        }
        
    }
}
