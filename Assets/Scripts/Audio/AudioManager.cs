using System;
using System.Linq;
using System.Collections;
using UnityEngine;
using Disjointed.Tools.Extensions;


namespace Disjointed.Audio
{
    
    public class AudioManager : MonoBehaviour
    //>---------------------------------------------------------------------------------------------
    //> |    PURPOSE   |  
    //>---------------------------------------------------------------------------------------------
    //> |    AUTHORS   |
    //> |   REFACTORS  |
    //> | CONTRIBUTORS |
    //>---------------------------------------------------------------------------------------------
    {
        //- LOCAL VARIABLES
        [SerializeField] private SFX[] soundEffects;
        [SerializeField] private AudioSource[] sources;

        //- EVENT TRIGGERS
        public static Action<string> PlaySFX;
        public static Action<SFX, int, bool> PlayTrack;
        public static Action<string, int, bool> FindAndPlayTrack;

        //> INITIALIZATION
        private void Awake()
        {
            AudioListener.volume = PlayerPrefs.GetFloat("GlobalVolume", 1f);

            PlaySFX += OnPlaySFX;
            PlayTrack += OnPlayTrack;
            FindAndPlayTrack += OnPlayTrack;

            StartCoroutine(CR_StartMusic(true));
        }

        //> MUSIC SEQUENCER
        private IEnumerator CR_StartMusic(bool loop)
        {
            var backgroundNoise = soundEffects.First(s => s.name == "CaveBackgroundNoise");
            var caveTheme1 = soundEffects.First(s => s.name == "CaveTheme1");
            var caveTheme2 = soundEffects.First(s => s.name == "CaveTheme2");

            OnPlayTrack(backgroundNoise, 0);
            yield return new WaitForSeconds(backgroundNoise.clip.length);
            OnPlayTrack(caveTheme1, 1);
            yield return new WaitForSeconds(caveTheme1.clip.length);
            OnPlayTrack(backgroundNoise, 0);
            yield return new WaitForSeconds(backgroundNoise.clip.length);
            OnPlayTrack(caveTheme2, 1);
            yield return new WaitForSeconds(caveTheme2.clip.length);
            
            if (loop) StartCoroutine(CR_StartMusic(loop));
        }

        private IEnumerator CR_PlayPlaylist(SFX[] tracks, int channel, bool shuffle, bool loop)
        {
            if (tracks.Length <= 0) Debug.LogWarning("Provided <colors=yellow>NO SONGS</color> in Playlist!");
            if (shuffle) tracks = tracks.Shuffle().ToArray();
            
            foreach (var track in tracks)
            {
                OnPlayTrack(track, channel);
                yield return new WaitForSeconds(track.clip.length);
            }
        }

        //> PLAY ONE SHOT SOUND CLIP
        private void OnPlaySFX(string name)
        {
            SFX sfx = soundEffects.FirstOrDefault(s => s.name == name);
            if (sfx is { }) sources[1].PlayOneShot(sfx.clip, sfx.volume);
            else Debug.LogWarning($"Unable to find sfx: <color=yellow>\"{name}\"</color>");
        }

        //> REPLACE STREAM SOUND CLIP
        private void OnPlayTrack(string track, int channel , bool loop = false)
        {
            SFX sfx = soundEffects.FirstOrDefault(s => s.name == track);
            OnPlayTrack(sfx, channel, loop);
        }

        private void OnPlayTrack(SFX sfx, int channel, bool loop = false)
        {
            if (sfx is { })
            {
                sources[channel].clip = sfx.clip;
                sources[channel].pitch = sfx.pitch;
                sources[channel].volume = sfx.volume;
                sources[channel].loop = loop;
                sources[channel].Play();
            }
            else Debug.LogWarning($"Unable to find track: <color=yellow>\"{name}\"</color>");
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