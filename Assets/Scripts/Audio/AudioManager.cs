using System;
using System.Collections;
using System.Linq;
using UnityEngine;


namespace Disjointed.Audio
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
            AudioListener.volume = PlayerPrefs.GetFloat("GlobalVolume", 1f);
            
            Play("CaveBackgroundNoise", 0, true);
            StartCoroutine(CR_Music());
        }

        private IEnumerator CR_Music()
        {
            yield return new WaitForSeconds(soundEffects.First(s => s.name == "CaveBackgroundNoise").clip.length);
            Play("CaveTheme1", 1);
            yield return new WaitForSeconds(1f);
            sources[0].volume = 0.25f;
            yield return new WaitForSeconds(soundEffects.First(s => s.name == "CaveTheme1").clip.length);
            sources[0].volume = 1f;
            yield return new WaitForSeconds(soundEffects.First(s => s.name == "CaveBackgroundNoise").clip.length);
            Play("CaveTheme2", 1);
            yield return new WaitForSeconds(1f);
            sources[0].volume = 0.25f;
            yield return new WaitForSeconds(soundEffects.First(s => s.name == "CaveTheme2").clip.length);

            StartCoroutine(CR_Music());
        }

        //> PLAY ONE SHOT SOUND AT POINT IN WORLD
        public void PlayAtPoint(string name, Vector3 point)
        {
            SFX sfx = soundEffects.FirstOrDefault(s => s.name == name);
            if (sfx is { }) AudioSource.PlayClipAtPoint(sfx.clip, point, sfx.volume);
            else Debug.LogWarning($"Unable to find sound: <color=yellow>{name}</color>");
        }

        //> PLAY ONE SHOT SOUND CLIP
        public void PlayOneShot(string name)
        {
            SFX sfx = soundEffects.FirstOrDefault(s => s.name == name);
            if (sfx is { }) sources[1].PlayOneShot(sfx.clip, sfx.volume);
            else Debug.LogWarning($"Unable to find sound: <color=yellow>{name}</color>");
        }

        //> REPLACE STREAM SOUND CLIP
        public void Play(string sound, int track, bool loop = false)
        {
            SFX sfx = soundEffects.FirstOrDefault(s => s.name == sound);
            if (sfx is { })
            {
                sources[track].clip = sfx.clip;
                sources[track].pitch = sfx.pitch;
                sources[track].volume = sfx.volume;
                sources[track].loop = loop;
                sources[track].Play();
            }
            else Debug.LogWarning($"Unable to find sound: <color=yellow>{name}</color>");
        }

        //> STOP STREAM SOUND CLIP
        public void Stop(int stream) => sources[stream].Stop();

        public static void SetVolume(float volume)
        {
            AudioListener.volume = volume;
            PlayerPrefs.SetFloat("GlobalVolume", volume);
        }
    }
}