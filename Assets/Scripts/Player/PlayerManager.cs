using UnityEngine;
using OGAM.Environment;
using OGAM.SceneManagement;


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
            
            if (lastCheckpoint is { }) transform.position = lastCheckpoint.position;
            else SceneSwitcher.ReloadScene();
        }
    }
}