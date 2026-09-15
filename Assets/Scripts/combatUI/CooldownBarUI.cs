using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CooldownBarUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image cooldownFill;
    [SerializeField] private Image backgroundImage; // Links to your Background image
    [SerializeField] private TextMeshProUGUI valueText;

    // Swaps the sprites based on the passed rarity
    public void SetVisuals(Rarity rarity)
    {
        if (cooldownFill == null || backgroundImage == null) return;
    }

    public void SetValues(float remaining, float maxCooldown)
    {
        float fill = remaining / maxCooldown;
        cooldownFill.fillAmount = fill;

        if (valueText != null)
        {
            valueText.text = remaining.ToString("F1") + "s";
        }
    }

    public void SetTextVisible(bool visible)
    {
        if (valueText != null)
            valueText.gameObject.SetActive(visible);
    }
}