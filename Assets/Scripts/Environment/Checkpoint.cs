using System;
using OGAM.Player;
using OGAM.Tools;
using UnityEngine;  

namespace OGAM.Environment
{
    public class Checkpoint : MonoBehaviour
    {
        public LayerMask playerMask;

        public Vector3 position => transform.position;
        
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!playerMask.Contains(collider.gameObject.layer)) return;

            var checkpointManager = collider.GetComponent<CheckpointManager>();
            if (checkpointManager is null) return;
            
            checkpointManager.SetCurrentCheckpoint(this);
                
        }
    }
}
