using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }

    public float CombatSpeedMultiplier { get; private set; } = 1f;
    public bool EnableScreenShake { get; private set; } = true;
    public bool EnableDamageNumbers { get; private set; } = true;

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
            return;
        }
    }

    private void Start()
    {
        LoadGameplaySettings();
    }

    private void LoadGameplaySettings()
    {
        SetCombatSpeed(PlayerPrefs.GetInt("CombatSpeedIndex", 0));
        SetScreenShake(PlayerPrefs.GetInt("ScreenShake", 1) == 1);
        SetDamageNumbers(PlayerPrefs.GetInt("DamageNumbers", 1) == 1);
    }

    public void SetCombatSpeed(int index)
    {
        PlayerPrefs.SetInt("CombatSpeedIndex", index);
        switch (index)
        {
            case 0: CombatSpeedMultiplier = 1f; break;
            case 1: CombatSpeedMultiplier = 1.5f; break;
            case 2: CombatSpeedMultiplier = 2f; break;
            default: CombatSpeedMultiplier = 1f; break;
        }

        gameManager currentCombat = Object.FindFirstObjectByType<gameManager>();
        if (currentCombat != null && currentCombat.isCombatActive())
        {
            Time.timeScale = CombatSpeedMultiplier;
        }
    }

    public void SetScreenShake(bool isEnabled)
    {
        EnableScreenShake = isEnabled;
        PlayerPrefs.SetInt("ScreenShake", isEnabled ? 1 : 0);
    }

    public void SetDamageNumbers(bool isEnabled)
    {
        EnableDamageNumbers = isEnabled;
        PlayerPrefs.SetInt("DamageNumbers", isEnabled ? 1 : 0);
    }
}
