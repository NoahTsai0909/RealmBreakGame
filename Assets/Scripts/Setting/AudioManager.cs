using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Mixer")]
    [SerializeField] private AudioMixer mainMixer;

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
        // Load saved volumes or default to 0.75 (75%) if no save exists
        SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume", 0.75f));
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 0.75f));
        SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 0.75f));
    }

    public void SetMasterVolume(float sliderValue)
    {
        // Prevent log10(0) which breaks the math
        float clampValue = Mathf.Clamp(sliderValue, 0.0001f, 1f);
        // Convert linear slider (0 to 1) to logarithmic decibels (-80dB to 0dB)
        mainMixer.SetFloat("MasterVolume", Mathf.Log10(clampValue) * 20f);
        // Save the setting to the hard drive
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
