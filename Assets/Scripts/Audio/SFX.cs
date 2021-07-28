
using UnityEngine;
using UnityEngine.Serialization;


namespace Disjointed.Audio
{
    //> CONTAINER FOR ALL IN GAME SOUND EFFECTS
    [System.Serializable] public class SFX
    {
        public string name; 
        public AudioClip clip;
        [Range(0, 2f)]public float pitch = 1f;
        [Range(0, 2f)]public float volume = 1f;
    }
}