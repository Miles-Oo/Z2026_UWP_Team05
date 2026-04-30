using System;
using System.Collections.Generic;
using UnityEngine;

public enum AudioCue
{
    None,
    Pause,
    Resume,
    Win,
    Lose,
    EnemyDeath,
    TowerBuild,
    ButtonClick,
    MusicGameplay,
    MusicMenu
}

[Serializable]
public class AudioCueClip
{
    public AudioCue cue;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private bool persistBetweenScenes = true;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioCueClip[] cueClips;

    private readonly Dictionary<AudioCue, AudioClip> _clipsByCue = new Dictionary<AudioCue, AudioClip>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (persistBetweenScenes)
        {
            DontDestroyOnLoad(gameObject);
        }

        EnsureAudioSources();
        BuildClipDictionary();
    }

    public void PlaySfx(AudioCue cue)
    {
        if (cue == AudioCue.None || sfxSource == null)
        {
            return;
        }

        if (_clipsByCue.TryGetValue(cue, out AudioClip clip) && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayMusic(AudioCue cue, bool loop = true)
    {
        if (musicSource == null)
        {
            return;
        }

        if (!_clipsByCue.TryGetValue(cue, out AudioClip clip) || clip == null)
        {
            return;
        }

        if (musicSource.clip == clip && musicSource.isPlaying)
        {
            return;
        }

        musicSource.loop = loop;
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    public void PauseMusic()
    {
        if (musicSource != null)
            musicSource.Pause();
    }

    public void ResumeMusic()
    {
        if (musicSource != null)
            musicSource.UnPause();
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = Mathf.Clamp01(volume);
        }
    }

    public void SetSfxVolume(float volume)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = Mathf.Clamp01(volume);
        }
    }

    private void EnsureAudioSources()
    {
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.playOnAwake = false;
            musicSource.loop = true;
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
            sfxSource.loop = false;
        }
    }

    private void BuildClipDictionary()
    {
        _clipsByCue.Clear();

        if (cueClips == null)
        {
            return;
        }

        foreach (AudioCueClip cueClip in cueClips)
        {
            if (cueClip == null || cueClip.cue == AudioCue.None || cueClip.clip == null)
            {
                continue;
            }

            _clipsByCue[cueClip.cue] = cueClip.clip;
        }
    }
}
