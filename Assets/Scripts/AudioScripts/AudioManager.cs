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

    // Start is called before the first frame update
    protected override void Awake() {
        base.Awake();

        DontDestroyOnLoad(gameObject);

        initSound(music, musicMixer);
        initSound(sfx, sfxMixer);
        initSound(spooky, sfxMixer);

        LoadVolume();
    }

    void initSound(Sound[] sfx, AudioMixerGroup mixer) {
        foreach (Sound s in sfx)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = mixer;
        }
    }

    public void Play(string name) {
        Sound s = findSound(name);
        if (s == null) {
            Debug.LogWarning("No sound: " + name + " was found");
            return;
        }

        // use: FindAnyObjectByType<AudioManager>().Play("name of sound")

        s.source.Play();
    }

    public void Pause(string name) {
        Sound s = findSound(name);
        if (s == null) {
            Debug.LogWarning("No sound: " + name + " was found");
            return;
        }

        s.source.Pause();
    }

    public void Continue(string name){
        Sound s = findSound(name);
        if (s == null){
            Debug.LogWarning("No sound: " + name + " was found");
            return;
        }

        s.source.UnPause();
    }

    public void Stop(string name) {
        Sound s = findSound(name);
        if (s == null) {
            Debug.LogWarning("No sound: " + name + " was found");
            return;
        }

        s.source.Stop();
    }

    Sound findSound(string name) {
        Sound s = Array.Find(music, sound => sound.name == name) 
            ?? Array.Find(sfx, sound => sound.name == name)
            ?? Array.Find(spooky, sound => sound.name == name);

        return s;
    }

    void LoadVolume()
    {
        float musicVol = PlayerPrefs.GetFloat(MUSIC_VOLUME, 0.75f);
        float sfxVol = PlayerPrefs.GetFloat(SFX_VOLUME, 0.75f);

        musicMixer.audioMixer.SetFloat(MUSIC_VOLUME, PlayerPrefs.GetFloat(MUSIC_VOLUME, Mathf.Log10(musicVol) * 20));
        sfxMixer.audioMixer.SetFloat(SFX_VOLUME, PlayerPrefs.GetFloat(SFX_VOLUME, Mathf.Log10(sfxVol) * 20));
    }
}