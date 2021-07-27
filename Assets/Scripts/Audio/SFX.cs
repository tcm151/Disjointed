
namespace TCM.Audio
{
    //> CONTAINER FOR ALL IN GAME SOUND EFFECTS
    [System.Serializable] public class SFX
    {
        public string name; 
        public float pitch = 1f;
        public float volume = 1f;
        
        //- RAW AUDIO FILE
        public UnityEngine.AudioClip audio;
    }
}