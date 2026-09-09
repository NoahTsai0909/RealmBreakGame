using UnityEngine;
using System.Collections.Generic;

public class VideoManager : MonoBehaviour
{
    public static VideoManager Instance { get; private set; }

    [HideInInspector] public Resolution[] availableResolutions;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Fetch all resolutions supported by the player's specific monitor
            availableResolutions = Screen.resolutions;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        LoadVideoSettings();
    }

    private void LoadVideoSettings()
    {
        // 1. Load VSync
        int vSync = PlayerPrefs.GetInt("VSync", 1); // Default to ON
        QualitySettings.vSyncCount = vSync;

        // 2. Load Display Mode
        int displayModeIndex = PlayerPrefs.GetInt("DisplayMode", 0); // Default to Exclusive Fullscreen
        FullScreenMode mode = IndexToFullScreenMode(displayModeIndex);

        // 3. Load Resolution (Default to the monitor's max resolution)
        int defaultResIndex = availableResolutions.Length - 1;
        int resIndex = PlayerPrefs.GetInt("ResolutionIndex", defaultResIndex);

        // Failsafe in case they switched monitors
        if (resIndex < 0 || resIndex >= availableResolutions.Length)
            resIndex = defaultResIndex;

        Resolution savedRes = availableResolutions[resIndex];

        // Apply everything at once!
        Screen.SetResolution(savedRes.width, savedRes.height, mode);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution res = availableResolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
    }

    public void SetDisplayMode(int modeIndex)
    {
        FullScreenMode mode = IndexToFullScreenMode(modeIndex);
        Screen.fullScreenMode = mode;
        PlayerPrefs.SetInt("DisplayMode", modeIndex);
    }

    public void SetVSync(bool isEnabled)
    {
        int vSyncValue = isEnabled ? 1 : 0;
        QualitySettings.vSyncCount = vSyncValue;
        PlayerPrefs.SetInt("VSync", vSyncValue);
    }

    // Helper method to translate the Dropdown index to Unity's FullScreenMode
    private FullScreenMode IndexToFullScreenMode(int index)
    {
        switch (index)
        {
            case 0: return FullScreenMode.ExclusiveFullScreen;
            case 1: return FullScreenMode.FullScreenWindow; // Borderless
            case 2: return FullScreenMode.Windowed;
            default: return FullScreenMode.ExclusiveFullScreen;
        }
    }
}
