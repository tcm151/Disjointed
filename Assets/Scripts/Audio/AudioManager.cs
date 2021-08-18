using System;
using System.Collections;
using System.Linq;
using UnityEngine;


namespace Disjointed.Audio
{
    public class AudioManager : MonoBehaviour
    {
        //- LOCAL VARIABLES
        [SerializeField] private SFX[] soundEffects;
        [SerializeField] private AudioSource[] sources;

        public static Action<string> PlaySFX;
        public static Action<string, int, bool> PlayTrack;

        //> INITIALIZATION
        private void Awake()
        {
            AudioListener.volume = PlayerPrefs.GetFloat("GlobalVolume", 1f);

            PlaySFX += OnPlaySFX;
            PlayTrack += OnPlayTrack;
            
            StartCoroutine(CR_MusicPlaylist());
        }

        private IEnumerator CR_MusicPlaylist()
        {
            OnPlayTrack("CaveBackgroundNoise", 0, true);
            yield return new WaitForSeconds(soundEffects.First(s => s.name == "CaveBackgroundNoise").clip.length);
            OnPlayTrack("CaveTheme1", 1);
            yield return new WaitForSeconds(1f);
            sources[0].volume = 0.5f;
            yield return new WaitForSeconds(soundEffects.First(s => s.name == "CaveTheme1").clip.length);
            sources[0].volume = 1f;
            yield return new WaitForSeconds(soundEffects.First(s => s.name == "CaveBackgroundNoise").clip.length);
            OnPlayTrack("CaveTheme2", 1);
            yield return new WaitForSeconds(1f);
            sources[0].volume = 0.5f;
            yield return new WaitForSeconds(soundEffects.First(s => s.name == "CaveTheme2").clip.length);

            StartCoroutine(CR_MusicPlaylist());
        }

        //> PLAY ONE SHOT SOUND CLIP
        private void OnPlaySFX(string name)
        {
            SFX sfx = soundEffects.FirstOrDefault(s => s.name == name);
            if (sfx is { }) sources[1].PlayOneShot(sfx.clip, sfx.volume);
            else Debug.LogWarning($"Unable to find sound: <color=yellow>{name}</color>");
        }

        //> REPLACE STREAM SOUND CLIP
        private void OnPlayTrack(string sound, int track, bool loop = false)
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
        private void Stop(int stream) => sources[stream].Stop();

        private static void SetVolume(float volume)
        {
            AudioListener.volume = volume;
            PlayerPrefs.SetFloat("GlobalVolume", volume);
        }
    }
}