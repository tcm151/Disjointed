using Disjointed.Player;
using UnityEngine;
using Disjointed.Tools;

namespace Disjointed.Environment
{
    public class DeathZone : MonoBehaviour
    {
        public LayerMask playerMask;
        
        private void OnTriggerEnter2D(Collider2D collider)
        {
            // ignore anything except the player
            if (!playerMask.Contains(collider.gameObject.layer)) return;

            var player = collider.GetComponent<PlayerManager>();
            if (player is null) return;
            
            player.Die();
        }
        
    }
}
