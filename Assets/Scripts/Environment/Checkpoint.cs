using Disjointed.Audio;
using UnityEngine;
using Disjointed.Tools.Extensions;
using Disjointed.Tools.Serialization;


namespace Disjointed.Environment
{
    public class Checkpoint : MonoBehaviour
    {
        public Serializer serializer;
        public LayerMask playerMask;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            // ignore anything except the thePlayer
            if (!playerMask.Contains(collider.gameObject.layer)) return;

            serializer.SaveGame();
            
            AudioManager.PlaySFX?.Invoke("Checkpoint");
        }
    }
}
