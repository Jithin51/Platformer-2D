using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips")]
    [SerializeField] private Sound[] musicSounds;
    [SerializeField] private Sound[] sfxSounds;

    private readonly Dictionary<string, Sound> music = new();
    private readonly Dictionary<string, Sound> sfx = new();

    private void Awake()
    {
        // Keep exactly one AudioManager across all scenes
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (var s in musicSounds) music[s.name] = s;
        foreach (var s in sfxSounds) sfx[s.name] = s;
    }

    public void PlaySFX(string name)
    {
        if (!sfx.TryGetValue(name, out Sound s))
        {
            Debug.LogWarning($"SFX not found: {name}");
            return;
        }
        sfxSource.PlayOneShot(s.clip, s.volume);
    }

    public void PlayMusic(string name, bool restart = false)
    {
        if (!music.TryGetValue(name, out Sound s))
        {
            Debug.LogWarning($"Music not found: {name}");
            return;
        }
        if (!restart && musicSource.clip == s.clip && musicSource.isPlaying) return;

        musicSource.clip = s.clip;
        musicSource.volume = s.volume;
        musicSource.loop = true;
        musicSource.Play();   // Play() always starts from the beginning
    }

    public void StopMusic() => musicSource.Stop();

    public void SetMusicVolume(float v) => musicSource.volume = v;
    public void SetSFXVolume(float v) => sfxSource.volume = v;
}