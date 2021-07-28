using System;
using System.Collections;
using System.Linq;
using UnityEngine;


namespace TCM.Audio
{
    public class AudioManager : MonoBehaviour
    {
        //- SINGLETON
        private static AudioManager instance;

        public static AudioManager Connect
        {
            get
            {
                if (!instance) Debug.LogError("No <color=red>AudioManager</color> in scene!");
                return instance;
            }
        }

        //- LOCAL VARIABLES
        [SerializeField] private AudioSource[] sources;
        [SerializeField] private SFX[] soundEffects;

        //> INITIALIZATION
        private void Awake()
        {
            instance = this;
            
            Play("CaveBackgroundNoise", 0, true);
            StartCoroutine(CR_Music());
        }

        private IEnumerator CR_Music()
        {
            yield return new WaitForSeconds(soundEffects.First(s => s.name == "CaveBackgroundNoise").clip.length);
            Play("CaveTheme1", 1);
            yield return new WaitForSeconds(soundEffects.First(s => s.name == "CaveTheme1").clip.length);
            yield return new WaitForSeconds(soundEffects.First(s => s.name == "CaveBackgroundNoise").clip.length);
            Play("CaveTheme2", 1);
            yield return new WaitForSeconds(soundEffects.First(s => s.name == "CaveTheme2").clip.length);

            StartCoroutine(CR_Music());
        }

        //> PLAY ONE SHOT SOUND AT POINT IN WORLD
        public void PlayAtPoint(string name, Vector3 point)
        {
            SFX sfx = soundEffects.First(s => s.name == name);
            AudioSource.PlayClipAtPoint(sfx.clip, point, sfx.volume);
            // else Debug.LogWarning($"Unable to find sound: <color=yellow>{name}</color>");
        }

        //> PLAY ONE SHOT SOUND CLIP
        public void PlayOneShot(string name)
        {
            SFX sfx = soundEffects.First(s => s.name == name);
            sources[0].PlayOneShot(sfx.clip, sfx.volume);
            // else Debug.LogWarning($"Unable to find sound: <color=yellow>{name}</color>");
        }

        //> REPLACE STREAM SOUND CLIP
        public void Play(string sound, int track, bool loop = false)
        {
            SFX sfx = soundEffects.First(s => s.name == sound);
            sources[track].clip = sfx.clip;
            sources[track].pitch = sfx.pitch;
            sources[track].volume = sfx.volume;
            sources[track].loop = loop;
            sources[track].Play();
        }

        //> STOP STREAM SOUND CLIP
        public void Stop(int stream) => sources[stream].Stop();
    }
}