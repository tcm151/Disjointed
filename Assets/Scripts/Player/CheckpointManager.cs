using UnityEngine;
using OGAM.Environment;


namespace OGAM.Player
{
    public class CheckpointManager : MonoBehaviour
    {
        private Checkpoint currentCheckpoint;

        public void SetCurrentCheckpoint(Checkpoint newCheckpoint) => currentCheckpoint = newCheckpoint;
        public void ReturnToCurrentCheckpoint() => transform.position = currentCheckpoint.position;
    }
}