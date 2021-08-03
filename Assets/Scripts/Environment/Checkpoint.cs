using UnityEngine;
using Disjointed.Player;
using Disjointed.Tools.Extensions;
using Disjointed.Tools.Serialization;


namespace Disjointed.Environment
{
    public class Checkpoint : MonoBehaviour
    {
        public Serializer serializer;
        public LayerMask playerMask;

        public Vector3 position => transform.position;
        
        private void OnTriggerEnter2D(Collider2D collider)
        {
            // ignore anything except the thePlayer
            if (!playerMask.Contains(collider.gameObject.layer)) return;

            serializer.SaveGame();
            Debug.Log("Saving Game!");
            
            var checkpointManager = collider.GetComponent<ThePlayer>();
            if (!checkpointManager) return;
            
            checkpointManager.SetCheckpoint(this);
        }
    }
}
