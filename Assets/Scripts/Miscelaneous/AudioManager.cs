using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.1f, 3f)] public float pitch = 1f;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Música")]
    public AudioClip[] gameMusic;

    [Header("SFX")]
    public Sound[] sfx;

    private float musicVolume = 0.2f;
    private float sfxVolume = 1f;
    private bool isMuted = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadVolumeSettings();
    }

    void Start()
    {
    }


    public void PlayMusic(int index)
    {
        if (index >= 0 && index < gameMusic.Length)
        {
            musicSource.clip = gameMusic[index];
            musicSource.loop = true;
            musicSource.volume = musicVolume;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySFX(string name)
    {
        Sound s = System.Array.Find(sfx, s => s.name == name);
        if (s == null)
        {
            Debug.LogWarning($" SFX '{name}' no encontrado.");
            return;
        }

        Debug.Log($" Reproduciendo SFX: {name}");
        sfxSource.pitch = s.pitch;
        //sfxSource.PlayOneShot(s.clip, s.volume * sfxVolume);
        sfxSource.PlayOneShot(s.clip);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = isMuted ? 0f : musicVolume;
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = isMuted ? 0f : sfxVolume;
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    public void MuteAll()
    {
        isMuted = !isMuted;
        musicSource.volume = isMuted ? 0f : musicVolume;
        sfxSource.volume = isMuted ? 0f : sfxVolume;
    }

    private void LoadVolumeSettings()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.2f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
    }
}
