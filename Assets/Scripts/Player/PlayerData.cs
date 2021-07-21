using Disjointed.Environment;
using Disjointed.Tools.SceneManagement;
using UnityEngine;


namespace Disjointed.Player
{
    public class PlayerData : MonoBehaviour
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