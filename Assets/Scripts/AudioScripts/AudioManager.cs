using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    public AudioMixerGroup musicMixer;
    public AudioMixerGroup sfxMixer;

    public Sound[] music;
    public Sound[] sfx;
    public Sound[] spooky;

    [HideInInspector]
    public const string MUSIC_VOLUME = "Music";
    public const string SFX_VOLUME = "sfx";

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        InitializeSounds(music, musicMixer);
        InitializeSounds(sfx, sfxMixer);
        InitializeSounds(spooky, sfxMixer);

        LoadVolume();
    }

    private void InitializeSounds(Sound[] sounds, AudioMixerGroup mixer)
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = mixer;
        }
    }

    public void Play(string name)
    {
        Sound s = FindSound(name);
        if (s == null)
        {
            Debug.LogWarning($"AudioManager: Sound '{name}' not found.");
            return;
        }
        s.source.Play();
    }

    public void Pause(string name)
    {
        Sound s = FindSound(name);
        if (s == null) return;
        s.source.Pause();
    }

    public void Continue(string name)
    {
        Sound s = FindSound(name);
        if (s == null) return;
        s.source.UnPause();
    }

    public void Stop(string name)
    {
        Sound s = FindSound(name);
        if (s == null) return;
        s.source.Stop();
    }

    public void StopAll()
    {
        foreach (Sound s in music) if (s.source.isPlaying) s.source.Stop();
        foreach (Sound s in sfx) if (s.source.isPlaying) s.source.Stop();
        foreach (Sound s in spooky) if (s.source.isPlaying) s.source.Stop();
    }

    private Sound FindSound(string name)
    {
        return Array.Find(music, sound => sound.name == name)
            ?? Array.Find(sfx, sound => sound.name == name)
            ?? Array.Find(spooky, sound => sound.name == name);
    }

    private void LoadVolume()
    {
        float musicVol = PlayerPrefs.GetFloat(MUSIC_VOLUME, 0.75f);
        float sfxVol = PlayerPrefs.GetFloat(SFX_VOLUME, 0.75f);

        musicMixer.audioMixer.SetFloat(MUSIC_VOLUME, Mathf.Log10(musicVol) * 20);
        sfxMixer.audioMixer.SetFloat(SFX_VOLUME, Mathf.Log10(sfxVol) * 20);
    }
}
