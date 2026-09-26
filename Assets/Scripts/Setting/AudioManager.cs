using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Mixer")]
    [SerializeField] private AudioMixer mainMixer;


    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    private Dictionary<AudioClip, float> clipLastPlayedTime = new Dictionary<AudioClip, float>();
    private float soundCooldown = 0.05f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume", 0.75f));
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 0.75f));
        SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 0.75f));
    }

    public void PlaySFX(AudioClip clip, float volumeMultiplier = 1f)
    {
        if (clip != null && sfxSource != null)
        {
            float currentTime = Time.unscaledTime;

            if (clipLastPlayedTime.TryGetValue(clip, out float lastTime))
            {
                if (currentTime - lastTime < soundCooldown)
                {
                    return;
                }
            }
            clipLastPlayedTime[clip] = currentTime;
            sfxSource.PlayOneShot(clip, volumeMultiplier);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null) return;
        if (musicSource.clip == clip) return; 

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void SetMasterVolume(float sliderValue)
    {
        float clampValue = Mathf.Clamp(sliderValue, 0.0001f, 1f);
        mainMixer.SetFloat("MasterVolume", Mathf.Log10(clampValue) * 20f);
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
    }

    public void SetMusicVolume(float sliderValue)
    {
        float clampValue = Mathf.Clamp(sliderValue, 0.0001f, 1f);
        mainMixer.SetFloat("MusicVolume", Mathf.Log10(clampValue) * 20f);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }

    public void SetSFXVolume(float sliderValue)
    {
        float clampValue = Mathf.Clamp(sliderValue, 0.0001f, 1f);
        mainMixer.SetFloat("SFXVolume", Mathf.Log10(clampValue) * 20f);
        PlayerPrefs.SetFloat("SFXVolume", sliderValue);
    }
}