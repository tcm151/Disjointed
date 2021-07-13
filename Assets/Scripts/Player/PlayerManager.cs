using UnityEngine;
using OGAM.Environment;


namespace OGAM.Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private Checkpoint lastCheckpoint;

        //> SET NEW CHECKPOINT
        public void SetCheckpoint(Checkpoint newCheckpoint) => lastCheckpoint = newCheckpoint;
        
        //> DEATH
        public void Die()
        {
            Debug.Log("You Died! :(");
            
            //@ revert to old save state and not just teleport 
            
            transform.position = lastCheckpoint.position;
        }
    }
}