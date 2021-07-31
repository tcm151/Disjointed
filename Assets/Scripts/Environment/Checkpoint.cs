using UnityEngine;
using Disjointed.ThePlayer;
using Disjointed.Tools.Extensions;


namespace Disjointed.Environment
{
    public class Checkpoint : MonoBehaviour
    {
        public LayerMask playerMask;

        public Vector3 position => transform.position;
        
        private void OnTriggerEnter2D(Collider2D collider)
        {
            // ignore anything except the player
            if (!playerMask.Contains(collider.gameObject.layer)) return;

            var checkpointManager = collider.GetComponent<Player>();
            if (checkpointManager is null) return;
            
            checkpointManager.SetCheckpoint(this);
                
        }
    }
}
