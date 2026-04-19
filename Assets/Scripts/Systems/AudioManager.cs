using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [System.Serializable]
    public class SoundEntry
    {
        public string key;
        public AudioClip clip;
        [Range(0, 1)] public float volume = 1f;
    }

    public SoundEntry[] sfxEntries;
    public AudioSource musicSource;
    public AudioSource sfxSource;

    private Dictionary<string, SoundEntry> sfxMap = new();

    bool musicOn = true;
    bool sfxOn = true;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (var e in sfxEntries)
            sfxMap[e.key] = e;

        musicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        sfxOn = PlayerPrefs.GetInt("SFXOn", 1) == 1;
        ApplySettings();
    }

    public void PlaySFX(string key)
    {
        if (!sfxOn || !sfxMap.ContainsKey(key)) return;
        var e = sfxMap[key];
        sfxSource.PlayOneShot(e.clip, e.volume);
    }

    public void ToggleMusic()
    {
        musicOn = !musicOn;
        PlayerPrefs.SetInt("MusicOn", musicOn ? 1 : 0);
        ApplySettings();
    }

    public void ToggleSFX()
    {
        sfxOn = !sfxOn;
        PlayerPrefs.SetInt("SFXOn", sfxOn ? 1 : 0);
        ApplySettings();
    }

    void ApplySettings()
    {
        if (musicSource != null) musicSource.mute = !musicOn;
    }

    public bool MusicOn => musicOn;
    public bool SFXOn => sfxOn;
}
